using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public class AttributeDeclaration : Declaration
    {
        public Expression[] Arguments { get; set; }

        public override void AddToScope(Scope scope)
        {
            
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append("[" + Name);
            if (Arguments != null && Arguments.Length > 0)
            {
                sb.Append("(");
                sb.Append(Arguments.Skip(1).Aggregate(Arguments[0].ToString(), (s, x) => s + ", " + x.ToString()));
                sb.Append(")");
            }
            sb.Append("]");
            sb.AppendLine();
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            return Arguments;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[" + Name);
            if (Arguments != null && Arguments.Length > 0)
            {
                sb.Append("(");
                sb.Append(Arguments.Skip(1).Aggregate(Arguments[0].ToString(), (s, x) => s + ", " + x.ToString()));
                sb.Append(")");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
