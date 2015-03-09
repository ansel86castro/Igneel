using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public class VariableInitializer:Expression
    {
        public Expression[] Expressions { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            foreach (var item in Expressions)
            {
                item.CheckSemantic(scope, log);
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append("{");
            Expressions[0].GenerateCode(sb,0);

            for (int i = 1; i < Expressions.Length; i++)
            {
                sb.Append(", ");
                Expressions[i].GenerateCode(sb, 0);
            }

            sb.Append("}");
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            return Expressions;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append(Expressions[0]);
            for (int i = 1; i < Expressions.Length; i++)
            {
                sb.AppendFormat(", {0}", Expressions[i]);
            }

            sb.Append("}");
            return sb.ToString();

        }
    }
}
