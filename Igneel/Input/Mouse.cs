using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public abstract class Mouse: ResourceAllocator
    {
        public abstract int X { get; }

        public abstract int Y { get; }

        public abstract int Z { get; }

        public abstract bool IsButtonPresed(MouseButton button);
       
    }
}
