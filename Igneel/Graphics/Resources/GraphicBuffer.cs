using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Flags]
    public enum ResBinding
    {
        None = 0,
        VertexBuffer = 1 << 0,
        StreamOutput = 1 << 1,
        ShaderResource = 1 << 2,
        IndexBuffer = 1 << 3
    }

    public interface GraphicBuffer : ShaderResource, IResourceAllocator, IGraphicResource
    {
        long SizeInBytes { get; }

        int Stride { get; }

        ResourceUsage Usage { get; }

        CpuAccessFlags CpuAccess { get; }

        ResBinding Binding { get; }

        IntPtr Map(MapType map = MapType.Read, bool doNotWait = false);

        void Unmap();
    }

    public abstract class GraphicBufferBase : GraphicResource, GraphicBuffer
    {
        protected long _lenght;
        protected ResourceUsage _usage;
        protected CpuAccessFlags _cpuAccesType;
        protected int _stride;
        protected ResBinding _binding;

        public GraphicBufferBase()
            : base(ResourceType.Buffer)
        {
            
        }
        /// <summary>
        /// Size in bytes
        /// </summary>
        public long SizeInBytes { get { return _lenght; } }

        public int Stride { get { return _stride; } set { _stride = value; } }

        /// <summary>
        /// Intended GPU usage of this resources
        /// </summary>
        public ResourceUsage Usage { get { return _usage; } }

        /// <summary>
        /// Intended CPU access type of this resources
        /// </summary>
        public CpuAccessFlags CpuAccess { get { return _cpuAccesType; } }

        public ResBinding Binding { get { return _binding; } }

        /// <summary>
        /// Get a pointer to the data contained in the resource and deny GPU access to the resource.
        /// </summary>
        /// <param name="map">Flag that specifies the CPU's permissions for the reading and writing of a resource</param>
        /// <param name="doNotWait">Flag that specifies what the CPU should do when the GPU is busy</param>
        /// <returns>Pointer to the buffer resource data</returns>
        public abstract IntPtr Map(MapType map = MapType.Read, bool doNotWait = false);

        /// <summary>
        /// Release the mapping pointer and allow gpu acces to this resources
        /// </summary>
        public abstract void Unmap();
       
    }

    public static class BufferUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">array</param>
        /// <param name="offset">Offset in bytes to start writing</param>
        public static void Write<T>(this GraphicBuffer buffer, T[] data, int offset = 0, bool discard = true)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            try
            {
                unsafe
                {
                    buffer.Write(ptr.ToPointer(), offset, Marshal.SizeOf(typeof(T)) * data.Length, discard);
                }
            }

            finally
            {
                handle.Free();
            }

        }

        public static void Write(this GraphicBuffer buffer,byte[] data, int offset, int bytes, bool discard = true)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            try
            {
                unsafe
                {
                   buffer. Write(ptr.ToPointer(), offset, bytes, discard);
                }
            }

            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">array</param>
        /// <param name="offset">Offset in bytes to start reading</param>
        public static void Read<T>(this GraphicBuffer buffer, T[] data, int offset = 0)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            try
            {
                unsafe
                {
                    buffer.Read(ptr.ToPointer(), offset, Marshal.SizeOf(typeof(T)) * data.Length);
                }
            }

            finally
            {
                handle.Free();
            }

        }

        public static void Read(this GraphicBuffer buffer, byte[] data, int offset, int bytes)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            try
            {
                unsafe
                {
                    buffer.Read(ptr.ToPointer(), offset, bytes);
                }
            }

            finally
            {
                handle.Free();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Data pointer</param>
        /// <param name="offset">Offset in bytes on the Buffer to start writing</param>
        /// <param name="bytes">Number of bytes to write</param>
        public static unsafe void Write(this GraphicBuffer buffer, void* data, int offset, int bytes, bool discard = true)
        {
            byte* pBuffer = (byte*)buffer.Map(discard ? MapType.Write_Discard : MapType.Write, false);

            ClrPlatform.Crl.CopyMemory(data, pBuffer + offset, bytes);

            buffer.Unmap();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Data pointer</param>
        /// <param name="offset">Offset in bytes on the Buffer to start reading</param>
        /// <param name="bytes">Number of bytes to read</param>
        public static unsafe void Read(this GraphicBuffer buffer, void* data, int offset, int bytes)
        {
            byte* pBuffer = (byte*)buffer.Map(MapType.Read, false);

            ClrPlatform.Crl.CopyMemory(pBuffer + offset, data, bytes);

            buffer.Unmap();
        }

        public static T[] ToArray<T>(this GraphicBuffer buffer)
        {
            T[] data = new T[buffer.SizeInBytes / Marshal.SizeOf(typeof(T))];
            buffer.Read(data);
            return data;
        }
    }
}
