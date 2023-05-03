namespace EMS.UN3
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    using EMS.UN3;
    using UnityEngine.Rendering;


    public struct Submesh
    {
        public UnityEngine.MeshFilter meshFilter;
        public UnityEngine.Mesh mesh;
        public int subMeshIndex;
    }

    public struct TextureImageLink
    {
        public UnityEngine.Texture texture;
        public string filename;
    }

    public struct MeshMaterialLink
    {
        public UnityEngine.MeshFilter meshFilter;
        public UnityEngine.Material material;
        public int subMeshIndex;
    }

    public struct MaterialTextureLink
    {
        public UnityEngine.Material material;
        public UnityEngine.Texture texture;
        public string textureUnityIdentifier;
        public bool needsCleanup;
    }

    public class SceneTransformer
    {




        //Transfom[] are the objects selected from scene to be exported
        public Scene selectedScene(Transform[] selection, ExportSettings exportSettings, Dictionary<byte[], string> imgscontainer)
        {





            string GetValidFileNameFromString(string file)
            {
                Array.ForEach(Path.GetInvalidFileNameChars(),
                    c => file = file.Replace(c.ToString(), String.Empty));

                return file;
            }

            //I am not sure about this
            List<string> commonTextures = new List<string>
            {
                "_BaseMap", "_EmissionMap", "_MetallicGlossMap", "_OcclusionMap", "_SpecGlossMap", "_BumpMap",
                "_ParallaxMap"
            };


            //Iterate each Transform that has mesh and create objects, materials and meshes


            //Create list of unique materials so can be reused...
            Dictionary<string, UnityEngine.Material> usedMaterials = new Dictionary<string, UnityEngine.Material>();

            //List of unique submeshes
            Dictionary<string, Submesh> usedSubmeshes = new Dictionary<string, Submesh>();

            //All mesh material links
            Dictionary<string, MeshMaterialLink> meshMaterialLinks = new Dictionary<string, MeshMaterialLink>();
            //All material texture links
            Dictionary<string, MaterialTextureLink>
                usedLightmapTextures = new Dictionary<string, MaterialTextureLink>();
            //All texture image links
            Dictionary<string, TextureImageLink> usedImages = new Dictionary<string, TextureImageLink>();
            //All textures
            List<UnityEngine.Texture> addedTextures = new List<UnityEngine.Texture>();
            //All lightmaps
            LightmapData[] lightmapData = UnityEngine.LightmapSettings.lightmaps;

            //------------------------------------------------------------------------------------------------------Prepare
            foreach (Transform transform in selection)
            {
                //---------------------------------------------------------------------------------------------------MATERIALS
                MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    UnityEngine.Material[] mats = meshFilter.GetComponent<Renderer>().sharedMaterials;

                    for (int i = 0; i < mats.Length; i++)
                    {
                        UnityEngine.Material mat = mats[i];

                        if (!mat)
                        {
                            continue;
                        }

                        if (!usedMaterials.ContainsValue(mat))
                        {
                            usedMaterials.Add(System.Guid.NewGuid().ToString(), mat);
                        }
                    }
                }

                //--------------------------------------------------------------------------------------------------SUBMESHES
                if (meshFilter != null)
                {
                    if (meshFilter.sharedMesh == null)
                    {
                        Debug.LogError(
                            "'" + transform.name + "' appears to be missing a mesh! Please verify its mesh filter.",
                            transform);
                    }
                    else
                    {
                        for (int i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
                        {
                            Submesh submesh;
                            submesh.mesh = meshFilter.sharedMesh;
                            submesh.meshFilter = meshFilter;
                            submesh.subMeshIndex = i;
                            usedSubmeshes.Add(System.Guid.NewGuid().ToString(), submesh);
                        }
                    }
                }

                //---------------------------------------------------------------------------------------------------Material-TEX links
                if (meshFilter != null)
                {
                    UnityEngine.Material[] mats = meshFilter.GetComponent<Renderer>().sharedMaterials;

                    for (int i = 0; i < mats.Length; i++)
                    {

                        if (!mats[i])
                        {
                            Debug.LogError("'" + transform.name + "' has a bad/missing material!", transform);
                            continue;
                        }

                        MeshMaterialLink meshMaterialLink;

                        meshMaterialLink.meshFilter = meshFilter;
                        meshMaterialLink.material = mats[i];
                        meshMaterialLink.subMeshIndex = i;

                        commonTextures.ForEach(delegate(String texName)
                        {

                            if (!meshMaterialLink.material.HasProperty(texName)) return;

                            UnityEngine.Texture tex = meshMaterialLink.material.GetTexture(texName);
                            if (tex != null && !addedTextures.Contains(tex))
                            {


                                TextureImageLink textureImageLink;
                                textureImageLink.texture = tex;
                                string texturePath = AssetDatabase.GetAssetPath(tex);

                                var textureExtension = System.IO.Path.GetExtension(texturePath).ToLower();
                                if (textureExtension != ".png" && textureExtension != ".jpg")
                                {
                                    // Other textures get converted to png automatically
                                    texturePath = System.IO.Path.ChangeExtension(texturePath, ".png");
                                }

                                textureImageLink.filename = GetValidFileNameFromString(meshMaterialLink.material.name) +
                                                            "_" + System.IO.Path.GetFileName(texturePath);

                                addedTextures.Add(tex);

                                usedImages.Add(System.Guid.NewGuid().ToString(), textureImageLink);
                            }
                        });

                        meshMaterialLinks.Add(System.Guid.NewGuid().ToString(), meshMaterialLink);

                    }


                }

                //---------------------------------------------------------------------------------------------------Lightmaps


                if (meshFilter != null && true && exportSettings.exportLightmaps) // settings
                {
                    for (int i = 0; i < lightmapData.Length; i++)
                    {
                        if (lightmapData[i].lightmapColor)
                        {
                            TextureImageLink textureImageLink;
                            textureImageLink.texture = lightmapData[i].lightmapColor;

                            string texturePath = AssetDatabase.GetAssetPath(lightmapData[i].lightmapColor);
                            string texturePathPNG = System.IO.Path.ChangeExtension(texturePath, ".png");
                            textureImageLink.filename = System.IO.Path.GetFileName(texturePathPNG);

                            UnityEngine.Material lightMapMaterial = new UnityEngine.Material(Shader.Find("Diffuse"));
                            lightMapMaterial.hideFlags = HideFlags.HideAndDontSave;
                            MaterialTextureLink materialTextureLink;
                            materialTextureLink.texture = textureImageLink.texture;
                            lightMapMaterial.name = "lightmap";
                            lightMapMaterial.SetTexture("_MainTex", materialTextureLink.texture);
                            materialTextureLink.material = lightMapMaterial;
                            materialTextureLink.textureUnityIdentifier = "_MainTex";
                            materialTextureLink.needsCleanup = true;

                            usedLightmapTextures.Add(System.Guid.NewGuid().ToString(), materialTextureLink);

                            usedImages.Add(System.Guid.NewGuid().ToString(), textureImageLink);
                        }


                        if (lightmapData[i].lightmapDir)
                        {
                            TextureImageLink textureImageLink;
                            textureImageLink.texture = lightmapData[i].lightmapDir;

                            string texturePath = AssetDatabase.GetAssetPath(lightmapData[i].lightmapDir);
                            string texturePathPNG = System.IO.Path.ChangeExtension(texturePath, ".png");
                            textureImageLink.filename = System.IO.Path.GetFileName(texturePathPNG);

                            usedImages.Add(System.Guid.NewGuid().ToString(), textureImageLink);
                        }
                    }
                }



            }

            Scene result = new();
            var myFog = new Fog();
            //-------------------------------------------------------------------------------------------------Copy geometry data
            List<Geometry> geometries = new List<Geometry>();
            foreach (KeyValuePair<string, Submesh> entry in usedSubmeshes)
            {
                var data = new Data();


                int[] subMeshTriangles = entry.Value.mesh.GetTriangles(entry.Value.subMeshIndex);

                //-------------------------------------------create bounding box
                var boundingSphere = new BoundingSphere();

                boundingSphere.Radius = entry.Value.mesh.bounds.extents.magnitude;
                boundingSphere.Center = new List<double>
                {
                    entry.Value.mesh.bounds.center.x, entry.Value.mesh.bounds.center.y, entry.Value.mesh.bounds.center.z
                };


                int lowestFaceIndex = int.MaxValue;
                for (int i = 0; i < subMeshTriangles.Length; i++)
                {
                    if (subMeshTriangles[i] < lowestFaceIndex)
                    {
                        lowestFaceIndex = subMeshTriangles[i];
                    }
                }

                int highestFaceIndex = -1;
                for (int i = 0; i < subMeshTriangles.Length; i++)
                {
                    if (subMeshTriangles[i] > highestFaceIndex)
                    {
                        highestFaceIndex = subMeshTriangles[i];
                    }
                }

                //--------------------------------------setup geometry data

                data.BoundingSphere = boundingSphere;

                var attributes = new Attributes();


                //Create data for verts
                var verts = new GeometryAttribute();
                verts.ItemSize = 3;
                verts.Type = "Float32Array";
                verts.Normalized = false;
                var vertsArray = new List<double>();

                int[] indices = entry.Value.mesh.triangles;
                int triangleCount = indices.Length / 3;
                List<Vector3> vertexesByFace = new();
                for (int i = 0; i < triangleCount; i++)
                {
                    vertexesByFace.Add(entry.Value.mesh.vertices[indices[i * 3]]);
                    vertexesByFace.Add(entry.Value.mesh.vertices[indices[i * 3 + 1]]);
                    vertexesByFace.Add(entry.Value.mesh.vertices[indices[i * 3 + 2]]);
                }

                foreach (var item in vertexesByFace)
                {
                    vertsArray.Add(item.x);
                    vertsArray.Add(item.y);
                    vertsArray.Add(item.z);
                }

                /*
                for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                {
                    Vector3 vertex = entry.Value.mesh.vertices[i];
                    vertsArray.Add(vertex.z);
                    vertsArray.Add(vertex.y);
                    vertsArray.Add(vertex.x);
                }
                */
                verts.Array = vertsArray;

                //Create data for normals
                var normals = new GeometryAttribute();
                normals.ItemSize = 3;
                normals.Type = "Float32Array";
                normals.Normalized = false;
                var normalsArray = new List<double>();

                //assign normals per face
                for (int i = 0; i < triangleCount; i++)
                {
                    Vector3 normal = entry.Value.mesh.normals[indices[i * 3]];
                    normalsArray.Add(normal.x);
                    normalsArray.Add(normal.y);
                    normalsArray.Add(normal.z);

                    normal = entry.Value.mesh.normals[indices[i * 3 + 1]];
                    normalsArray.Add(normal.x);
                    normalsArray.Add(normal.y);
                    normalsArray.Add(normal.z);

                    normal = entry.Value.mesh.normals[indices[i * 3 + 2]];
                    normalsArray.Add(normal.x);
                    normalsArray.Add(normal.y);
                    normalsArray.Add(normal.z);
                }

                /*
                for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                {
                    if (entry.Value.mesh.normals.Length > 0)
                    {
                        Vector3 normal = entry.Value.mesh.normals[i];
                        normalsArray.Add(normal.x);
                        normalsArray.Add(normal.y);
                        normalsArray.Add(normal.z);
                    }
                }
                */
                normals.Array = normalsArray;
                //Create data for uvs.
                var uvs = new GeometryAttribute();
                var uvs2 = new GeometryAttribute();
                uvs.ItemSize = 2;
                uvs2.ItemSize = 2;
                uvs.Type = "Float32Array";
                uvs2.Type = "Float32Array";
                uvs.Normalized = false;
                uvs2.Normalized = false;
                var uvsArray = new List<double>();
                var uvs2Array = new List<double>();
                Vector2[] meshUv1 = entry.Value.mesh.uv;
                Vector2[] meshUv2 = entry.Value.mesh.uv2;

                if (entry.Value.meshFilter.GetComponent<Renderer>().lightmapIndex >= 0 && meshUv2.Length == 0)
                {
                    meshUv2 = (Vector2[])meshUv1.Clone();
                }

                if (entry.Value.meshFilter.GetComponent<Renderer>().lightmapIndex >= 0)
                {

                    if (entry.Value.mesh.uv2.Length > 0)
                    {
                        for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                        {

                            meshUv2[i].x *= entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.x;
                            meshUv2[i].y *= entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.y;

                            meshUv2[i].x += entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.z;
                            meshUv2[i].y += entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.w;


                        }
                    }
                    else
                    {
                        for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                        {
                            meshUv1[i].x *= entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.x;
                            meshUv1[i].y *= entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.y;

                            meshUv1[i].x += entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.z;
                            meshUv1[i].y += entry.Value.meshFilter.GetComponent<Renderer>().lightmapScaleOffset.w;

                        }
                    }


                }

                if (meshUv1.Length > 0)
                {
                    //Assign uvs per face
                    for (int i = 0; i < triangleCount; i++)
                    {
                        uvsArray.Add(meshUv1[indices[i * 3]].x);
                        uvsArray.Add(meshUv1[indices[i * 3]].y);

                        uvsArray.Add(meshUv1[indices[i * 3 + 1]].x);
                        uvsArray.Add(meshUv1[indices[i * 3 + 1]].y);

                        uvsArray.Add(meshUv1[indices[i * 3 + 2]].x);
                        uvsArray.Add(meshUv1[indices[i * 3 + 2]].y);
                    }

                    /*
                    for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                    {
                        Vector3 uv = meshUv1[i];
                        uvsArray.Add(uv.x);
                        uvsArray.Add(uv.y);
                    }
                    */
                }

                int lightmapIndex = entry.Value.meshFilter.GetComponent<Renderer>().lightmapIndex;
                if (entry.Value.mesh.uv2.Length > 0)
                {
                    //Assign uvs per face
                    for (int i = 0; i < triangleCount; i++)
                    {
                        Vector3 uv = meshUv2[indices[i * 3]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);

                        uv = meshUv2[indices[i * 3 + 1]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);

                        uv = meshUv2[indices[i * 3 + 2]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);
                    }


                    /*
                    for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                    {
                        Vector3 uv = meshUv2[i];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);

                    }
                    */
                }
                else if (entry.Value.meshFilter.GetComponent<Renderer>().lightmapIndex >= 0 &&
                         entry.Value.mesh.uv2.Length == 0)
                {
                    //Assign uvs per face
                    for (int i = 0; i < triangleCount; i++)
                    {
                        Vector3 uv = meshUv1[indices[i * 3]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);

                        uv = meshUv1[indices[i * 3 + 1]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);

                        uv = meshUv1[indices[i * 3 + 2]];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);
                    }

                    /*
                    for (int i = lowestFaceIndex; i <= highestFaceIndex; i++)
                    {
                        Vector3 uv = meshUv1[i];
                        uvs2Array.Add(uv.x);
                        uvs2Array.Add(uv.y);
                    }
                    */
                }

                uvs.Array = uvsArray;
                uvs2.Array = uvs2Array;


                attributes.Position = verts;
                attributes.Normal = normals;
                attributes.Uv = uvs;
                attributes.Uv2 = uvs2;

                data.Attributes = attributes;




                var geometry = new Geometry();

                geometry.Uuid = entry.Key;
                geometry.Type = "BufferGeometry";
                geometry.Name = entry.Value.mesh.name + "_" + entry.Value.subMeshIndex;
                geometry.Data = data;

                geometries.Add(geometry);
            }

            result.Geometries = new(geometries.ToArray());
            //add geometries to result ------ end

            //------------------------------------------------------------------------------------------------Copy materials data
            List<Material> matsInScene = new List<Material>();
            List<Texture> Textures = new List<Texture>();


            foreach (KeyValuePair<string, MeshMaterialLink> meshMaterialLink in meshMaterialLinks)
            {

                UnityEngine.Material mat = meshMaterialLink.Value.material;
                Debug.Log(mat.shader.name);





                //----------------------------------------------------------------------------------------------------------------------------------DIFFERENT MATERIALS
                try
                {

                    var fromThisMaterial = fromLitMaterial(meshMaterialLink, usedMaterials, usedLightmapTextures, usedImages, exportSettings, lightmapData);
                    matsInScene.Add(fromThisMaterial.Item1);
                    Textures.AddRange(fromThisMaterial.Item2);


                }
                catch
                {
                    throw new Exception("Custom shaders are not supported yet");
                }

            }
            
            //------------------------------------------------------------------------------------Add materials and textures together
            result.Textures = new(Textures.ToArray());
            result.Materials = new(matsInScene.ToArray());

            //---------------------------------------------------------------------------------------------CREATE IMAGES AND TEXTURES

            List<Image> Images = new List<Image>();

            foreach (KeyValuePair<string, TextureImageLink> entry in usedImages)
            {
                try
                {
                    var image = new Image();
                    image.Uuid = entry.Key;

                    var originalTexture = (Texture2D)(entry.Value.texture);

                    //check if the texture is a lightmap
                    bool isLightmap = false;

                    foreach (var item in usedLightmapTextures)
                    {
                        if (entry.Value.texture == item.Value.texture)
                            isLightmap = true;
                    }

                    string serialised;
                    if (isLightmap)
                    {
                        Color[] pix = originalTexture.GetPixels(0, 0, originalTexture.width, originalTexture.height);

                        Texture2D destinationTexture = new Texture2D(originalTexture.width, originalTexture.height,
                            TextureFormat.RGB24, false);



                        for (int i = 0; i < pix.Length; i++)
                        {
                            // http://graphicrants.blogspot.jp/2009/04/rgbm-color-encoding.html
                            pix[i].r *= pix[i].a * exportSettings.LightmapContrast;
                            pix[i].g *= pix[i].a * exportSettings.LightmapContrast;
                            pix[i].b *= pix[i].a * exportSettings.LightmapContrast;
                        }

                        destinationTexture.hideFlags = HideFlags.HideAndDontSave;
                        destinationTexture.SetPixels(pix);
                        destinationTexture.Apply();

                        if (exportSettings.imagesSeparateFiles)
                        {
                            imgscontainer.Add(destinationTexture.EncodeToPNG(), entry.Key);
                            serialised = entry.Key + ".jpg";
                        }
                        else
                        {
                            serialised = Convert.ToBase64String(ResizeImage.ResizePngToJpg(destinationTexture.EncodeToPNG(), 1024));
                        }
                        
                    }
                    else
                    {
                        //use asset database to read texture from file
                        var path = AssetDatabase.GetAssetPath(originalTexture);
                        var bytes = File.ReadAllBytes(path);

                        if (exportSettings.imagesSeparateFiles)
                        {
                            imgscontainer.Add(bytes, entry.Key);
                            serialised = entry.Key + ".jpg";
                        }
                        else
                        {
                            serialised = Convert.ToBase64String(ResizeImage.ResizePngToJpg(bytes, 1024));
                        }
                        

                    }

                    if (exportSettings.imagesSeparateFiles)
                    {
                        image.Url = $"{entry.Key}.jpg";  
                    }
                    else
                    {
                        image.Url = "data:image/jpg;base64," + serialised;
                    }
                    
                    Images.Add(image);

                }
                catch
                {
                    Debug.LogError(
                        "Unsupported texture format. Select all your texture files and mark as Uncompressed and enable read/write");
                    Debug.LogError("Texture name: " + entry.Value.texture.name);
                    throw new Exception(
                        "Unsupported texture format. Select all your texture files and mark as Uncompressed and enable read/write");
                }


            }

            result.Images = Images;


            //-----------------------------------------------------------------------------------------CREATE OBJECTS

            List<Object> objects = new List<Object>();

            //Create parent object
            var parentObject = new Object();
            parentObject.Uuid = System.Guid.NewGuid().ToString();
            parentObject.Type = "Scene";
            parentObject.Name = "Scene";
            parentObject.Layers = 1;
            parentObject.Matrix = new List<float> { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
            //Add skybox to images and textures
            if (exportSettings.exportSkybox)
            {
                var skybox = new Image();
                var skyboxTexture = new Texture();
                skybox.Uuid = System.Guid.NewGuid().ToString();
                //                -------------------------------------------------------------------------- Skyboxcubemap
                var ske = AssetDatabase.GetAssetPath(RenderSettings.skybox.GetTexture("_Tex"));
                skybox.Url = "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(ske));
                skyboxTexture.Uuid = System.Guid.NewGuid().ToString();
                skyboxTexture.Name = RenderSettings.skybox.GetTexture("_Tex").name;
                skyboxTexture.Image = skybox.Uuid;
                skyboxTexture.Mapping = 303;
                skyboxTexture.Wrap = new List<long> { 1001, 1001 };
                parentObject.Background = skyboxTexture.Uuid;
                if (exportSettings.skyboxAsEnviromentalLightSource)
                {
                    parentObject.Environment = skyboxTexture.Uuid;
                }

                result.Textures.Add(skyboxTexture);
                result.Images.Add(skybox);
            }


            //Add all objects to the parent object

            foreach (var transform in selection)
            {
                if (exportSettings.exportLights)
                {
                    if (transform.GetComponent<Light>() != null)
                    {
                        var lightToExport = transform.GetComponent<Light>();

                        var lightPositionmatrix = compose(transform.position, transform.rotation, transform.localScale);

                        var light = new Object();
                        light.Uuid = System.Guid.NewGuid().ToString();
                        switch (lightToExport.type)
                        {
                            case LightType.Directional:
                                light.Type = "DirectionalLight";
                                break;
                            case LightType.Point:
                                light.Type = "PointLight";
                                light.Distance = lightToExport.range;
                                break;
                            case LightType.Spot:
                                light.Type = "SpotLight";
                                Debug.LogError("Spot lights are not supported yet");
                                continue;
                                //Convert from degrees to radians/2
                                var ang = lightToExport.spotAngle * Mathf.Deg2Rad * 0.5f;
                                light.Angle = ang;
                                light.Penumbra = ((lightToExport.spotAngle - lightToExport.innerSpotAngle) /
                                                  lightToExport.spotAngle);
                                light.Distance = lightToExport.range * 100;
                                break;
                        }

                        light.Name = lightToExport.name;
                        light.Layers = 1;
                        light.Matrix = lightPositionmatrix;
                        light.Intensity = lightToExport.intensity;
                        light.Color =
                            long.Parse(
                                ColorUtility.ToHtmlStringRGB(new Color(lightToExport.color.r, lightToExport.color.g,
                                    lightToExport.color.b)), System.Globalization.NumberStyles.HexNumber);
                        var shadow = new Shadow();
                        var camera = new Camera();
                        camera.Uuid = System.Guid.NewGuid().ToString();
                        if (lightToExport.shadows != LightShadows.None)
                        {
                            light.CastShadow = true;
                            

                        }

                        switch (lightToExport.type)
                        {
                            case LightType.Directional:
                                camera.Type = "OrthographicCamera";
                                camera.Layers = 1;
                                camera.Zoom = 1;
                                camera.Near = 0.5f;
                                camera.Far = 1000;
                                camera.Bottom = -5;
                                camera.Top = 5;
                                camera.Left = -5;
                                camera.Right = 5;

                                shadow.Bias = exportSettings.directionalLightShadowBias;
                                shadow.NormalBias = exportSettings.directionalLightNormalShadowBias;

                                shadow.Camera = camera;
                                light.Shadow = shadow;

                                break;
                            case LightType.Point:
                                camera.Type = "PerspectiveCamera";
                                camera.Layers = 1;
                                camera.Zoom = 1;
                                camera.Near = 0.5f;
                                camera.Far = 1000;
                                camera.Bottom = -5;
                                camera.Top = 5;
                                camera.Left = -5;
                                camera.Right = 5;
                                camera.Fov = 90;
                                camera.Focus = 10;
                                camera.Aspect = 1;
                                camera.FilmGauge = 35;
                                camera.FilmOffset = 0;

                                shadow.Bias = exportSettings.pointLightShadowBias;
                                shadow.NormalBias = exportSettings.pointLightNormalShadowBias;

                                shadow.Camera = camera;
                                light.Shadow = shadow;
                                
                                
                                break;
                            case LightType.Spot:
                                camera.Type = "PerspectiveCamera";
                                camera.Layers = 1;
                                camera.Zoom = 1;
                                camera.Near = 0.5f;
                                camera.Far = 1000;
                                camera.Fov = 36;
                                camera.Focus = 10;
                                camera.Aspect = 1;
                                camera.FilmGauge = 35;
                                camera.FilmOffset = 0;
                                break;
                        }
                        

                        
                        objects.Add(light);


                        continue;
                    }

                }

                if (transform.GetComponent<MeshFilter>() == null)
                {
                    continue;
                }

                List<float> compose(Vector3 position, Quaternion rotation, Vector3 scale)
                {
                    //Mirror position along the z axis
                    position = new Vector3(position.x, position.y, -position.z);

                    //Change the rotation in such way that y becomes -y also x
                    rotation = Quaternion.Euler(-rotation.eulerAngles.x, -rotation.eulerAngles.y, rotation.eulerAngles.z);


                    //flip scale along x axis
                    scale = new Vector3(scale.x, scale.y, -scale.z);

                    List<float> te = new List<float>();

                    float x = rotation.x, y = rotation.y, z = rotation.z, w = rotation.w;
                    float x2 = x + x, y2 = y + y, z2 = z + z;
                    float xx = x * x2, xy = x * y2, xz = x * z2;
                    float yy = y * y2, yz = y * z2, zz = z * z2;
                    float wx = w * x2, wy = w * y2, wz = w * z2;

                    float sx = scale.x, sy = scale.y, sz = scale.z;

                    te.Add((1 - (yy + zz)) * sx);
                    te.Add((xy + wz) * sx);
                    te.Add((xz - wy) * sx);
                    te.Add(0);

                    te.Add((xy - wz) * sy);
                    te.Add((1 - (xx + zz)) * sy);
                    te.Add((yz + wx) * sy);
                    te.Add(0);

                    te.Add((xz + wy) * sz);
                    te.Add((yz - wx) * sz);
                    te.Add((1 - (xx + yy)) * sz);
                    te.Add(0);

                    te.Add(position.x);
                    te.Add(position.y);
                    te.Add(position.z);
                    te.Add(1);

                    return te;

                    //Many hours went into this, its as hacky as it gets -Adrian
                }

                //var matrix4 = matrixfromTransform(transform);


                var matrix4i = new Matrix4x4();
                matrix4i = Matrix4x4.identity;



                var thisObject = new Object();
                thisObject.Uuid = System.Guid.NewGuid().ToString();
                thisObject.Type = "Mesh";
                thisObject.Name = transform.name;
                thisObject.Layers = 1;
                thisObject.Matrix = compose(transform.position, transform.rotation, transform.localScale);



                /*
                new List<float> { matrix4[0,0], matrix4[0, 1], matrix4[0, 2], matrix4[0, 3],
                                                  matrix4[1, 0], matrix4[1, 1], matrix4[1, 2], matrix4[1, 3],
                                                  matrix4[2, 0], matrix4[2, 1], matrix4[2, 2], matrix4[2, 3],
                                                  matrix4[3, 0], matrix4[3, 1], matrix4[3, 2], matrix4[3, 3] };
                */



                MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    //Check if its casting shadows
                    if (meshFilter.GetComponent<Renderer>().shadowCastingMode == ShadowCastingMode.On)
                    {
                        thisObject.CastShadow = true;
                    }

                    //Check if receiving shadows
                    if (meshFilter.GetComponent<Renderer>().receiveShadows)
                    {
                        thisObject.ReceiveShadow = true;
                    }



                    for (int i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
                    {

                        string geoKey = null;
                        foreach (KeyValuePair<string, Submesh> usedSubmesh in usedSubmeshes)
                        {
                            if (usedSubmesh.Value.meshFilter == meshFilter && usedSubmesh.Value.subMeshIndex == i)
                            {
                                geoKey = usedSubmesh.Key;
                            }
                        }

                        string matKey = null;
                        foreach (KeyValuePair<string, MeshMaterialLink> meshMaterialLink in meshMaterialLinks)
                        {
                            if (meshMaterialLink.Value.meshFilter == meshFilter &&
                                i == meshMaterialLink.Value.subMeshIndex)
                            {
                                matKey = meshMaterialLink.Key;
                            }
                        }

                        if (geoKey != null && matKey != null)
                        {


                            var userData = new UserData();
                            var transformData = new TransformData();
                            transformData.InheritType = 1;
                            transformData.EulerOrder = "ZYX";
                            transformData.Rotation = new List<double>
                            {
                                transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                                transform.rotation.eulerAngles.z
                            };
                            var parentMatrix = new ParentMatrix();
                            parentMatrix.Elements = new List<double>
                            {
                                matrix4i[0, 0], matrix4i[0, 1], matrix4i[0, 2], matrix4i[0, 3],
                                matrix4i[1, 0], matrix4i[1, 1], matrix4i[1, 2], matrix4i[1, 3],
                                matrix4i[2, 0], matrix4i[2, 1], matrix4i[2, 2], matrix4i[2, 3],
                                matrix4i[3, 0], matrix4i[3, 1], matrix4i[3, 2], matrix4i[3, 3]
                            };




                            parentMatrix.IsMatrix4 = true;
                            transformData.ParentMatrix = parentMatrix;
                            var parentMatrixWorld = new ParentMatrix();
                            parentMatrixWorld.Elements = new List<double>
                            {
                                matrix4i[0, 0], matrix4i[0, 1], matrix4i[0, 2], matrix4i[0, 3],
                                matrix4i[1, 0], matrix4i[1, 1], matrix4i[1, 2], matrix4i[1, 3],
                                matrix4i[2, 0], matrix4i[2, 1], matrix4i[2, 2], matrix4i[2, 3],
                                matrix4i[3, 0], matrix4i[3, 1], matrix4i[3, 2], matrix4i[3, 3]
                            };
                            parentMatrixWorld.IsMatrix4 = true;
                            transformData.ParentMatrixWorld = parentMatrixWorld;
                            thisObject.Layers = 1;
                            userData.TransformData = transformData;
                            thisObject.UserData = userData;

                            foreach (KeyValuePair<string, Submesh> usedSubmesh in usedSubmeshes)
                            {
                                if (usedSubmesh.Value.meshFilter == meshFilter && usedSubmesh.Value.subMeshIndex == i)
                                {
                                    thisObject.Geometry = geoKey;



                                    foreach (KeyValuePair<string, MeshMaterialLink> meshMaterialLink in
                                             meshMaterialLinks)
                                    {
                                        if (meshMaterialLink.Value.meshFilter == meshFilter &&
                                            i == meshMaterialLink.Value.subMeshIndex)
                                        {
                                            thisObject.Material = meshMaterialLink.Key;
                                        }
                                    }

                                    break;
                                }
                            }


                        }

                    }
                }

                objects.Add(thisObject);

            }



            parentObject.Children = objects;
            result.Object = parentObject;
            var metaData = new Metadata();
            metaData.Version = 4.5;
            metaData.Type = "Scene";
            metaData.Generator = "Object3D.toJSON";
            result.Metadata = metaData;

            if (exportSettings.exportFog)
            {
                parentObject.Fog = myFog;
            }

            if (exportSettings.exportAmbientLight)
            {
                //Check if ambient light is not skybox
                if (RenderSettings.ambientMode != AmbientMode.Skybox)
                {
                    var lightToExport = RenderSettings.ambientLight;

                    var ambientLight = new Object();
                    ambientLight.Uuid = System.Guid.NewGuid().ToString();
                    ambientLight.Type = "AmbientLight";
                    ambientLight.Name = "AmbientLight";
                    ambientLight.Layers = 1;
                    ambientLight.Color = long.Parse(
                        ColorUtility.ToHtmlStringRGB(new Color(lightToExport.r, lightToExport.g,
                            lightToExport.b)), System.Globalization.NumberStyles.HexNumber);
                    ambientLight.Intensity = (lightToExport.r + lightToExport.g + lightToExport.b) * 2.5f;
                    ambientLight.Matrix = new List<float> { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
                    parentObject.Children.Add(ambientLight);

                }

                
            }


            return result;


        }




        (Material material, List<Texture> textures) fromLitMaterial(KeyValuePair<string, MeshMaterialLink> meshMaterialLink, Dictionary<string, UnityEngine.Material> allMats, 
            Dictionary<string, MaterialTextureLink> allLightmaps, Dictionary<string, TextureImageLink> allImages, ExportSettings exportSettings, LightmapData[] lightmapData)
        {
            UnityEngine.Material mat = meshMaterialLink.Value.material;
            //Implement lit material URP as MeshStandardMaterial        
            var material = new Material();

            material.Uuid = meshMaterialLink.Key;
            material.Name = mat.name;
            material.Type = "MeshStandardMaterial";
            material.Color = long.Parse(ColorUtility.ToHtmlStringRGB(new Color(mat.color.r, mat.color.g, mat.color.b)), System.Globalization.NumberStyles.HexNumber);
            material.Emissive = long.Parse(ColorUtility.ToHtmlStringRGB(new Color(mat.GetColor("_EmissionColor").r, mat.GetColor("_EmissionColor").g, mat.GetColor("_EmissionColor").b)), System.Globalization.NumberStyles.HexNumber);
            material.NormalScale = new List<float> { mat.GetFloat("_BumpScale"), mat.GetFloat("_BumpScale") };
            //material.Metalness = mat.GetFloat("_Metallic"); Let me know if this is needed;
            //material.Roughness = mat.GetFloat("_Glossiness");


            var textures = new List<Texture>();
            foreach (KeyValuePair<string, UnityEngine.Material> usedMaterial in allMats)
            {
                Texture TexFromMapName(string mapName)
                {
                    var texture = new Texture();
                    texture.Uuid = usedMaterial.Key + mapName;
                    texture.Name = mat.GetTexture(mapName).name;
                    TextureImageLink myLink = allImages.Select(x => x.Value).FirstOrDefault(x => x.texture == mat.GetTexture(mapName));
                    texture.Image = allImages.FirstOrDefault(x => x.Value.texture == myLink.texture).Key;
                    return texture;
                }

                if (usedMaterial.Value == mat)
                {


                    if (mat.HasProperty("_BaseMap") && mat.GetTexture("_BaseMap"))
                    {
                        material.Map = usedMaterial.Key + "_BaseMap";
                        textures.Add(TexFromMapName("_BaseMap"));
                        

                        if (mat.enabledKeywords.Any(x=> x.name == "_SURFACE_TYPE_TRANSPARENT"))
                        {
                            Debug.LogWarning("Transparent materials are glitchy. Recommend using opaque");

                            material.AlphaMap = usedMaterial.Key + "_BaseMap";
                            material.Transparent = true;
                        }

                    }
                    if (mat.HasProperty("_EmissionMap") && mat.GetTexture("_EmissionMap"))
                    {
                        material.EmissiveMap = usedMaterial.Key + "_EmissionMap";
                        material.EmissiveIntensity = exportSettings.EmissionIntensity;
                        textures.Add(TexFromMapName("_EmissionMap"));
                    }
                    else
                    {
                        material.EmissiveIntensity = exportSettings.EmissionIntensity;
                    }
                    if (mat.HasProperty("_MetallicGlossMap") && mat.GetTexture("_MetallicGlossMap"))
                    {


                        //Only if metallic workflow
                        if (mat.GetFloat("_WorkflowMode") == 0)
                        {
                            material.MetalnessMap = usedMaterial.Key + "_MetallicGlossMap";
                            //material.RoughnessMap = usedMaterial.Key + "_MetallicGlossMap";
                            textures.Add(TexFromMapName("_MetallicGlossMap"));
                            material.Metalness = 1f;
                            material.Roughness = 0.5f;
                        }
                        else
                        {
                            material.RoughnessMap = usedMaterial.Key + "_MetallicGlossMap";
                            material.Roughness = mat.GetFloat("_Metallic");
                            textures.Add(TexFromMapName("_MetallicGlossMap"));
                            material.Roughness = 0.5f;
                            material.Metalness = 0.3f;

                            Debug.LogError("Specular workflow not fully supported: " + mat.name);
                        }
                        
                    }
                    if (mat.HasProperty("_OcclusionMap") && mat.GetTexture("_OcclusionMap"))
                    {
                        material.AoMap = usedMaterial.Key + "_OcclusionMap";
                        textures.Add(TexFromMapName("_OcclusionMap"));
                    }
                    if (mat.HasProperty("_SpecGlossMap") && mat.GetTexture("_SpecGlossMap"))
                    {
                        material.RoughnessMap = usedMaterial.Key + "_SpecGlossMap";
                        textures.Add(TexFromMapName("_SpecGlossMap"));
                    }
                    if (mat.HasProperty("_BumpMap") && mat.GetTexture("_BumpMap"))
                    {
                        material.NormalMap = usedMaterial.Key + "_BumpMap";
                        material.DisplacementBias = 0.1f;
                        material.DisplacementScale = 1f;
                        textures.Add(TexFromMapName("_BumpMap"));
                    }
                    if (mat.HasProperty("_ParallaxMap") && mat.GetTexture("_ParallaxMap"))
                    {
                        material.DisplacementMap = usedMaterial.Key + "_ParallaxMap";
                        textures.Add(TexFromMapName("_ParallaxMap"));

                    }
                    
                    


                    material.LightMapIntensity = exportSettings.LightmapIntensity;

                }
            }



            // Add lightmaps
            int lightmapIndex = meshMaterialLink.Value.meshFilter.GetComponent<Renderer>().lightmapIndex;

            // Only if the object is lightmapped
            if (lightmapIndex >= 0)
            {
                LightmapData usedLightMap = lightmapData[lightmapIndex];

                foreach (KeyValuePair<string, MaterialTextureLink> usedTexture in allLightmaps)
                {
                    material.LightMap = usedTexture.Key + "_MainTex";
                    var texture = new Texture();
                    texture.Uuid = usedTexture.Key + "_MainTex";
                    texture.Name = material.Name + "_LightMap";
                    TextureImageLink myLink = allImages.Select(x => x.Value).FirstOrDefault(x => x.texture == usedTexture.Value.texture);
                    texture.Image = allImages.FirstOrDefault(x => x.Value.texture == myLink.texture).Key;
                    textures.Add(texture);
                }
            }

            return (material, Textures: textures);
        }



    }
}    
