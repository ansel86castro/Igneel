using Igneel.Assets;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Techniques;

namespace Igneel.SceneComponents
{
    [Asset("FRAME_OBJECT")]
    public class ScreenDecal:GraphicObject<ScreenDecal>
    {
        Texture2D _texture;

        public ScreenDecal(string name)            
        {
            this.Name = name;
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }                  
    }

    public class ScreenDecalRender : GraphicRender<ScreenDecal, RenderQuadEffect>
    {
        Sprite.IShaderInput _input;

        public ScreenDecalRender()
        {
            _input = Effect.Map<Sprite.IShaderInput>(true);
        }

        public override void Draw(ScreenDecal component)
        {
            var g = GraphicDeviceFactory.Device;
            Sprite sprite = Service.Require<Sprite>();

            g.PS.SetResource(0, component.Texture);
            g.PS.SetSampler(0, SamplerState.Linear);

            sprite.Begin();

            sprite.SetFullScreenTransform(_input, Matrix.Identity);
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
            Register<DefaultTechnique, ScreenDecalRender>();
            Register<SceneShadowMapping, ScreenDecalRender>();
        }
    }
}
