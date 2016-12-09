using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewPort:IEquatable<ViewPort>
    {
        public int TopLeftX;
        public int TopLeftY;
        public int Width;
        public int Height;
        public float MinDepth;
        public float MaxDepth;

        public ViewPort(int x, int y, int width, int height)
        {
            TopLeftX = x;
            TopLeftY = y;
            Width = width;
            Height = height;
            MinDepth = 0;
            MaxDepth = 1;
        }

        public bool Equals(ViewPort other)
        {
            return TopLeftX == other.TopLeftX && TopLeftY == other.TopLeftY &&
                    Width == other.Width && Height == other.Height &&
                    MinDepth == other.MinDepth && MaxDepth == other.MaxDepth;
        }
        
    }

   
}
