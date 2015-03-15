
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public struct Bounds3
    {
        public Vector3 Min, Max;

        public Bounds3(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }


    }
}
