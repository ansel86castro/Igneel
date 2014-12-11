using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Flags]
    public enum BufferBinding
    {
        None          = 0 ,
        VertexBuffer  = 1 << 0,
        StreamOutput  = 1 << 1,
        ShaderResource= 1 << 2
    }

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
        public BufferBinding Binding;

        public IntPtr Data;
    }

    public interface ShaderBuffer : GraphicBuffer, ShaderResource
    {
        BufferBinding Binding { get; }
    }

    public abstract class ShaderBufferBase : GraphicBufferBase, ShaderBuffer
    {
        protected BufferBinding binding;

        public BufferBinding Binding
        {
            get { return binding; }
        }
    }
}
