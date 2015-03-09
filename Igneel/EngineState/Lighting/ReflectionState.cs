
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class ReflectionState:EnabilitableState
    {
        public ReflectionState()
        {
            EnvironmentMapSize = 128;
            EnvironmentMapZn = 1;
            EnvironmentMapZf = 1000;
        }

        
        public int EnvironmentMapSize { get; set; }

        
        public float EnvironmentMapZn { get; set; }

        
        public float EnvironmentMapZf { get; set; }

        
        public bool BDREnable { get; set; }

        public bool UseDefaultTechnique { get; set; }
    }
}
