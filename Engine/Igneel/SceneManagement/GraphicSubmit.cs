using System;
using Igneel.Rendering;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
    [Serializable]
    public struct GraphicSubmit
    {
        public static readonly GraphicSubmit Empty = new GraphicSubmit();

        public Scene Scene;
        public IGraphicObject Graphic;
        public Frame Node;
        public IGraphicRender Render;
        public bool IsTransparent;

        public override string ToString()
        {
            if (Node != null)
                return Node.ToString();
            return Graphic.ToString();
        }

        public void Draw(PixelClipping clipping)
        {
            if (Render != null)
                Render.Draw(this, clipping);
        }

        public GraphicSubmit UpdateRender()
        {
            Render = Graphic.GetRender() as IGraphicRender;
            return this;
        }
    }   
}
