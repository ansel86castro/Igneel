using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Resources
{

    public abstract class ComputeBuffer: ResourceAllocator
    {
        protected CPUAccess cpuAccess;
        protected GPUAccess gpuAccess;
        protected IntPtr pter;
        protected int lenght;

        public ComputeBuffer(CPUAccess cpuAccess, GPUAccess gpuAccess, int lenght)
        {
            this.cpuAccess = cpuAccess;
            this.gpuAccess = gpuAccess;
            this.lenght = lenght;
        }

        public CPUAccess CpuAccess => cpuAccess;

        public GPUAccess GpuAcess => gpuAccess;

        public int Lenght => lenght;

        public IntPtr DataPointer => pter;

        public bool IsLocked { get; protected set; }

        public abstract IntPtr Lock(CPUAccess cpuAccess = CPUAccess.Read);

        public abstract void UnLock();

        public abstract ComputeBuffer Clone();
    }
    

    public static class BufferExt
    {
        public static ComputeBufferView<T> ViewAs<T>(this ComputeBuffer buffer, int offset = 0)
            where T:struct
        {
            if (!buffer.IsLocked)
                throw new InvalidOperationException($"Must call {nameof(buffer)}.{nameof(ComputeBuffer.Lock)} first");

            var elemetSize = ClrRuntime.Runtime.SizeOf<T>();
            return new ComputeBufferView<T>(buffer.DataPointer + offset * elemetSize, buffer.Lenght / elemetSize);
        }

        public static void Write(this ComputeBuffer buffer, byte [] data, int offset = 0)
        {
            buffer.Lock();

            try
            {
                ClrRuntime.Runtime.Copy(data, buffer.DataPointer, offset, data.Length);
            }
            finally
            {
                buffer.UnLock();
            }
        }
    }
}
