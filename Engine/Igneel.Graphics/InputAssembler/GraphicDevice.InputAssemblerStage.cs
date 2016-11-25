using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct IAInitialization
    {
        public int NbVertexBuffers;
    }

    public struct BufferBind
    {
        public int offset;
        public int stride;
        public GraphicBuffer buffer;

        public void Dispose()
        {
            buffer.Dispose();
        }
    }

    public abstract partial class GraphicDevice
    {               

        private BufferBind[] _iaVertexBufferBind;
        private BufferBind _iaIndexBufferBind;

        protected InputLayout _iaInputLayout;
        protected IAPrimitive _iaPrimitiveType;

        public InputLayout InputDefinition
        {
            get
            {
                return _iaInputLayout;
            }
            private set
            {               
                if (_iaInputLayout != value)
                {
                    _iaInputLayout = value;
                    IASetInputLayout(value);
                }
            }
        }

        public IAPrimitive PrimitiveTopology 
        {
            get { return _iaPrimitiveType; }
            set
            {
                _iaPrimitiveType = value;
                IASetPrimitiveType(value);
            }
        }

        #region Abstract Protected

        protected void InitIA()
        {
            var init = GetIAInitialization();
            _iaVertexBufferBind = new BufferBind[init.NbVertexBuffers];
        }

        protected abstract IAInitialization GetIAInitialization();        

        protected abstract void IASetInputLayout(InputLayout value);

        protected abstract void IASetPrimitiveType(IAPrimitive value);

        protected abstract void IASetVertexBufferImpl(int slot, GraphicBuffer vertexBuffer, int offset, int stride);

        protected abstract void IASetIndexBufferImpl(GraphicBuffer indexBuffer, int offset);

        #endregion

        #region Abstract Public

        public abstract InputLayout CreateInputLayout(VertexElement[] elements, ShaderCode signature);

        public abstract GraphicBuffer CreateVertexBuffer(int size, int stride, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, ResourceBinding binding = ResourceBinding.VertexBuffer , IntPtr data = default(IntPtr));

        public abstract GraphicBuffer CreateIndexBuffer(int size, IndexFormat format = IndexFormat.Index16, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, IntPtr data = default(IntPtr));

        public abstract void UpdateBuffer(GraphicBuffer buffer, int offset, IntPtr pterData, int dataSize);
        
        #endregion

        public void SetVertexBuffer(int slot, GraphicBuffer vertexBuffer, int offset = 0)
        {
            BufferBind bind;
            bind.buffer= vertexBuffer;
            bind.offset = offset;
            bind.stride = vertexBuffer.Stride;
            _iaVertexBufferBind[slot] = bind;

            IASetVertexBufferImpl(slot, vertexBuffer, offset, vertexBuffer.Stride);
        }

        public void SetVertexBuffer(int slot, GraphicBuffer vertexBuffer, int offset, int stride)
        {
            BufferBind bind;
            bind.buffer = vertexBuffer;
            bind.offset = offset;
            bind.stride = stride;
            _iaVertexBufferBind[slot] = bind;

            IASetVertexBufferImpl(slot, vertexBuffer, offset, stride);
        }

        public void SetIndexBuffer(GraphicBuffer indexBuffer, int offset = 0)
        {
            _iaIndexBufferBind.buffer = indexBuffer;
            _iaIndexBufferBind.offset = offset;
            _iaIndexBufferBind.stride = indexBuffer.Stride;

            IASetIndexBufferImpl(indexBuffer, offset);
        }

        public GraphicBuffer GetVertexBuffer(int slot, out int offset)
        {
            offset = _iaVertexBufferBind[slot].offset;
            return _iaVertexBufferBind[slot].buffer;
        }             

        public GraphicBuffer GetVertexBuffer(out int offset)
        {
            offset = _iaIndexBufferBind.offset;
            return _iaIndexBufferBind.buffer;
        }

        public GraphicBuffer CreateVertexBuffer(int size, int stride, Array data, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, ResourceBinding binding = ResourceBinding.VertexBuffer)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var pter = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            GraphicBuffer buffer;
            try
            {
                buffer = CreateVertexBuffer(size, stride, usage, cpuAcces, binding ,pter);
            }
            finally
            {

                handle.Free();
            }
            return buffer;
        }

        public GraphicBuffer CreateIndexBuffer(int size, Array data,  IndexFormat format = IndexFormat.Index16, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var pter = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            GraphicBuffer buffer;
            try
            {
                buffer = CreateIndexBuffer(size, format, usage, cpuAcces, pter);
            }
            finally
            {

                handle.Free();
            }
            return buffer;
        }

        public GraphicBuffer CreateVertexBuffer<T>(ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, ResourceBinding binding = ResourceBinding.VertexBuffer, T[] data = null)
            where T:struct
        {
            int stride = ClrRuntime.Runtime.SizeOf<T>();
            int size = data.Length * stride;
            return CreateVertexBuffer(size, stride, data, usage, cpuAcces, binding);
        }

        public GraphicBuffer CreateVertexBuffer<T>(int stride, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, ResourceBinding binding = ResourceBinding.VertexBuffer, T[] data = null)
            where T : struct
        {
            int size = data.Length * ClrRuntime.Runtime.SizeOf<T>();
            return CreateVertexBuffer(size, stride,data, usage, cpuAcces, binding);
        }

        public GraphicBuffer CreateIndexBuffer<T>(ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, T[] data = null)
            where T : struct
        {
            int stride = ClrRuntime.Runtime.SizeOf<T>();
            int size = data.Length * stride;
            IndexFormat format = stride == 2 ? IndexFormat.Index16 : IndexFormat.Index32;
            return CreateIndexBuffer(size, data, format, usage, cpuAcces);
        }

        protected virtual void DisposeIA()
        {

        }

    }

}
