using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Preprocessors
{
    public class Define : PrepNode
    {
        public string Simbol { get; set; }

        public string Value { get; set; }

        public override string GetContent(Preprocessor p)
        {
            if (p != null)
                p.AddMacro(Simbol, Value);
            return "\n";
        }
    }
}
