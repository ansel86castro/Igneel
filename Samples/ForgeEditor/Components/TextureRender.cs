using Igneel;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeEditor.Components
{
    class TextureRender
    {
        Effect untranformed;
        private Sprite.IShaderInput input;
        private GraphicDevice device;

        public TextureRender(GraphicDevice device)
        {
            this.device = device;
        }

        public void RenderTexture(Texture2D texture, int x = 0, int y = 0, int width = 256, int height = 256)
        {
            if (untranformed == null)
            {
                untranformed = Igneel.Rendering.Effect.GetEffect<RenderQuadEffect>(GraphicDeviceFactory.Device);
                input = untranformed.Map<Igneel.Rendering.Sprite.IShaderInput>();
            }

            var sprite = Service.Require<Sprite>();
            device.PS.SetResource(0, texture);
            device.PS.SetSampler(0, SamplerState.Linear);

            sprite.Begin();
            sprite.SetTrasform(input, new Igneel.Rectangle(x, y, width, height), Igneel.Matrix.Identity);
            sprite.DrawQuad(untranformed);
            sprite.End();

            device.PS.SetResource(0, null);
        }
    }
}
