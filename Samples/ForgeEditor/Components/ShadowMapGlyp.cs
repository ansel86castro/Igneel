using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;

namespace ForgeEditor.Components
{
    public class ShadowMapGlyp : FrustumGlyp
    {
        private static TextureRender textureRender;
        private ShadowMapTechnique sm;

        public ShadowMapGlyp(Scene scene, ShadowMapTechnique sm) : base(scene, sm.Camera)
        {
            this.sm = sm;
        }

        public override void Draw(Effect effect)
        {
            base.Draw(effect);

            if (textureRender == null)
            {
                textureRender = new TextureRender(GraphicDeviceFactory.Device);            
            }

            textureRender.RenderTexture(sm.DepthTexture);
        }
    }
}
