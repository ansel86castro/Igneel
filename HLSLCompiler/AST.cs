using Igneel.Compiling.Declarations;
using Igneel.Compiling.Preprocessors;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public interface ISemanticChecker
    {
        void CheckSemantic(Scope scope, ErrorLog log);
    }
    public interface ISemanticCheker<T>
    {
        void CheckSemantic(Scope scope, ErrorLog log , T arg);
    }

    public interface ICodeGenerator<T>
    {
        void GenerateCode(T arg);
    }    

    public abstract class ASTNode
    {            
        public int Line { get; set; }

        public int Column { get; set; }        

        public abstract void CheckSemantic(Scope scope, ErrorLog log);

        public abstract void GenerateCode(StringBuilder sb, int tabsOffset);

        public abstract IEnumerable<ASTNode> GetNodes();
    }

    public class HLSLProgram
    {
        public Include[] Includes { get; set; }

        public Declaration[] Declarations { get; set; }

      
    }
}
