using Igneel.Compiling.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class TypeRef
    {
        public int Line;
        public int Column;
        public string Name;
        public int Elements;
        public TypeRef[] GenericArgs;

        public TypeRef() { }

        public TypeRef(string name, int elements = 0, TypeRef[] genericArguments = null)
        {
            this.Name = name;
            this.Elements = elements;
            this.GenericArgs = genericArguments;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name);

            if (GenericArgs != null && GenericArgs.Length > 0)
            {
                sb.Append("<");

                sb.Append(GenericArgs.Skip(1).Aggregate(GenericArgs[0].ToString(), (s, x) => s + ", " + x.ToString()));

                sb.Append(">");
            }
            if (Elements > 0)
            {
                sb.AppendFormat("[{0}]", Elements);
            }
            return sb.ToString();
        }

        public TypeDeclaration GetTypeDeclaration(Scope scope, ErrorLog log)
        {
            TypeDeclaration type = null;
            if (GenericArgs != null && GenericArgs.Length > 0)
            {
                TypeDeclaration[] genericArguments = new TypeDeclaration[GenericArgs.Length];
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    genericArguments[i] = GenericArgs[i].GetTypeDeclaration(scope, log);                    
                }
                type = scope.GetType(Name, genericArguments);
            }
            else
            {
                type = scope.GetType(Name);
            }           
            if(type == null)
                log.Error("Type '" + ToString() + "'not fount", Line, Column);
            else if(Elements > 0)
                type = type.MakeArrayType(Elements);            

            return type;
        }
     
    }
}
