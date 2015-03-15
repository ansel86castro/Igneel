using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{        
    public class SceneNodeBinding : RenderBinding<SceneNode>
    {      
        IWorldMap mapping;         

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<IWorldMap>();        
        }
        
        public override void OnBind(SceneNode obj)
        {            
            if (mapping != null)
            {
                mapping.World = obj.GlobalPose;
            }
        }

        public override void OnUnBind(SceneNode value)
        {
            //if (mapping != null)
            //{
            //    mapping.World = Matrix.Identity;
            //}
        }
    }

    //public class SkelletalNodeWorldBinding : RenderBinding<SceneNode>
    //{
    //    Matrix worldMatrix = Matrix.Identity;
    //    protected EffectHandle world;
    //    protected EffectHandle worldViewProj;

    //    public SkelletalNodeWorldBinding(ShaderEffect effect)
    //        : base(effect)
    //    {
    //        world = effect.TryGetGlobalSematic(ShaderSemantics.World);
    //        worldViewProj = effect.TryGetGlobalSematic(ShaderSemantics.WORLDVIEWPROJ);
    //    }

    //    public override void Bind(SceneNode obj)
    //    {
    //        if (world != null)
    //            Effect.SetValue(world, worldMatrix);
    //        else if (worldViewProj != null)
    //            Effect.SetValue(worldViewProj, SceneManager.Scene.ActiveCamera.ViewProj);
    //    }

    //    public override void UnBind(SceneNode value)
    //    {
            
    //    }
    //}
}
