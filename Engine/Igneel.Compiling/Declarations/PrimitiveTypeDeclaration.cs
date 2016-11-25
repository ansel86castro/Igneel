using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class PrimitiveTypeDeclaration : TypeDeclaration
    {
        public PrimitiveTypeDeclaration(string name)
        {
            this.Name = name;
            IsPrimitive = true;
            IsBuildIn = true;
            IsChecked = true;
        }

        public int Rows { get; set; }

        public int Colums { get; set; }     

        public FunctionDeclaration[] Methods { get; set; }

        public MemberDeclaration[] Members { get; set; }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {

        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
          
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            if (Methods != null)
            {
                foreach (var item in Methods)
                {
                    yield return item;
                }
            }
        }

        public virtual MemberDeclaration GetMember(string memberName, Scope scope, ErrorLog log)
        {
            if (Members == null)
                return null;

            var members = Members;
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].Name == memberName)
                    return members[i];
            }
            return null;
        }

        public FunctionDeclaration GetFunction(string functionName, TypeDeclaration[] argsTypes)
        {
            foreach (var item in Methods)
            {
                if (item.Name == functionName && item.Match(argsTypes))
                {
                    return item;
                }
            }
            return null;
        }

        //public override TypeDeclaration GetCompatibleType(TypeDeclaration other)
        //{
        //    if (other is VaryingType)
        //    {                
        //        return other.GetCompatibleType(this);
        //    }

        //    if (ReflectionType.Class == TypeClass.Object || 
        //        ReflectionType.Class == TypeClass.Struct || 
        //        ReflectionType.Class == TypeClass.Undefined) return null;

        //    if (ReflectionType.Class == TypeClass.Vector &&
        //        (other.ReflectionType.Class == TypeClass.Vector ||
        //        other.ReflectionType.Class == TypeClass.Scalar))
        //        return this;

        //    if (other.Class == TypeClass.Vector &&
        //        (ReflectionType.Class == TypeClass.Vector ||
        //        ReflectionType.Class == TypeClass.Scalar))
        //        return other;

        //    if (ReflectionType.Class == other.ReflectionType.Class && ReflectionType.Class ==  TypeClass.Matrix)
        //        return this;
        //    if (ReflectionType.Class == TypeClass.Matrix && other.ReflectionType.Class == TypeClass.Scalar)
        //        return this;
        //    if (other.ReflectionType.Class == TypeClass.Matrix && ReflectionType.Class == TypeClass.Scalar)
        //        return other;

        //    if(ReflectionType.Class == other.ReflectionType.Class && ReflectionType.Type == other.ReflectionType.Type)
        //        return this;

        //    return ShaderRuntime.Unknow;
        //}
    }
      
}
