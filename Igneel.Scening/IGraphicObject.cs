
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
      
    public interface IGraphicObject: IRenderable
    {
        bool IsTransparent { get; }

        bool Visible { get; set; }

        RenderBinder RenderParam { get; set; }

        Render GetRender();

        void Draw(SceneNode node, Render render, PixelClipping clipping = PixelClipping.None);
    }   
}
