using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public interface IGraphicContext
    {
        int BackBufferWidth { get; }

        int BackBufferHeight { get; }

        Format BackBufferFormat { get; }

        Format DepthStencilFormat { get; }

        Multisampling Sampling { get; }

        PresentionInterval Presentation { get; }

        bool FullScreen { get; }
        
    }

    public abstract class GraphicContext:IGraphicContext
    {
        private bool fullScreen = false;
        private Format backBufferFormat = Format.B8G8R8X8_UNORM;
        private Format depthStencilFormat = Format.D24_UNORM_S8_UINT;
        private int backBufferWidth = 800;
        private int backBufferHeight = 600;
        private Multisampling sampling = new Multisampling(1, 0);
        private PresentionInterval interval = PresentionInterval.Default;

        public int BackBufferWidth
        {
            get { return backBufferWidth; }
            set { backBufferWidth = value; }
        }

        public int BackBufferHeight
        {
            get { return backBufferHeight; }
            set { backBufferHeight = value; }
        }

        public Format BackBufferFormat
        {
            get { return backBufferFormat; }
            set { backBufferFormat = value; }
        }

        public Format DepthStencilFormat
        {
            get { return depthStencilFormat; }
            set { depthStencilFormat = value; }
        }

        public Multisampling Sampling
        {
            get { return sampling; }
            set { sampling = value; }
        }

        public PresentionInterval Presentation
        {
            get { return interval; }
            set { interval = value; }
        }

        public bool FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }
    }

   
}
