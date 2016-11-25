using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public struct BufferView
    {
        IntPtr pter;
        int stride;
        int offset;       
        public BufferView(IntPtr baseAddr, VertexDescriptor vd, IASemantic semantic, int usageIndex)
        {
            pter = baseAddr;
            offset = vd.OffsetOf(semantic, usageIndex);
            stride = vd.Size;           
        }

        public unsafe IntPtr this[int index]
        {
            get
            {
               return pter + index * stride + offset;               
            }
        }
        public unsafe IntPtr this[uint index]
        {
            get
            {       
                return (IntPtr)((uint)pter + index * (uint)stride + (uint)offset);
            }            
        }     

        public static BufferView PositionReader(IntPtr baseAddr ,VertexDescriptor vd)
        {
            return new BufferView(baseAddr, vd, IASemantic.Position, 0);
        }
        public static BufferView NormalReader(IntPtr baseAddr, VertexDescriptor vd)
        {
            return new BufferView(baseAddr, vd, IASemantic.Normal, 0);
        }
        public static BufferView TangentReader(IntPtr baseAddr, VertexDescriptor vd)
        {
            return new BufferView(baseAddr, vd, IASemantic.Tangent, 0);
        }
        public static BufferView Texture0Reader(IntPtr baseAddr, VertexDescriptor vd)
        {
            return new BufferView(baseAddr, vd, IASemantic.TextureCoordinate, 0);
        }        
    }

    public unsafe struct IndexBufferView
    {     
        byte* pter;
        int stride;
        int count;

        public IndexBufferView(IntPtr baseAddr, bool sixteenBits, int count)
        {
            this.pter = (byte*)baseAddr;        
            stride = sixteenBits ? 2 : 4;
            this.count = count;
        }

        public int Count { get { return count; } }

        public int this[int index]
        {
            get
            {
                return stride  == 2 ? (int)(*(ushort*)(pter + index * stride)) : *(int*)(pter + index * stride);
            }
            set
            {
                if (stride == 2)
                    *(ushort*)(pter + index * stride) = (ushort)value;
                else
                    *(int*)(pter + index * stride) = value;

            }
        }
    }
   
    public unsafe struct BufferView<T>: IEnumerable<T> where T:struct
    {
        byte * pter;
        int stride;      
        int count;       

        public BufferView(IntPtr addr, int count)
        {
            this.pter = (byte*)addr;
            this.count = count;
            stride = ClrRuntime.Runtime.SizeOf<T>();
        }

        public BufferView(IntPtr baseAddr, int stride, int count)
        {
            pter = (byte*)baseAddr;          
            this.stride = stride;
            this.count = count;
        }

        public BufferView(IntPtr baseAddr, VertexDescriptor vd, IASemantic semantic, int usageIndex, int count)
        {
            pter = (byte*)baseAddr + vd.OffsetOf(semantic, usageIndex);            
            stride = vd.Size;
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

        public int Count { get { return count; } }    

        public unsafe IntPtr GetPtr(int index)
        {
            return (IntPtr)(pter + index * stride);
        }

        public void GetValue(int index ,out T value)
        {
            value = ClrRuntime.Runtime.GetValue<T>(pter + index * stride);
        }

        public void SetValue(int index, ref T value)
        {
            ClrRuntime.Runtime.SetValue(value, pter + index * stride);
        }

        #region IEnumerable<T> Members

        public BufferView<T>.AttributeEnumerator GetEnumerator()
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
            BufferView<T> reader;

            public AttributeEnumerator(BufferView<T> reader)
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
                if (index >= reader.Count)
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

    public unsafe struct IndexedBufferView<T> : IEnumerable<T>
        where T : struct 
    {
        byte* vertexPter;
        int vstride;      

        byte* indicesPter;
        int istride;
        int icount;

        public IndexedBufferView(IntPtr vertexPter, int vstride, IntPtr indicesPter, int istride, int icount)           
        {
            this.vertexPter =(byte*)vertexPter;
            this.vstride = vstride;           
            this.indicesPter = (byte*)indicesPter;
            this.istride = istride;
            this.icount = icount;
        }

        public IndexedBufferView(IntPtr vertexPter, VertexDescriptor vd, IASemantic semantic, int usageIndex, IntPtr indicesPter, int istride, int icount)           
        {
            this.vertexPter = (byte*)vertexPter + vd.OffsetOf(semantic, usageIndex);
            this.vstride = vd.Size;         
            this.indicesPter = (byte*)indicesPter;
            this.istride = istride;
            this.icount = icount;      
        }

        public int Count { get { return icount; } }

        public T this[int index]
        {
            get
            {
                index = istride == 2 ? (int)(*(ushort*)(indicesPter + index * istride)) :
                                        *(int*)(indicesPter + index * istride);

                return ClrRuntime.Runtime.GetValue<T>(vertexPter + index * vstride);
            }
            set
            {
                index = istride == 2 ? (int)(*(ushort*)(indicesPter + index * istride)) :
                                        *(int*)(indicesPter + index * istride);

                ClrRuntime.Runtime.SetValue(value, vertexPter + index * vstride);
            }
        }
      
        public IntPtr GetPtr(int index)
        {
            index = istride == 2 ? (int)(*(ushort*)(indicesPter + index * istride)) :
                                       *(int*)(indicesPter + index * istride);

            return (IntPtr)(vertexPter + index * vstride);
        }

        public void GetValue(int index, out T value)
        {
            index = istride == 2 ? (int)(*(ushort*)(indicesPter + index * istride)) :
                                      *(int*)(indicesPter + index * istride);

            value = ClrRuntime.Runtime.GetValue<T>(vertexPter + index * vstride);
        }

        public void SetValue(int index, ref T value)
        {
            index = istride == 2 ? (int)(*(ushort*)(indicesPter + index * istride)) :
                                        *(int*)(indicesPter + index * istride);

            ClrRuntime.Runtime.SetValue(value, vertexPter + index * vstride);
            
        }

        #region IEnumerable<T> Members

        public IndexedBufferView<T>.AttributeEnumerator GetEnumerator()
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
            IndexedBufferView<T> reader;

            public AttributeEnumerator(IndexedBufferView<T> reader)
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
                if (index >= reader.Count)
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
