using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using Igneel.Collections;
using Igneel.Graphics;
using Igneel.Assets;

namespace Igneel.Rendering
{
    public abstract class Render : Resource,  IEnabletable, IRenderInputBinder       
    {      
        private bool _enable = true;
        internal Effect effect;        
               
        protected Render(Effect effect)
        {
            this.effect = effect;
            effect.OnRenderCreated(this);                     
           
        }                      
       
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
            }
        }

        public Effect Effect
        {
            get
            {
                return effect;
            }
            //protected set 
            //{ 
            //    effect = value;                
            //}
        }     
      
        public abstract void Bind<TData>(TData value);

        public abstract void UnBind<TData>(TData value);

        public abstract IRenderBinding<TData> GetBinding<TData>();

        public abstract Render BindWith<TData>(IRenderBinding<TData> binding);

        public abstract Render GeRenderForMaterial(GraphicMaterial material);

        public Render BindWith<TBind, TData>() where TBind : RenderBinding<TData>, new()
        {
            var binding = new TBind();
            binding.Render = this;
            BindWith(binding);
            return this;
        }

        public Render BindWith<TData>(Action<TData> bind, Action<TData> unbind = null)
        {
            BindWith(new CallbackRenderBinding<TData>(this, bind, unbind));
            return this;
        }      
                
        public override string ToString()
        {
            if (effect != null) return effect.ToString();
            return base.ToString();
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (effect != null)
                    effect.Dispose();
            }
        }
       
         
    }

    public abstract class Render<T> : Render
    {       
        protected Render(Effect effect) : base(effect) { }

        public abstract void Draw(T comp);
    }

    public abstract class Render<T, TEffect> : Render<T>
        where TEffect :Effect
    {
        protected Render(TEffect effect):base(effect)
        { }
    
        protected Render(GraphicDevice device)
            : base(Effect.GetEffect<TEffect>(device))
        { 

        }

        public override sealed void Bind<TData>(TData value)
        {
            var binding = Binding<TData, TEffect>.Value;
            if (binding != null)
            {
                binding.Bind(value);
            }
        }

        public override sealed void UnBind<TData>(TData value)
        {
            var bindinGSetter = Binding<TData, TEffect>.Value;
            if (bindinGSetter != null)
            {
                bindinGSetter.UnBind(value);
            }
        }

        public sealed override IRenderBinding<TData> GetBinding<TData>()
        {
            return Binding<TData, TEffect>.Value;
        }

        public override sealed Render BindWith<TData>(IRenderBinding<TData> binding)
        {
            if (binding != null)
                binding.Render = this;

            Binding<TData, TEffect>.Value = binding;
            return this;
        }

        public override Render GeRenderForMaterial(GraphicMaterial material)
        {
            return material.GetRender<T, TEffect>();
        }
    }

   

    //public abstract class NodeRender<TShader, T> : Render<TShader>
    //    where T : IGraphicObject
    //    where TShader : Effect, new()
    //{
    //    SceneNode node;
    //    T component;
    //    PixelClipping clipping;

    //    public NodeRender()
    //    {
                        
    //    }            

    //    public SceneNode CurrentNode { get { return node; } }                 

    //    protected override void InitializeBindinGS()
    //    {
    //        base.InitializeBindinGS();
    //        SetBinding(new SceneNodeWorldBinding());
    //    }

    //    public void DrawNode(SceneNode node, IGraphicObject obj, PixelClipping clipping)
    //    {
    //        DrawNode(node, (T)obj, clipping);
    //    }

    //    public void DrawNode(SceneNode node, T component, PixelClipping clipping)
    //    {
    //        this.component = component;
    //        this.clipping = clipping;
    //        this.node = node;

    //        nbEntries--;           
    //        var renderArGS = component.RenderParam;            
    //        var technique = node.Technique;

    //        if (renderArGS != null)
    //        {
    //            var render = renderArGS.GetInstanceRender<T, TShader>();
    //            if (render != null)
    //            {
    //                render.DrawNode(node, component, clipping);
    //                return;
    //            }               
    //            renderArGS.Bind(this);
    //        }

    //        Bind(clipping);
    //        Bind(node);
    //        BindScene();            

    //        if (technique != null)
    //            technique.Bind(this);
           
    //        Draw(component);

    //        if (technique != null)
    //        {
    //            technique.NbEntries--;                
    //           technique.UnBind(this);
    //        }

    //        if (renderArGS != null)
    //            renderArGS.UnBind(this);

    //        UnBind(node);

    //        if (nbEntries == 0)
    //            UnBindScene();
    //    }      

    //    public abstract void Draw(T component);

    //    protected override void PipelineActivated(Effect arg1, ShadingPipeline arg2)
    //    {
    //        BindGlobals = true;

    //        Bind(clipping);
    //        Bind(node);
    //        var renderArGS = component.RenderParam;

    //        if (renderArGS != null)            
    //            renderArGS.Bind(this);

    //        BindScene();

    //        var technique = node.Technique;
    //        if (technique != null)
    //            technique.Bind(this);
    //    }
    //}
}
