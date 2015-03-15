using Igneel.Physics;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class PhysicDisplayTechnique:Technique
    {
        public override void Apply()
        {
            if (SceneManager.Scene != null && SceneManager.Scene.Physics != null)
            {
                var scene = SceneManager.Scene.Physics;

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

    public class IDPhysicRenderTechnique : PhysicDisplayTechnique { }

   
}
