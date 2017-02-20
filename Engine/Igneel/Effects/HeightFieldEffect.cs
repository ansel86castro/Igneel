using System;
using Igneel.Rendering;
using Igneel.Graphics;
using Igneel.Rendering.Bindings;
using Igneel.Components.Terrain;

namespace Igneel.Effects
{
    public class HeightFieldEffect:Effect
    {
        public HeightFieldEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]
             {
                Tech("tech0")
                    .Pass<HeightFieldVertex>("TerrainPhongVS", "TerrainPhongPS")           
             };
        }

        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new LightBinding())
                  .BindWith(new AmbientLightBinding())
                  .BindWith(new SceneNodeBinding())
                  .BindWith(new LayeredMaterialBinding())
                  .BindWith(new HeightFieldSectionBinding())
                  .BindWith(new PlaneReflectionBinding())
                  .BindWith(new ClipSceneTechniqueBinding())
                  .BindWith(new PixelClippingBinding());
        }
    }
}
