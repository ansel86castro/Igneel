using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct RSInitialization
    {
        public RasterizerState RasterizerState { get; set; }
        public ViewPort Viewport { get; set; }
        public Rectangle ScissorRect { get; set; }
    }
    public abstract partial class GraphicDevice
    {        
        private  RasterizerState _rsState;
        private ViewPort _rsVewPort;
        private Rectangle _rsScissorRect;

        public RasterizerState RSState
        {
            get { return _rsState; }
            set 
            {
                if (value == null) throw new ArgumentNullException();
                if (_rsState != value)
                {                    
                    RSSetState(value);

                    _rsState = value;
                }
            }
        }      

        public ViewPort RSViewPort
        {
            get { return _rsVewPort; }
            set
            {                
                RSSetViewPort(value);

                _rsVewPort = value;
            }
        }

        public Rectangle RSScissorRect
        {
            get { return _rsScissorRect; }
            set
            {                
                RSSetScissorRects(value);
                _rsScissorRect = value;
            }
        }

        protected void InitRS()
        {
            RSInitialization ini = GetRSInitialization();
            _rsState = ini.RasterizerState;
            _rsVewPort = ini.Viewport;
            _rsScissorRect = ini.ScissorRect;

            RasterizerStack.Push(_rsState);
        }

        protected abstract RSInitialization GetRSInitialization();

        protected abstract void RSSetState(RasterizerState value);

        protected abstract void RSSetViewPort(ViewPort vp);

        protected abstract void RSSetScissorRects(Rectangle rec);                       

        public abstract RasterizerState CreateRasterizerState(RasterizerDesc desc);
    }

}
