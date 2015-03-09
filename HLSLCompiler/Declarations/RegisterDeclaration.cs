using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class RegisterDeclaration : ASTNode
    {
        public Expression Register { get; set; }

        public RegisterDeclaration(Expression reg, int line, int column)
        {
            Register = reg;
            Line = line;
            Column = column;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            throw new NotImplementedException();
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(":register(");
            Register.GenerateCode(sb, 0);
            sb.Append(")");
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Register;
        }

        public override string ToString()
        {
            return ":register(" + Register + ")";
        }
    }

    public class PackOffsetDeclaration : RegisterDeclaration
    {
        public PackOffsetDeclaration(Expression reg, int line, int column)
            :base(reg, line,column)
        {
          
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(":packoffset(");
            Register.GenerateCode(sb, 0);
            sb.Append(")");
        }     
    }
}
