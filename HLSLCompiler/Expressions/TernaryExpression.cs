using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public class TernaryExpression : Expression
    {
        public TernaryExpression(Expression condition, Expression onTrue, Expression onFalse)
        {
            Condition = condition;
            OnTrue = onTrue;
            OnFalse = onFalse;
        }
        public Expression Condition { get; set; }

        public Expression OnTrue { get; set; }

        public Expression OnFalse { get; set; }


        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Condition.CheckSemantic(scope, log);
            if (!Condition.Type.Match(ShaderRuntime.Boolean))
            {
                log.Error("The condition is not or cant be converted to bool", Line, Column);
            }
            OnTrue.CheckSemantic(scope, log);         
            OnFalse.CheckSemantic(scope, log);
            Type = OnTrue.Type;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
           
            Condition.GenerateCode(sb, tabsOffset);
            sb.Append('?');
            OnTrue.GenerateCode(sb, 0);
            sb.Append(':');
            OnFalse.GenerateCode(sb, 0);

        }

        public override string ToString()
        {
            return String.Format("{0}? {1} : {2}", Condition, OnTrue, OnFalse);
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Condition;
            yield return OnTrue;
            yield return OnFalse;
        }
    }
}
