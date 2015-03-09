using Igneel.Assets;
using Igneel.Components;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
     
    public abstract class RenderBinder : IAssetProvider
    {
     
        protected RenderBinder()
        {

        }      

        public abstract Asset CreateAsset();

        public void OnAssetDestroyed(AssetReference assetRef) { }

        public abstract void Bind(Render render);

        public abstract void UnBind(Render render);

        public abstract RenderBinder ChainPrevius<TParam>(TParam param) where TParam : IAssetProvider;

        public abstract RenderBinder ChainNext<TParam>(TParam param) where TParam : IAssetProvider;

        public abstract GraphicObjectRender<TComp> GetInstanceRender<TComp ,TShader>() where TComp : IGraphicObject;

        public abstract void SetInstanceRender<TComp, TShader>(GraphicObjectRender<TComp> render) where TComp : IGraphicObject;

        public static RenderBinder Create<T>(T value) where T : IAssetProvider
        {
            return new RenderBinder<T>(value);
        }
       
    }

    public sealed class RenderBinder<T> : RenderBinder where T : IAssetProvider
    {
        static class InstanceRenderStorage<TComp, TShader> where TComp : IGraphicObject
        {
            public static GraphicObjectRender<TComp> Render;
        }

        [Serializable]
        class RenderParamAsset : Asset
        {
            AssetReference bindRefe;
            Asset previus;
            public RenderParamAsset(RenderBinder<T> param)
                : base(param)
            {
                if (param._value != null)
                    bindRefe = Manager.GetAssetReference(param._value);
                if (param._previus != null)
                    previus = param._previus.CreateAsset();
            }

            public override IAssetProvider CreateProviderInstance()
            {
                T bind = default(T);
                RenderBinder previusParam = null;
                if (bindRefe != null)
                    bind = (T)Manager.GetAssetProvider(bindRefe);
                if (previus != null)
                    previusParam = (RenderBinder)previus.CreateProviderInstance();

                return new RenderBinder<T>(bind, previusParam);
            }
        }        

        RenderBinder _previus;
        T _value;

        public RenderBinder(T value)
        {
            this._value = value;
        }

        public RenderBinder(T value, RenderBinder previus)
            : this(value)
        {
            this._previus = previus;
        }       

        public T Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        public RenderBinder Previus { get { return _previus; } set { _previus = value; } }

        public override void Bind(Render render)
        {
            if (_previus != null)
                _previus.Bind(render);

            render.Bind(_value);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(_value);

            if (_previus != null)
                _previus.UnBind(render);            
        }

        public override RenderBinder ChainPrevius<TParam>(TParam param)
        {
            return new RenderBinder<T>(_value, new RenderBinder<TParam>(param));
        }

        public override RenderBinder ChainNext<TParam>(TParam param)
        {
            return new RenderBinder<TParam>(param, new RenderBinder<T>(_value));
        }

        public override Asset CreateAsset()
        {
            return new RenderParamAsset(this);
        }

        public override GraphicObjectRender<TComp> GetInstanceRender<TComp, TShader>()
        {
            return InstanceRenderStorage<TComp,TShader>.Render;
        }

        public override void SetInstanceRender<TComp, TShader>(GraphicObjectRender<TComp> render)
        {
            InstanceRenderStorage<TComp, TShader>.Render = render;
        }
    }
}
