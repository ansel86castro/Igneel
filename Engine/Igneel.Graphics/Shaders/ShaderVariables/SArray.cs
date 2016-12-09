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
    public struct SArray<T>
        where T : struct
    {
        public T[] Array;
        public int Offset;
        public int Count;

        public SArray(T[] array, int offset, int count)
        {
            this.Array = array;
            this.Offset = offset;
            this.Count = count;
        }

        public SArray(T[] array, int count)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = count;
        }

        public SArray(T[] array)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = array.Length;
        }
    }
}
