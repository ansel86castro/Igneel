using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Resources
{
   
    public unsafe struct ComputeBufferView<T> : IEnumerable<T> where T : struct
    {
        byte* pter;
        int stride;
        int count;

        public ComputeBufferView(IntPtr addr, int count)
        {
            this.pter = (byte*)addr;
            this.count = count;
            stride = ClrRuntime.Runtime.SizeOf<T>();
        }

        public ComputeBufferView(IntPtr baseAddr, int stride, int count)
        {
            pter = (byte*)baseAddr;
            this.stride = stride;
            this.count = count;
        }       

        public T this[int index]
        {
            get
            {                
                return ClrRuntime.Runtime.GetValue<T>(pter + index * stride);
            }
            set
            {
                ClrRuntime.Runtime.SetValue(value, pter + index * stride);
            }

        }

        public byte* BasePter { get { return pter; } }

        public int Stride { get { return stride; } }

        public int Lenght { get { return count; } }

        public unsafe IntPtr GetPtr(int index)
        {
            return (IntPtr)(pter + index * stride);
        }

        public void GetValue(int index, out T value)
        {
            value = ClrRuntime.Runtime.GetValue<T>(pter + index * stride);
        }

        public void SetValue(int index, ref T value)
        {
            ClrRuntime.Runtime.SetValue(value, pter + index * stride);
        }

        #region IEnumerable<T> Members

        public ComputeBufferView<T>.AttributeEnumerator GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }
    

        #endregion

        public struct AttributeEnumerator : IEnumerator<T>
        {
            int index;
            ComputeBufferView<T> reader;

            public AttributeEnumerator(ComputeBufferView<T> reader)
            {
                this.reader = reader;
                this.index = -1;
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return reader[index]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                index = -1;
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return reader[index]; }
            }

            public bool MoveNext()
            {
                index++;
                if (index >= reader.Lenght)
                    return false;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion
        }

    }
}
