using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public interface IRenderBinding<T>
    {        
        void Bind(T value);

        void UnBind(T value);

        T BindedValue { get; }

        Render Render { get; set; }
    }

   
     
    public abstract class RenderBinding<T> : IRenderBinding<T>
    {
        protected Effect effect;       
        protected Render render;
        private T bindedValue;

        public RenderBinding()
        {

        }

        public RenderBinding(Render render)          
        {
            this.render = render;
            this.effect = render.Effect;         
            OnEffectChanged(effect);
        }
        
        public Render Render
        {
            get { return render; }
            set
            {
                if (value == null) throw new ArgumentNullException();

                if (render != value)
                {
                    render = value;                    
                    effect = render.Effect;                  
                    OnEffectChanged(render.Effect);
                }
            }
        }

        protected Effect Effect
        {
            get { return effect; }          
        }

        public T BindedValue
        {
            get { return bindedValue; }
        }

        protected virtual void OnEffectChanged(Effect effect)
        {
          
        }

        public void Bind(T value)
        {
            bindedValue = value;
            OnBind(value);
        }        

        public void UnBind(T value) 
        {
            bindedValue = default(T);
            OnUnBind(value);
        }
        

        public abstract void OnBind(T value);
        public abstract void OnUnBind(T value);
    }

    public abstract class RenderBinding<T, TMAP> : RenderBinding<T>
        where TMAP :class
    {
        protected TMAP mapping;
        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<TMAP>();
        }
    }

    public class CallbackRenderBinding<T> : RenderBinding<T>
    {       
        private Action<T> bind;
        private Action<T> unBind;

        public CallbackRenderBinding()
        {

        }

        public CallbackRenderBinding(Action<T> binding)
        {
            this.bind = binding;
        }

        public CallbackRenderBinding(Render render,
            Action<T> bindCallback, 
            Action<T> unbindCallback):base(render)
        {          
            this.bind = bindCallback;
            this.unBind = unbindCallback;
        }

        public Action<T> BindCallback { get { return bind; } set { bind = value; } }

        public Action<T> UnBindCallback { get { return unBind; } set { unBind = value; } }

        public override void OnBind(T value)
        {
            if(bind!=null)
                bind(value);
        }

        public override void OnUnBind(T value)
        {
            if (unBind != null)            
                unBind(value);            
        }

    }

    public struct CallbackBindEventArg<TItem, TMap>
    {
        public TItem Value;
        public TMap Map;

        public CallbackBindEventArg(TItem value, TMap map)
        {          
            this.Value = value;
            this.Map = map;
        }
    }

    public class CallbackRenderBinding<TItem, TMap> : RenderBinding<TItem>
        where TMap:class
    {
        TMap map;

        public CallbackRenderBinding()
        {

        }

        public bool EnableFullMap { get; set; }

        public TMap Map { get { return map; } }

        public event EventHandler<CallbackBindEventArg<TItem, TMap>> BindCallback;

        public event EventHandler<CallbackBindEventArg<TItem, TMap>> UnBindCallback;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            map = effect.Map<TMap>(EnableFullMap);
        }

        public override void OnBind(TItem value)
        {
            BindCallback(this, new CallbackBindEventArg<TItem, TMap>(value, map));
        }

        public override void OnUnBind(TItem value)
        {
            UnBindCallback(this, new CallbackBindEventArg<TItem, TMap>(value, map));
        }
    }

    static class Binding<TBind, TShader>
        where TShader:Effect
    {
        public static IRenderBinding<TBind> Value;
    }  
}
