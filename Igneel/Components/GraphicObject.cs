using Igneel.Assets;
using Igneel.Design;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Effects;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{      

    [TypeConverter(typeof(DesignTypeConverter))]
    public class GraphicObject<T> : NodeObject, IGraphicObject 
        where T:class ,IGraphicObject
    {                
        bool visible = true;
        private bool isTransparent;
        private bool castShadow;
        private bool castReflection;
        private bool castRefraction;
        private RenderBinder renderParam;
        static bool isRegister;
        T instance;

        public GraphicObject()            
        {
            RegisterRenders();

            instance = (T)(object)this;
        }      

        private static void RegisterRenders()
        {
            if (!isRegister)
            {
                var registerSrv = Service.Get<IRenderRegistrator<T>>();
                if (registerSrv != null)
                {
                    Engine.Lock();
                    try
                    {
                        registerSrv.RegisterRenders();
                        Service.Set<IRenderRegistrator<T>>(null);

                        Engine.UpdateRenderStacks<T>();
                    }
                    finally
                    {
                        Engine.Unlock();
                    }
                }
                isRegister = true;
            }
        }

        [AssetMember]
        public bool IsTransparent { get { return isTransparent; } set { isTransparent = value; } }

        [AssetMember]
        public bool Visible { get { return visible; } set { visible = value; } }

        [AssetMember]
        public bool CastShadow
        {
            get
            {
                return castShadow;
            }
            set
            {
                castShadow = value;
            }
        }

        [AssetMember]
        public bool CastReflection
        {
            get
            {
                return castReflection;
            }
            set
            {
                castReflection = value;
            }
        }

        [AssetMember]
        public bool CastRefraction
        {
            get
            {
                return castRefraction;
            }
            set
            {
                castRefraction = value;
            }
        }

        [AssetMember(storeAs:StoreType.Reference)]
        public RenderBinder RenderParam
        {
            get
            {
                return renderParam;
            }
            set
            {
                renderParam = value;
            }
        }

        public Render GetRender()
        {
            return Engine.GetRender<T>();
        }

        public void Draw(SceneNode node, Render render, PixelClipping clipping = PixelClipping.None)
        {
            if (render != null)
            {
                var graphicRender = (GraphicObjectRender<T>)render;
                graphicRender.Draw(node, instance, clipping);
            }
        }

        public void Draw()
        {
            Draw(null, Engine.GetRender<T>(), PixelClipping.None);
        }

        public override int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection)
        {
            if (!visible)
                return 0;
            var render = Engine.GetRender<T>();
            if (render != null)
            {
                collection.Add(new DrawingEntry
                    {
                        Node = node,
                        GraphicObject = this,
                        IsTransparent = isTransparent,
                        Render = Engine.GetRender<T>()
                    });
                return 1;
            }
            return 0;
        }

        public static GraphicObjectRender<TEffect, T> SetRender<Tech, TEffect>(Action<T, GraphicObjectRender<TEffect, T>> renderMethod)
            where TEffect : Effect
            where Tech : Technique
        {            
            var render = new DelegateRender<TEffect, T>(renderMethod);
            Engine.Lock();
            try
            {
                Engine.RegisterRender<T, Tech>(render);
                Engine.UpdateRenderStacks<T>();
            }
            finally
            {
                Engine.Unlock();
            }
            return render;
        }

        public void SetNullRender<Tech>()
            where Tech : Technique
        {
            Engine.RegisterRender<T, Tech>(null);
        }
    }

    [TypeConverter(typeof(DesignTypeConverter))]
    public class ExclusiveGraphicObject<T> : ExclusiveNodeObject, IGraphicObject
        where T : class ,IGraphicObject
    {
        bool visible = true;
        private bool isTransparent;
        private bool castShadow;
        private bool castReflection;
        private bool castRefraction;
        private RenderBinder renderParam;
        static bool isRegister;
        private T instance;

        protected ExclusiveGraphicObject()
        {
            RegisterRenders();
            instance = (T)(object)this;
        }

        public static void RegisterRenders()
        {
            if (!isRegister)
            {
                var registerSrv = Service.Get<IRenderRegistrator<T>>();
                if (registerSrv != null)
                {
                    Engine.Lock();
                    try
                    {
                        registerSrv.RegisterRenders();
                        Service.Set<IRenderRegistrator<T>>(null);

                        Engine.UpdateRenderStacks<T>();
                    }
                    finally
                    {
                        Engine.Unlock();
                    }
                }
                isRegister = true;
            }
        }

        [AssetMember]
        public bool IsTransparent { get { return isTransparent; } set { isTransparent = value; } }

        [AssetMember]
        public bool Visible { get { return visible; } set { visible = value; } }

        [AssetMember]
        public bool CastShadow
        {
            get
            {
                return castShadow;
            }
            set
            {
                castShadow = value;
            }
        }

        [AssetMember]
        public bool CastReflection
        {
            get
            {
                return castReflection;
            }
            set
            {
                castReflection = value;
            }
        }

        [AssetMember]
        public bool CastRefraction
        {
            get
            {
                return castRefraction;
            }
            set
            {
                castRefraction = value;
            }
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public RenderBinder RenderParam
        {
            get
            {
                return renderParam;
            }
            set
            {
                renderParam = value;
            }
        }

        public Render GetRender()
        {
            return Engine.GetRender<T>();
        }

        public void Draw(SceneNode node, Render render, PixelClipping clipping = PixelClipping.None)
        {
            if (render != null)
            {
                var graphicRender = (GraphicObjectRender<T>)render;
                graphicRender.Draw(node, instance, clipping);
            }
        }

        public void Draw()
        {
            Draw(null, Engine.GetRender<T>() ,PixelClipping.None);
        }

        public override int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection)
        {
            if (!visible)
                return 0;
            var render = Engine.GetRender<T>();
            if (render != null)
            {
                collection.Add(new DrawingEntry
                {
                    Node = node,
                    GraphicObject = this,
                    IsTransparent = isTransparent,
                    Render = Engine.GetRender<T>()
                });
                return 1;
            }
            return 0;
        }

        public static GraphicObjectRender<TEffect, T> SetRender<Tech, TEffect>(Action<T, GraphicObjectRender<TEffect, T>> renderMethod)
            where TEffect : Effect
            where Tech : Technique
        {
            var render = new DelegateRender<TEffect, T>(renderMethod);
            Engine.Lock();
            try
            {
                Engine.RegisterRender<T, Tech>(render);
                Engine.UpdateRenderStacks<T>();
            }
            finally
            {
                Engine.Unlock();
            }
            return render;
        }

        public void SetNullRender<Tech>()
            where Tech : Technique
        {
            Engine.RegisterRender<T, Tech>(null);
        }
    }
}
