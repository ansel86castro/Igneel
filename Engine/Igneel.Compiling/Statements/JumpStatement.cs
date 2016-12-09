using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Statements
{
    public class JumpStatement : Statement
    {
        public JumpType Jump { get; set; }

        public Expression ReturnExp { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            if (Jump == JumpType.Return)
            {
                if (ReturnExp != null)
                {
                    ReturnExp.CheckSemantic(scope, log);
                    var function = scope.CurrentFunction;
                    if (!ReturnExp.Type.Match(function.ReturnType))
                        log.Error("Function return's type do not match", Line, Column);
                }
                else if(scope.CurrentFunction.ReturnType != ShaderRuntime.Void)
                    log.Error("Function must return void", Line, Column);
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(StringConverter.GetString(Jump));
            if (ReturnExp != null)
            {                
                ReturnExp.GenerateCode(sb, 1);
            }
            sb.Append(';');
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield return ReturnExp;
        }

        public override string ToString()
        {

            return ReturnExp != null ? String.Format("{0} {1}", StringConverter.GetString(Jump), ReturnExp) :
                StringConverter.GetString(Jump);
        }

       
    }
}
