using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Rendering
{
    public abstract class GraphicRender<T, TEffect> : Render<T, TEffect> ,IGraphicRender
        where T:class, IGraphicObject
        where TEffect :Effect
    {

        public GraphicSubmit Current;

        public PixelClipping Clipping;

        public GraphicRender(GraphicDevice device)
            : base(device) { }

        public GraphicRender() : base(GraphicDeviceFactory.Device) 
        { 

        }

        public void Draw(GraphicSubmit comp, PixelClipping clipping)
        {
            Current = comp;
            Clipping = clipping;

            var graphic = comp.Graphic;
            var node = comp.Node;
            var scene = comp.Scene;

            var material = graphic.Material;
            if (material != null)
            {
                material.Bind(this);
            }

            Bind(clipping);

            FrameTechnique technique = null;
            if (node != null)
            {
                Bind(node);
                technique = node.Technique;
                if (technique != null)
                    technique.Bind(this);
            }

            scene.BindScene(this);

            Draw((T)(graphic));

            if (technique != null)
            {
                technique.NbEntries--;
                technique.UnBind(this);
            }
        }
    }
}
