using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class StructDeclaration : TypeDeclaration
    {
        public StructDeclaration()
        {
            IsBuildIn = false;
            IsPrimitive = false;
            IsChecked = false;
        }

        public bool IsMainInput { get; set; }

        public StructMemberDeclaration[] Members { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            base.CheckSemantic(scope, log);
            
            if (Members != null)
            {
                int offset = 0;
                foreach (var item in Members)
                {
                    item.DeclaringType = this;
                    item.CheckSemantic(scope, log);
                    item.Offset = offset;
                    offset += item.Type.Size;                   
                }
                Size = offset;
            }

            ReflectionType = new Graphics.ShaderReflectionType
            {
                Name = Name,
                Columns = 0,
                Rows = 0,
                Class = Graphics.TypeClass.Struct,
                Register = Graphics.RegisterSet.Float4,
                Type = Graphics.ShaderType.UserDefined,
            };

            ReflectionType.Members = new Graphics.ShaderReflectionVariable[Members.Length];          
            for (int i = 0; i < Members.Length; i++)
            {
                Graphics.ShaderReflectionVariable v = new Graphics.ShaderReflectionVariable
                {
                    Name = Members[i].Name,
                    Size = Members[i].Type.Size,
                    Location = Members[i].Offset,
                    Semantic = Members[i].Semantic,
                    Type = Members[i].Type.ReflectionType
                };
            }
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            sb.AppendLine();
            sb.Append('\t', tabsOffset);
            sb.AppendFormat("struct {0}", Name);
            sb.AppendLine();

            sb.Append('\t', tabsOffset); sb.Append("{");

            foreach (var member in Members)
            {
                member.GenerateCode(sb, tabsOffset + 1);
                sb.Append(";");
                sb.AppendLine();
            }

            sb.Append('\t', tabsOffset); sb.Append("};");
        }

        public override IEnumerable<ASTNode> GetNodes()
        {
            return Members;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("struct " + Name);
            sb.AppendLine();
            sb.Append("{");
            sb.AppendLine();
            foreach (var item in Members)
            {
                sb.AppendLine(item.ToString());
            }

            sb.AppendLine();
            sb.Append("};");
            sb.AppendLine();
            return sb.ToString();
        }

        public StructMemberDeclaration GetMember(string MemberName)
        {
            if (Members == null)
                return null;
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].Name == MemberName)
                    return Members[i];
            }
            return null;
        }
     
    }
}
