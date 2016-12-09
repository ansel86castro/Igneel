using System;

namespace Igneel.Rendering.Presenters
{
    public abstract class GraphicPresenter
    {
        public event Action<GraphicPresenter> RenderBegin;

        public event EventHandler<Size> SizeChanged;

        /// <summary>
        /// Perform User Rendering Logic. It is not necesary to call GraphicDevice.Present
        /// </summary>
        public event Action Rendering;

        private bool _enable = true;

        public bool Enable { get { return _enable; } set { _enable = value; } }

        public void RenderFrame()
        {           
            if(_enable)
                Render();            
        }

        protected abstract void Render();

        protected  void OnRender()
        {
            if (Rendering != null)
                Rendering();
        }

        protected void OnRenderBegin()
        {
            if (RenderBegin != null)
                RenderBegin(this);
        }

        protected virtual void OnSizeChanged(Size size)
        {
            var eventHandler = SizeChanged;
            if (eventHandler != null)
                eventHandler(this, size);
        }

        public abstract void Resize(Size size);
    }

  
}
