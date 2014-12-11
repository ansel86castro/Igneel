using Igneel.Components;
using Igneel.Design;
using Igneel.Graphics;
using Igneel.Rendering.Effects;
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

    public interface ISyncronizableBinding<T> : IRenderBinding<T>
        where T : IShadingInput
    {

    }

    [TypeConverter(typeof(DesignTypeConverter))]  
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

    public class GenericRenderBinding<T> : RenderBinding<T>
    {       
        private Action<T> bind;
        private Action<T> unBind;

        public GenericRenderBinding(Render render,
            Action<T> bindCallback, 
            Action<T> unbindCallback):base(render)
        {          
            this.bind = bindCallback;
            this.unBind = unbindCallback;
        }

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

    static class Binding<TBind, TShader>
        where TShader:Effect
    {
        public static IRenderBinding<TBind> Value;
    }  
}
