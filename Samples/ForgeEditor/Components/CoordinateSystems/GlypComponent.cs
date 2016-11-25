using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Graphics;

namespace ForgeEditor.Components.CoordinateSystems
{
    public class GlypComponent
    {
        public GraphicBuffer VertexBuffer { get; set; }
        public GraphicBuffer IndexBufffer { get; set; }
        public int Id { get; set; }
    }
}
