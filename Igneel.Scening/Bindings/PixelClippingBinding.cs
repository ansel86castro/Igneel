﻿using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class PixelClippingBinding : RenderBinding<PixelClipping, PixelClippingMap>
    {
       
        public static DepthStencilState depthStateNoWrite;      

        public PixelClippingBinding()
        {
            if (depthStateNoWrite == null)
                depthStateNoWrite = GraphicDeviceFactory.Device.CreateDepthStencilState(new DepthStencilStateDesc(true)
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
               GraphicDeviceFactory.Device.DepthTest = depthStateNoWrite;
                mapping.NoRenderOpaque = true;
                mapping.NoRenderTransparency = false;

            }
            else if (value == PixelClipping.Transparent)
            {
                GraphicDeviceFactory.Device.DepthTest = SceneTechnique.DephtState;

                mapping.NoRenderOpaque = false;
                mapping.NoRenderTransparency = true;
            }
            else
            {
                GraphicDeviceFactory.Device.DepthTest = SceneTechnique.DephtState;
                mapping.NoRenderOpaque = false;
                mapping.NoRenderTransparency = false;
            }
        }

        public override void OnUnBind(PixelClipping value)
        {
            if (value == PixelClipping.Opaque)
               GraphicDeviceFactory.Device.DepthTest = SceneTechnique.DephtState;
        }
    }
}