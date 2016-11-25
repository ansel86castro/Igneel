using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.GLSL
{
    public abstract class AstRewriter
    {
         public abstract AstNode Transform(AstNode node);
    }
}
