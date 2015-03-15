﻿using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Igneel.Rendering;
using Igneel.Rendering;
using Igneel.Scenering.Effects;

namespace Igneel.Scenering
{
    public class ScreenDecal:GraphicObject<ScreenDecal>, INameable
    {
        Texture2D texture;

        public ScreenDecal(string name)            
        {
            this.Name = name;
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }      

        public string Name { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }

    public class ScreenDecalRender : GraphicObjectRender<RenderQuadEffect, ScreenDecal>
    {
        Sprite.IShaderInput input;
        public ScreenDecalRender()
        {
            input = Effect.Map<Sprite.IShaderInput>(true);
        }
        public override void Draw(ScreenDecal component)
        {
            var g = GraphicDeviceFactory.Device;
            Sprite sprite = Service.Require<Sprite>();

            g.PS.SetResource(0, component.Texture);
            g.PS.SetSampler(0, SamplerState.Linear);

            sprite.Begin();

            sprite.SetFullScreenTransform(input, Matrix.Identity);
            //sprite.SetTrasform(input, new Rectangle(0, 0, g.OMBackBuffer.Width , g.OMBackBuffer.Height ), Matrix.Identity);

            sprite.DrawQuad(Effect);

            sprite.End();
        }
    }

    [Registrator]
    public class ScreenDecalRegistrator : Registrator<ScreenDecal>
    {
        public override void RegisterRenders()
        {
            Register<SceneTechnique, ScreenDecalRender>();
            Register<SceneShadowMapping, ScreenDecalRender>();
        }
    }
}