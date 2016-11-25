using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Effects;
using Igneel;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Techniques;

namespace ForgeEditor.Components.CoordinateSystems
{
    public class DecalGlypRender : GraphicRender<DecalGlyp, DecalGlypEffect>
    {

        public override void Draw(DecalGlyp comp)
        {
            var effect = Effect;
            var device = effect.Device;
            var decal = comp.ScreenRectangle;
            var renderTarget = comp.RenderTarget;

            device.PrimitiveTopology = IAPrimitive.TriangleList;
            device.Blend = DefaultTechnique.Transparent;
            device.DepthTest = DefaultTechnique.DephtState;

            device.SaveRenderTarget();
            renderTarget.SetTarget(device);

            var oldvp = device.ViewPort;
            device.ViewPort = new ViewPort(0, 0, decal.Width, decal.Height);
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, new Color4(0, 0, 0, 0), 1, 0);

            var scene = Engine.Scene;
            var camera = scene.ActiveCamera;

            effect.Input.View = camera.View;
            effect.Input.Proj = Matrix.PerspectiveFovLh((float)decal.Width / decal.Height, Numerics.ToRadians(60), 1, 100);
            effect.Input.World = Matrix.Identity;

            foreach (var item in comp.Components)
            {
                device.SetVertexBuffer(0, item.VertexBuffer, 0);
                device.SetIndexBuffer(item.IndexBufffer);

                int indexCount = (int)(item.IndexBufffer.SizeInBytes / item.IndexBufffer.Stride);

                effect.OnRender(this);
                foreach (var pass in effect.Passes(0))
                {
                    effect.Apply(pass);
                    device.DrawIndexed(indexCount, 0, 0);
                }
                effect.EndPasses();
            }

            device.RestoreRenderTarget();
            device.ViewPort = oldvp;

            comp.DrawDecal();
        }
    }
}
