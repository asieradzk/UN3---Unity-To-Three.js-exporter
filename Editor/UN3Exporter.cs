namespace EMS.UN3
{



    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;
    using System.IO;
    using System;
    using UnityEditor.SceneManagement;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UN3Exporter : EditorWindow
    {
        [MenuItem("Window/Ex Machina Soft/Export selection to three.js")]
        public static void ShowExample()
        {
            UN3Exporter wnd = GetWindow<UN3Exporter>();
            wnd.titleContent = new GUIContent("UN3 Exporter");
        }

        public bool overwrite;

        public string OutputFolder;

        public float LightmapContrast;

        public float LightmapIntensity;

        public float EmissionIntensity;

        public float pointLightShadowBias;
        public float pointLightNormalShadowBias;
        public float directionalLightShadowBias;
        public float directionalLightNormalShadowBias;

        public string targetPath;

        public bool exportSkybox;

        public bool skyboxAsEnviromentalLightSource;

        public bool exportLightmaps;

        public bool exportAmbientLight;

        public bool exportFog;

        public bool exportLights;

        public bool imagesAsSeparateFiles;





        public void CreateGUI()
        {
            overwrite = false;
            exportSkybox = false;
            skyboxAsEnviromentalLightSource = false;
            exportLightmaps = true;
            exportAmbientLight = true;
            exportFog = false;
            exportLights = true;
            imagesAsSeparateFiles = true;


            LightmapContrast = 0.4f;
            LightmapIntensity = 3f;
            EmissionIntensity = 1f;

            pointLightShadowBias = 0.000001f;
            pointLightNormalShadowBias = 4f;

            directionalLightShadowBias = 0.000001f;
            directionalLightNormalShadowBias = 4f;




            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/ExMachinaSoft/UN3/Editor/UN3Exporter.uss");

            //-------------Root and Style Sheet
            VisualElement root = rootVisualElement;

            var un3logoGrapgics = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("un3 logo")[0]), typeof(Texture2D));

            VisualElement un3logo = new UnityEngine.UIElements.Image() { image = un3logoGrapgics };
            un3logo.styleSheets.Add(styleSheet);
            root.Add(un3logo);

            //--------------------------------------------Top logo


            VisualElement OutputFolderName = new Label("Target folder:");
            root.Add(OutputFolderName);
            targetPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            targetPath = targetPath.Replace('\\', '/');
            VisualElement folderPath = new HelpBox(targetPath, (HelpBoxMessageType)MessageType.None) { };
            folderPath.styleSheets.Add(styleSheet);
            root.Add(folderPath);

            //---------------------------------------Folder path box

            VisualElement pickFolderButton = new Button(() =>
            {
                string path = EditorUtility.OpenFolderPanel("Select folder", "", "");
                if (path.Length != 0)
                {
                    targetPath = path;
                //Update text in folderPath
                    folderPath.Q<Label>().text = targetPath;

                }
            })
            { text = "Export Folder" };
            pickFolderButton.styleSheets.Add(styleSheet);
            root.Add(pickFolderButton);
            VisualElement overWriteToggle = new Toggle("Overwrite existing files?");
            overWriteToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("overwrite"));
            root.Add(overWriteToggle);


            //----------------------------------------Pick folder button


            VisualElement togglesLabel = new Label("Toggles:");
            togglesLabel.styleSheets.Add(styleSheet);
            root.Add(togglesLabel);

            VisualElement skyboxToggle = new Toggle("Export Skybox?");
            skyboxToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("exportSkybox"));
            root.Add(skyboxToggle);

            VisualElement skyboxEnvToggle = new Toggle("Skybox is environment?");
            skyboxEnvToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("skyboxAsEnviromentalLightSource"));
            root.Add(skyboxEnvToggle);

            VisualElement lightmapsToggle = new Toggle("Export Lightmaps?");
            lightmapsToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("exportLightmaps"));
            root.Add(lightmapsToggle);

            VisualElement ambientLightToggle = new Toggle("Export Ambient Light?");
            ambientLightToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("exportAmbientLight"));
            root.Add(ambientLightToggle);

            VisualElement fogToggle = new Toggle("Export Fog?");
            fogToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("exportFog"));
            root.Add(fogToggle);

            VisualElement lightsToggle = new Toggle("Export Lights?");
            lightsToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("exportLights"));
            root.Add(lightsToggle);
            
            VisualElement imagesToggle = new Toggle("Images Separate?");
            imagesToggle.Q<Toggle>().BindProperty(new SerializedObject(this).FindProperty("imagesAsSeparateFiles"));
            root.Add(imagesToggle);
            

            VisualElement mapsLabel = new Label("Maps:");
            mapsLabel.styleSheets.Add(styleSheet);
            root.Add(mapsLabel);


            VisualElement contrastRange = new Slider("Lightmap Contrast:", 0f, 10f);
            contrastRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("LightmapContrast"));
            root.Add(contrastRange);

            VisualElement contrastField = new FloatField();
            contrastField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("LightmapContrast"));
            root.Add(contrastField);

            VisualElement intensityRange = new Slider("Lightmap Intensity:", 0f, 10f);
            intensityRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("LightmapIntensity"));
            root.Add(intensityRange);

            VisualElement intensityField = new FloatField();
            intensityField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("LightmapIntensity"));
            root.Add(intensityField);

            VisualElement emissionRange = new Slider("Emission Intensity:", 0f, 10f);
            emissionRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("EmissionIntensity"));
            root.Add(emissionRange);

            VisualElement emissionField = new FloatField();
            emissionField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("EmissionIntensity"));
            root.Add(emissionField);

            VisualElement pointLightShadowBiasRange = new Slider("Point Light Shadow Bias:", 0f, 0.0001f);
            pointLightShadowBiasRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("pointLightShadowBias"));
            root.Add(pointLightShadowBiasRange);

            VisualElement pointLightShadowBiasField = new FloatField();
            pointLightShadowBiasField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("pointLightShadowBias"));
            root.Add(pointLightShadowBiasField);


            VisualElement polintLightShadowNormalBiasRange = new Slider("Point Light Normal Shadow Bias:", 0f, 10f);
            polintLightShadowNormalBiasRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("pointLightNormalShadowBias"));
            root.Add(polintLightShadowNormalBiasRange);

            VisualElement polintLightShadowNormalBiasField = new FloatField();
            polintLightShadowNormalBiasField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("pointLightNormalShadowBias"));
            root.Add(polintLightShadowNormalBiasField);

            VisualElement directionalLightShadowBiasRange = new Slider("Directional Light Shadow Bias:", 0f, 0.0001f);
            directionalLightShadowBiasRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("directionalLightShadowBias"));
            root.Add(directionalLightShadowBiasRange);

            VisualElement directionalLightShadowBiasField = new FloatField();
            directionalLightShadowBiasField.Q<FloatField>().BindProperty(new SerializedObject(this).FindProperty("directionalLightShadowBias"));
            root.Add(directionalLightShadowBiasField);

            VisualElement directionalLightNormalShadowBiasRange = new Slider("Directional Light Normal Shadow Bias:", 0f, 10f);
            directionalLightNormalShadowBiasRange.Q<Slider>().BindProperty(new SerializedObject(this).FindProperty("directionalLightNormalShadowBias"));
            root.Add(directionalLightNormalShadowBiasRange);

            

            //----------------------------------------Toggles

            VisualElement exportButton = new Button(() =>
            {
                Export();
            })
            { text = "Export" };
            exportButton.styleSheets.Add(styleSheet);
            root.Add(exportButton);

            var logoGraphics = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("emslogo")[0]), typeof(Texture2D));

            VisualElement logo = new UnityEngine.UIElements.Image() { image = logoGraphics };
            logo.styleSheets.Add(styleSheet);
            root.Add(logo);







            void Export()
            {
                Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);


                if (selection.Length == 0)
                {
                    EditorUtility.DisplayDialog("Nothing selected.", "Please select one or more objects.", "");
                    return;
                }

                List<Transform> allSelectedObjects = new List<Transform>();
                foreach (Transform selectedObject in selection)
                {
                    allSelectedObjects.AddRange(selectedObject.GetComponentsInChildren<Transform>(true));
                }
                Transform[] allSelectedObjectsArray = allSelectedObjects.ToArray();

               

                //Create the settings object with current settings
                var settings = new ExportSettings() { LightmapContrast = LightmapContrast, 
                    LightmapIntensity = LightmapIntensity, 
                    EmissionIntensity = EmissionIntensity,
                    exportSkybox = exportSkybox,
                    skyboxAsEnviromentalLightSource = skyboxAsEnviromentalLightSource,
                    exportLightmaps = exportLightmaps,
                    exportAmbientLight = exportAmbientLight,
                    exportFog = exportFog,
                    exportLights = exportLights,
                    pointLightShadowBias = pointLightShadowBias,
                    pointLightNormalShadowBias = pointLightNormalShadowBias,
                    directionalLightShadowBias = directionalLightShadowBias,
                    directionalLightNormalShadowBias = directionalLightNormalShadowBias,
                    imagesSeparateFiles = imagesAsSeparateFiles
                };


                //Create a folder in the output directory with the name of the scene
                //If the folder already exists, use "(1/2/3...n)" format and store the path in a variable
                //If the folder doesn't exist, create it and store the path in a variable
                var savePath = targetPath + "/" + EditorSceneManager.GetActiveScene().name;
                if (Directory.Exists(savePath))
                {
                    int i = 1;
                    while (Directory.Exists(savePath + "(" + i + ")"))
                    {
                        i++;
                    }
                    savePath = savePath + "(" + i + ")";
                }

                Directory.CreateDirectory(savePath);

                //Export scene to .json in the savePath
                var fullFilePath = savePath + "/" + EditorSceneManager.GetActiveScene().name + ".json";

                var transfomer = new SceneTransformer();
                Dictionary<byte[], string> texturesToSave = new();
                var serialisedScene = JsonConvert.SerializeObject(transfomer.selectedScene(allSelectedObjectsArray, settings, texturesToSave));

                File.WriteAllText(fullFilePath, serialisedScene); 
                foreach (var texture in texturesToSave) 
                {
                    //its .png
                    File.WriteAllBytes(savePath + "/" + texture.Value + ".jpg", ResizeImage.ResizePngToJpg(texture.Key, 512));
                }


                EditorUtility.DisplayDialog("Completed.", "Exported " + allSelectedObjectsArray.Length + " objects to " + fullFilePath, "");

                EditorUtility.RevealInFinder(fullFilePath);


                



            }


        }
    }
}