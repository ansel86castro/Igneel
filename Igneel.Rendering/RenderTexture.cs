using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Rendering
{
    public class RenderTexture2D : ResourceAllocator
    {      
        Texture2D texture;      
        RenderTarget target;
        DepthStencil depthStencil;      
      
        public RenderTexture2D(int width, int height, Format targetFormat, Format depthFormat = Format.UNKNOWN,  Multisampling sampling = default(Multisampling), bool readable = false)
        {
            if (sampling.Count == 0)
                sampling.Count = 1;
            var device = GraphicDeviceFactory.Device;
            texture = device.CreateTexture2D(new Texture2DDesc { 
                Width = width, 
                Height = height, 
                Format = targetFormat, 
                SamplerDesc = sampling, 
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource, 
                ArraySize  =1,
                MipLevels = 1,
                Options = ResourceOptionFlags.None,
                Usage =  ResourceUsage.Default,
                CPUAccessFlags = readable? CpuAccessFlags.Read: CpuAccessFlags.None });

            target = device.CreateRenderTarget(texture);

            if (depthFormat != Format.UNKNOWN)
                depthStencil = device.CreateDepthStencil(new DepthStencilDesc(width, height, depthFormat, sampling, false));
        }

        public Texture2D Texture { get { return texture; } }

        public RenderTarget Target { get { return target; } }

        public DepthStencil DephtBuffer { get { return depthStencil; } }

        public int Width { get { return texture.Width; } }

        public int Height { get { return texture.Height; } }

        public Format TargetFormat { get { return texture.Format; } }

        public Format DepthFormat { get { return depthStencil != null ? depthStencil.SurfaceFormat : Format.UNKNOWN; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {                             
                if (depthStencil != null)
                    depthStencil.Dispose();

                target.Dispose();
                texture.Dispose();
            }
            base.OnDispose(disposing);
        }

        public void SetTarget(GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            if (depthStencil != null)
                device.SetRenderTarget(target, depthStencil);
            else
                device.SetRenderTarget(target);
        }

        public void SetTexture(int slot = 0, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            device.PS.SetResource(slot, texture);
        }

        public MappedTexture2D Map()
        {
            return texture.Map(0, MapType.Read);
        }

        public void UnMap()
        {
            texture.UnMap(0);
        }
    }

    public class RenderTextureCube : ResourceAllocator
    {      
        Texture2D texture;
        RenderTarget[] targets = new RenderTarget[6];
        DepthStencil depthStencil;           
       
        public RenderTextureCube(int edgeSize, Format targetFormat, Format depthFormat, Multisampling sampling = default(Multisampling))
        {
            if (sampling.Count == 0)
                sampling.Count = 1;
            var device = GraphicDeviceFactory.Device;
            texture = device.CreateTexture2D(new Texture2DDesc 
            { 
                ArraySize = 6,
                Height = edgeSize,
                Width = edgeSize, 
                Format = targetFormat, 
                SamplerDesc = sampling, 
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource ,
                Usage = ResourceUsage.Default,
                MipLevels = 1,
                CPUAccessFlags = CpuAccessFlags.None,                    
                Options  = ResourceOptionFlags.TextureCube
            });

            for (int i = 0; i < 6; i++)
            {
                int index = TextureBase.CalcSubresource(0, i, 1);
                targets[i] = device.CreateRenderTarget(texture ,index);
            }

            if (depthFormat != Format.UNKNOWN)
                depthStencil = device.CreateDepthStencil(
                   new DepthStencilDesc
                   {
                       Width = edgeSize,
                       Height = edgeSize,
                       Format = depthFormat,
                       Sampling = sampling,                      
                       Dimension = DepthStencilDimension.TEXTURECUBE
                   });
        }

        public Texture2D Texture { get { return texture; } }

        public RenderTarget[] Target { get { return targets; } }

        public DepthStencil DephtBuffer { get { return depthStencil; } }

        public int EdgeSize { get { return texture.Width; } }

        public Format TargetFormat { get { return texture.Description.Format; } }

        public Format DepthFormat { get { return depthStencil != null ? depthStencil.SurfaceFormat : Format.UNKNOWN; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                texture.Dispose();
                Array.ForEach(targets, x => x.Dispose());                
                depthStencil.Dispose();
            }
            base.OnDispose(disposing);
        }

        public void SetTarget(int face, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            if (depthStencil != null)
                device.SetRenderTarget(targets[face], depthStencil);
            else
                device.SetRenderTarget(targets[face]);
        }

        public void SetTexture(int slot = 0, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            device.PS.SetResource(slot, texture);
        }
    }

    public static class GraphicDeviceExtensor
    {
        //public static void OMSetRenderTarget(this GraphicDevice device, RenderTexture2D renderTexture)
        //{
        //    if (renderTexture.DephtBuffer != null)
        //    {
        //        device.OMSetRenderTarget(renderTexture.Target, renderTexture.DephtBuffer);
        //    }
        //    else
        //        device.OMSetRenderTarget(renderTexture.Target);
        //}
    }
}
