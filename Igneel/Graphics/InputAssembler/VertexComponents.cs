using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Byte4
    {
        public byte X;
        public byte Y;
        public byte Z;
        public byte W;

        public Byte4(int v)
        {
            X = (byte)(v >> 24);
            Y = (byte)(v >> 16);
            Z = (byte)(v >> 8);
            W = (byte)(v >> 0);
        }

        public Byte4(byte x, byte y, byte z, byte w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > 3) throw new InvalidOperationException();
                fixed (byte* pter = &W)
                {
                    return pter[index];
                }
            }
            set
            {
                if (index < 0 || index > 3) throw new InvalidOperationException();
                fixed (byte* pter = &W)
                {
                    pter[index] = value;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[X:{0} ,Y:{1} ,Z:{2} ,W:{3}]", X, Y, Z, W);
        }

    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Int4
    {
        public int W;
        public int X;
        public int Y;
        public int Z;

        public Int4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return string.Format("[X:{0} ,Y:{1} ,Z:{2} ,W:{3}]", X, Y, Z, W);
        }

        public static explicit operator Int4(int value)
        {
            return new Int4(value, value, value, value);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Short4
    {
        public short W;
        public short X;
        public short Y;
        public short Z;

        public Short4(short x, short y, short z, short w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return string.Format("[X:{0} ,Y:{1} ,Z:{2} ,W:{3}]", X, Y, Z, W);
        }

        public static explicit operator Short4(short value)
        {
            return new Short4(value, value, value, value);
        }
    }
}
