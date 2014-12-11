using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using Igneel.Assets;
using Igneel.Collections;
using Igneel.Graphics;
using Igneel.Rendering.Effects;
using Igneel.Components;


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
               
        internal Render(Effect effect)
        {
            this.effect = effect;
            effect.OnRenderCreated(this);

            UpdateConstantsCallback = UpdateConstantsMethod;
            effect.EffectHasChanged += effect_EffectHasChanged;
        }
      

        void effect_EffectHasChanged(object sender, EffectChangedEventArg e)
        {
            var callback = UpdateConstantsCallback;
            if (callback != null)
            {
                callback(this);
            }
        }

        public Action<Render> UpdateConstantsCallback { get; set; }

        [Category("RenderTechnique")]
        [Browsable(true)]
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
        [Browsable(false)]
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
            BindWith(new GenericRenderBinding<T>(this, bind, unbind));
            return this;
        }      
        
        public override string ToString()
        {
            if (effect != null) return effect.ToString();
            return base.ToString();
        }

        protected virtual void UpdateConstantsMethod(Render obj)
        {
            var scene = Engine.Scene;
            var camera = scene.ActiveCamera;
            var ambient = scene.AmbientLight;
            var light = Light.Current;
            var technique = Engine.ActiveTechnique;

            if (camera != null)
            {
                Bind(camera);
                camera.IsGPUSync = true;
            }
            if (Engine.Lighting.EnableAmbient)
            {
                Bind(ambient);
                ambient.IsGPUSync = true;
            }
            if (light != null)
            {
                Bind(light);
                light.IsGPUSync = true;
                var node = light.Node;
                if (node.Technique != null && node.Technique.Enable)
                    node.Technique.Bind(this);
            }
            technique.Bind(this);            
        }

        public void BindScene()
        {
            var scene = Engine.Scene;
            var camera = scene.ActiveCamera;
            var ambient = scene.AmbientLight;
            var light = Light.Current;
            var technique = Engine.ActiveTechnique;

            if (camera!=null && !camera.IsGPUSync)
            {
                Bind(camera);
                camera.IsGPUSync = true;
            }
            if (Engine.Lighting.EnableAmbient && !ambient.IsGPUSync)
            {
                Bind(ambient);
                ambient.IsGPUSync = true;
            }
            if (light != null)
            {
                if (!light.IsGPUSync)
                {
                    Bind(light);
                    light.IsGPUSync = true;
                }
                var node = light.Node;
                if (node.Technique != null && node.Technique.Enable)
                    node.Technique.Bind(this);
            }
            technique.Bind(this);            
        }

        public void UnBindScene()
        {
            var scene = Engine.Scene;
            if (scene.ActiveCamera != null)
                UnBind(scene.ActiveCamera);

            UnBind(Light.Current);

            var technique = Engine.ActiveTechnique;
            technique.UnBind(this);
        }       

        protected override void OnDispose(bool disposing)
        {
            if (disposing && effect!=null)
            {
                effect.EffectHasChanged -= effect_EffectHasChanged;
                effect.Dispose();
            }

            base.OnDispose(disposing);
        }        
                
    }

    public abstract class Render<T> : Render
    {       

        internal Render(Effect effect) : base(effect) { }

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

    public abstract class GraphicObjectRender<T> : Render<T>        
    {       
        public GraphicObjectRender(Effect effect) : base(effect) { }

        public abstract void Draw(SceneNode node, T obj, PixelClipping clipping);
    }

    public abstract class GraphicObjectRender<TEffect, TItem> : GraphicObjectRender<TItem>
        where TEffect :Effect
        where TItem : IGraphicObject
    {

        protected SceneNode Node;
        protected TItem Component;
        protected PixelClipping Clipping;

        protected GraphicObjectRender()
            : base(Service.Require<TEffect>())
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

        public override  void Draw(SceneNode node, TItem comp, PixelClipping clipping)
        {
            this.Component = comp;
            this.Clipping = clipping;
            this.Node = node;
        
            var renderArgs = comp.RenderParam;          
            if (renderArgs != null)
            {
                var render = renderArgs.GetInstanceRender<TItem, TEffect>();
                if (render != null)
                {
                    render.Draw(node, comp, clipping);
                    return;
                }
                renderArgs.Bind(this);
            }

            Bind(clipping);                  

            NodeTechnique technique = null;
            if (node != null)
            {                
                Bind(node);
                technique = node.Technique;

                if (technique != null)
                    technique.Bind(this);
            }

            BindScene();     

            Draw(comp);
            
            if (technique != null)
            {
                technique.NbEntries--;
                technique.UnBind(this);
            }

            //if (renderArgs != null)
            //    renderArgs.UnBind(this);

            //if(node!=null)
            //    UnBind(node);
           
              //UnBindScene();
        }      
    }

    public class DelegateRender<TEffect, TItem> : GraphicObjectRender<TEffect, TItem>
        where TEffect : Effect
        where TItem : IGraphicObject
    {
        Action<TItem, GraphicObjectRender<TEffect, TItem>> renderCallback;
        public DelegateRender(Action<TItem, GraphicObjectRender<TEffect, TItem>> renderCallback)
        {
            this.renderCallback = renderCallback;
        }

        public override void Draw(TItem component)
        {
            renderCallback(component, this);
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
