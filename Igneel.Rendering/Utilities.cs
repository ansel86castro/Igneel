using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering 
{
    interface IRenderRegistry
    {
        Render Render { get; set; }
        void PushRender();
        void PopRender();
        bool IsLazy { get; set; }

    }

    class RenderRegistry<TComp> : IRenderRegistry, IEquatable<RenderRegistry<TComp>>
        where TComp : class
    {
        private bool isLazy;
        private Render<TComp> render;

        public Render<TComp> Render
        {
            get
            {
                return render;
            }
            set
            {
                render = value;
            }
        }
      

        public bool IsLazy
        {
            get
            {
                return isLazy;
            }
            set
            {
                isLazy = value;
            }
        }

        protected virtual Render<TComp> CreateRender()
        {
            return null;
        }

        public void PushRender()
        {
            if (isLazy && (render == null || render.Disposed))
                render = CreateRender();

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
            if (render != null)
                return render.ToString();
            return base.ToString();
        }

        Render IRenderRegistry.Render
        {
            get
            {
                return render;
            }
            set
            {
                render = (Render<TComp>)value;
            }
        }
    }


    class RenderRegistry<TComp, TRender> : RenderRegistry<TComp>
        where TComp : class
        where TRender : Render<TComp>
    {

        protected override Render<TComp> CreateRender()
        {
            return Service.Require<TRender>();
        }
    }

    public class TechniqueRegistry
    {
        internal Technique technique;
        internal List<IRenderRegistry> renders = new List<IRenderRegistry>();

        public Technique Technique
        {
            get { return technique; }
        }

        public override string ToString()
        {
            if (technique != null)
                return technique.ToString();
            else
                return "Empty";
        }
    }

    static class TRCache<TTech> where TTech : Technique
    {
        public static TechniqueRegistry Registry = new TechniqueRegistry();

        public static List<IRenderRegistry> RenderRegistries
        {
            get { return Registry.renders; }
        }

        public static Technique Technique { get { return Registry.technique; } }
    }

    class RenderStack<T>
    {
        public readonly static Stack<Render<T>> Renders = new Stack<Render<T>>();
    }      
}
