using System;
using System.Collections.Generic;
using Igneel.Assets;
using Igneel.Rendering;
using Igneel.SceneManagement;

namespace Igneel.SceneComponents
{
    public abstract class GraphicObject<TRenderStack> : FrameComponent, IGraphicObject
        where TRenderStack : class, IGraphicObject
    {
        private bool _visible = true;
        private bool _isTransparent;
        private bool _castShadow;
        private bool _castReflection;
        private bool _castRefraction;
        private GraphicMaterial _graphicMaterial;

        static bool _isRegister;            

        public int RenderId { get; set; }

        [AssetMember]
        public bool IsTransparent { get { return _isTransparent; } set { _isTransparent = value; } }

        [AssetMember]
        public bool Visible { get { return _visible; } set { _visible = value; } }

        [AssetMember]
        public bool CastShadow
        {
            get
            {
                return _castShadow;
            }
            set
            {
                _castShadow = value;
            }
        }

        [AssetMember]
        public bool CastReflection
        {
            get
            {
                return _castReflection;
            }
            set
            {
                _castReflection = value;
            }
        }

        [AssetMember]
        public bool CastRefraction
        {
            get
            {
                return _castRefraction;
            }
            set
            {
                _castRefraction = value;
            }
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public GraphicMaterial Material
        {
            get
            {
                return _graphicMaterial;
            }
            set
            {
                _graphicMaterial = value;
            }
        }

        protected GraphicObject()
        {
            RegisterRenders();
        }

        public static void RegisterRenders()
        {
            if (!_isRegister)
            {
                var registerSrv = Service.Get<IRenderRegistrator<TRenderStack>>();
                if (registerSrv != null)
                {
                    registerSrv.RegisterRenders();

                    Service.Set<IRenderRegistrator<TRenderStack>>(null);

                    RenderManager.UpdateRenderStacks<TRenderStack>();
                }
                _isRegister = true;
            }
        }

        public Render GetRender()
        {
            Render render = null;
            var material = Material;

            if (material != null)
            {
                if (material.Render != null)
                    render = material.Render;
                else
                {
                    render = RenderManager.GetRender<TRenderStack>();
                    var materialRender = render.GeRenderForMaterial(material);
                    if (materialRender != null)
                        render = materialRender;
                }
            }
            else
            {
                render = RenderManager.GetRender<TRenderStack>();
            }
            return render;
        }

        public override int SubmitGraphics(Scene scene, Frame node, ICollection<GraphicSubmit> collection)
        {
            if (!_visible)
                return 0;

            var render = GetRender() as Igneel.Rendering.IGraphicRender;
            if (render != null)
            {
                collection.Add(new GraphicSubmit
                    {
                        Node = node,
                        Scene = scene,
                        Graphic = this,
                        IsTransparent = _isTransparent,
                        Render = render
                    });
                return 1;
            }
            return 0;
        }

        public static GraphicRender<TRenderStack, TEffect> SetRender<TTechnique, TEffect>(
            Action<TRenderStack, GraphicRender<TRenderStack, TEffect>> renderMethod)
            where TEffect : Effect
            where TTechnique : Technique
        {
            var render = new RelayRender<TRenderStack, TEffect>(renderMethod);

            RenderManager.RegisterRender<TRenderStack, TTechnique>(render);
            RenderManager.UpdateRenderStacks<TRenderStack>();
            return render;
        }

        public static void SetRender<TTechnique, TEffect>(Render<TRenderStack, TEffect> render)
            where TEffect : Effect
            where TTechnique : Technique
        {
            RenderManager.RegisterRender<TRenderStack, TTechnique>(render);
            RenderManager.UpdateRenderStacks<TRenderStack>();
        }

        public static void SetNullRender<TTechnique>()
            where TTechnique : Technique
        {
            RenderManager.RegisterRender<TRenderStack, TTechnique>(null);
        }

        #region IRenderable Members

        public void Draw()
        {
            var render = (Render<TRenderStack>)GetRender();
            if (render != null)
                render.Draw(ClrRuntime.Runtime.StaticCast<TRenderStack>(this));
        }

        #endregion
    }

   
    //public class ExclusiveGraphicObject<T> : ExclusiveNodeObject, IGraphicObject
    //    where T : class ,IGraphicObject
    //{
    //    bool _visible = true;
    //    private bool _isTransparent;
    //    private bool _castShadow;
    //    private bool _castReflection;
    //    private bool _castRefraction;
    //    private GraphicMaterial _renderParam;
    //    static bool _isRegister;       

    //    protected ExclusiveGraphicObject()
    //    {
    //        RegisterRenders();          
    //    }

    //    public static void RegisterRenders()
    //    {
    //        if (!_isRegister)
    //        {
    //            var registerSrv = Service.Get<IRenderRegistrator<T>>();
    //            if (registerSrv != null)
    //            {
    //                Engine.Lock();
    //                try
    //                {
    //                    registerSrv.RegisterRenders();
    //                    Service.Set<IRenderRegistrator<T>>(null);

    //                    RenderManager.UpdateRenderStacks<T>();
    //                }
    //                finally
    //                {
    //                    Engine.Unlock();
    //                }
    //            }
    //            _isRegister = true;
    //        }
    //    }

    //    [AssetMember]
    //    public bool IsTransparent { get { return _isTransparent; } set { _isTransparent = value; } }

    //    [AssetMember]
    //    public bool Visible { get { return _visible; } set { _visible = value; } }

    //    [AssetMember]
    //    public bool CastShadow
    //    {
    //        get
    //        {
    //            return _castShadow;
    //        }
    //        set
    //        {
    //            _castShadow = value;
    //        }
    //    }

    //    [AssetMember]
    //    public bool CastReflection
    //    {
    //        get
    //        {
    //            return _castReflection;
    //        }
    //        set
    //        {
    //            _castReflection = value;
    //        }
    //    }

    //    [AssetMember]
    //    public bool CastRefraction
    //    {
    //        get
    //        {
    //            return _castRefraction;
    //        }
    //        set
    //        {
    //            _castRefraction = value;
    //        }
    //    }

    //    [AssetMember(storeAs: StoreType.Reference)]
    //    public GraphicMaterial Material
    //    {
    //        get
    //        {
    //            return _renderParam;
    //        }
    //        set
    //        {
    //            _renderParam = value;
    //        }
    //    }

    //    public Render GetRender()
    //    {
    //        return RenderManager.GetRender<T>();
    //    }

    //    public void Draw(Frame node, Render render, PixelClipping clipping = PixelClipping.None)
    //    {
    //        if (render != null)
    //        {
    //            var graphicRender = ClrRuntime.Runtime.StaticCast<IGraphicObjectRender<T>>(render);
    //            graphicRender.Draw(node, ClrRuntime.Runtime.StaticCast<T>(this), clipping);
    //        }
    //    }

    //    public void Draw()
    //    {
    //        Draw(null, RenderManager.GetRender<T>(), PixelClipping.None);
    //    }

    //    public override int SubmitGraphics(Frame node, ICollection<GraphicSubmit> collection)
    //    {
    //        if (!_visible)
    //            return 0;
    //        var render = RenderManager.GetRender<T>();
    //        if (render != null)
    //        {
    //            collection.Add(new GraphicSubmit
    //            {
    //                Node = node,
    //                Graphic = this,
    //                IsTransparent = _isTransparent,
    //                Render = RenderManager.GetRender<T>()
    //            });
    //            return 1;
    //        }
    //        return 0;
    //    }

    //    public static GraphicRender<TEffect, T> SetRender<TEch, TEffect>(Action<T, GraphicRender<TEffect, T>> renderMethod)
    //        where TEffect : Effect
    //        where TEch : Technique
    //    {
    //        var render = new DelegateRender<TEffect, T>(renderMethod);
    //        Engine.Lock();
    //        try
    //        {
    //            RenderManager.RegisterRender<T, TEch>(render);
    //            RenderManager.UpdateRenderStacks<T>();
    //        }
    //        finally
    //        {
    //            Engine.Unlock();
    //        }
    //        return render;
    //    }

    //    public void SetNullRender<TEch>()
    //        where TEch : Technique
    //    {
    //        RenderManager.RegisterRender<T, TEch>(null);
    //    }
    //}
}
