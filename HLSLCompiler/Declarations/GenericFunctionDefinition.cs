using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    public class GenericFunctionDefinition:StdFunctionDeclaration
    {
        public List<GenericFunction> genericFunctions = new List<GenericFunction>();
        private GenericArgumentType[] _genericArguments;

        public GenericFunctionDefinition(string name)
            : base(name)
        {
            IsGenericFunctionDefinition = true;
        }

        public GenericFunctionDefinition(string name,
        TypeDeclaration returnType,
        params ParameterDeclaration[] parameters)
            : base(name, returnType, parameters)
        {
            IsGenericFunctionDefinition = true;
           
        }

        public GenericArgumentType[] GenericArguments
        {
            get { return _genericArguments; }
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
    
        public GenericFunctionDefinition WithArgs(params GenericArgumentType[] genericArguments)
        {
            this.GenericArguments = genericArguments;
            return this;
        }

        public GenericFunction MakeGenericFunction(TypeDeclaration[] parameterTypes)
        {                
            foreach (var item in genericFunctions)
            {
                if (item.Match(parameterTypes))
                    return item;
            }
            var parameters = Parameters;
            var genArgs = GenericArguments;
            var genericParameters = new TypeDeclaration[genArgs.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                for (int j = 0; j < genArgs.Length; j++)
                {
                    if(parameters[i].Type==genArgs[j])
                    {
                        if (genericParameters[j] == null)
                            genericParameters[j] = parameterTypes[i];
                        else if (genericParameters[j] != parameterTypes[i])
                            return null;
                    }
                }
            }

            GenericFunction function = new GenericFunction(Name)
            {
                DeclaringType = DeclaringType,
                GenericParameters = genericParameters,
                Body = Body,
                GenericDefinition = this,
                ReturnSemantic = ReturnSemantic,
                Attributes = Attributes,
                ReturnType = ReturnType.ResolveGenerics(genericParameters)
            };

            genericFunctions.Add(function);

            if (parameters != null)
            {               
                ParameterDeclaration[] genParameters = new ParameterDeclaration[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    genParameters[i] = parameters[i].ResolveGenerics(genericParameters);
                    genParameters[i].Function = function;
                }
                function.Parameters = genParameters;
            }

            return function;
        }

        public override FunctionDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
        {
            bool change = false;
            var retType = ReturnType.ResolveGenerics(genericParameters);
            change = retType != ReturnType;
            ParameterDeclaration[] parameters = null;

            if (Parameters != null)
            {
                parameters = new ParameterDeclaration[Parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = Parameters[i];
                    var @param = p.ResolveGenerics(genericParameters);
                    parameters[i] = param;
                    if (param != p)
                        change = true;
                }
            }

            if (!change)
                return this;

            return new GenericFunctionDefinition(Name)
            {
                DeclaringType = DeclaringType,
                GenericArguments = GenericArguments,
                Body = Body,            
                ReturnSemantic = ReturnSemantic,
                Attributes = Attributes,
                ReturnType = retType,
                Parameters = parameters
            };
        }

     
    }
}
