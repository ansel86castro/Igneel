using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace Igneel.Rendering.Pendings
{
    [Registrator]
    public class HeightFieldRegistrator : Registrator<HeightField, HeightFieldRegistrator>
    {
        public override void RegisterRenders()
        {
            Register<DefaultTechnique, HeightFieldRender<HeightFieldEffect>>();
        }
    }
}
