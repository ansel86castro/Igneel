using Igneel.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Rendering
{
    public class RenderTexture2D : ResourceAllocator
    {      
        Texture2D _texture;      
        RenderTarget _target;
        DepthStencil _depthStencil;      
      
        public RenderTexture2D(int width, int height, Format targetFormat, Format depthFormat = Format.UNKNOWN,  Multisampling sampling = default(Multisampling), bool readable = false)
        {
            if (sampling.Count == 0)
                sampling.Count = 1;
            var device = GraphicDeviceFactory.Device;
            _texture = device.CreateTexture2D(new Texture2DDesc { 
                Width = width, 
                Height = height, 
                Format = targetFormat, 
                SamplerDesc = sampling, 
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource, 
                ArraySize  =1,
                MipLevels = 1,
                Options = ResourceOptionFlags.None,
                Usage =  ResourceUsage.Default,
                CPUAccessFlags= readable? CpuAccessFlags.Read: CpuAccessFlags.None });

            _target = device.CreateRenderTarget(_texture);

            if (depthFormat != Format.UNKNOWN)
                _depthStencil = device.CreateDepthStencil(new DepthStencilDesc(width, height, depthFormat, sampling, false));
        }

        public Texture2D Texture { get { return _texture; } }

        public RenderTarget Target { get { return _target; } }

        public DepthStencil DephtBuffer { get { return _depthStencil; } }

        public int Width { get { return _texture.Width; } }

        public int Height { get { return _texture.Height; } }

        public Format TargetFormat { get { return _texture.Format; } }

        public Format DepthFormat { get { return _depthStencil != null ? _depthStencil.SurfaceFormat : Format.UNKNOWN; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {                             
                if (_depthStencil != null)
                    _depthStencil.Dispose();

                _target.Dispose();
                _texture.Dispose();
            }
            base.OnDispose(disposing);
        }

        public void SetTarget(GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            if (_depthStencil != null)
                device.SetRenderTarget(_target, _depthStencil);
            else
                device.SetRenderTarget(_target);
        }

        public void SetTexture(int slot = 0, GraphicDevice device = null)
        {
            if (device == null) device = GraphicDeviceFactory.Device;

            device.PS.SetResource(slot, _texture);
        }

        public MappedTexture2D Map()
        {
            return _texture.Map(0, MapType.Read);
        }

        public void UnMap()
        {
            _texture.UnMap(0);
        }
    }
}
