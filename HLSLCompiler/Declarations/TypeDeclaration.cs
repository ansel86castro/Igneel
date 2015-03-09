using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    
    public abstract class TypeDeclaration : Declaration, IEquatable<TypeDeclaration>
    {        
        public ShaderReflectionType ReflectionType { get; set; }     

        public bool IsPrimitive { get; set; }

        public bool IsBuildIn { get; set; }

        public bool IsGenericType { get; protected set; }

        public bool IsGenericTypeDefinition { get; protected set; }

        public bool IsArrayType { get; protected set; }

        public bool IsGenericArgumentDefinition { get; protected set; }

        public int Size { get; set; }

        public virtual bool Match(TypeDeclaration other)
        {
            if (Name == other.Name)
            {
                return true;
            }
            if (IsBuildIn)
            {
                if (ReflectionType.Class == TypeClass.Scalar && other.ReflectionType.Class == TypeClass.Vector &&
                   ReflectionType.Type == other.ReflectionType.Type)
                    return true;
            }
            return false;
        }

        public TypeArrayDeclaration MakeArrayType(int elements)
        {
            if (IsArrayType)
            {
                TypeArrayDeclaration type = (TypeArrayDeclaration)this;
                if (type.Elements == elements)
                    return type;
            }
            return new TypeArrayDeclaration(this, elements);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {            
            TypeDeclaration t = obj as TypeDeclaration;
            if (t == null) return false;
            return Name == t.Name;
        }

        public bool Equals(TypeDeclaration other)
        {
            if(other == null) return false;
           return other.Name == Name;
        }

        public override void AddToScope(Scope scope)
        {
            if (scope.ContainsType(Name))
            {
                scope.Log.Error(string.Format("Type '{0}' already defined", Name), Line, Column);
            }
            else
            {
                scope.AddType(this);
            }
        }

        public override void CheckSemantic(Scope scope, ErrorLog log)
        {
            IsChecked = true;
        }

        public virtual void AppendDeclaration(StringBuilder sb, int tabsOffset)
        {
            sb.Append('\t', tabsOffset);
            sb.Append(Name);            
        }      

        public virtual TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            return this;
        }
      
    }

   
}
