using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{
    public class UnaryExpression:Expression
    {
        public UnaryOperator Operator { get; set; }

        public Expression Expression { get; set; }

        public UnaryExpression(Expression e, UnaryOperator op)
        {
            Expression = e;
            Operator = op;
            Line = e.Line;
            Column = e.Column;
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Expression.CheckSemantic(scope, log);
            Type = Expression.Type;

            if (Operator == UnaryOperator.Not && Expression.Type != ShaderRuntime.Boolean)
            {
                log.Error("operator not allowed", Line, Column);
            }
            else if (Operator == UnaryOperator.PostDec ||
                    Operator == UnaryOperator.PostInc ||
                    Operator == UnaryOperator.PreInc ||
                    Operator == UnaryOperator.PreDec)
            {
                var prim_type = Expression.Type as PrimitiveTypeDeclaration;
                if (prim_type == null)
                    log.Error("operator not allowed", Line, Column);
                else if(prim_type.ReflectionType.Class != Graphics.TypeClass.Matrix &&
                    prim_type.ReflectionType.Class != Graphics.TypeClass.Vector &&
                    prim_type.ReflectionType.Class != Graphics.TypeClass.Scalar)
                {
                    log.Error("operator not allowed", Line, Column);
                }
                else if(prim_type == ShaderRuntime.Boolean)
                    log.Error("operator not allowed", Line, Column);
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);            
            if(Operator == UnaryOperator.Not || Operator == UnaryOperator.PreDec || Operator == UnaryOperator.PreInc || Operator == UnaryOperator.Neg )
                sb.Append(StringConverter.GetString(Operator));

            Expression.GenerateCode(sb, 0);

            if(Operator == UnaryOperator.PostInc || Operator == UnaryOperator.PostDec)
                sb.Append(StringConverter.GetString(Operator));
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            if (Expression != null)
                yield return Expression;            
        }

        public override string ToString()
        {          
            return StringConverter.GetString(Operator) + Expression;
        }
    }

    public class CastExpression : UnaryExpression
    {
        public CastExpression(Expression cast, Expression value)
            : base(value, UnaryOperator.Cast)
        {
            if(cast is ParenEncloseExpression)
            {
                Cast = ((ParenEncloseExpression)cast).Value;
            }
            else
                Cast = cast;
            Line = cast.Line;
            Column = cast.Column;
        }        

        public Expression Cast { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Expression.CheckSemantic(scope, log);
            if (Cast is VariableExpression)
            {
                var v = (VariableExpression)Cast;
                Type = scope.GetType(v.Name);
            }
            else if (Cast is IndexArrayExpression)
            {
                var arr = (IndexArrayExpression)Cast;
                var v = arr.Left as VariableExpression;
                if (v == null)
                {
                    log.Error("Invalid Casting type", Line, Column);
                    Type = ShaderRuntime.Unknow;
                }
                else
                {
                    Type = scope.GetType(v.Name);
                    if(Type == null)
                        log.Error("Casting type not fount '"+v.Name+"'", Line, Column);
                    var indexer = arr.Indexer as LiteralExpression<int>;
                    if(indexer == null)
                        log.Error("Invalid Casting type", Line, Column);
                    else
                        Type = Type.MakeArrayType(indexer.Value);
                }
            }         
            else
            {
                log.Error("Invalid Casting type", Line, Column);
                Type = ShaderRuntime.Unknow;
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append('(');

            Cast.GenerateCode(sb, 0);
            //Type.AppendDeclaration(sb, 0);
            sb.Append(')');
            Expression.GenerateCode(sb, 0);
        }

        public override string ToString()
        {
            return String.Format("{0}{1}", Cast, Expression);
        }
    }
}
