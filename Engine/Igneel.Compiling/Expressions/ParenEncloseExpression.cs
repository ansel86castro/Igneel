using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public class ParenEncloseExpression:Expression
    {
        public Expression Value { get; set; }

        public ParenEncloseExpression(Expression value, int line, int column)
        {
            this.Value = value;
            Line = line;
            Column = column;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Value.CheckSemantic(scope, log);
            Type = Value.Type;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append('(');
            Value.GenerateCode(sb, 0);
            sb.Append(')');
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return "(" + Value + ")";
        }
    }
}
