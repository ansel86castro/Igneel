using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class SingleViewPresenter : GraphicPresenter
    {
        SwapChain swapChaing;     

        public SingleViewPresenter()
        {
            swapChaing = Engine.Graphics.DefaultSwapChain;
        }

        protected override void Render()
        {
            var scene = Engine.Scene;
            var graphics = Engine.Graphics;
            var backBuffer = graphics.OMBackBuffer;

            graphics.OMSetRenderTarget(backBuffer, graphics.OMBackDepthStencil);
            graphics.RSViewPort = new ViewPort(0, 0, backBuffer.Width, backBuffer.Height);
                               
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer| ClearFlags.Stencil, Engine.BackColor, 1, 0);

            OnRenderBegin();

            if (scene != null)
            {
                Engine.ApplyTechnique();
                if (scene.Physics != null && scene.Physics.Visible)
                {
                    PhysicDisplayTechnique tech = Service.Require<PhysicDisplayTechnique>();
                    tech.Apply();
                }
            }
            
            OnRender();

            swapChaing.Present();
        }

        public override void Resize(Size size)
        {
            Engine.Graphics.ResizeBackBuffer(size.Width, size.Height);
        }
    }
}
