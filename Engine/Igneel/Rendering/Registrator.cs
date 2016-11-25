using Igneel.Effects;
using Igneel.Techniques;
using Igneel.Rendering;
using Igneel.SceneComponents;

namespace Igneel.Rendering
{
    [Registrator]
    public class PartSystemRegistrator : Registrator<ParticleSystem>
    {
        public override void RegisterRenders()
        {
            Register<DefaultTechnique, ParticleSystemRender<ParticleSystemEffect>>();
        }
    }
}
