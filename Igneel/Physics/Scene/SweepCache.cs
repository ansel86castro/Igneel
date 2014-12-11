using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public abstract class SweepCache:ResourceAllocator
    {
        public abstract void SetVolume(Box box);
    }
}
