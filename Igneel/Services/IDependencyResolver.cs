using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Services
{    
    public interface IDependencyResolver<out T> where T : class
    {
        IEnumerable<T> GetDependencies();
    }
}
