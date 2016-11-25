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


    public abstract class RenderBinding<T, TMap> : RenderBinding<T>
        where TMap :class
    {
        protected TMap Mapping;
        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            Mapping = effect.Map<TMap>();
        }
    }

    public class CallbackRenderBinding<TItem, TMap> : RenderBinding<TItem>
        where TMap:class
    {
        TMap _map;

        public CallbackRenderBinding()
        {

        }

        public bool EnableFullMap { get; set; }

        public TMap Map { get { return _map; } }

        public event EventHandler<CallbackBindEventArg<TItem, TMap>> BindCallback;

        public event EventHandler<CallbackBindEventArg<TItem, TMap>> UnBindCallback;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            _map = effect.Map<TMap>(EnableFullMap);
        }

        public override void OnBind(TItem value)
        {
            BindCallback(this, new CallbackBindEventArg<TItem, TMap>(value, _map));
        }

        public override void OnUnBind(TItem value)
        {
            UnBindCallback(this, new CallbackBindEventArg<TItem, TMap>(value, _map));
        }
    }
}
