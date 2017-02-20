using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;

namespace Igneel.Components
{
    public class TransparencyChecker
    {
        RenderTexture2D _renderTexture;
        Sprite _sprite;
        Effect _effect;
        SamplerState _samState;
        private BlendState blend;

        public TransparencyChecker()
        {
            _renderTexture = new RenderTexture2D(1, 1, Format.R8G8B8A8_UNORM, Format.UNKNOWN, Multisampling.Disable, true);

            _effect = Effect.GetEffect<CheckTransparenceEffect>(GraphicDeviceFactory.Device) ;
            _sprite = Service.Require<Sprite>();

            _samState = GraphicDeviceFactory.Device.CreateSamplerState(new SamplerDesc(
                addressU:TextureAddressMode.Clamp, 
                addressV: TextureAddressMode.Clamp, 
                filter: Filter.MinMagMipPoint));

            blend = Engine.Graphics.CreateBlendState(new BlendDesc(
                blendEnable:false,
                srcBlend:Blend.One,
                destBlend:Blend.One,
                 blendOp:BlendOperation.Add,
                 srcBlendAlpha:Blend.One,
                 destBlendAlpha:Blend.One,
                 blendOpAlpha:BlendOperation.Add));
        }

        public bool IsTransparent(Texture2D texture)
        {
            var device = GraphicDeviceFactory.Device;

            var oldvp = device.ViewPort;
            device.ViewPort = new ViewPort(0, 0, 1, 1);

            device.SaveRenderTarget();
            _renderTexture.SetTarget();

            device.PS.SetResource(0, texture);
            device.PS.SetSampler(0, _samState);

            _sprite.Begin();

            var deviceBlend = device.Blend;
            device.Blend = blend;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);

            _sprite.SetFullScreenTransform(_effect);
            _sprite.DrawQuad(_effect);

            _sprite.End();

            device.RestoreRenderTarget();
            device.ViewPort = oldvp;
            device.Blend = deviceBlend;

            byte alpha = 0;
            var rec = _renderTexture.Texture.Map(0, MapType.Read);
            unsafe
            {
                alpha = *(byte*)rec.DataPointer;                
            }
            _renderTexture.Texture.UnMap(0);

            return alpha > 0;

        }
    }
}
