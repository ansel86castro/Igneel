using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;

namespace Igneel.SceneManagement
{
    public interface IComponentInstance: IGraphicsProvider
    {        
        Frame Node { get; }
    }

}
