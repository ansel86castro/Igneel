using System;
using Igneel.Assets;
using Igneel.Rendering;

namespace Igneel.Rendering
{
     
    public abstract class GraphicMaterial : Resource, IVisualMaterial
    {     
        protected GraphicMaterial()
        {

        }

        public int VisualId { get; set; }

        public Render Render { get; set; }

        /// <summary>
        /// Binds the Material to the Pipeline
        /// </summary>
        /// <param name="render"></param>
        public abstract void Bind(Render render);

        /// <summary>
        /// Unbinds the material from the pipeline
        /// </summary>
        /// <param name="render"></param>
        public abstract void UnBind(Render render);
     

        /// <summary>
        /// Returns the render for this material based on the component and shader type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEffect"></typeparam>
        /// <returns></returns>
        public abstract Render<T> GetRender<T, TEffect>();

        public abstract void SetRender<T, TEffect>(Render<T> render);       
       
    }

    public class GraphicMaterial<TData> : GraphicMaterial
    {       
        static class InstanceRenderStorage<T, TEffect>
        {
            public static Render<T> Render;
        }
        
        TData _value;        

        public GraphicMaterial(TData value)
        {
            this._value = value;
        }

        [AssetMember]
        public TData Value
        {
            get { return _value; }
            set { _value = value; }
        }        

        public override void Bind(Render render)
        {         
            render.Bind(_value);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(_value);            
        }     

        public override Render<T> GetRender<T, TEffect>()
        {
            return InstanceRenderStorage<T,TEffect>.Render;
        }

        public override void SetRender<T, TEffect>(Render<T> render)
        {
            InstanceRenderStorage<T, TEffect>.Render = render;
        }

        //[Serializable]
        //class RenderParamAsset : Asset
        //{
        //    AssetReference _bindRefe;
        //    Asset previus;
        //    public RenderParamAsset(GraphicMaterial<T> param)
        //        : base(param)
        //    {
        //        if (param._value != null)
        //            _bindRefe = Manager.GetAssetReference(param._value);
        //        if (param._previus != null)
        //            previus = param._previus.CreateAsset();
        //    }

        //    public override IAssetProvider CreateProviderInstance()
        //    {
        //        T bind = default(T);
        //        GraphicMaterial previusParam = null;
        //        if (_bindRefe != null)
        //            bind = (T)Manager.GetAssetProvider(_bindRefe);
        //        if (previus != null)
        //            previusParam = (GraphicMaterial)previus.CreateProviderInstance();

        //        return new GraphicMaterial<T>(bind, previusParam);
        //    }
        //}        

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                IResource res = _value as IResource;
                if (res != null)
                {
                    res.Dispose();
                }
            }
        }
    }
}
