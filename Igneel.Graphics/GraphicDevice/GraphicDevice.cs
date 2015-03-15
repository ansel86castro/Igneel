using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum DeviceProfile
    {
        Direct3D,
        OpenGL
    }

    public abstract partial class GraphicDevice: ResourceAllocator
    {               
        private DeviceInfo _deviceInfo;
        protected GraphicDeviceDesc _desc;        

        public GraphicDevice(GraphicDeviceDesc desc)
        {
            rasStack = new RasterizerStateStack(this);
            blendStack = new BlendStateStack(this);
            depthStack = new DepthStencilStateStack(this);

            _desc = desc;            
 
            _deviceInfo = InitDevice(desc);           
            InitIA();         
            InitShading();
            InitRS();
            InitOM();                                      
        }

        /// <summary>
        /// returns the shader model in the format 000. For example SM 4.0 would be 400 , SM 4.1 would be 410 
        /// and SM 5.0 500
        /// </summary>
        public int ShaderModelVersion { get; protected set; }

        /// <summary>
        /// Says what api model the implementation is using
        /// </summary>
        public DeviceProfile Profile { get; protected set; }

        public DeviceInfo Info { get { return _deviceInfo; } protected set { _deviceInfo = value; } }

        public GraphicDeviceDesc Description { get { return _desc; } }

        public void Clear(ClearFlags flags, int color, float depth, Color4 stencil)
        {
            Clear(flags, new Color4(color), depth, stencil.ToArgb());
        }

        public void Clear(ClearFlags flags, int color, float depth, int stencil)
        {
            Clear(flags, new Color4(color), depth, stencil);
        }

        #region Abstracts

        /// <summary>
        /// Get the support of a given format on the installed video device
        /// </summary>
        /// <param name="format">enumeration that describes a format for which to check for support</param>
        /// <param name="support">A bitfield of FormatSupport enumeration values describing how the specified format is supported on the installed device. The values are ORed together</param>
        /// <returns></returns>
        public abstract bool CheckFormatSupport(Format format, BindFlags binding, ResourceType type);

        /// <summary>
        /// Get the number of quality levels available during multisampling
        /// </summary>
        /// <param name="format">The texture format</param>
        /// <param name="multySampleCount">The number of samples during multisampling</param>
        /// <returns>Number of quality levels supported by the adapter</returns>
        public abstract int CheckMultisampleQualityLevels(Format format, int multySampleCount, bool windowed);

        protected abstract DeviceInfo InitDevice(GraphicDeviceDesc desc);        

        public abstract SamplerState CreateSamplerState(SamplerDesc desc);

        public abstract Texture1D CreateTexture1D(Texture1DDesc desc, IntPtr[] data = null);

        public abstract Texture2D CreateTexture2D(Texture2DDesc desc, MappedTexture2D[] data = null);

        public abstract Texture3D CreateTexture3D(Texture3DDesc desc, MappedTexture3D[] data = null);

        public abstract Texture1D CreateTexture1DFromFile(string filename);

        public abstract Texture1D CreateTexture1DFromStream(Stream stream);

        public abstract Texture2D CreateTexture2DFromFile(string filename);

        public abstract Texture2D CreateTexture2DFromStream(Stream stream);

        public abstract Texture3D CreateTexture3DFromFile(string filename);

        public abstract Texture3D CreateTexture3DFromStream(Stream stream);

        public abstract void GenerateMips(Texture texture);

        public abstract void Clear(ClearFlags flags, Color4 color, float depth, int stencil);

        public abstract void Draw(int vertexCount, int startVertexIndex);

        public abstract void DrawIndexed(int indexCount, int startIndex, int baseVertexIndex);

        public abstract void DrawAuto();
        
        //public abstract void Draw(int startVertexIndex, int primitiveCount);

        //public abstract void DrawIndexed(int baseVertexIndex, int minVertexIndex , int numVertices, int startIndex, int primitiveCount);

        //public unsafe abstract void DrawUser(int primitiveCount, void * vertices , int vertexStride );

        //public unsafe abstract void DrawIndexedUser(int minVertexIndex, int numVertices, int primitiveCount , void* indices, IndexFormat format, void* vertices, int vertexStride);

        public abstract void ResizeBackBuffer(int width, int height);

        /// <summary>
        /// Down Sample a multisample texture
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcSub"></param>
        /// <param name="desc"></param>
        /// <param name="destSub"></param>
        public abstract void ResolveSubresource(Texture src, int srcSub, Texture desc, int destSub);

        /// <summary>
        /// Copy a region from a source resource to a destination resource.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcSubRes"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="desc"></param>
        /// <param name="destSubRes"></param>
        /// <param name="destRec"></param>
        public abstract void CopySubResource(Texture destTexture, int destSubRes, int dstX , int dstY, int dstZ, Texture srcTexture, int srcSubRes, DataBox destBox);

        /// <summary>
        /// Copy a source texture to a destination texture
        /// </summary>
        /// <param name="src"></param>
        /// <param name="desc"></param>
        public abstract void CopyTexture(Texture src, Texture desc);

        /// <summary>
        /// The CPU copies data from memory to a subresource created in non-mappable memory
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="subResource"></param>
        /// <param name="box"></param>
        /// <param name="srcPointer"></param>
        /// <param name="srcRowPith"></param>
        /// <param name="srcDepthPitch"></param>
        public void UpdateSubResource(Texture dest, int subResource, DataBox box, IntPtr srcPointer, int srcRowPith, int srcDepthPitch)
        {
            unsafe
            {
                UpdateSubResource(dest, subResource, &box, srcPointer.ToPointer(), srcRowPith, srcDepthPitch);
            }
        }

        /// <summary>
        /// The CPU copies data from memory to a subresource created in non-mappable memory
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="subResource"></param>
        /// <param name="box"></param>
        /// <param name="srcPointer"></param>
        /// <param name="srcRowPith"></param>
        /// <param name="srcDepthPitch"></param>
        public void UpdateSubResource(Texture dest, int subResource, IntPtr srcPointer, int srcRowPith, int srcDepthPitch)
        {
            unsafe
            {
                UpdateSubResource(dest, subResource, (DataBox*)0, srcPointer.ToPointer(), srcRowPith, srcDepthPitch);
            }
        }

        /// <summary>
        /// The CPU copies data from memory to a subresource created in non-mappable memory
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="subResource"></param>
        /// <param name="box"></param>
        /// <param name="srcPointer"></param>
        /// <param name="srcRowPith"></param>
        /// <param name="srcDepthPitch"></param>
        protected unsafe abstract void UpdateSubResource(Texture dest, int subResource, DataBox* box, void* srcPointer, int srcRowPith, int srcDepthPitch);        

        #endregion

        //public void DrawUser<T>(int primitiveCount, T[] vertexes)
        //{
        //    GCHandle h = GCHandle.Alloc(vertexes, GCHandleType.Pinned);
        //    try
        //    {
        //        unsafe
        //        {
        //            void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(vertexes, 0).ToPointer();
        //            DrawUser(primitiveCount, pter, ClrRuntime.Runtime.SizeOf<T>());
        //        }
        //    }
        //    finally
        //    {
        //        h.Free();
        //    }

        //}

        //public void DrawIndexedUser<T, I>(int minVertexIndex, int numVertices, int primitiveCount, I[] indices, T[] vertices)
        //{
        //    GCHandle hv = GCHandle.Alloc(vertices, GCHandleType.Pinned);
        //    GCHandle hi = GCHandle.Alloc(indices, GCHandleType.Pinned);
        //    try
        //    {
        //        unsafe
        //        {
        //            void* pVertices = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0).ToPointer();
        //            void* pIndices = Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0).ToPointer();
        //            IndexFormat indexFormat = Marshal.SizeOf(typeof(I)) == 2 ? IndexFormat.Index16 : IndexFormat.Index32;

        //            DrawIndexedUser(minVertexIndex, numVertices, primitiveCount, pIndices, indexFormat, pVertices, ClrRuntime.Runtime.SizeOf<T>());
        //        }
        //    }
        //    finally
        //    {
        //        hv.Free();
        //        hi.Free();
        //    }
        //}

        public InputLayout CreateInputLayout<T>(ShaderCode bytecode)
          where T : struct
        {
            var vd = VertexDescriptor.GetDescriptor<T>();
            return CreateInputLayout(vd.Elements, bytecode);
        }

        protected virtual void DisposeShader() { }

        protected virtual void DisposeRS() { }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                DisposeIA();
                DisposeShader();
                DisposeRS();
                DisposeOM();

            }
        }


      
    }

}
