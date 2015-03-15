using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Graphics
{
    public abstract class Surface:ResourceAllocator, ISurface
    {
        protected int _width;
        protected int _height;
        protected Format _format;
        protected Multisampling _multisampling;       

        public Surface() { }

        public Surface(int width, int height, Format format, Multisampling msaa)
        {
            _width = width;
            _height = height;
            _format = format;
            _multisampling = msaa;
        }
       
        public int Width { get { return _width; } }

        public int Height { get { return _height; } }

        public Format SurfaceFormat { get { return _format; } }

        public Multisampling Sampling { get { return _multisampling; } }
      
    }

    public abstract class RenderTarget : Surface 
    {
        public RenderTarget() { }
        public RenderTarget(int width, int height, Format format, Multisampling msaa)
            :base(width, height, format, msaa)
        {
            
        }       

        public abstract void GetRenderTargetData(Texture2D texture);

        public event Action<RenderTarget> Resized;

        public void OnResized()
        {
            if (Resized != null)
                Resized(this);
        }
      
    }


    public abstract class DepthStencil : Surface 
    {
        public DepthStencil() { }
        
        public DepthStencil(int width, int height, Format format, Multisampling msaa)
            :base(width, height, format, msaa)
        {
            
        }
    }   
}
