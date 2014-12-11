using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    [Serializable]
    public struct DrawingEntry
    {
        public static readonly DrawingEntry Empty = new DrawingEntry();

        public IGraphicObject GraphicObject;
        public SceneNode Node;
        public Render Render;
        public bool IsTransparent;

        public override string ToString()
        {
            if (Node != null)
                return Node.ToString();
            return GraphicObject.ToString();
        }

        public void Draw(PixelClipping clipping)
        {
            GraphicObject.Draw(Node, Render, clipping);
        }

        public DrawingEntry UpdateRender()
        {
            Render = GraphicObject.GetRender();
            return this;
        }
    }   
}
