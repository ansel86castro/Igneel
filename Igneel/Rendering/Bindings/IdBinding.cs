using Igneel.Components;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class IdBinding : RenderBinding<SceneNode>
    {
        public interface IWorldMapping
        {
            Matrix World { get; set; }
            int gId { get; set; }
        }

        IWorldMapping mapping;
        
        protected override void OnEffectChanged(Effect effect)
        {         
            base.OnEffectChanged(effect);

            mapping = effect.Map<IWorldMapping>();
        }

        public override void OnBind(SceneNode obj)
        {
            if (mapping != null)
            {
                mapping.World = obj.GlobalPose;
                mapping.gId = obj.Id;
            }
        }

        public override void OnUnBind(SceneNode value)
        {
          
        }
    }
}
