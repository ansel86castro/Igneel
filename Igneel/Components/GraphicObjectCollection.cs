using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public class GraphicObjectCollection : NodeObjectColletion, IGraphicObject
    {
        private bool visible;

        public bool IsTransparent
        {
            get { return false; }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = false;
            }
        }

        public Rendering.RenderBinder RenderParam { get; set; }       

        public void Draw(SceneNode node, Rendering.PixelClipping clipping = PixelClipping.None)
        {
          
        }

        public void Draw()
        {
            
        }
    }
}
