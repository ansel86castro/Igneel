using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{

    public enum ScalarType
    {
        Void, Float, Int, Bool
    }

    public enum ObjectType
    {
        Undefined, Texture2D, Texture3D, TextureCube, Sampler, Buffer, Object
    }

    [Flags]
    public enum VarStorage
    {
        Undefined = 0, Uniform = 1 << 0, Static = 1 << 1, Const = 1 << 2
    }

    

    public enum ParamQualifier
    {
       Undefined, In, Out, InOut, Uniform
    }
    public enum InterpolationMode
    {
        Undefined, Centroid, Linear, NoInterpolation
    }

    public enum BinaryOperator
    {
        Addition, Substraction, Multiplication, Division,
        Less, Greater, Equals, LessEquals, GreaterEquals, NotEqual,
        Or, And, Xor,

    }    

    public enum AssignOp
    {
        Assign, AddAssign, SubAssign, MulAssign, DivAssign
    }
    
    public enum UnaryOperator
    {
        Not, PostInc, PostDec, PreInc, PreDec, Neg, Cast
    }

    public enum JumpType
    {
        Continue, Break, Discard, Return
    }

    public static class StringConverter
    {
         public static string GetString(VarStorage storage)
        {
            switch (storage)
            {
                case VarStorage.Undefined:
                    return "";
                case VarStorage.Uniform:
                    return "uniform";                            
            }
            string s = "";
            if ((storage & VarStorage.Static) == VarStorage.Static)
                s += "static";
            if ((storage & VarStorage.Const) == VarStorage.Const)
                s += " const";
            return s;
        }

         public static string GetString(BinaryOperator op)
         {

             switch (op)
             {
                 case BinaryOperator.Less:
                     return "<";
                 case BinaryOperator.Greater:
                     return ">";
                 case BinaryOperator.Equals:
                     return "==";
                 case BinaryOperator.LessEquals:
                     return "<=";
                 case BinaryOperator.GreaterEquals:
                     return ">=";
                 case BinaryOperator.NotEqual:
                     return "!=";
                 case BinaryOperator.Addition:
                     return "+";
                 case BinaryOperator.Division:
                     return "/";
                 case BinaryOperator.Multiplication:
                     return "*";
                 case BinaryOperator.Substraction:
                     return "-";
                 case BinaryOperator.And:
                     return "&&";
                 case BinaryOperator.Or:
                     return "||";
                 case BinaryOperator.Xor:
                     return "^";
                 default:
                     return null;
             }
         }

         public static string GetString(JumpType jump)
         {

             switch (jump)
             {
                 case JumpType.Continue:
                     return "continue";
                 case JumpType.Break:
                     return "break";
                 case JumpType.Discard:
                     return "discard";
                 case JumpType.Return:
                     return "return";
                 default:
                     return null;
             }
         }

         public static string GetString(UnaryOperator op)
         {
             switch (op)
             {
                 case UnaryOperator.Not:
                     return "!";
                 case UnaryOperator.PreInc:
                 case UnaryOperator.PostInc:
                     return "++";
                 case UnaryOperator.PostDec:                     
                 case UnaryOperator.PreDec:
                     return "--";
                 case UnaryOperator.Neg:
                     return "-";
                 case UnaryOperator.Cast:
                     return null;
                 default:
                     return null;
             }
         }

         public static string GetString(InterpolationMode interpolation)
         {
             switch (interpolation)
             {
                 case InterpolationMode.Undefined:
                     return "";
                 case InterpolationMode.Centroid:
                     return "centroid";
                 case InterpolationMode.Linear:
                     return "linear";
                 case InterpolationMode.NoInterpolation:
                     return "nointerpolation";
                 default:
                     return null;
             }
         }

         public static string GetString(ParamQualifier qualifier)
         {
             switch (qualifier)
             {
                 case ParamQualifier.Undefined:
                     return "";
                 case ParamQualifier.In:
                     return "in";
                 case ParamQualifier.Out:
                     return "out";
                 case ParamQualifier.InOut:
                     return "inout";
                 case ParamQualifier.Uniform:
                     return "uniform";
                 default:
                     return null;
             }
         }
    }

}
