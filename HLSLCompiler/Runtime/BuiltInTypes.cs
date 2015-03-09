using Igneel.Compiling.Declarations;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Runtime
{
    public class FloatType:PrimitiveTypeDeclaration
    {
        public FloatType()
            :base("float")
        {          
            Size = sizeof(float);
            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Scalar,
                Columns = 1,
                Elements = 1,
                Name = this.Name,
                Register = Graphics.RegisterSet.Float4,
                 Type = Graphics.ShaderType.Float
            };
        }

        public override bool Match(TypeDeclaration other)
        {
            return base.Match(other) || other == ShaderRuntime.Int || other == ShaderRuntime.Boolean;
        }
    }

    public class IntType : PrimitiveTypeDeclaration
    {
        public IntType()
            : base("int")
        {         
            Size = sizeof(int);
            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Scalar,
                Columns = 1,
                Elements = 1,
                Name = this.Name,
                Register = Graphics.RegisterSet.Float4,
                Type = Graphics.ShaderType.Int
            };
        }

        public override bool Match(TypeDeclaration other)
        {
            return base.Match(other) || other == ShaderRuntime.Float || other == ShaderRuntime.Boolean;
        }
    }

    public class BoolType : PrimitiveTypeDeclaration
    {
        public BoolType()
            : base("bool")
        {         
            Size = sizeof(int);
            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Scalar,
                Columns = 1,
                Elements = 1,
                Name = this.Name,
                Register = Graphics.RegisterSet.Float4,
                Type = Graphics.ShaderType.Bool
            };
        }

        public override bool Match(TypeDeclaration other)
        {
            return base.Match(other) || other == ShaderRuntime.Int || other == ShaderRuntime.Float;
        }
    }

    public class StringType : PrimitiveTypeDeclaration
    {
        public StringType()
            : base("string")
        {
            Size = -1;
            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Object,
                Type = Graphics.ShaderType.String,
                Name = "string",
                Register = Graphics.RegisterSet.Undefined
            };
        }
    }

    public class DimentionMatchType : GenericArgumentType
    {                
         public DimentionMatchType(string baseType,Scope scope)
             :base("DimentionMatchType")
        {
            BaseType = baseType;
            this.Scope = scope;
        }


         public string BaseType { get; set; }
      
         public Scope Scope { get; set; }

         public override TypeDeclaration ResolveGenerics(TypeDeclaration[] genericParameters)
         {
             var type = genericParameters[Position];
             string typeName = BaseType;
             if(type.ReflectionType.Rows > 1)
                 typeName+=type.ReflectionType.Rows;

             if (type.ReflectionType.Columns > 1)
             {
                 typeName += (type.ReflectionType.Rows > 1) ? "x" + type.ReflectionType.Rows :
                            type.ReflectionType.Columns.ToString();
             }
             return Scope.GetType(typeName);
         }
    }

    public class SamplerType : PrimitiveTypeDeclaration
    {
        public SamplerType(string name)
            : base(name)
        {

        }

        public override bool Match(TypeDeclaration other)
        {
            return other == ShaderRuntime.SamplerState || other == ShaderRuntime.SamplerComparisonState;
        }
    }
  
}
