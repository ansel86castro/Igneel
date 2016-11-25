using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.SceneComponents
{
    public interface IMaterialContainer : IGraphicObject
    {
        BasicMaterial[] Materials { get; set; }

        int[] TransparentMaterials { get; }
    }
}
