

using Igneel.Graphics;
using Igneel.Physics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{
    public class PhysicsState : EngineState
    {      
        public PhysicsState()
        {
            Color = (Vector3)new Color4(0, 0, 1);
            Alpha = 1.0f;           
        }       
       
        public bool EnableHardwareSimulation { get; set; }

         

        
        public Vector3 Color { get; set; }

           
        public float Alpha { get; set; }
    }
}
