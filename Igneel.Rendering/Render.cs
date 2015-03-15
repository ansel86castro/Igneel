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

namespace Igneel.Rendering
{

    public interface IComponentRender<in T>
    {
        void Draw(T component);
    }

    public enum PixelClipping 
    { 
        None = 0,
        Transparent = 1,
        Opaque  = 1 << 1,
    }   

    public abstract class Render : ResourceAllocator,  IEnabletable       
    {      
        private bool enable = true;
        internal Effect effect;        
               
        protected Render(Effect effect)
        {
            this.effect = effect;
            effect.OnRenderCreated(this);                     
           
        }
              

        //public Action<Render> UpdateConstantsCallback { get; set; }

        [Category("RenderTechnique")]
       
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
            }
        }

        [Category("ShaderTechnique")]
       
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
      
        public abstract void Bind<T>(T value);

        public abstract void UnBind<T>(T value);

        public abstract IRenderBinding<T> GetBinding<T>();

        public abstract Render BindWith<T>(IRenderBinding<T> binding);       

        public Render BindWith<T, TBind>() where T : RenderBinding<TBind>, new()
        {
            var binding = new T();
            binding.Render = this;
            BindWith(binding);
            return this;
        }

        public Render BindWith<T>(Action<T> bind, Action<T> unbind = null)
        {
            BindWith(new CallbackRenderBinding<T>(this, bind, unbind));
            return this;
        }      
        
        public override string ToString()
        {
            if (effect != null) return effect.ToString();
            return base.ToString();
        }         

        protected override void OnDispose(bool disposing)
        {
            if (disposing && effect!=null)
            {               
                effect.Dispose();
            }

            base.OnDispose(disposing);
        }        
                
    }

    public abstract class Render<T> : Render
    {       
        protected Render(Effect effect) : base(effect) { }

        public abstract void Draw(T comp);
    }

    public abstract class Render<TComp, TEffect> : Render<TComp>
        where TEffect :Effect
    {
        public Render()
            : base(Effect.GetEffect<TEffect>())
        { 

        }

        public override sealed void Bind<T>(T value)
        {
            var binding = Binding<T, TEffect>.Value;
            if (binding != null)
            {
                binding.Bind(value);
            }
        }

        public override sealed void UnBind<T>(T value)
        {
            var bindingSetter = Binding<T, TEffect>.Value;
            if (bindingSetter != null)
            {
                bindingSetter.UnBind(value);
            }
        }

        public sealed override IRenderBinding<T> GetBinding<T>()
        {
            return Binding<T, TEffect>.Value;
        }

        public override sealed Render BindWith<T>(IRenderBinding<T> binding)
        {
            if (binding != null)
                binding.Render = this;

            Binding<T, TEffect>.Value = binding;
            return this;
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

    //    protected override void InitializeBindings()
    //    {
    //        base.InitializeBindings();
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
    //        var renderArgs = component.RenderParam;            
    //        var technique = node.Technique;

    //        if (renderArgs != null)
    //        {
    //            var render = renderArgs.GetInstanceRender<T, TShader>();
    //            if (render != null)
    //            {
    //                render.DrawNode(node, component, clipping);
    //                return;
    //            }               
    //            renderArgs.Bind(this);
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

    //        if (renderArgs != null)
    //            renderArgs.UnBind(this);

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
    //        var renderArgs = component.RenderParam;

    //        if (renderArgs != null)            
    //            renderArgs.Bind(this);

    //        BindScene();

    //        var technique = node.Technique;
    //        if (technique != null)
    //            technique.Bind(this);
    //    }
    //}

    public static class RenderExtensors
    {      
        public static TRender Binding<TRender>(this TRender render, Action<TRender>bindingAction)
            where TRender : Render
        {
            bindingAction(render);
            return  render;
        }



        //public static TRender Binding<TRender, T>(this TRender render)
        //    where TRender:ShaderRender
        //{
        //    var binding = Activator.CreateInstance(typeof(T), render);
        //    render.SetBinding(
        //}
    }
}
