using Igneel.Components;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Igneel.Rendering
{
    [Registrator]
    public class PartSystemRegistrator : Registrator<ParticleSystem>
    {
        public override void RegisterRenders()
        {
            Register<SceneTechnique, ParticleSystemRender<ParticleSystemEffect>>();
        }
    }
}
