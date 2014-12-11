using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class ClipSceneTechniqueBinding : RenderBinding<ClipPlaneSceneTechnique, ClipSceneTechniqueBinding.IClipMap>
    {
        public interface IClipMap
        {
            Vector4 clipPlane { get; set; }
        }
        ICameraMap camMap;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            camMap = effect.Map<ICameraMap>();
        }

        public override void OnBind(ClipPlaneSceneTechnique value)
        {         
            var plane = value.Plane;

            var refleMatrix = value.ReflectionMatrix;            
            var effect = Effect;

            mapping.clipPlane = (Vector4)plane;

            camMap.View = refleMatrix * camMap.View;
            camMap.ViewProj = refleMatrix * camMap.ViewProj;            
        }

        public override void OnUnBind(ClipPlaneSceneTechnique value)
        {
            mapping.clipPlane = new Vector4();
        }
    }
    
}
