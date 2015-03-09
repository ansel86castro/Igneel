using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Expressions
{   

    public class VariableExpression : Expression
    {
        public string Name { get; set; }

        public VariableDeclaration Variable { get; protected set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            if (!scope.ContainsVariable(Name))
            {
                log.Error(string.Format("Variable {0} not defined", Name), Line, Column);
            }

            Variable = scope.GetVariable(Name);
            Type = Variable.Type;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(Name);
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield break;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public class MemberExpression : Expression
    {
        public string MemberName { get; set; }

        public  Expression Left { get; set; }

        public MemberDeclaration Member { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Left.CheckSemantic(scope, log);
            if (Left.Type is StructDeclaration)
            {
                Member = ((StructDeclaration)Left.Type).GetMember(MemberName);
                if (Member == null)
                {
                    log.Error(string.Format("Member {0} is not defined", MemberName), Line, Column);
                }
            }
            else if (Left.Type is PrimitiveTypeDeclaration)
            {
                Member = ((PrimitiveTypeDeclaration)Left.Type).GetMember(MemberName, scope, log);
                if (Member == null)
                {
                    log.Error(string.Format("Member {0} is not defined", MemberName), Line, Column);
                }
            }
            if (Member != null)
                Type = Member.Type;
            else
            {
                Type = ShaderRuntime.Unknow;
                log.Error("Type not defined", Line, Column);
            }

        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Left.GenerateCode(sb, tabsOffset);
            sb.Append(".");
            sb.Append(MemberName);
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Left;
            yield return Member;
        }

        public override string ToString()
        {
            return Left.ToString() + "." + MemberName;
        }
    }

    public class RealArguments:Expression
    {
        public string ParameterName { get; set; }

        public Expression Value { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Value.CheckSemantic(scope, log);
            Type = Value.Type;
            if (Type == null)
                Type = ShaderRuntime.Unknow;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Value.GenerateCode(sb, tabsOffset);
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return ParameterName != null ? String.Format("{0} = {1}", ParameterName, Value) :
                                            Value.ToString();
                
        }
    }

    public class FunCallExpression : Expression
    {
       
        public string FunctionName { get; set; }

        public Expression Left { get; set; }

        public RealArguments[] Parameters { get; set; }

        public FunctionDeclaration Function { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            TypeDeclaration []args_types = null;
            if (Parameters != null && Parameters.Length > 0)
            {
                args_types = new TypeDeclaration[Parameters.Length];
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i].CheckSemantic(scope, log);                    
                    args_types[i] = Parameters[i].Type;
                }                
            }

            if (Left != null)
            {
                Left.CheckSemantic(scope, log);
                var type = Left.Type as PrimitiveTypeDeclaration;
                if (type == null)
                    log.Error("Functions are only allowed in runtime types", Line, Column);
                else
                {
                    Function = type.GetFunction(FunctionName, args_types);                   
                }
            }
            else
            {
                Function = scope.GetFunction(FunctionName, args_types);                
            }

            if (Function == null)
            {
                log.Error(string.Format("Function {0} not defined", FunctionName), Line, Column);
                Type = ShaderRuntime.Unknow;
            }
            else
                Type = Function.ReturnType;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            if (Left != null)
            {
                Left.GenerateCode(sb, tabsOffset);
                sb.Append(".");
            }
            else
                sb.Append('\t', tabsOffset);

            sb.Append(FunctionName + "(");

            if (Parameters != null && Parameters.Length > 0)
            {
                Parameters[0].GenerateCode(sb, 0);
                foreach (var item in Parameters.Skip(1))
                {
                    sb.Append(", ");
                    item.GenerateCode(sb, 0);
                }
            }

            sb.Append(")");
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            if (Left != null)
                yield return Left;
            if (Parameters != null)
            {
                foreach (var item in Parameters)
                {
                    yield return item;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Left != null)
            {
                sb.Append(Left + ".");
            }
            sb.Append(FunctionName);
            sb.Append("(");
            if (Parameters != null && Parameters.Length > 0)
            {
                sb.Append(Parameters[0].ToString());
                for (int i = 1; i < Parameters.Length; i++)
                {
                    sb.Append("," + Parameters[i].ToString());
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
    }

    public class IndexArrayExpression : Expression
    {
        public Expression Left { get; set; }

        public Expression Indexer { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            Left.CheckSemantic(scope, log);
            var type = Left.Type as TypeArrayDeclaration;
            if (type == null)
            {
                log.Error("Expression type is not an array", Line, Column);
                Type = ShaderRuntime.Unknow;
            }
            else
            {
                Type = type.ElementType;
            }

            Indexer.CheckSemantic(scope, log);
            if (Indexer.Type != ShaderRuntime.Int && Indexer.Type != ShaderRuntime.Float)
            {
                log.Error(String.Format("Indexer '{0}' type not allowed", Indexer.Type != null ? Indexer.Type.Name : ""), Line, Column);
            }          
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            Left.GenerateCode(sb, tabsOffset);
            sb.Append("[");

            Indexer.GenerateCode(sb, 0);

            sb.Append("]");
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            yield return Left;
            yield return Indexer;
        }

        public override string ToString()
        {
            return String.Format("{0}[{1}]", Left, Indexer);
        }
    }   
}
