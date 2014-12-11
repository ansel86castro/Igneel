using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public class AdapterNotFoundException:Exception
    {
        public AdapterNotFoundException(int adapterIndex)
        {
            this.AdapterIndex = adapterIndex;
        }

        public int AdapterIndex { get; private set; }
    }

    public class ShaderCompileException : Exception
    {
        public ShaderCompileException(string message)
            : base(message)
        {

        }
    }
}
