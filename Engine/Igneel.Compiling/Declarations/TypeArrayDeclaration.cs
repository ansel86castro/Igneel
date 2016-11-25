using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class TypeArrayDeclaration: TypeDeclaration
    {
        public TypeArrayDeclaration(TypeDeclaration type, int elements)
        {
            this.ElementType = type;
            this.Elements = elements;
            Name = type.Name;
            Size = type.Size * elements;            
        }
       
        public TypeDeclaration ElementType { get; set; }

        public int Elements { get; set; }

        public override bool Match(TypeDeclaration other)
        {
            return base.Match(other);
        }

        public override string ToString()
        {
            if(ElementType == null)
                return base.ToString();
            return ElementType.ToString() + String.Format("[0]", Elements);
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
           
        }

        public override void GenerateCode(StringBuilder sb, int tabsOffset)
        {
            
        }

        public override IEnumerable<AstNode> GetNodes()
        {
            yield break;
        }       

        public override TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            var g = ElementType.ResolveGenerics(genericParameters);
            if (g == ElementType)
                return this;
            return g.MakeArrayType(Elements);
        }
      
    }
}
