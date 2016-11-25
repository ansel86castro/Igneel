using Igneel.SceneComponents;

namespace Igneel.Rendering.Bindings
{     

    public sealed class CameraBinding: RenderBinding<Camera>
    {
        ICameraMap _map;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            _map = effect.Map<ICameraMap>();
        }

        public sealed override void OnBind(Camera obj)
        {
            if (_map != null)
            {
                _map.EyePos = obj.Position;           
                _map.ViewProj = obj.ViewProj;                
            }            
        }

        public override void OnUnBind(Camera value)
        {
           
        }
    }
   
}
