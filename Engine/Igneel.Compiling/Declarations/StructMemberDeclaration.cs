using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public class MemberDeclaration : VariableDeclaration
    {
        public int Offset { get; set; }

        public TypeDeclaration DeclaringType { get; set; }

        public MemberDeclaration MakeGenericMember(TypeDeclaration memberType)
        {
            return new MemberDeclaration
            {
                Name = Name,                
                Column = Column,
                Line = Line,
                Type = memberType,
                Offset = Offset,
                Register = Register,
                Storage = Storage,
                IsUsed = IsUsed,
                Initializer = Initializer
            };
        }

        public MemberDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {          
            var memberType = Type.ResolveGenerics(genericParameters);
            if (memberType == Type)
                return this;

            return new MemberDeclaration
             {
                 Name = Name,
                 Column = Column,
                 Line = Line,
                 Type = memberType,
                 Offset = Offset,
                 Register = Register,
                 Storage = Storage,
                 IsUsed = IsUsed,
                 Initializer = Initializer,
                 TypeInfo = TypeInfo
             };

        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield break;
        }
    }

    public class StructMemberDeclaration : MemberDeclaration
    {
        public string Semantic { get; set; }       
       
        public InterpolationMode Interpolation { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            IsChecked = true;
            Type = TypeInfo.GetTypeDeclaration(scope, log);
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            base.GenerateCode(sb, tabsOffset);
            if (Semantic != null)
            {
                sb.Append(" :" + Semantic);
            }
            if (Initializer != null)
            {
                sb.Append(" = ");
                Initializer.GenerateCode(sb, tabsOffset);
            }
        }      

        public override IEnumerable<AstNode> GetNodes()
        {
            yield break;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(StringConverter.GetString(Interpolation));            
            sb.Append(" ");
            if (TypeInfo != null)
                sb.Append(TypeInfo);
            sb.Append(" " + Name);
            if (Semantic != null)
                sb.Append(" :"+Semantic);
            if (Initializer != null)
            {
                sb.AppendFormat(" = {0}", Initializer);
            }
            return sb.ToString();
        }
    }
}
