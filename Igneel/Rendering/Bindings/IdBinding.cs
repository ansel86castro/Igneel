using Igneel.Components;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class IdBinding : RenderBinding<SceneNode, IdBinding.IIdBindingMapping>
    {
        public interface IIdBindingMapping : IWorldMap, IIdMap
        {
            Matrix World { get; set; }
            int Id { get; set; }
        }

        IIdBindingMapping mapping;
        
        protected override void OnEffectChanged(Effect effect)
        {         
            base.OnEffectChanged(effect);

            mapping = effect.Map<IIdBindingMapping>();
        }

        public override void OnBind(SceneNode obj)
        {
            if (mapping != null)
            {
                mapping.World = obj.GlobalPose;
                mapping.Id = obj.Id;
            }
        }

        public override void OnUnBind(SceneNode value)
        {
          
        }
    }
}
