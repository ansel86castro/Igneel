using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    [Flags]
    public enum ButtonState : int
    {
        Up = 0,
        Press = 1,
        Down = 2
    }
}
