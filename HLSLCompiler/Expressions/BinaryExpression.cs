using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public abstract class BinaryExpression:Expression
    {
        protected BinaryExpression()
        {

        }
        protected BinaryExpression(Expression left, Expression rigth, BinaryOperator op)
        {
            Left = left;
            Right = rigth;
            Operator = op;
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public BinaryOperator Operator { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Left.CheckSemantic(scope, log);
            Right.CheckSemantic(scope, log);

            var op = scope.GetFunction(StringConverter.GetString(Operator),Left.Type, Right.Type);
            if (op == null)
            {
                log.Error(string.Format("Operator not found for '{0}' and '{1}'", Left.Type, Right.Type), Line, Column);
                Type = ShaderRuntime.Unknow;
            }
            else
                Type = op.ReturnType;         
        } 

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Left.GenerateCode(sb, tabsOffset);
            sb.Append(" ");
            sb.Append(StringConverter.GetString(Operator));
            sb.Append(" ");
            Right.GenerateCode(sb, 0);
        }      

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Left);
            sb.AppendFormat(" {0} ", StringConverter.GetString(Operator));
            sb.Append(Right);           
            return sb.ToString();
        }

        public static BinaryExpression AndExpression(Expression left, Expression right)
        {
            return new BinaryLogical { Left = left, Right = right, Operator = BinaryOperator.And };
        }

        public static BinaryExpression OrExpression(Expression left, Expression right)
        {
            return new BinaryLogical { Left = left, Right = right, Operator = BinaryOperator.Or  };
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Left;
            yield return Right;
        }
    }

    public class BinaryLogical : BinaryExpression
    {
        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Left.CheckSemantic(scope, log);
            Right.CheckSemantic(scope, log);
            if (Left.Type == null || Right.Type == null)
            {
                log.Error("Type not found", Line, Column);
                Type = ShaderRuntime.Unknow;
                return;
            }

            if (Left.Type.ReflectionType.Type != Graphics.ShaderType.Bool ||
               Right.Type.ReflectionType.Type != Graphics.ShaderType.Bool)
            {
                log.Error("Expression type not allowed", Line, Column);
                Type = ShaderRuntime.Unknow;
                return;
            }

            Type = Math.Max(Left.Type.ReflectionType.Columns, Right.Type.ReflectionType.Columns) == Left.Type.ReflectionType.Columns ?
                Left.Type :
                Right.Type;

        }
    }

    public class BinaryRelational : BinaryExpression
    {
        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Left.CheckSemantic(scope, log);
            Right.CheckSemantic(scope, log);            

            Type = ShaderRuntime.Boolean;
        }
       
      
    }

    public class BinaryAritmetic : BinaryExpression
    {
        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Right.CheckSemantic(scope, log);
            Left.CheckSemantic(scope, log);            

            Type = Math.Max(Left.Type.ReflectionType.Columns, Right.Type.ReflectionType.Columns) == Left.Type.ReflectionType.Columns ?
             Left.Type :
             Right.Type;
        }
      
    }
}
