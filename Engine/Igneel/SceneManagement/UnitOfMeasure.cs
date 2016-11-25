using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.SceneManagement
{
    public class UnitOfMeasure
    {
        public UOMType Type { get; set; }

        /// <summary>
        /// How many real-world meters in one distance unit as a floating-point number. 
        /// For example, 1.0 for the Type "Meter"; 1000 for the Type "Kilometer"; 0.3048 for the name "Foot
        /// </summary>
        public float Meters { get; set; }
    }
}
