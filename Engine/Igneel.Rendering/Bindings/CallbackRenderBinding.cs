using System;

namespace Igneel.Rendering
{
    public class CallbackRenderBinding<T> : RenderBinding<T>
    {       
        private Action<T> _bind;
        private Action<T> _unBind;

        public CallbackRenderBinding()
        {

        }

        public CallbackRenderBinding(Action<T> binding)
        {
            this._bind = binding;
        }

        public CallbackRenderBinding(Render render,
            Action<T> bindCallback, 
            Action<T> unbindCallback):base(render)
        {          
            this._bind = bindCallback;
            this._unBind = unbindCallback;
        }

        public Action<T> BindCallback { get { return _bind; } set { _bind = value; } }

        public Action<T> UnBindCallback { get { return _unBind; } set { _unBind = value; } }

        public override void OnBind(T value)
        {
            if(_bind!=null)
                _bind(value);
        }

        public override void OnUnBind(T value)
        {
            if (_unBind != null)            
                _unBind(value);            
        }

    }
}