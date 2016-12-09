using Igneel.Graphics;
using System;
using Igneel.SceneComponents;

namespace Igneel.Rendering
{
    public class RelayRender<TItem, TEffect> : GraphicRender<TItem, TEffect>
        where TEffect : Effect
        where TItem :class, IGraphicObject
    {
        Action<TItem, GraphicRender<TItem, TEffect>> _renderCallback;

        public RelayRender(GraphicDevice device) : base(device) { }

        public RelayRender(Action<TItem, GraphicRender<TItem, TEffect>> renderCallback)
        {
            this._renderCallback = renderCallback;
        }

        public override void Draw(TItem component)
        {
            _renderCallback(component, this);
        }
       
    }
}