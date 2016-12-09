using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Statements
{
    public abstract class LoopStatement : Statement
    {
        public AttributeDeclaration Attribute { get; set; }

        public Expression Condition { get; set; }

        public AstNode Body { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Condition.CheckSemantic(scope, log);
            if (!Condition.Type.Match(ShaderRuntime.Boolean))
            {
                log.Error("The type can not be converted to bool", Line, Column);
            }

            Body.CheckSemantic(scope, log);
        }        
    }
    

    public class ForStatement : LoopStatement
    {
        public AstNode[] Increment { get; set; }

        public AstNode[] Initializer { get; set; }        

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            var forScope = new Scope(scope);
            if (Initializer != null && Initializer.Length > 0)
            {
                foreach (var item in Initializer)
                {
                    item.CheckSemantic(forScope, log);
                }
            }

            if (Increment != null && Increment.Length > 0)
            {
                foreach (var item in Increment)
                {
                    item.CheckSemantic(forScope, log);
                }
            }
            base.CheckSemantic(forScope, log);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            if (Attribute != null)
            {
                Attribute.GenerateCode(sb, tabsOffset);            
            }
            sb.Append('\t', tabsOffset);
            sb.Append("for(");

            if (Initializer != null && Initializer.Length > 0)
            {
                Initializer[0].GenerateCode(sb, 0);                
                for (int i = 1; i < Initializer.Length; i++)
                {
                    sb.Append(", ");
                    Initializer[i].GenerateCode(sb, 0);
                }
            }
            sb.Append("; ");

            Condition.GenerateCode(sb, 0);

            sb.Append("; ");

            if (Increment != null && Increment.Length > 0)
            {
                Increment[0].GenerateCode(sb, 0);
                for (int i = 1; i < Increment.Length; i++)
                {
                    sb.Append(", ");
                    Increment[i].GenerateCode(sb, 0);
                }
            }

            sb.Append(")");
            sb.AppendLine();

            Body.GenerateCode(sb, tabsOffset);            
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            if (Initializer != null)
            {
                foreach (var item in Initializer)
                {
                    yield return item;   
                }                
            }
            yield return Condition;
            if (Increment != null)
            {
                foreach (var item in Increment)
                {
                    yield return item;
                }
            }
            yield return Body;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Attribute != null)
            {
                sb.Append(Attribute);
                sb.AppendLine();
            }
            sb.Append("for(");
            if (Initializer != null && Initializer.Length > 0)
            {
                sb.Append(Initializer[0]);
                for (int i = 1; i < Initializer.Length; i++)
                {
                    sb.AppendFormat(", {0}", Initializer[i]);
                }             
            }
            sb.Append(";");

            sb.Append(Condition);

            sb.Append(";");

            if (Increment != null &&Increment.Length > 0)
            {                
                sb.Append(Increment[0]);
                for (int i = 1; i < Increment.Length; i++)
                {
                    sb.AppendFormat(", {0}", Increment[i]);
                }
            }

            sb.Append(")");
            sb.AppendLine();

            sb.Append(Body);

            sb.AppendLine();

            return sb.ToString();
        }
    }


    public class WhileStatement : LoopStatement
    {     
        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append("while(");
            Condition.GenerateCode(sb, 0);
            sb.Append(")");
            sb.AppendLine();
            Body.GenerateCode(sb, tabsOffset);
        }

        public override IEnumerable<AstNode> GetNodes()
        {        
            yield return Condition;            
            yield return Body;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("while(");
            sb.Append(Condition);
            sb.Append(")");
            sb.AppendLine();
            sb.Append("{");

            sb.Append(Body);

            sb.Append("}");
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public class DoWhileStatement : LoopStatement
    {
        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append("do");
            sb.AppendLine();
            Body.GenerateCode(sb, tabsOffset);
            sb.AppendLine();
            sb.Append('\t', tabsOffset);
            sb.Append("while(");
            Condition.GenerateCode(sb, 0);
            sb.Append(");");
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield return Condition;
            yield return Body;
        }

        public override string ToString()
        {           
            return String.Format(
                @"do
                  {
                     {0}
                  } while ({1});",
                 Condition, Body);
        }
    }
}
