using Igneel.Physics;
using Igneel.Rendering;

namespace Igneel.Techniques
{
    public class PhysicDisplayTechnique:Technique
    {
        public override void Apply()
        {
            if (Engine.Scene != null && Engine.Scene.Physics != null)
            {
                var scene = Engine.Scene.Physics;

                var render = Service.Require<ActorRender>();
                if (render != null)
                {
                    foreach (var actor in scene.Actors)
                    {
                        render.Draw(actor);
                    }
                    if (CharacterControllerManager.Instance != null)
                    {
                        foreach (var item in CharacterControllerManager.Instance.Controllers)
                        {
                            render.DrawController(item);
                        }
                    }
                }                
            }
        }        
    }

    public class IdPhysicRenderTechnique : PhysicDisplayTechnique { }

   
}
