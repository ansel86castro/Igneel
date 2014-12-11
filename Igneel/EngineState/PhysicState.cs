using Igneel.Design;
using Igneel.Design.UITypeEditors;
using Igneel.Physics;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class PhysicsState : EngineState
    {      
        public PhysicsState()
        {
            Color = (Vector3)System.Drawing.Color.LightGreen;
            Alpha = 1.0f;           
        }       
       
        public bool EnableHardwareSimulation { get; set; }

         

        [Editor(typeof(UIColorTypeEditor), typeof(UITypeEditor))]
        public Vector3 Color { get; set; }

        [Editor(typeof(UIInmediateNumericEditor), typeof(UITypeEditor))]   
        public float Alpha { get; set; }
    }
}
