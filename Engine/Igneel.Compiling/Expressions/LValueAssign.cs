using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Expressions
{
    public class LValueAssign : Expression
    {
        public LValueAssign(AssignOp op, Expression lvalue, Expression value)
        {
            Operator = op;
            LValue = lvalue;
            Value = value;
            Line = lvalue.Line;
            Column = lvalue.Column;
        }

        public AssignOp Operator { get; set; }

        public Expression LValue { get; set; }

        public Expression Value { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            LValue.CheckSemantic(scope, log);
            Value.CheckSemantic(scope, log);

            if (LValue.Type != null)
            {
                //if (!LValue.Type.Match(Value.Type))
                //{
                //    log.Error("The types do not match", Line, Column);
                //}
                Type = LValue.Type;
            }

            //if (Type != null)
            //{
            //    if (Operator == AssignOp.AddAssign ||
            //       Operator == AssignOp.SubAssign ||
            //       Operator == AssignOp.MulAssign ||
            //       Operator == AssignOp.DivAssign)
            //    {
            //        if (!(Type.IsPrimitive &&
            //            (Type.ReflectionType.Class == Graphics.TypeClass.Vector ||
            //            Type.ReflectionType.Class == Graphics.TypeClass.Scalar ||
            //            Type.ReflectionType.Class == Graphics.TypeClass.Matrix)))
            //        {
            //            log.Error("The " + GetValue(Operator) + " is only allowed on type " + Type.Name, Line, Column);
            //        }
            //    }
            //}
            if (Type == null)
                Type = ShaderRuntime.Unknow;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            LValue.GenerateCode(sb, tabsOffset);
            sb.AppendFormat(" {0} ", GetValue(Operator));
            Value.GenerateCode(sb, 0);         
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield return LValue;
            yield return Value;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", LValue, GetValue(Operator), Value);
        }

        private string GetValue(AssignOp op)
        {
            switch (op)
            {
                case AssignOp.Assign:
                    return "=";
                case AssignOp.AddAssign:
                    return "+=";
                case AssignOp.SubAssign:
                    return "-=";
                case AssignOp.MulAssign:
                    return "*=";
                case AssignOp.DivAssign:
                    return "/=";              
            }
            return null;
        }
    }

    //public class MemberAssign : LValueAssign
    //{
    //    public MemberAssign(AssignOp op, LValueExpression lvalue, Expression value)
    //        : base(op, lvalue, value) { }

    //    public MemberRefExpression Left { get; set; }

    //    public override void CheckSemantic(Scope scope, ErrorLog log)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void GenerateCode(StringBuilder sb, int tabsOffset)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class VariableAssign : LValueAssign
    //{
    //    public VariableAssign(AssignOp op, LValueExpression lvalue, Expression value)
    //        : base(op, lvalue, value) { }

    //    public VarRefExpression Left { get; set; }

    //    public override void CheckSemantic(Scope scope, ErrorLog log)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void GenerateCode(StringBuilder sb, int tabsOffset)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class ArrayAssign : LValueAssign
    //{
    //    public IndexArrayExpression Left { get; set; }

    //    public override void CheckSemantic(Scope scope, ErrorLog log)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void GenerateCode(StringBuilder sb, int tabsOffset)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
