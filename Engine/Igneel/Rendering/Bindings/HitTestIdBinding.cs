using Igneel.SceneManagement;
using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
    public class HitTestIdBinding : RenderBinding<HitTestId>
    {
        IIdMap _mapping;
        
        protected override void OnEffectChanged(Effect effect)
        {         
            base.OnEffectChanged(effect);

            _mapping = effect.Map<IIdMap>();
        }

        public override void OnBind(HitTestId obj)
        {
            if (_mapping != null)
            {               
                var color = Color4.FromRgba(obj.Id);
                int  v = color.ToRgba();
                _mapping.Id = color;
            }
        }

        public override void OnUnBind(HitTestId value)
        {
            if (_mapping != null)
            {
                _mapping.Id = new Color4();
            }
        }
    }
}
