using Igneel.SceneComponents;
using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{    
    class SkyDomeBinding:RenderBinding<SkyDome, ISkyDomeMap>
    {       
          
        public override void OnBind(SkyDome obj)
        {
            Mapping.LightIntensity = obj.LuminanceIntensity;
            Mapping.ViewProj = Engine.Scene.ActiveCamera.ViewProj;
            Mapping.DiffuseMap = obj.Texture.ToSampler();            
        }

        public override void OnUnBind(SkyDome value)
        {
           
        }
    }
}
