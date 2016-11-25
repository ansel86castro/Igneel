using Igneel.Compiling.Expressions;
using Igneel.Compiling.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public abstract class FunctionDeclaration:Declaration
    {
        ParameterDeclaration[] _parameters;

        public PrimitiveTypeDeclaration DeclaringType { get; set; }

        public bool IsBuildIn { get; set; }

        public bool IsGenericFunctionDefinition { get; protected set; }

        public bool IsGenericFunction { get; protected set; }    

        public AttributeDeclaration[] Attributes { get; set; }

        public ParameterDeclaration[] Parameters { get { return _parameters; }
            set
            {
                _parameters = value;
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                {
                    _parameters[i].Function = this;
                    _parameters[i].Position = i;
                }                
            }
        }        

        public TypeRef ReturnTypeInfo { get; set; }

        public TypeDeclaration ReturnType { get; set; }

        public BlockStatement Body { get; set; }

        public string ReturnSemantic { get; set; }

        public FunctionDeclaration(string name)         
        {
            this.Name = name;
        }

        public FunctionDeclaration() { }

        public override IEnumerable<AstNode> GetNodes()
        {
            if (Attributes != null)
            {
                foreach (var item in Attributes)
                {
                    yield return item;
                }
            }

            if (Parameters != null)
            {
                foreach (var item in Parameters)
                {
                    yield return item;
                }
            }
            yield return Body;
        }

        public override void AddToScope(Scope scope)
        {
            scope.AddFunction(this);
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            if (Attributes != null)
            {
                foreach (var attr in Attributes)
                {
                    attr.CheckSemantic(scope, log);
                }
            }

            Scope functionScope = new Scope(scope)
            {
                CurrentFunction = this
            };

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    var p = Parameters[i];
                    p.Position = i;
                    p.Function = this;
                    p.CheckSemantic(functionScope, log);
                }
            }

            ReturnType = ReturnTypeInfo.GetTypeDeclaration(scope, log);           
            Body.CheckSemantic(functionScope, log);
            IsChecked = true;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            if (Attributes != null)
            {
                foreach (var item in Attributes)
                {
                    item.GenerateCode(sb, tabsOffset);
                    sb.AppendLine();
                }
            }

            ReturnType.AppendDeclaration(sb, tabsOffset);
            if (ReturnTypeInfo.Elements > 0)
            {
                sb.AppendFormat("[{0}]", ReturnTypeInfo.Elements);
            }
            sb.Append(" " + Name);
            sb.Append("(");

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
            if (ReturnSemantic != null)
                sb.Append(":" + ReturnSemantic);
            sb.AppendLine();
            Body.GenerateCode(sb, tabsOffset);     
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Attributes != null)
            {
                foreach (var item in Attributes)
                {
                    sb.Append(item);
                }               
            }

            if (ReturnTypeInfo != null)
            {
                sb.Append(ReturnTypeInfo);
            }

            sb.Append(" " + Name);
            sb.Append("(");

            if (Parameters != null && Parameters.Length > 0)
            {
                sb.Append(Parameters.Skip(1).Aggregate(Parameters[0].ToString(), (s, x) => s + ", " + x.ToString()));
            }
            sb.Append(")");
            if (ReturnSemantic != null)
                sb.Append(":" + ReturnSemantic);
            sb.AppendLine();

            sb.Append(Body);

            return sb.ToString();
        }

        public bool Match(TypeDeclaration[] parameters)
        {
            if (parameters == null && _parameters == null)
                return true;

            if (_parameters == null && (parameters != null || parameters.Length > 0))
                return false;

            if ((parameters == null||parameters.Length == 0) && (_parameters == null || _parameters.Length == 0))
            {
                return true;
            }
            
            for (int i = 0; i < _parameters.Length; i++)
            {
                var p0 = _parameters[i];
                if (parameters == null || i >= parameters.Length)
                {
                    if (!p0.Opcional)
                        return false;
                }
                else
                {
                    var p1 = parameters[i];
                    if (!p0.Type.Match(p1))
                        return false;
                }
            }
            return true;
        }

        public virtual FunctionDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            return this;
        }
    }
  
}
