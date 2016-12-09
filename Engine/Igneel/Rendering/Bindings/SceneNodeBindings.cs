using Igneel.SceneManagement;

namespace Igneel.Rendering.Bindings
{        
    public class SceneNodeBinding : RenderBinding<Frame>
    {      
        IWorldMap _mapping;         

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            _mapping = effect.Map<IWorldMap>();        
        }
        
        public override void OnBind(Frame obj)
        {            
            if (_mapping != null)
            {
                _mapping.World = obj.GlobalPose;
            }
        }

        public override void OnUnBind(Frame value)
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
    //            Effect.SetValue(worldViewProj, Engine.Scene.ActiveCamera.ViewProj);
    //    }

    //    public override void UnBind(SceneNode value)
    //    {
            
    //    }
    //}
}
