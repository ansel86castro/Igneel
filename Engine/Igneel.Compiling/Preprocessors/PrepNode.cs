using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Preprocessors
{
    public abstract class PrepNode
    {
        public PrepNode()
        {
            NewLine = "";
        }

        public int Line { get; set; }

        public int Column { get; set; }

        public string NewLine { get; set; }

        public abstract string GetContent(Preprocessor p);
    }
}
