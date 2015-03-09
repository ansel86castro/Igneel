using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class ClipSceneTechniqueBinding : RenderBinding<ClipPlaneSceneTechnique, Igneel.Rendering.Bindings.ClipSceneTechniqueBinding.IClipSceneTechMap>
    {             
        public interface IClipSceneTechMap:ICameraMap,IClipPlaneMap
        {

        }
      
        public override void OnBind(ClipPlaneSceneTechnique value)
        {         
            var plane = value.Plane;

            var refleMatrix = value.ReflectionMatrix;            
            var effect = Effect;

            mapping.ClipPlane = (Vector4)plane;
            mapping.ViewProj = refleMatrix * Engine.Scene.ActiveCamera.ViewProj;            
        }

        public override void OnUnBind(ClipPlaneSceneTechnique value)
        {
            mapping.ClipPlane = new Vector4();
        }
    }
    
}
