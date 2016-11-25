using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
    public class ClipSceneTechniqueBinding : RenderBinding<ClipPlaneSceneTechnique, ClipSceneTechniqueBinding.IClipSceneTechMap>
    {             
        public interface IClipSceneTechMap:ICameraMap,IClipPlaneMap
        {

        }
      
        public override void OnBind(ClipPlaneSceneTechnique value)
        {         
            var plane = value.Plane;

            var refleMatrix = value.ReflectionMatrix;            
            var effect = Effect;

            Mapping.ClipPlane = (Vector4)plane;
            Mapping.ViewProj = refleMatrix * Engine.Scene.ActiveCamera.ViewProj;            
        }

        public override void OnUnBind(ClipPlaneSceneTechnique value)
        {
            Mapping.ClipPlane = new Vector4();
        }
    }
    
}
