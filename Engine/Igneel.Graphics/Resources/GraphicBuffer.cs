using ClrRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Flags]
    public enum ResourceBinding
    {
        None = 0,
        VertexBuffer = 1 << 0,
        StreamOutput = 1 << 1,
        ShaderResource = 1 << 2,
        IndexBuffer = 1 << 3
    }

    //public interface GraphicBuffer : ShaderResource, IResourceAllocator, IGraphicResource
    //{
    //    long SizeInBytes { get; }

    //    int Stride { get; }

    //    ResourceUsage Usage { get; }

    //    CpuAccessFlags CpuAccess { get; }

    //    ResourceBinding Binding { get; }

    //    IntPtr Map(MapType map = MapType.Read, bool doNotWait = false);

    //    void Unmap();
    //}

    public abstract class GraphicBuffer : GraphicResource, IShaderResource
    {
        private long _lenght;
        private ResourceUsage _usage;
        private CpuAccessFlags _cpuAccesType;
        private int _stride;
        private ResourceBinding _binding;

        protected GraphicBuffer()
            : base(ResourceType.Buffer)
        {
            
        }

        protected GraphicBuffer(long size, int stride, ResourceUsage usage, CpuAccessFlags access, ResourceBinding binding)
            : base(ResourceType.Buffer)
        {
            _lenght = size;
            _stride = stride;
            _usage = usage;
            _cpuAccesType = access;
            _binding = binding;
        }


        /// <summary>
        /// Size in bytes
        /// </summary>
        public long SizeInBytes { get { return _lenght; } protected set { _lenght = value; } }

        public int Stride { get { return _stride; } set { _stride = value; } }

        /// <summary>
        /// Intended GPU usage of this resources
        /// </summary>
        public ResourceUsage Usage { get { return _usage; } protected set { _usage = value; } }

        /// <summary>
        /// Intended CPU access type of this resources
        /// </summary>
        public CpuAccessFlags CpuAccess { get { return _cpuAccesType; } protected set { _cpuAccesType = value; } }

        public ResourceBinding Binding { get { return _binding; } protected set { _binding = value; } }

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
            where T:struct
        {
            unsafe
            {
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var ptr = ClrRuntime.Runtime.GetPointer(data, 0);

                try
                {

                    buffer.Write(ptr, offset, ClrRuntime.Runtime.SizeOf<T>() * data.Length, discard);
                }

                finally
                {
                    handle.Free();
                }
            }

        }

        public static void Write(this GraphicBuffer buffer, byte[] data, int offset, int bytes, bool discard = true)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            unsafe
            {
                var ptr = ClrRuntime.Runtime.GetPointer(data, 0);
                try
                {

                    buffer.Write(ptr, offset, bytes, discard);

                }

                finally
                {
                    handle.Free();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">array</param>
        /// <param name="offset">Offset in bytes to start reading</param>
        public static void Read<T>(this GraphicBuffer buffer, T[] data, int offset = 0)
            where T :struct
        {
            unsafe
            {
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var ptr = ClrRuntime.Runtime.GetPointer(data, 0);

                try
                {
                   
                        buffer.Read(ptr, offset, ClrRuntime.Runtime.SizeOf<T>() * data.Length);
                   
                }

                finally
                {
                    handle.Free();
                }
            }

        }

        public static void Read(this GraphicBuffer buffer, byte[] data, int offset, int bytes) 
        {
            unsafe
            {
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var ptr = ClrRuntime.Runtime.GetPointer(data, 0);

                try
                {

                    buffer.Read(ptr, offset, bytes);

                }

                finally
                {
                    handle.Free();
                }
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

            Runtime.Copy(data, pBuffer + offset, bytes);

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

            Runtime.Copy(pBuffer + offset, data, bytes);

            buffer.Unmap();
        }

        public static T[] ToArray<T>(this GraphicBuffer buffer) where T : struct
        {
            T[] data = new T[buffer.SizeInBytes / ClrRuntime.Runtime.SizeOf<T>()];
            buffer.Read(data);
            return data;
        }
    }
}
