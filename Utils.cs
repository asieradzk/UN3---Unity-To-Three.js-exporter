using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMS.UN3
{
    public partial class Scene
    {
        [JsonProperty("metadata", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Metadata Metadata { get; set; }

        [JsonProperty("geometries", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Geometry> Geometries { get; set; }

        [JsonProperty("materials", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Material> Materials { get; set; }

        [JsonProperty("textures", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Texture> Textures { get; set; }

        [JsonProperty("images", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> Images { get; set; }

        [JsonProperty("object", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Object Object { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("attributes", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Attributes Attributes { get; set; }

        [JsonProperty("boundingSphere", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public BoundingSphere BoundingSphere { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("position", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public GeometryAttribute Position { get; set; }

        [JsonProperty("normal", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public GeometryAttribute Normal { get; set; }

        [JsonProperty("uv", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public GeometryAttribute Uv { get; set; }

        [JsonProperty("uv2", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public GeometryAttribute Uv2 { get; set; }
    }

    public partial class GeometryAttribute
    {
        [JsonProperty("itemSize", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? ItemSize { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("array", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Array { get; set; }

        [JsonProperty("normalized", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Normalized { get; set; }
    }

    public partial class BoundingSphere
    {
        [JsonProperty("center", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Center { get; set; }

        [JsonProperty("radius", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Radius { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("url", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public partial class Material
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("color", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Color { get; set; }

        [JsonProperty("emissive", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Emissive { get; set; }

        [JsonProperty("emissiveIntensity", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? EmissiveIntensity { get; set; }

        [JsonProperty("specular", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Specular { get; set; }

        [JsonProperty("shininess", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Shininess { get; set; }

        [JsonProperty("metalness", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Metalness { get; set; }

        [JsonProperty("map", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Map { get; set; }
        [JsonProperty("aoMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string AoMap { get; set; }
        [JsonProperty("lightMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string LightMap { get; set; }

        [JsonProperty("metalnessMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string MetalnessMap { get; set; }

        [JsonProperty("roughness", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Roughness { get; set; }


        [JsonProperty("roughnessMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string RoughnessMap { get; set; }

        [JsonProperty("alphaMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string AlphaMap { get; set; }

        [JsonProperty("transparent", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? Transparent { get; set; }


        [JsonProperty("normalMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string NormalMap { get; set; }

        [JsonProperty("normalMapType", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? NormalMapType { get; set; }

        [JsonProperty("normalScale", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<float> NormalScale { get; set; }

        [JsonProperty("displacementMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string DisplacementMap { get; set; }

        [JsonProperty("displacementScale", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float DisplacementScale { get; set; }

        [JsonProperty("displacementBias", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float DisplacementBias { get; set; }

        [JsonProperty("lightMapIntensity", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float LightMapIntensity { get; set; }

        [JsonProperty("emissiveMap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string EmissiveMap { get; set; }

        [JsonProperty("reflectivity", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Reflectivity { get; set; }

        [JsonProperty("refractionRatio", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? RefractionRatio { get; set; }

        [JsonProperty("depthFunc", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? DepthFunc { get; set; }

        [JsonProperty("depthTest", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? DepthTest { get; set; }

        [JsonProperty("depthWrite", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? DepthWrite { get; set; }

        [JsonProperty("colorWrite", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? ColorWrite { get; set; }

        [JsonProperty("stencilWrite", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? StencilWrite { get; set; }

        [JsonProperty("stencilWriteMask", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilWriteMask { get; set; }

        [JsonProperty("stencilFunc", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilFunc { get; set; }

        [JsonProperty("stencilRef", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilRef { get; set; }

        [JsonProperty("stencilFuncMask", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilFuncMask { get; set; }

        [JsonProperty("stencilFail", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilFail { get; set; }

        [JsonProperty("stencilZFail", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilZFail { get; set; }

        [JsonProperty("stencilZPass", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? StencilZPass { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("version", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Version { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("generator", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Generator { get; set; }
    }

    public partial class Object
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("layers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Layers { get; set; }

        [JsonProperty("matrix", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Matrix { get; set; }

        [JsonProperty("background", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Background { get; set; }

        [JsonProperty("environment", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Environment { get; set; }

        [JsonProperty("children", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Object> Children { get; set; }

        [JsonProperty("fog", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Fog Fog { get; set; }


        [JsonProperty("geometry", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Geometry { get; set; }

        [JsonProperty("material", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Material { get; set; }



        [JsonProperty("userData", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public UserData UserData { get; set; }

        [JsonProperty("color", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Color { get; set; }

        [JsonProperty("intensity", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Intensity { get; set; }

        [JsonProperty("distance", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float? Distance { get; set; }

        [JsonProperty("decay", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float? Decay { get; set; }

        [JsonProperty("shadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Shadow Shadow { get; set; }

        [JsonProperty("castShadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? CastShadow { get; set; }

        [JsonProperty("angle", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float? Angle { get; set; }

        [JsonProperty("penumbra", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float? Penumbra { get; set; }


        [JsonProperty("receiveShadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReceiveShadow { get; set; }


    }

    public partial class ObjectChild
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("layers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Layers { get; set; }

        [JsonProperty("matrix", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Matrix { get; set; }

        [JsonProperty("color", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Color { get; set; }

        [JsonProperty("intensity", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Intensity { get; set; }

        [JsonProperty("distance", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Distance { get; set; }

        [JsonProperty("decay", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Decay { get; set; }

        [JsonProperty("shadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Shadow Shadow { get; set; }

        [JsonProperty("children", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<PurpleChild> Children { get; set; }

        [JsonProperty("angle", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Angle { get; set; }

        [JsonProperty("penumbra", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Penumbra { get; set; }

        [JsonProperty("castShadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? CastShadow { get; set; }

        [JsonProperty("receiveShadow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReceiveShadow { get; set; }

        [JsonProperty("userData", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public UserData UserData { get; set; }
    }

    public partial class PurpleChild
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("userData", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public UserData UserData { get; set; }

        [JsonProperty("layers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Layers { get; set; }

        [JsonProperty("matrix", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Matrix { get; set; }

        [JsonProperty("geometry", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Geometry { get; set; }

        [JsonProperty("material", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Material { get; set; }

        [JsonProperty("children", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<FluffyChild> Children { get; set; }
    }

    public partial class FluffyChild
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("userData", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public UserData UserData { get; set; }

        [JsonProperty("layers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Layers { get; set; }

        [JsonProperty("matrix", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Matrix { get; set; }

        [JsonProperty("geometry", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Geometry { get; set; }

        [JsonProperty("material", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Material { get; set; }
    }

    public partial class UserData
    {
        [JsonProperty("transformData", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public TransformData TransformData { get; set; }
    }

    public partial class TransformData
    {
        [JsonProperty("inheritType", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? InheritType { get; set; }

        [JsonProperty("eulerOrder", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string EulerOrder { get; set; }

        [JsonProperty("rotation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Rotation { get; set; }

        [JsonProperty("parentMatrix", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ParentMatrix ParentMatrix { get; set; }

        [JsonProperty("parentMatrixWorld", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ParentMatrix ParentMatrixWorld { get; set; }

        [JsonProperty("scale", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Scale { get; set; }
    }

    public partial class ParentMatrix
    {
        [JsonProperty("isMatrix4", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMatrix4 { get; set; }

        [JsonProperty("elements", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Elements { get; set; }
    }

    public partial class Shadow
    {
        [JsonProperty("camera", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Camera Camera { get; set; }

        [JsonProperty("bias", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float Bias;

        [JsonProperty("nomalBias", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float NormalBias;

        [JsonProperty("radius", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float Radius;
    }

    public partial class Camera
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("layers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Layers { get; set; }

        [JsonProperty("fov", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Fov { get; set; }

        [JsonProperty("zoom", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Zoom { get; set; }

        [JsonProperty("near", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Near { get; set; }

        [JsonProperty("far", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Far { get; set; }

        [JsonProperty("focus", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Focus { get; set; }

        [JsonProperty("aspect", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Aspect { get; set; }

        [JsonProperty("filmGauge", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? FilmGauge { get; set; }

        [JsonProperty("filmOffset", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? FilmOffset { get; set; }

        [JsonProperty("left", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Left { get; set; }

        [JsonProperty("right", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Right { get; set; }

        [JsonProperty("top", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Top { get; set; }

        [JsonProperty("bottom", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Bottom { get; set; }
    }

    public partial class Fog
    {
        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("color", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Color { get; set; }

        [JsonProperty("density", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? Density { get; set; }

        [JsonProperty("near", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? near { get; set; }

        [JsonProperty("far", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double? far { get; set; }

        public Fog()
        {
            if (RenderSettings.fogMode == FogMode.Linear)
            {
                Type = "linear";
                Color = long.Parse(ColorUtility.ToHtmlStringRGB(RenderSettings.fogColor), System.Globalization.NumberStyles.HexNumber);
                near = RenderSettings.fogStartDistance;
                far = RenderSettings.fogEndDistance;
            }
            else
            {
                Type = "FogExp2";
                Color = long.Parse(ColorUtility.ToHtmlStringRGB(RenderSettings.fogColor), System.Globalization.NumberStyles.HexNumber);
                Density = RenderSettings.fogDensity;
            }

        }

    }

    public partial class Texture
    {
        [JsonProperty("uuid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("image", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        [JsonProperty("mapping", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Mapping { get; set; }

        [JsonProperty("repeat", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Repeat { get; set; }

        [JsonProperty("offset", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Offset { get; set; }

        [JsonProperty("center", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Center { get; set; }

        [JsonProperty("rotation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Rotation { get; set; }

        [JsonProperty("wrap", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Wrap { get; set; }

        [JsonProperty("format", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Format { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }

        [JsonProperty("encoding", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Encoding { get; set; }

        [JsonProperty("minFilter", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? MinFilter { get; set; }

        [JsonProperty("magFilter", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? MagFilter { get; set; }

        [JsonProperty("anisotropy", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Anisotropy { get; set; }

        [JsonProperty("flipY", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? FlipY { get; set; }

        [JsonProperty("premultiplyAlpha", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? PremultiplyAlpha { get; set; }

        [JsonProperty("unpackAlignment", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? UnpackAlignment { get; set; }
    }

    public partial class Scene
    {
        public static Scene FromJson(string json) => JsonConvert.DeserializeObject<Scene>(json);
    }

    public static class Serialize
    {
        //public static string ToJson(this Scene self) => JsonConvert.SerializeObject(self));
    }
}
