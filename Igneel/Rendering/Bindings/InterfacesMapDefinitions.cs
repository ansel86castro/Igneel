using Igneel.Components;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public interface IWorldMap
    {
        Matrix World { get; set; }
    }

    public interface IIdMap
    {
        int Id { get; set; }
    }

    public interface IViewMap
    {
        Matrix View { get; set; }        
    }

    public interface IProjectionMap
    {
        Matrix Proj { get; set; }
    }

    public interface IViewProjectionMap
    {
        Matrix ViewProj { get; set; }
    }
   

    public interface ICameraMap: IViewProjectionMap
    {
        Vector3 EyePos { get; set; }
    }

    public interface IAmbientMap
    {
        Vector3 AmbientColor { get; set; }
    }

    public interface IHemisphericalAmbientMap
    {
        Vector3 SkyColor { get; set; }
        Vector3 GroundColor { get; set; }
        Vector3 NorthPole { get; set; }        
    }   

    public interface ILightMap
    {
        ShaderLight Light { get; set; }
    }

    public interface PixelClippingMap
    {
        bool NoRenderTransparency { get; set; }
        bool NoRenderOpaque { get; set; }
    }

    public interface IMeshMaterialMap
    {
        bool USE_DIFFUSE_MAP { get; set; }

        bool USE_SPECULAR_MAP { get; set; }

        LayerSurface Surface { get; set; }

        Sampler<Texture2D> DiffuseMap { get; set; }

        Sampler<Texture2D> SpecularMap { get; set; }

        Sampler<Texture2D> NormalMap { get; set; }

    }

    public interface ISkinMap
    {
        SArray<Matrix> WorldArray { get; set; }
    }

    public interface ISkyDomeMap:IViewProjectionMap
    {
        Sampler<Texture2D> DiffuseMap { get; set; }

        float LightIntensity { get; set; }
    }

    public interface IEnvironmentMap
    {
        Sampler<Texture2D> EnvironmentMap { get; set; }

        bool USE_ENVIROMENT_MAP { get; set; }
    }

    public interface IPlanarReflecionMap
    {
        bool USE_REFLECTION_MAP { get; set; }

        Sampler<Texture2D> ReflectionMap { get; set; }

        bool USE_REFRACTION_MAP { get; set; }

        Sampler<Texture2D> RefractionMap { get; set; }
    }

    public interface IClipPlaneMap
    {
        Vector4 ClipPlane { get; set; }
    }
}
