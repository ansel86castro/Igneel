using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgeEditor.Effects;
using Igneel;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace ForgeEditor.Components.CoordinateSystems
{
    public class DecalGlyp:  GraphicObject<DecalGlyp>, IDecalGlyp
    {
        GlypComponent[] components;    
        RenderTexture2D renderTarget;
        Effect untranformed;
        Sprite.IShaderInput input;
        Igneel.Rectangle decal;
        DecalGlypHitTest hitTester;

        static bool init;

        public DecalGlyp()
            : this(new Igneel.Rectangle(0, 0, 128, 128))
        {

        }

        public DecalGlyp(Igneel.Rectangle screenRectangle)
        {
            IsDesignOnly = true;
            IsTransparent = false;
            decal = screenRectangle;
            
            renderTarget = new RenderTexture2D(decal.Width, decal.Height, Format.R8G8B8A8_UNORM_SRGB, Format.D24_UNORM_S8_UINT);
            hitTester = new DecalGlypHitTest(decal.Width, decal.Height, Format.D24_UNORM_S8_UINT, this);

            components = new GlypComponent[0];

            SetupRender();            
        }

        #region IDecalGlyp Members

        public GlypComponent[] Components { get { return components; } set { components = value; } }

        public Igneel.Rendering.RenderTexture2D RenderTarget
        {
            get { return renderTarget; }
        }

        public Igneel.Rectangle ScreenRectangle
        {
            get
            {
                return decal;
            }
            set
            {
                decal = value;

                hitTester.Resize(decal.Width, decal.Height);
                if (renderTarget != null)
                    renderTarget.Dispose();

                if (renderTarget.Width != decal.Width || renderTarget.Height != decal.Height)
                    renderTarget = new RenderTexture2D(decal.Width, decal.Height, Format.R8G8B8A8_UNORM_SRGB, Format.D24_UNORM_S8_UINT);
            }
        }

        public void Translate(int x, int y)
        {
            int halfWidth = decal.Width / 2;
            int halfHeight = decal.Height / 2;

            decal.X = Math.Max(x - halfWidth, 0);
            decal.Y = Math.Max(y - halfHeight, 0);
        }

        private static void SetupRender()
        {
            if (!init)
            {
                SetRender<DefaultTechnique, DecalGlypEffect>(new DecalGlypRender());
                SetNullRender<DepthSceneRender>();
                init = true;
            }
        }

        public void DrawDecal()
        {
            RenderTexture(Engine.Graphics, renderTarget.Texture, decal.X, decal.Y, decal.Width, decal.Height);
        }

        private void RenderTexture(GraphicDevice device, Texture2D texture, int x = 0, int y = 0, int width = 256, int height = 256)
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

        public GlypComponent DoHitTest(Vector2 screenLocation)
        {
            if (!decal.Contains(screenLocation))
                return null;

            screenLocation.X -= decal.X;
            screenLocation.Y -= decal.Y;

            hitTester.Mode = HitTestTechnique.HitTestMode.Single;
            hitTester.HitTestLocation = screenLocation;
            hitTester.Apply();

            var result = hitTester.HitTestResults.FirstOrDefault(x => x.Data != null);
            return result.Data as GlypComponent;
        }

        #endregion
    }
}
