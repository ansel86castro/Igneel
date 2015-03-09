using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class StdFunctionDeclaration : FunctionDeclaration
    {

        public StdFunctionDeclaration(string name)
            : base(name)
        {
            IsBuildIn = true;
            IsChecked = true;
        }

        public StdFunctionDeclaration(string name, TypeDeclaration returnType, params ParameterDeclaration[] parameters)
            : base(name)
        {
            Parameters = parameters;
            ReturnType = returnType;
            IsBuildIn = true;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            throw new NotImplementedException();
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            throw new NotImplementedException();
        }
    }

    
}
