using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{    
    class SkyDomeBinding:RenderBinding<SkyDome, SkyDomeBinding.ISkyDomeMap>
    {
        public interface ISkyDomeMap
        {
            Matrix ViewProj { get; set; }
            float lightIntensity { get; set; }
        }
          
        public override void OnBind(SkyDome obj)
        {
            mapping.lightIntensity = obj.LuminanceIntensity;
            mapping.ViewProj = Engine.Scene.ActiveCamera.ViewProj;

            Engine.Graphics.PSStage.SetResource(0, obj.Texture);         
        }

        public override void OnUnBind(SkyDome value)
        {
           
        }
    }
}
