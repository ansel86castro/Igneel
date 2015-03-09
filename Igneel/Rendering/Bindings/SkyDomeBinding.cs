using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{    
    class SkyDomeBinding:RenderBinding<SkyDome, ISkyDomeMap>
    {       
          
        public override void OnBind(SkyDome obj)
        {
            mapping.LightIntensity = obj.LuminanceIntensity;
            mapping.ViewProj = Engine.Scene.ActiveCamera.ViewProj;
            mapping.DiffuseMap = obj.Texture.ToSampler();            
        }

        public override void OnUnBind(SkyDome value)
        {
           
        }
    }
}
