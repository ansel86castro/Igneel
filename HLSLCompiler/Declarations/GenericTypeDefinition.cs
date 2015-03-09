using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class GenericTypeDefinition : PrimitiveTypeDeclaration
    {
        public static List<GenericPrimitiveType> genericTypes = new List<GenericPrimitiveType>();
        private GenericArgumentType[] _genericArguments;

        public GenericTypeDefinition(string name, params GenericArgumentType[] types)
            : base(name)
        {
            this.GenericArguments = types;
            IsGenericArgumentDefinition = true;
        }

        public GenericArgumentType[] GenericArguments { get { return _genericArguments; }
            set
            {
                _genericArguments = value;
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        value[i].Position = i;
                    }
                }
            }
        }       

        public override TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            var genArgs = GenericArguments;
            if (genArgs.Length != genericParameters.Length)
                throw new ArgumentException();

            foreach (var item in genericTypes)
            {
                if (item.Match(genericParameters))
                    return item;
            }
            for (int i = 0; i < genArgs.Length; i++)
            {
                if (!genArgs[i].Match(genericParameters[i]))
                    throw new ArgumentException("genericParameters do not match the generic arguments of the type definition");
            }

            var genType = new GenericPrimitiveType(Name);

            if (Members != null)
            {
                var members = Members;
                MemberDeclaration[] genMembers = new MemberDeclaration[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    genMembers[i] = members[i].ResolveGenerics(genericParameters);
                    genMembers[i].DeclaringType = genType;
                }
                genType.Members = genMembers;
            }
            if (Methods != null)
            {
                var functions = Methods;
                FunctionDeclaration[] genFunctions = new FunctionDeclaration[functions.Length];
                for (int i = 0; i < functions.Length; i++)
                {
                    genFunctions[i] = functions[i].ResolveGenerics(genericParameters);
                }
            }

            genericTypes.Add(genType);
            return genType;
        }      
    }

    public class GenericArgumentType : PrimitiveTypeDeclaration
    {
        public GenericArgumentType(string name)
            : base(name)
        {
            IsGenericArgumentDefinition = true;   
        }

        public GenericArgumentType(string name, TypeClass[] classContraint, ShaderType[] typeConstraint)
            : this(name)
        {
            ClassConstrains = classContraint;
            TypeConstrains = typeConstraint;
        }

        public int Position { get; set; }

        public TypeClass[] ClassConstrains { get; set; }

        public ShaderType[] TypeConstrains { get; set; }

        public int[] ColumnsConstrains { get; set; }

        public int[] RowsConstrains { get; set; }

        public override bool Match(TypeDeclaration other)
        {
            var reflectionType = other.ReflectionType;
            if (ClassConstrains != null)
            {
                var index = Array.IndexOf(ClassConstrains, reflectionType.Class);
                if (index < 0)
                    return false;
            }
            if (TypeConstrains != null)
            {
                var index = Array.IndexOf(TypeConstrains, reflectionType.Type);
                if (index < 0)
                    return false;
            }
            if (ColumnsConstrains != null)
            {
                var index = Array.IndexOf(ColumnsConstrains, reflectionType.Columns);
                if (index < 0)
                    return false;
            }
            if (RowsConstrains != null)
            {
                var index = Array.IndexOf(RowsConstrains, reflectionType.Rows);
                if (index < 0)
                    return false;
            }
            return true;
        }

        public override TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            return genericParameters[Position];
        }
    }

    public class GenericPrimitiveType : PrimitiveTypeDeclaration
    {
        public GenericPrimitiveType(string name)
            : base(name)
        {
            IsGenericType = true;
        }

        public TypeDeclaration[] GenericParameters { get; set; }

        public GenericTypeDefinition GenericDefinition { get; set; }

        public bool Match(TypeDeclaration[] genericParameters)
        {
            var genTypes = GenericParameters;
            if (genericParameters.Length != genTypes.Length)
                return false;

            for (int i = 0; i < genTypes.Length; i++)
            {
                if (!genTypes[i].Match(genericParameters[i]))
                    return false;
            }
            return true;
        }
       
        public override TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            var genArgs = GenericParameters;
            TypeDeclaration[] resolveTypeArgs = new TypeDeclaration[genArgs.Length];
            bool changed = false;
            for (int i = 0; i < genArgs.Length; i++)
            {
                resolveTypeArgs[i] = genArgs[i].ResolveGenerics(genericParameters);
                if (resolveTypeArgs[i] != genArgs[1])
                    changed = true;                
            }

            if (!changed)
                return this;

            GenericPrimitiveType resolveType = new GenericPrimitiveType(Name)
            {
                GenericDefinition = GenericDefinition,
                GenericParameters = resolveTypeArgs
            };


            if (Members != null)
            {
                var members = Members;
                MemberDeclaration[] genMembers = new MemberDeclaration[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    genMembers[i] = members[i].ResolveGenerics(genericParameters);
                    genMembers[i].DeclaringType = resolveType;
                }
                resolveType.Members = genMembers;
            }
            if (Methods != null)
            {
                var methods = Methods;
                FunctionDeclaration[] genMethods = new FunctionDeclaration[methods.Length];
                for (int i = 0; i < genMethods.Length; i++)
                {
                    genMethods[i] = methods[i].ResolveGenerics(genericParameters);
                    genMethods[i].DeclaringType = resolveType;
                }
                resolveType.Methods = genMethods;
            }

            return resolveType;
        }

        public override void AppendDeclaration(StringBuilder sb, int tabsOffset)
        {
            base.AppendDeclaration(sb, tabsOffset);
            sb.Append("<");

            GenericParameters[0].AppendDeclaration(sb, 0);
            foreach (var item in GenericParameters.Skip(1))
            {
                sb.Append(", ");
                item.AppendDeclaration(sb, 0);
            }
            sb.Append(">");
        }
    }
}
