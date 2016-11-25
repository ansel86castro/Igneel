using System;

namespace Igneel.Rendering
{
    public abstract class RenderBinding<T> : IRenderBinding<T>
    {
        protected Effect effect;       
        protected Render render;
        private T _bindedValue;

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
            get { return _bindedValue; }
        }

        protected virtual void OnEffectChanged(Effect effect)
        {
          
        }

        public void Bind(T value)
        {
            _bindedValue = value;
            OnBind(value);
        }        

        public void UnBind(T value) 
        {
            _bindedValue = default(T);
            OnUnBind(value);
        }
        

        public abstract void OnBind(T value);
        public abstract void OnUnBind(T value);
    }
}