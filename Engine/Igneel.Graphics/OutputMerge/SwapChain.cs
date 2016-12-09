using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayMode
    {
        public int Width;
        public int Height;
        int RefreshRate;
        Format Format;
    }
   
    public abstract class SwapChain:ResourceAllocator
    {
        protected RenderTarget _backBuffers;
        protected IGraphicContext context;

        public SwapChain(IGraphicContext context)
        {
            this.context = context;
        }

        public RenderTarget BackBuffer { get { return _backBuffers; } }

        public IGraphicContext Context { get { return context; } }

        public GraphicDevice Device { get; set; }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _backBuffers.Dispose();
                if (Device != null)
                    Device.RemoveSwapChain(this);
            }
        }

        //public abstract void MakeCurrent();

        public abstract void Present();

        public abstract void ResizeBackBuffer(int width, int height);        
    }
}
