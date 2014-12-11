using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public abstract class Keyboard: ResourceAllocator
    {
        public abstract bool IsKeyPressed(Keys key);
       
    }
}
