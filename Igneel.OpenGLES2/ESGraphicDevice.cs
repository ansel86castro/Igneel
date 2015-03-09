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
    }
}

