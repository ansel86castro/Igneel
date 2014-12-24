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

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SwapChainDesc
    {
        public int BackBufferWidth;
        public int BackBufferHeight;
        public Format BackBufferFormat;        
        public Multisampling Sampling;
        public IntPtr OutputWindow;
        public PresentionInterval Presentation;

    }
    public abstract class SwapChain:ResourceAllocator
    {
        protected RenderTarget _backBuffers;        
        protected SwapChainDesc _desc;

        public RenderTarget BackBuffer { get { return _backBuffers; } }

        public SwapChainDesc Desc { get { return _desc; } protected set { _desc = value; } }       

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _backBuffers.Dispose();
                if (Device != null)
                    Device.RemoveSwapChain(this);
            }
        }

        public abstract void Present();

        public abstract void ResizeBackBuffer(int width, int height);

        internal GraphicDevice Device { get; set; }
    }
}
