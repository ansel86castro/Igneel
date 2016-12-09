using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace Igneel.Rendering.Pendings
{
    public class HeightFieldRegistrator : Registrator<HeightField, HeightFieldRegistrator>
    {
        public override void RegisterRenders()
        {
            Register<DefaultTechnique, HeightFieldRender<HeightFieldEffect>>(() =>
                {
                    var render = new HeightFieldRender<HeightFieldEffect>();                    
                    render.BindWith(new HeightFieldMaterialBind(render));
                    render.BindWith(new HeightFieldBind(render));

                    return render;
                });
        }
    }
}
