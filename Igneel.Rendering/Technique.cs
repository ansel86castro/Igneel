using Igneel.Assets;
using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public abstract class Technique :ResourceAllocator,  IEnabletable
    {
        internal bool enable = true;

        public Technique()
        {

        }       

        [AssetMember]
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
            }
        }

        public abstract void Apply();               

        public static void Require<T>() where T : class, IResourceAllocator, new()
        {
            var tech = Service.Get<T>();
            if (tech == null)
            {
                var factory = Service.Get<IFactory<T>>();
                if (factory == null)
                {
                    Service.Set<IFactory<T>>(new SingletonDisposableFactoryNew<T>());
                }
            }
        }

        public static void Dispose<T>() where T : class, IDisposable
        {
            var tech = Service.Get<T>();
            if (tech != null)
                tech.Dispose();
        }

        public virtual void Bind(Render render) { }

        public virtual void UnBind(Render render) { }
       
    }

    public abstract class Technique<T> : Technique
        where T:Effect, new()
    {
        protected Effect effect;
        public Effect Effect { get { return effect; } }

        public Technique()
        {
            effect = Rendering.Effect.GetEffect<T>();
        }
    }


    public abstract class BindableTechnique<T> : Technique
    {
        T binding;

        public BindableTechnique() { }      

        public T Binding { get { return binding; } set { binding = value; } }

        public override void Bind(Render render)
        {
            render.Bind(binding);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(binding);
        }
    }

  
}
