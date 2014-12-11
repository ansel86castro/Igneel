using Igneel.Components;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{    
    public interface IAmbientMap
    {
        Vector3 sky { get; set; }
        Vector3 ground { get; set; }
        Vector3 northPole { get; set; }
        Vector3 ambient { get; set; }
        bool hemisphere { get; set; }      
    }

    public interface ILightMap : IAmbientMap
    {
        ShaderLight light { get; set; }
    }

    public class LightBinding : RenderBinding<LightInstance, ILightMap>
    {

        public LightBinding()
        {

        }

        public override void OnBind(LightInstance value)
        {
            if (value == null)
            {
                if (mapping != null)
                {
                    mapping.light = new ShaderLight();
                }
                return;
            }

            var ambient = value.Instance.Ambient;
            if (Engine.Lighting.EnableAmbient)
            {
                Bind(ambient, true);
            }
            else
                Bind(null, false);

            if (mapping != null)
            {
                mapping.light = value.GetShaderLight();               
            }            
         
        }

        private void Bind(GlobalLigth value, bool addSceneAmbient)
        {
            var effect = Effect;
            float ambientIntensity = 0;
            Vector3 skyColor = new Vector3();
            Vector3 groundColor = new Vector3();
            Vector3 ambient = new Vector3();
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

            mapping.sky = skyColor;
            mapping.ground = groundColor;
            mapping.northPole = nortPole;
            mapping.ambient = ambient;
            mapping.hemisphere = Engine.Lighting.HemisphericalAmbient;

        }

        public override void OnUnBind(LightInstance value)
        {
            if (value == null) return;

            var node = value.Node;
            if (node.Technique != null && node.Technique.Enable)
                node.Technique.UnBind(render);
        }
       
    }

    public class AmbientLightBinding : RenderBinding<GlobalLigth, IAmbientMap>
    {       

        public override void OnBind(GlobalLigth value)
        {
            if (value == null || mapping ==null) return;

            if (Engine.Lighting.EnableAmbient)
            {
                var ambientIntensity = value.GlobalAmbientIntensity;
                if (Engine.Lighting.HemisphericalAmbient)
                {
                    mapping.sky = value.SkyColor * ambientIntensity;
                    mapping.ground = value.GroundColor * ambientIntensity;
                    mapping.northPole = value.NorthPole;
                    mapping.hemisphere = true;

                }
                else
                {
                    mapping.ambient = value.GlobalAmbient * ambientIntensity;
                    mapping.hemisphere = false;
                }
            }
            else
            {
                mapping.ambient = new Vector3();
                mapping.sky = new Vector3();
                mapping.ground = new Vector3();
            }
        }        

        public override void OnUnBind(GlobalLigth value)
        {
           
        }
    }
}
