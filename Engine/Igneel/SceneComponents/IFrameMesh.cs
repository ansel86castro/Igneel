using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Components;

namespace Igneel.SceneComponents
{
    public interface IFrameMesh : IGraphicObject
    {
        Mesh Mesh { get; }
    }
}
