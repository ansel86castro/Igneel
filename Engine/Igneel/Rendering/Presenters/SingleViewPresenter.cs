using Igneel.Techniques;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;

namespace Igneel.Presenters
{
    public class SingleViewPresenter : GraphicPresenter
    {
        SwapChain _swapChaing;     

        public SingleViewPresenter()
        {
            _swapChaing = GraphicDeviceFactory.Device.DefaultSwapChain;
        }

        protected override void Render()
        {
            var scene = Engine.Scene;
            var graphics = GraphicDeviceFactory.Device;
            var backBuffer = _swapChaing.BackBuffer;            

            //swapChaing.MakeCurrent();

            graphics.SetRenderTarget(backBuffer, graphics.BackDepthBuffer);
            graphics.ViewPort = new ViewPort(0, 0, backBuffer.Width, backBuffer.Height);
                               
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer| ClearFlags.Stencil, Engine.BackColor, 1, 0);

            OnRenderBegin();

            if (scene != null)
            {
                scene.Draw();                
            }
            
            OnRender();

            _swapChaing.Present();
        }

        public override void Resize(Size size)
        {
            GraphicDeviceFactory.Device.ResizeBackBuffer(size.Width, size.Height);
            OnSizeChanged(size);
        }
    }
}
