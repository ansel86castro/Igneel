using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Declarations
{
    public class GenericFunction : StdFunctionDeclaration
    {
        public GenericFunction(string name)
            : base(name)
        {

        }

        public TypeDeclaration[] GenericParameters { get; set; }

        public GenericFunctionDefinition GenericDefinition { get; set; }

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

            return new GenericFunction(Name)
            {
                DeclaringType = DeclaringType,
                GenericParameters = genericParameters,
                Body = Body,
                GenericDefinition = GenericDefinition,
                ReturnSemantic = ReturnSemantic,
                Attributes = Attributes,
                ReturnType = retType,
                Parameters = parameters
            };
        }
    }
}
