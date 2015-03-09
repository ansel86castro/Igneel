using Igneel.Compiling.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class UserFunctionDeclaration : FunctionDeclaration
    {
        public UserFunctionDeclaration()
        {
            IsBuildIn = false;
            IsChecked = false;
        }
    }
}
