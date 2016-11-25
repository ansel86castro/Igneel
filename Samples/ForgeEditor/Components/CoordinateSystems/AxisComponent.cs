using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Components.CoordinateSystems;
using Igneel;
using Igneel.Graphics;

namespace ForgeEditor.Components
{
  
    public class AxisComponent:GlypComponent
    {      
        public Color4 Color { get; set; }
        public AxisName Axix { get; set; }
    }
}
