using Igneel.Graphics;
using Igneel.Scenering;
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
            swapChaing = GraphicDeviceFactory.Device.DefaultSwapChain;
        }

        protected override void Render()
        {
            var scene = SceneManager.Scene;
            var graphics = GraphicDeviceFactory.Device;
            var backBuffer = swapChaing.BackBuffer;            

            //swapChaing.MakeCurrent();

            graphics.SetRenderTarget(backBuffer, graphics.BackDepthBuffer);
            graphics.ViewPort = new ViewPort(0, 0, backBuffer.Width, backBuffer.Height);
                               
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer| ClearFlags.Stencil, Engine.BackColor, 1, 0);

            OnRenderBegin();

            if (scene != null)
            {
                RenderManager.ApplyTechnique();
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
            GraphicDeviceFactory.Device.ResizeBackBuffer(size.Width, size.Height);
        }
    }
}
