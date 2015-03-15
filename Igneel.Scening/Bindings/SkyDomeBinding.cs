using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{    
    class SkyDomeBinding:RenderBinding<SkyDome, ISkyDomeMap>
    {       
          
        public override void OnBind(SkyDome obj)
        {
            mapping.LightIntensity = obj.LuminanceIntensity;
            mapping.ViewProj = SceneManager.Scene.ActiveCamera.ViewProj;
            mapping.DiffuseMap = obj.Texture.ToSampler();            
        }

        public override void OnUnBind(SkyDome value)
        {
           
        }
    }
}
