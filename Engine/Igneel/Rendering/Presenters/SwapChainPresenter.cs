using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Presenters;

namespace Igneel.Presenters
{
    public class SwapChainPresenter:GraphicPresenter,IResourceAllocator
    {        
        SwapChain _swapChain;       
        Size _displaySize;
        private ViewPort _viewport;
        DepthStencil _depthBuffer;
        private bool _disposed;
        Format _backBufferFormat;
        Format _depthStencilFormat;
        Multisampling _msaa;        

        public SwapChainPresenter(IGraphicContext context)
        {
            if (_msaa.Count == 0)
                _msaa.Count = 1;

            _displaySize = new Size(context.BackBufferWidth, context.BackBufferHeight);
            this._backBufferFormat = context.BackBufferFormat;
            this._depthStencilFormat = context.DepthStencilFormat;
            this._msaa =  context.Sampling;
           
            _viewport = new ViewPort(0, 0, _displaySize.Width, _displaySize.Height);
            _swapChain = GraphicDeviceFactory.Device.CreateSwapChain(context);

            _depthBuffer = GraphicDeviceFactory.Device.CreateDepthStencil(
                context.BackBufferWidth,
                context.BackBufferHeight,
                _depthStencilFormat,
                _msaa);                        
        }

        public Size DisplaySize
        {
            get { return _displaySize; }
            set
            {
                _displaySize = value;
                Resize(value);
            }
        }       

        protected override void Render()
        {
            var graphics = GraphicDeviceFactory.Device;
            var scene = Engine.Scene;

            //swapChain.MakeCurrent();

            graphics.SetRenderTarget(_swapChain.BackBuffer, _depthBuffer);
            graphics.ViewPort = _viewport;                        
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

            OnRenderBegin();
            if (scene != null)
                RenderManager.ApplyTechnique();

            OnRender();
            
            _swapChain.Present();    
        }

        public void Begin(Color4 backColor)
        {
            var graphics = GraphicDeviceFactory.Device;

            graphics.SetRenderTarget(_swapChain.BackBuffer, _depthBuffer);            

            graphics.ViewPort = _viewport;            
            graphics.Clear(ClearFlags.Target | ClearFlags.ZBuffer, backColor, 1, 0);
        }

        public void End()
        {
            _swapChain.Present();    
        }

        public bool Disposed
        {
            get { return _disposed; }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _swapChain.Dispose();
                _depthBuffer.Dispose();
                _disposed = true;
            }
        }

        public override void Resize(Size size)
        {
            _viewport = new ViewPort(0, 0, size.Width, size.Height);

            _swapChain.ResizeBackBuffer(size.Width, size.Height);

            if (_depthBuffer != null)
                _depthBuffer.Dispose();

            _depthBuffer = GraphicDeviceFactory.Device.CreateDepthStencil(size.Width, size.Height, _depthStencilFormat, _msaa);

            OnSizeChanged(size);
        }
    }
}
