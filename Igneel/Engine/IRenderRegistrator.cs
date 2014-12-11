using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class RegistratorAttribute : Attribute
    {

    }

    /// <summary>
    /// Register Render for Techniques
    /// </summary>
    public interface IRenderRegistrator
    {
        void RegisterRenders();

        void RegisterInstance();
    }

    public interface IRenderRegistrator<T> : IRenderRegistrator
    {

    }

    public static class RenderRegistrator
    {
        
    }
}
