using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Compiling.Statements;

namespace Igneel.Compiling.Statements
{
    public class BlockStatement : Statement
    {
        public Statement[] Statements { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {          
            foreach (var item in Statements)
            {
                item.CheckSemantic(scope, log);
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset); 
            sb.Append('{');
            sb.AppendLine();
            foreach (var item in Statements)
            {
                item.GenerateCode(sb, tabsOffset + 1);
                sb.AppendLine();
            }
            sb.Append('\t', tabsOffset);
            sb.Append("}");            

        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            return Statements;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            foreach (var item in Statements)
            {
                sb.Append(item.ToString());
                if (!(item is LoopStatement || item is SelectionStatement || item is BlockStatement))
                {
                    sb.Append(";");
                }
                sb.AppendLine();
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}
