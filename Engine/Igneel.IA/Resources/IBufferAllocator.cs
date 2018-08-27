using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Resources
{
    public interface IBufferAllocator
    {
        ComputeBuffer AllocateBuffer(int lenght, Array data = null);        
    }


    public class CPUBufferAllocator : IBufferAllocator
    {
        public ComputeBuffer AllocateBuffer(int lenght, Array data = null)
        {
            if (data != null)
            {
                return new CpuBuffer(data, lenght);
            }
            else
                return new CpuBuffer(lenght);
        }       
    }
}
