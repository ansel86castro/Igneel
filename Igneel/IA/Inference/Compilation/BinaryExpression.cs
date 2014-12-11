using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.Inference.Compilation
{    
    public abstract class BinaryExpression:Expresion
    {
        public Expresion Left;
        public Expresion Right;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            Left.CheckSemantic(log, scope);
            Right.CheckSemantic(log, scope);

            if (Left.Type != Right.Type)
            {
                log.AddError("Expression type mismatch at", Line, Column);
            }
            Type = Left.Type;
        }
    }

    public enum AritmeticOperator
    {
        Add, Sub, Mul, Div, Inv
    }

    public enum LogicalOperator
    {
        And,Or,Xor,Not
    }

    public enum RelationalOperator
    {
        Equal, Less, Greater , LessEqual, GreaterEqual, NotEqual
    }

    public class AritmeticBinaryExpression : BinaryExpression
    {
        public AritmeticOperator Op;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            base.CheckSemantic(log, scope);

            if (Type != typeof(float))
                log.AddError("the type is not float", Line, Column);
        }
    }

    public class LogicalBinaryExpression : BinaryExpression
    {
        public AritmeticOperator Op;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            base.CheckSemantic(log, scope);

            if (Type != typeof(bool))
                log.AddError("the type is not float", Line, Column);
        }
    }

    public class RelationalBinaryExpression : BinaryExpression
    {
        public RelationalOperator Op;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            base.CheckSemantic(log, scope);

            switch (Op)
            {
                case RelationalOperator.GreaterEqual:
                case RelationalOperator.LessEqual:
                case RelationalOperator.Greater:
                case RelationalOperator.Less:
                    if (Type != typeof(float))
                        log.AddError("the type is not float", Line, Column);
                    break;
            }
        }
    }

    public class NotExpresion : Expresion
    {
        public Expresion Exp;


        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            Exp.CheckSemantic(log, scope);
            Type = Exp.Type;
            if (Type != typeof(bool))
                log.AddError("The expression must be boolean", Line, Column);
        }
    }

    public class InvExpression : Expresion
    {
        public Expresion Exp;

        public override void CheckSemantic(ICompilationLog log, CompilationScope scope)
        {
            Exp.CheckSemantic(log, scope);
            Type = Exp.Type;
            if (Type != typeof(float))
                log.AddError("The expression must be boolean", Line, Column);
        }
    }
}
