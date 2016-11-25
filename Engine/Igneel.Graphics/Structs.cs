using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Igneel.Graphics
{      
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DataBox
    {
        public int Left;
        public int Top;
        public int Front;
        public int Right;
        public int Bottom;
        public int Back;

    }
}
