using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Resources
{
    public class CpuBuffer : ComputeBuffer
    {
        Array buffer;
        GCHandle handle;
         

        private CpuBuffer(CPUAccess cPUAccess, GPUAccess gPUAccess, int lenght)
           : base(CPUAccess.ReadWrite, GPUAccess.None, lenght)
        {

        }

        public CpuBuffer(int lenght)
            :base(CPUAccess.ReadWrite, GPUAccess.None, lenght)
        {
            buffer = new byte[lenght];
        }

        public CpuBuffer(Array data, int lenght)
            : base(CPUAccess.ReadWrite, GPUAccess.None, lenght)
        {
            this.buffer = data;
        }

        public override ComputeBuffer Clone()
        {
            CpuBuffer clone = new CpuBuffer(CpuAccess, GpuAcess, Lenght);
            clone.buffer =(Array)buffer.Clone();
            return clone;
        }

        public override IntPtr Lock(CPUAccess cpuAccess = CPUAccess.Read)
        {
            lock (buffer)
            {
                if (IsLocked)
                {
                    return pter;
                }
                     
                handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                pter = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                IsLocked = true;
            }

            return pter;
        }

        public override void UnLock()
        {
            lock (buffer)
            {
                if (!IsLocked)
                    return;

                handle.Free();
                pter = IntPtr.Zero;
            }
        }
    }
}
