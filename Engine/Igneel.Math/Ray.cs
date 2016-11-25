using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;
    }
}
