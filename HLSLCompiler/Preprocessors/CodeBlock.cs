using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Preprocessors
{
    public class CodeBlock:PrepNode
    {
        public string Content { get; set; }

        public override string GetContent(Preprocessor p)
        {
            return Content + NewLine;
            
        }
    }

    public class Comment : CodeBlock
    {
        public override string GetContent(Preprocessor p)
        {
            return Content + NewLine;
        }
    }
}
