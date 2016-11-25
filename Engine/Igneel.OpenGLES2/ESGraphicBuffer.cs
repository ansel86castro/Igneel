using Igneel.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.OpenGLES2
{
    public class ESGraphicBuffer : GraphicBufferBase
    {
        public int buffer;
        byte[] bufferData;
        GCHandle handle;
        All _type;
        public ESGraphicBuffer(int size, int stride, All type, ResourceUsage usage, CpuAccessFlags cpuAcces, 
            ResBinding binding, IntPtr data)
        {
            _type = type;
            this._lenght = size;
            this._stride = stride;
            this._usage = usage;
            this._cpuAccesType = cpuAcces;
            this._binding = binding;
            
            GL.GenBuffers(1, out buffer);
            GL.BindBuffer(type, buffer);
            GL.BufferData(type, new IntPtr(size), data, Utils.GetUsage(usage));
            
            if (cpuAcces != CpuAccessFlags.None)
            {
                bufferData = new byte[size];
                if(data!= IntPtr.Zero)
                    ClrRuntime.Runtime.Copy(data, bufferData, 0, size);
            }
        }
        public override IntPtr Map(MapType map = MapType.Read, bool doNotWait = false)
        {
            if (bufferData != null)
            {
                handle = GCHandle.Alloc(bufferData, GCHandleType.Pinned);
                return ClrRuntime.Runtime.GetPtr(bufferData, 0);
            }
            else
                throw new InvalidOperationException();
        }

        public override void Unmap()
        {
            if (bufferData != null)
            {
                handle.Free();
                GL.BufferData(_type, new IntPtr(_lenght), bufferData, Utils.GetUsage(_usage));
            }
            else
                throw new InvalidOperationException();
        }
    }
}
