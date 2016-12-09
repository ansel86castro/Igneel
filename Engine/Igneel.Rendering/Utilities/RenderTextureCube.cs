using System;
using Igneel.Graphics;

namespace Igneel.Rendering
{
    public class RenderTextureCube : ResourceAllocator
    {      
        Texture2D _texture;
        RenderTarget[] _targets = new RenderTarget[6];
        DepthStencil _depthStencil;           
       
        public RenderTextureCube(int edgeSize, Format targetFormat, Format depthFormat, Multisampling sampling = default(Multisampling))
        {
            if (sampling.Count == 0)
                sampling.Count = 1;
            var device = GraphicDeviceFactory.Device;
            _texture = device.CreateTexture2D(new Texture2DDesc 
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
                int index =  Graphics.Texture.CalcSubresource(0, i, 1);
                _targets[i] = device.CreateRenderTarget(_texture ,index);
            }

            if (depthFormat != Format.UNKNOWN)
                _depthStencil = device.CreateDepthStencil(
                    new DepthStencilDesc
                    {
                        Width = edgeSize,
                        Height = edgeSize,
                        Format = depthFormat,
                        Sampling = sampling,                      
                        Dimension = DepthStencilDimension.TEXTURECUBE
                    });
        }

        public Texture2D Texture { get { return _texture; } }

        public RenderTarget[] Target { get { return _targets; } }

        public DepthStencil DephtBuffer { get { return _depthStencil; } }

        public int EdgeSize { get { return _texture.Width; } }

        public Format TargetFormat { get { return _texture.Description.Format; } }

        public Format DepthFormat { get { return _depthStencil != null ? _depthStencil.SurfaceFormat : Format.UNKNOWN; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _texture.Dispose();
                Array.ForEach(_targets, x => x.Dispose());                
                _depthStencil.Dispose();
            }
            base.OnDispose(disposing);
        }

        public void SetTarget(int face, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            if (_depthStencil != null)
                device.SetRenderTarget(_targets[face], _depthStencil);
            else
                device.SetRenderTarget(_targets[face]);
        }

        public void SetTexture(int slot = 0, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            device.PS.SetResource(slot, _texture);
        }
    }
}