using Igneel.Rendering;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public interface IGraphicObjectRender<T> 
    {    
        void Draw(SceneNode node, T obj, PixelClipping clipping);

        //protected virtual void UpdateConstantsMethod(Render obj)
        //{
        //    var scene = SceneManager.Scene;
        //    var camera = scene.ActiveCamera;
        //    var ambient = scene.AmbientLight;
        //    var light = Light.Current;
        //    var technique = RenderManager.ActiveTechnique;

        //    if (camera != null)
        //    {
        //        Bind(camera);
        //        camera.IsGPUSync = true;
        //    }
        //    if (EngineState.Lighting.EnableAmbient)
        //    {
        //        Bind(ambient);
        //        ambient.IsGPUSync = true;
        //    }
        //    if (light != null)
        //    {
        //        Bind(light);
        //        light.IsGPUSync = true;
        //        var node = light.Node;
        //        if (node.Technique != null && node.Technique.Enable)
        //            node.Technique.Bind(this);
        //    }
        //    technique.Bind(this);
        //}
    }

    public abstract class GraphicObjectRender<TEffect, TItem> : Render<TItem,TEffect> ,IGraphicObjectRender<TItem>
        where TEffect : Effect
        where TItem : IGraphicObject
    {

        protected SceneNode Node;
        protected TItem Component;
        protected PixelClipping Clipping;

        protected GraphicObjectRender()
            : base()
        {

        }

        public void Draw(SceneNode node, TItem comp, PixelClipping clipping)
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

            Scene.BindScene(this);

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
   
}
