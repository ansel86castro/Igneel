using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class AdapterNotFoundException:Exception
    {
        public AdapterNotFoundException(int adapterIndex)
        {
            this.AdapterIndex = adapterIndex;
        }

        public int AdapterIndex { get; private set; }
    }

    public class ShaderCompilationException : Exception
    {
        public ShaderCompilationException(string message)
            : base(message)
        {

        }
    }
}
