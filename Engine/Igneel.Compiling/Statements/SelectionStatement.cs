using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Statements
{
    public class SelectionStatement : Statement
    {
        public AttributeDeclaration Attribute { get; set; }

        public SelectionStatement(Expression condition, AstNode onTrue)
        {
            Condition = condition;
            OnTrue = onTrue;
            Line = condition.Line;
            Column = condition.Column;
        }

        public SelectionStatement(Expression condition, AstNode onTrue, AstNode onFalse)
            :this(condition, onTrue)
        {
            OnFalse = onFalse;
        }

        public Expression Condition { get; set; }

        public AstNode OnTrue { get; set; }

        public AstNode OnFalse { get; set; }


        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Condition.CheckSemantic(scope, log);
            if (!Condition.Type.Match(ShaderRuntime.Boolean))
            {
                log.Error("The condition is not or cant be converted to bool", Line, Column);
            }
            OnTrue.CheckSemantic(scope, log);
            if (OnFalse != null)
                OnFalse.CheckSemantic(scope, log);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            if (Attribute != null)
            {
                Attribute.GenerateCode(sb, tabsOffset);
            }
            sb.Append('\t', tabsOffset);           
            sb.Append("if("); Condition.GenerateCode(sb, 0); sb.Append(')'); sb.AppendLine();

            int tabs = (OnTrue is BlockStatement) ? tabsOffset : tabsOffset + 1;
            OnTrue.GenerateCode(sb, tabs);
            sb.AppendLine();

            if (OnFalse != null)
            {
                sb.Append('\t', tabsOffset);
                sb.Append("else");

                tabs = (OnFalse is BlockStatement) ? tabsOffset : tabsOffset + 1;
                if (OnFalse is SelectionStatement)
                {                 
                    sb.Append(' ');
                }
                else
                {                  
                    sb.AppendLine();
                }
                OnFalse.GenerateCode(sb, tabs);
            }
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield return Condition;
            yield return OnTrue;
            if (OnFalse != null)
                yield return OnFalse;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Attribute != null)
            {
                sb.Append(Attribute);
                sb.AppendLine();
            }
            sb.Append("if(" + Condition + ")");
            sb.AppendLine();
            sb.Append(OnTrue);           
            sb.AppendLine();
            if (OnFalse != null)
            {
                sb.Append("else");
                sb.AppendLine();
                sb.Append(OnFalse);
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
