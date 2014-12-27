using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class SwapChainPresenter:GraphicPresenter,IResourceAllocator
    {        
        SwapChain swapChain;       
        Size displaySize;
        private ViewPort viewport;
        DepthStencil depthBuffer;
        private bool disposed;
        Format backBufferFormat;
        Format depthStencilFormat;
        Multisampling msaa;
        IntPtr outputWindow;

        public SwapChainPresenter(int width ,
                                int height,
                                Format backBufferFormat,
                                Format depthStencilFormat, 
                                Multisampling msaa, 
                                IntPtr outputWindow)
        {
            if (msaa.Count == 0)
                msaa.Count = 1;

            displaySize = new Size(width, height);
            this.backBufferFormat = backBufferFormat;
            this.depthStencilFormat = depthStencilFormat;
            this.msaa = msaa;
            this.outputWindow = outputWindow;          

            SwapChainDesc chainDesc = new SwapChainDesc
            {
                BackBufferFormat = backBufferFormat,
                BackBufferHeight = height,
                BackBufferWidth = width,
                Sampling = msaa,
                OutputWindow = outputWindow
            };

            viewport = new ViewPort(0, 0, width, height);
            swapChain = Engine.Graphics.CreateSwapChain(chainDesc);

            depthBuffer = Engine.Graphics.CreateDepthStencil(
                chainDesc.BackBufferWidth,
                chainDesc.BackBufferHeight,
                depthStencilFormat,
                msaa);                        
        }

        public Size DisplaySize
        {
            get { return displaySize; }
            set
            {
                displaySize = value;
                OnSizeChanged(value);
            }
        }

        private void OnSizeChanged(Size size)
        {
            viewport = new ViewPort(0, 0, size.Width, size.Height);

            swapChain.ResizeBackBuffer(size.Width, size.Height);

            if (depthBuffer != null)
                depthBuffer.Dispose();

            depthBuffer = Engine.Graphics.CreateDepthStencil(size.Width, size.Height, depthStencilFormat, msaa);
        }

        protected override void Render()
        {       
            var graphics = Engine.Graphics;
            var scene = Engine.Scene;

            graphics.SetRenderTarget(swapChain.BackBuffer, depthBuffer);

            graphics.ViewPort = viewport;                        
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

            OnRenderBegin();
            if (scene != null)
                Engine.ApplyTechnique();

            OnRender();
            
            swapChain.Present();    
        }

        public void Begin(Color4 backColor)
        {
            var graphics = Engine.Graphics;

            graphics.SetRenderTarget(swapChain.BackBuffer, depthBuffer);            

            graphics.ViewPort = viewport;            
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, backColor, 1, 0);
        }

        public void End()
        {
            swapChain.Present();    
        }

        public bool Disposed
        {
            get { return disposed; }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                swapChain.Dispose();
                depthBuffer.Dispose();
                disposed = true;
            }
        }

        public override void Resize(Size size)
        {
            OnSizeChanged(size);
        }
    }
}
