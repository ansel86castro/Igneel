using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{
    public class LightBinding : RenderBinding<LightInstance, LightBinding.ILightMap>
    {
        public interface ILightMap : Igneel.Rendering.Bindings.ILightMap, IAmbientMap, IHemisphericalAmbientMap
        {
            bool HemisphericalLighting { get; set; }
        }     

        public override void OnBind(LightInstance value)
        {
            if (value == null)
            {
                if (mapping != null)
                {
                    mapping.Light = new ShaderLight();
                }
                return;
            }

            var ambient = value.Instance.Ambient;
            if (EngineState.Lighting.EnableAmbient)
            {
                Bind(ambient, true);
            }
            else
                Bind(null, false);

            if (mapping != null)
            {
                mapping.Light = value.GetShaderLight();               
            }            
         
        }

        private void Bind(GlobalLigth value, bool addSceneAmbient)
        {
            var effect = Effect;
            float ambientIntensity = 0;
            Color3 skyColor = new Color3();
            Color3 groundColor = new Color3();
            Color3 ambient = new Color3();
            var global = SceneManager.Scene.AmbientLight;
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

            mapping.SkyColor = skyColor;
            mapping.GroundColor = groundColor;
            mapping.NorthPole = nortPole;
            mapping.AmbientColor = ambient;
            mapping.HemisphericalLighting = EngineState.Lighting.HemisphericalAmbient;

        }

        public override void OnUnBind(LightInstance value)
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
            if (value == null || mapping ==null) return;

            if (EngineState.Lighting.EnableAmbient)
            {
                var ambientIntensity = value.GlobalAmbientIntensity;
                if (EngineState.Lighting.HemisphericalAmbient)
                {
                    mapping.SkyColor = value.SkyColor * ambientIntensity;
                    mapping.GroundColor = value.GroundColor * ambientIntensity;
                    mapping.NorthPole = value.NorthPole;
                    mapping.HemisphericalLighting = true;

                }
                else
                {
                    mapping.AmbientColor = value.GlobalAmbient * ambientIntensity;
                    mapping.HemisphericalLighting = false;
                }
            }
            else
            {
                mapping.AmbientColor = new Vector3();
                mapping.SkyColor = new Vector3();
                mapping.GroundColor = new Vector3();
            }
        }        

        public override void OnUnBind(GlobalLigth value)
        {
           
        }
    }
}
