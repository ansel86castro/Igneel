using Igneel.Compiling.Expressions;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    

    public abstract class VariableDeclaration : Declaration
    {
        public TypeRef TypeInfo { get; set; }

        public VarStorage Storage { get; set; }

        public RegisterSet Register { get; set; }

        public TypeDeclaration Type { get; set; }

        public Expression Initializer { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StringConverter.GetString(Storage));
            sb.Append(" ");
            if (TypeInfo != null)
                sb.Append(TypeInfo);
            sb.Append(" " + Name);            
            if (Initializer != null)
            {
                sb.AppendFormat(" = {0}", Initializer);
            }
            return sb.ToString();
        }

        public override void AddToScope(Scope scope)
        {
            if (scope.ContainsVariable(Name, false))
            {
                scope.Log.Error(String.Format("Variable '{0}' already defined", Name), Line, Column);
            }
            else
                scope.AddVariable(this);
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {          
            Type = TypeInfo.GetTypeDeclaration(scope, log);

            if (Initializer != null)
            {
                Initializer.CheckSemantic(scope, log);
                if (Initializer is VariableInitializer)
                {
                    Initializer.Type = Type;
                }
                else if (!Initializer.Type.Match(Type))
                {
                    log.Error(String.Format("Initializer Expression do not match variable declaration '{0}'", Name), Line, Column);
                }
            }
            IsChecked = true;
        }

        /// <summary>
        /// Generates
        /// uniform float4 world[4] : WORLD = {....}
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tabsOffset"></param>
        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {            
            sb.Append('\t', tabsOffset);
            if (Storage != VarStorage.Undefined)
            {
                sb.Append(StringConverter.GetString(Storage));
                sb.Append(" ");
                tabsOffset = 0;
            }
            Type.AppendDeclaration(sb, 0);
            sb.Append(" " + Name);
            if (TypeInfo.Elements > 0)
            {
                sb.AppendFormat("[{0}]", TypeInfo.Elements);
            }            
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            if (Initializer != null)
                yield return Initializer;
            else
                yield break;
        }
    }

    public class GlobalVariableDeclaration : VariableDeclaration
    {
        public RegisterDeclaration Register { get; set; }

        public PackOffsetDeclaration PackOffset { get; set; }

        public string Semantic { get; set; }

        public string Shader { get; set; }             

        public int Offset { get; set; }        

        public ConstantBufferDeclaration ConstanBuffer { get; set; }       

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            base.GenerateCode(sb, tabsOffset);
            if (Semantic != null)
            {
                sb.AppendFormat(":{0}", Semantic);
            }

            if (Initializer != null)
            {
                sb.Append(" = ");
                Initializer.GenerateCode(sb, 0);
                sb.Append(" ");
            }

            if (PackOffset != null)
            {
                PackOffset.GenerateCode(sb, 0);
                sb.Append(" ");
            }
            if (Register != null)
                Register.GenerateCode(sb, 0);

            sb.Append(";");            
        }      

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StringConverter.GetString(Storage));
            sb.Append(" ");
            if (TypeInfo != null)
                sb.Append(TypeInfo);
            sb.Append(" " + Name);
            if (Semantic != null)
                sb.Append(" :" + Semantic);
            if (Initializer != null)
            {
                sb.AppendFormat(" = {0}", Initializer);
            }            
            if (PackOffset != null)
                sb.Append(PackOffset);
            if (Register != null)
                sb.Append(Register);
            return sb.ToString();
        }

        public override bool IsUsed
        {
            get
            {
                return base.IsUsed;
            }
            set
            {
                base.IsUsed = value;
                if (ConstanBuffer != null)
                {
                    ConstanBuffer.SetIsUsed(this);
                }
            }
        }
    }

    public class LocalDeclaration : VariableDeclaration
    {      
        public FunctionDeclaration Function { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            AddToScope(scope);
            base.CheckSemantic(scope, log);
            IsChecked = true;
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            base.GenerateCode(sb, tabsOffset);

            if (Initializer != null)
            {
                sb.Append(" = ");
                Initializer.GenerateCode(sb, 0);              
            }
        }
      
    }
}
