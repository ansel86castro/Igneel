using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class PixelClippingBinding : RenderBinding<PixelClipping, PixelClippingBinding.PixelClippingMap>
    {
        public interface PixelClippingMap
        {
            bool fNoRenderTransparency { get; set; }
            bool NoRenderOpaque { get; set; }
        }

        public static DepthStencilState depthStateNoWrite;      

        public PixelClippingBinding()
        {
            if (depthStateNoWrite == null)
                depthStateNoWrite = Engine.Graphics.CreateDepthStencilState(new DepthStencilStateDesc(true)
                {
                    WriteEnable = false
                });
        }

        public override void OnBind(PixelClipping value)
        {
            var effect = Effect;

            if (mapping == null) return;

            if (value == PixelClipping.Opaque)
            {
                Engine.Graphics.DepthTest = depthStateNoWrite;
                mapping.NoRenderOpaque = true;
                mapping.fNoRenderTransparency = false;

            }
            else if (value == PixelClipping.Transparent)
            {
                Engine.Graphics.DepthTest = SceneTechnique.DephtState;

                mapping.NoRenderOpaque = false;
                mapping.fNoRenderTransparency = true;
            }
            else
            {
                Engine.Graphics.DepthTest = SceneTechnique.DephtState;
                mapping.NoRenderOpaque = false;
                mapping.fNoRenderTransparency = false;
            }
        }

        public override void OnUnBind(PixelClipping value)
        {
            if (value == PixelClipping.Opaque)
                Engine.Graphics.DepthTest = SceneTechnique.DephtState;
        }
    }
}
