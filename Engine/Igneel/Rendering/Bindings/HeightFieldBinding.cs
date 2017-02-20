using Igneel.Components.Terrain;
using Igneel.SceneComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public interface IHeightFieldMap
    {
        Vector2 Offset { get; set; }
    }

    public class HeightFieldSectionBinding : RenderBinding<HeightFieldSection, IHeightFieldMap>
    {
        public override void OnBind(HeightFieldSection value)
        {
            Mapping.Offset = value.Offset;
        }

        public override void OnUnBind(HeightFieldSection value)
        {
            Mapping.Offset = new Vector2();
        }
    }
}
