using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace ForgeEditor
{
    public class IdTechnique : DefaultTechnique
    {

        public RenderTexture2D RenderTarget { get; set; }

        public static void Register()
        {
            Service.SetFactory<FrameMeshRender<RenderMeshIdEffect>>(new SingletonDisposableFactoryNew<FrameMeshRender<RenderMeshIdEffect>>());
            RenderManager.RegisterRender<FrameMesh, IdTechnique, FrameMeshRender<RenderMeshIdEffect>>();
        }

        public override void Apply()
        {
            var scene = Engine.Scene;
            var device = Engine.Graphics;
            scene.UpdateVisibleComponents();

            device.SaveRenderTarget();

            RenderTarget.SetTarget(device);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, new Color4(1, 1, 0, 0), 1, 0);

            foreach (var entry in scene.VisibleComponents)
            {
                var item = entry.UpdateRender();
                item.Draw(PixelClipping.None);
            }

            device.RestoreRenderTarget();
        }
    }
}
