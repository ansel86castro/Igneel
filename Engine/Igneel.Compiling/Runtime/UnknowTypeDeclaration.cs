using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Compiling.Runtime
{
    public class UnknowTypeDeclaration:PrimitiveTypeDeclaration
    {
        public UnknowTypeDeclaration()
            : base("unknow")
        {
            this.ReflectionType = new Graphics.ShaderReflectionType()
            {
                Class = Graphics.TypeClass.Undefined,
                Type = Graphics.ShaderType.Unsupported,
                Members = new Graphics.ShaderReflectionVariable[0],
                Name = Name,
                Register = Graphics.RegisterSet.Undefined
            };
        }
        public override bool Match(TypeDeclaration other)
        {
            return other is UnknowTypeDeclaration;
        }
    }
}
