using Igneel.Components;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class HeightFieldRegistrator : Registrator<HeightField, HeightFieldRegistrator>
    {
        public override void RegisterRenders()
        {
            Register<SceneTechnique, HeightFieldRender<HeightFieldEffect>>(() =>
                {
                    var render = new HeightFieldRender<HeightFieldEffect>();                    
                    render.BindWith(new HeightFieldMaterialBind(render));
                    render.BindWith(new HeightFieldBind(render));

                    return render;
                });
        }
    }
}
