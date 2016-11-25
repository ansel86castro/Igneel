using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public class ParameterDeclaration:LocalDeclaration
    {
        public ParameterDeclaration() { }

        public ParameterDeclaration(TypeDeclaration type, string name ,int position=0)
        {            
            Type = type;
            Name = name;
            Position = position;
        }

        public bool Opcional { get; set; }

        public int Position { get; set; }

        public string Semantic { get; set; }

        public ParamQualifier Qualifier { get; set; }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            if (Qualifier != ParamQualifier.Undefined)
            {
                sb.Append(StringConverter.GetString(Qualifier));
                sb.Append(" ");
                tabsOffset = 0;
            }
            var init = Initializer;
            Initializer = null;
            base.GenerateCode(sb, tabsOffset);
            Initializer = init;
            if (Semantic != null)
            {
                sb.AppendFormat(":{0}", Semantic);
            }
        }

        public override string ToString()
        {
            return StringConverter.GetString(Qualifier) + " "+ base.ToString();
        }

        public ParameterDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            var paramType = Type.ResolveGenerics(genericParameters);
            if (paramType == Type)
                return this;

            ParameterDeclaration p = (ParameterDeclaration)MemberwiseClone();
            p.Type = paramType;
            return p;
        }
    }
}
