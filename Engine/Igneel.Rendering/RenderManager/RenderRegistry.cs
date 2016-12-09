using System;

namespace Igneel.Rendering
{
    class RenderRegistry<TComp> : IRenderRegistry, IEquatable<RenderRegistry<TComp>>
        where TComp : class
    {
        private bool _isLazy;
        private Render _render;

        public Render Render
        {
            get
            {
                return _render;
            }
            set
            {
                _render = value;
            }
        }
      

        public bool IsLazy
        {
            get
            {
                return _isLazy;
            }
            set
            {
                _isLazy = value;
            }
        }

        protected virtual Render CreateRender()
        {
            return null;
        }

        public void PushRender()
        {
            if (_isLazy && (_render == null || _render.Disposed))
                _render = CreateRender();

            RenderManager.PushRender<TComp>(Render);
        }

        public void PopRender()
        {
            RenderManager.PopRender<TComp>();
        }

        public override bool Equals(object obj)
        {
            if (obj is RenderRegistry<TComp>)
                return ((RenderRegistry<TComp>)obj).Render == Render;
            return false;
        }

        public bool Equals(RenderRegistry<TComp> other)
        {
            return other.Render == Render;
        }

        public override int GetHashCode()
        {
            return Render.GetHashCode();
        }

        public override string ToString()
        {
            if (_render != null)
                return _render.ToString();
            return base.ToString();
        }

        Render IRenderRegistry.Render
        {
            get
            {
                return _render;
            }
            set
            {
                _render = (Render<TComp>)value;
            }
        }
    }
}