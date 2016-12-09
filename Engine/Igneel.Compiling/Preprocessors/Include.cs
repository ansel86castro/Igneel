using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Preprocessors
{
    public class Include : PrepNode
    {
        public Include()
        {
            
        }
        public bool IsSystemPath { get; set; }

        public string Filename { get; set; }

        public override string GetContent(Preprocessor p)
        {
            if (p != null)
                p.AddInclude(Filename);

            return "\n";
        }
    }
}
