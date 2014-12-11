using Igneel.Design;
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

        [LockOnSet]
        public int EnvironmentMapSize { get; set; }

        [LockOnSet]
        public float EnvironmentMapZn { get; set; }

        [LockOnSet]
        public float EnvironmentMapZf { get; set; }

        [LockOnSet]
        public bool BDREnable { get; set; }

        public bool UseDefaultTechnique { get; set; }
    }
}
