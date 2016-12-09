using Igneel.Graphics;
using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
    public class PixelClippingBinding : RenderBinding<PixelClipping, PixelClippingMap>
    {
       
        public static DepthStencilState DepthStateNoWrite;      

        public PixelClippingBinding()
        {
            if (DepthStateNoWrite == null)
                DepthStateNoWrite = GraphicDeviceFactory.Device.CreateDepthStencilState(new DepthStencilStateDesc(true)
                {
                    WriteEnable = false
                });
        }

        public override void OnBind(PixelClipping value)
        {
            var effect = Effect;

            if (Mapping == null) return;

            if (value == PixelClipping.Opaque)
            {
               GraphicDeviceFactory.Device.DepthTest = DepthStateNoWrite;
                Mapping.NoRenderOpaque = true;
                Mapping.NoRenderTransparency = false;

            }
            else if (value == PixelClipping.Transparent)
            {
                GraphicDeviceFactory.Device.DepthTest = DefaultTechnique.DephtState;

                Mapping.NoRenderOpaque = false;
                Mapping.NoRenderTransparency = true;
            }
            else
            {
                GraphicDeviceFactory.Device.DepthTest = DefaultTechnique.DephtState;
                Mapping.NoRenderOpaque = false;
                Mapping.NoRenderTransparency = false;
            }
        }

        public override void OnUnBind(PixelClipping value)
        {
            if (value == PixelClipping.Opaque)
               GraphicDeviceFactory.Device.DepthTest = DefaultTechnique.DephtState;
        }
    }
}
