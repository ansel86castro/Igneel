using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.SceneManagement
{
    public interface ICullable : IBoundable
    {
        ICullRegion CullRegion { get; set; }
    }
}
