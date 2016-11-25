using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Inference.Compilation
{
    public abstract class ASTNode
    {
        public int Line;
        public int Column;
        public Type Type { get; set; }  

        public abstract void CheckSemantic(ICompilationLog log, CompilationScope scope);
    }

    public abstract class Expresion :ASTNode
    {
        public object Result;      
    }    

  
}
