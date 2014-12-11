using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public abstract class GraphicPresenter
    {
        public event Action<GraphicPresenter> RenderBegin;      
        public event Action Rendering;
        private bool enable = true;

        public bool Enable { get { return enable; } set { enable = value; } }

        public void RenderFrame()
        {           
            if(enable)
                Render();            
        }

        protected abstract void Render();

        public void OnRender()
        {
            if (Rendering != null)
                Rendering();
        }

        public void OnRenderBegin()
        {
            if (RenderBegin != null)
                RenderBegin(this);
        }

        public abstract void Resize(Size size);
    }

  
}
