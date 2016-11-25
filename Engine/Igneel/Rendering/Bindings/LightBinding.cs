using Igneel.Components;
using Igneel.SceneComponents;
using Igneel.States;

namespace Igneel.Rendering.Bindings
{
    public class LightBinding : RenderBinding<FrameLight, LightBinding.ILightMap>
    {
        public interface ILightMap : Bindings.ILightMap, IAmbientMap, IHemisphericalAmbientMap
        {
            bool HemisphericalLighting { get; set; }
        }     

        public override void OnBind(FrameLight value)
        {
            if (value == null)
            {
                if (Mapping != null)
                {
                    Mapping.Light = new ShaderLight();
                }
                return;
            }

            var ambient = value.Light.Ambient;
            if (EngineState.Lighting.EnableAmbient)
            {
                Bind(ambient, true);
            }
            else
                Bind(null, false);

            if (Mapping != null)
            {
                Mapping.Light = value.GetShaderLight();               
            }            
         
        }

        private void Bind(GlobalLigth value, bool addSceneAmbient)
        {
            var effect = Effect;
            float ambientIntensity = 0;
            Color3 skyColor = new Color3();
            Color3 groundColor = new Color3();
            Color3 ambient = new Color3();
            var global = Engine.Scene.AmbientLight;
            Vector3 nortPole = global.NorthPole;

            if (value != null)
            {

                ambientIntensity = value.GlobalAmbientIntensity;
                skyColor = value.SkyColor * ambientIntensity;
                groundColor = value.GroundColor * ambientIntensity;
                ambient = value.GlobalAmbient * ambientIntensity;
                nortPole = value.NorthPole;
            }
            if (addSceneAmbient)
            {
                skyColor += global.SkyColor * global.GlobalAmbientIntensity;
                groundColor += global.GroundColor * global.GlobalAmbientIntensity;
                ambient += global.GlobalAmbient * global.GlobalAmbientIntensity;
            }

            Mapping.SkyColor = skyColor;
            Mapping.GroundColor = groundColor;
            Mapping.NorthPole = nortPole;
            Mapping.AmbientColor = ambient;
            Mapping.HemisphericalLighting = EngineState.Lighting.HemisphericalAmbient;

        }

        public override void OnUnBind(FrameLight value)
        {
            if (value == null) return;

            var node = value.Node;
            if (node.Technique != null && node.Technique.Enable)
                node.Technique.UnBind(render);
        }
       
    }

    public class AmbientLightBinding : RenderBinding<GlobalLigth, AmbientLightBinding.IGlobalLightMap>
    {
        public interface IGlobalLightMap : IAmbientMap, IHemisphericalAmbientMap
        {
            bool HemisphericalLighting { get; set; }
        }


        public override void OnBind(GlobalLigth value)
        {
            if (value == null || Mapping ==null) return;

            if (EngineState.Lighting.EnableAmbient)
            {
                var ambientIntensity = value.GlobalAmbientIntensity;
                if (EngineState.Lighting.HemisphericalAmbient)
                {
                    Mapping.SkyColor = value.SkyColor * ambientIntensity;
                    Mapping.GroundColor = value.GroundColor * ambientIntensity;
                    Mapping.NorthPole = value.NorthPole;
                    Mapping.HemisphericalLighting = true;

                }
                else
                {
                    Mapping.AmbientColor = value.GlobalAmbient * ambientIntensity;
                    Mapping.HemisphericalLighting = false;
                }
            }
            else
            {
                Mapping.AmbientColor = new Vector3();
                Mapping.SkyColor = new Vector3();
                Mapping.GroundColor = new Vector3();
            }
        }        

        public override void OnUnBind(GlobalLigth value)
        {
           
        }
    }
}
