using Igneel.Components;
using Igneel.Rendering;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Importers.BVH
{
    public class BVHSkelletonRender:Render<BvhNode>
    {
        Mesh box;        
        public BVHSkelletonRender()
            :base(Rendering.Effect.GetEffect<RenderMeshIdEffect>())
        {
            box = Mesh.CreateBox(2, 2, 2);            
        }
        public override void Draw(BvhNode comp)
        {
            var scene = Engine.Scene;
            effect.Constants.World = comp.GlobalTransform;
            effect.Constants.ViewProj = scene.ActiveCamera.ViewProj;
            effect.Constants.gId = new Vector4(0, 0, 0, 1);

            box.Draw(Engine.Graphics, effect);

            foreach (var item in comp.Nodes)
            {

                Draw(item);
            }
        }
       

        public override void Bind<T>(T value)
        {
            throw new NotImplementedException();
        }

        public override void UnBind<T>(T value)
        {
            throw new NotImplementedException();
        }

        public override IRenderBinding<T> GetBinding<T>()
        {
            throw new NotImplementedException();
        }

        public override Render BindWith<T>(IRenderBinding<T> binding)
        {
            throw new NotImplementedException();
        }
    }
}
