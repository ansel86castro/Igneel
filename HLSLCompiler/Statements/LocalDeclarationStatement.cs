using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Statements
{
    public class SingleLocalDeclarationStement : Statement
    {
        public LocalDeclaration Local { get; set; }

        public SingleLocalDeclarationStement(LocalDeclaration local)
        {
            Local = local;
            Column = local.Column;
            Line = local.Line;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Local.CheckSemantic(scope, log);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Local.GenerateCode(sb, tabsOffset);
            sb.Append(';');
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Local;
        }
    }

    public class MultipleLocalDeclarationStatement : Statement
    {
        public MultipleLocalDeclarationStatement(LocalDeclaration[] locals)
        {
            Locals = locals;
        }

        public LocalDeclaration[] Locals { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            if (Locals != null && Locals.Length > 0)
            {
                //Locals[0].Storage = Storage;
                //Locals[0].CheckSemantic(scope, log);
                foreach (var item in Locals)
                {               
                    item.CheckSemantic(scope, log);
                }
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Locals[0].GenerateCode(sb, tabsOffset);
            foreach (var item in Locals.Skip(1))
            {
                sb.Append(", ");

                //item.Type.AppendDeclaration(sb, 0);
                sb.Append(" " + item.Name);
                if (item.TypeInfo.Elements > 0)
                {
                    sb.AppendFormat("[{0}]", item.TypeInfo.Elements);
                }

                if (item.Initializer != null)
                {
                    sb.Append(" = ");
                    item.Initializer.GenerateCode(sb, 0);
                }

            }
            sb.Append(';');
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            if (Locals != null)
            {
                foreach (var item in Locals)
                {
                    yield return item;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();         
            for (int i = 0; i < Locals.Length; i++)
            {
                sb.Append(Locals[i]);
            }
            return sb.ToString();
        }
    }
}
