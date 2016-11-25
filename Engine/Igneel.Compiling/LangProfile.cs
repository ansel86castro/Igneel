using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public enum Language
    {
        Hlsl,Glsl
    }
    public class LangProfile
    {
        public static LangProfile Target { get; set; }

        public Language Lang { get; private set; }

        public int Version { get; private set; }
    }
}
