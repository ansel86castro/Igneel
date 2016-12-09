using Igneel.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;

namespace Igneel.OpenGLES2
{
    public partial class ESGraphicDevice : GraphicDevice
    {
        public ESGraphicDevice(GraphicDeviceDesc desc)
            : base(desc)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The device.</returns>
        /// <param name="desc">Desc.</param>
        protected override DeviceInfo InitDevice(GraphicDeviceDesc desc)
		{            
            int width;
            GL.GetInteger(All.MaxTextureSize, out width);
            int maxTextures;
            GL.GetInteger(All.MaxTextureImageUnits,out maxTextures);

            DeviceInfo id = new DeviceInfo()
            {
                DeviceId = 0,
                DeviceName = GL.GetString(All.Renderer),
                MaxTextureWidth = width,
                MaxTextureHeight = width,
                RefreshRate = 60,
                DisplayHeight = desc != null ? desc.Context.BackBufferHeight : 0,
                DisplayWidth = desc != null ? desc.Context.BackBufferWidth : 0,
                DriverType = GraphicDeviceType.Hardware,
                VertexProcessing = VertexProcessing.Hardware,
                DisplayFormat = Format.B8G8R8A8_UNORM,
                MSAA = desc != null ? desc.Context.Sampling : new Multisampling(1, 0),
                SimultaneousRTCount = 4,
                MaxSimultaneousTextures = maxTextures
            };


            return id;

		}

        public override bool CheckFormatSupport(Format format, BindFlags binding, ResourceType type)
        {
            switch (binding)
            {
                case BindFlags.DepthStencil:
                    return type == ResourceType.Buffer && (
                        format == Format.D16_UNORM ||
                         format == Format.D24_UNORM_S8_UINT ||
                          format == Format.D32_FLOAT ||
                          format == Format.D32_FLOAT_S8X24_UINT);              
                case BindFlags.RenderTarget:
                    {

                    }
                    break;
                case BindFlags.ShaderResource:
                    return type == ResourceType.Texture1D || type == ResourceType.Texture2D || type == ResourceType.Texture3D                    
                case BindFlags.None:
                    throw new ArgumentException("binding");
            }
            return true;
        }

        public override int CheckMultisampleQualityLevels(Format format, int multySampleCount, bool windowed)
        {
            int msaa;
            GL.GetInteger(All.SampleBuffers, out msaa);
            if (msaa == 1)
            {
                int count;
                GL.GetInteger(All.Samples, out count);
                return count == multySampleCount ? 1 : 0;
            }

            return 0;
        }

        public override void Clear(ClearFlags flags, Color4 color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }

        public override void CopySubResource(Texture destTexture, int destSubRes, int dstX, int dstY, int dstZ, Texture srcTexture, int srcSubRes, DataBox destBox)
        {
            throw new NotImplementedException();
        }

        public override void CopyTexture(Texture src, Texture desc)
        {
            throw new NotImplementedException();
        }

        public override BlendState CreateBlendState(BlendDesc desc)
        {
            throw new NotImplementedException();
        }

        public override DepthStencil CreateDepthStencil(DepthStencilDesc desc)
        {
            throw new NotImplementedException();
        }

        public override DepthStencilState CreateDepthStencilState(DepthStencilStateDesc desc)
        {
            throw new NotImplementedException();
        }

        public override GraphicBuffer CreateIndexBuffer(int size,
            IndexFormat format = IndexFormat.Index16,
            ResourceUsage usage = ResourceUsage.Default,
            CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite,
            IntPtr data = default(IntPtr))
        {
            return new ESGraphicBuffer(size, format == IndexFormat.Index16?2:4, All.ElementArrayBuffer, usage, cpuAcces, ResBinding.IndexBuffer, data);
        }

        public override InputLayout CreateInputLayout(VertexElement[] elements, ShaderCode signature)
        {
            throw new NotImplementedException();
        }

        public override ShaderProgram CreateProgram(ShaderProgramDesc desc)
        {
            throw new NotImplementedException();
        }

        public override RasterizerState CreateRasterizerState(RasterizerDesc desc)
        {
            throw new NotImplementedException();
        }

        public override RenderTarget CreateRenderTarget(Texture2D texture, int subResource = 0, int count = 1)
        {
            throw new NotImplementedException();
        }

        public override SamplerState CreateSamplerState(SamplerDesc desc)
        {
            throw new NotImplementedException();
        }

        protected override SwapChain CreateSwapChainImp(IGraphicContext context)
        {
            throw new NotImplementedException();
        }

        public override Texture1D CreateTexture1D(Texture1DDesc desc, IntPtr[] data = null)
        {
            throw new NotImplementedException();
        }

        public override Texture1D CreateTexture1DFromFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override Texture1D CreateTexture1DFromStream(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public override Texture2D CreateTexture2D(Texture2DDesc desc, MappedTexture2D[] data = null)
        {
            throw new NotImplementedException();
        }

        public override Texture2D CreateTexture2DFromFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override Texture2D CreateTexture2DFromStream(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public override Texture3D CreateTexture3D(Texture3DDesc desc, MappedTexture3D[] data = null)
        {
            throw new NotImplementedException();
        }

        public override Texture3D CreateTexture3DFromFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override Texture3D CreateTexture3DFromStream(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public override GraphicBuffer CreateVertexBuffer(int size, int stride, 
            ResourceUsage usage = ResourceUsage.Default,
            CpuAccessFlags cpuAcces = CpuAccessFlags.ReadWrite, 
            ResBinding binding = ResBinding.VertexBuffer,
            IntPtr data = default(IntPtr))
        {
            return new ESGraphicBuffer(size, stride, All.ArrayBuffer, usage, cpuAcces, binding, data);
        }

        public override void Draw(int vertexCount, int startVertexIndex)
        {
            throw new NotImplementedException();
        }

        public override void DrawAuto()
        {
            throw new NotImplementedException();
        }

        public override void DrawIndexed(int indexCount, int startIndex, int baseVertexIndex)
        {
            throw new NotImplementedException();
        }

        public override void GenerateMips(Texture texture)
        {
            throw new NotImplementedException();
        }

        protected override IAInitialization GetIAInitialization()
        {
            throw new NotImplementedException();
        }

        protected override OMInitialization GetOMInitialization()
        {
            throw new NotImplementedException();
        }

        protected override RSInitialization GetRSInitialization()
        {
            throw new NotImplementedException();
        }

        protected override ShadingInitialization GetShadingInitialization()
        {
            throw new NotImplementedException();
        }

        protected override void IASetIndexBufferImpl(GraphicBuffer indexBuffer, int offset)
        {
            throw new NotImplementedException();
        }

        protected override void IASetInputLayout(InputLayout value)
        {
            throw new NotImplementedException();
        }

        protected override void IASetPrimitiveType(IAPrimitive value)
        {
            throw new NotImplementedException();
        }

        protected override void IASetVertexBufferImpl(int slot, GraphicBuffer vertexBuffer, int offset, int stride)
        {
            throw new NotImplementedException();
        }

        protected override void OMSetBlendState(BlendState state)
        {
            throw new NotImplementedException();
        }

        protected override void OMSetDepthStencilState(DepthStencilState state)
        {
            throw new NotImplementedException();
        }

        protected override void OMSetRenderTargetImpl(RenderTarget renderTarget, DepthStencil dephtStencil)
        {
            throw new NotImplementedException();
        }

        protected override void OMSetRenderTargetsImp(int numTargets, RenderTarget[] renderTargets, DepthStencil dephtStencil)
        {
            throw new NotImplementedException();
        }

        protected override void RSSetScissorRects(Rectangle rec)
        {
            throw new NotImplementedException();
        }

        protected override void RSSetState(RasterizerState value)
        {
            throw new NotImplementedException();
        }

        protected override void RSSetViewPort(ViewPort vp)
        {
            throw new NotImplementedException();
        }

        public override void ResizeBackBuffer(int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void ResolveSubresource(Texture src, int srcSub, Texture desc, int destSub)
        {
            throw new NotImplementedException();
        }

        protected override void SetProgram(ShaderProgram program)
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffer(GraphicBuffer buffer, int offset, IntPtr pterData, int dataSize)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateSubResource(Texture dest, int subResource, DataBox* box, void* srcPointer, int srcRowPith, int srcDepthPitch)
        {
            throw new NotImplementedException();
        }
    }
}

