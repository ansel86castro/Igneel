using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
   

    public struct BufferDesc
    {
        /// <summary>
        /// Size in Bytes
        /// </summary>
        public long SizeInBytes;
        /// <summary>
        /// Vertex stride used in the case of a VertexBuffer Binding 
        /// </summary>
        public int Stride;
        /// <summary>
        /// Intended Usage
        /// </summary>
        public ResourceUsage Usage;
        /// <summary>
        /// CPU Access Right
        /// </summary>
        public CpuAccessFlags Access;
        /// <summary>
        /// Binding of the Resource to the Pipeline. Can be any combination of the values 
        /// </summary>
        public ResBinding Binding;

        public IntPtr Data;
    }

    public interface ShaderBuffer : GraphicBuffer, ShaderResource
    {
        ResBinding Binding { get; }
    }

    public abstract class ShaderBufferBase : GraphicBufferBase, ShaderBuffer
    {
        protected ResBinding binding;

        public ResBinding Binding
        {
            get { return binding; }
        }
    }
}
