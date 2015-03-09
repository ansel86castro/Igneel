using Igneel.Compiling.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Runtime
{
    public abstract class VectorType : PrimitiveTypeDeclaration
    {
       protected TypeDeclaration _scalarType;       
        public VectorType(string name)
            : base(name)
        {

        }

        public TypeDeclaration ScalarType { get { return _scalarType; } }

        public override MemberDeclaration GetMember(string memberName, Scope scope, ErrorLog log)
        {         
            if (memberName.Length == 1)
            {
                return GetMember(memberName[0]);
            }

            if (memberName.Length > 4)
            {
                return null;
            }

            var memberType = scope.GetType(_scalarType.Name + memberName.Length);

            for (int i = 0; i < memberName.Length; i++)
            {
                if (GetMember(memberName[i]) == null)
                {
                    return null;
                }
            }


            MemberDeclaration dec = new MemberDeclaration
            {
                Name = memberName,
                DeclaringType = this,
                Type = memberType
            };
            return dec;
        }

        protected abstract MemberDeclaration GetMember(char name);        
    }

    public class Vector2Type : VectorType
    {
        public Vector2Type(Scope scope, string scalar)
            : base(scalar + "2")
        {
            Colums = 2;
            _scalarType = scope.GetType(scalar);

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(_scalarType, "x"),
              new ParameterDeclaration(_scalarType, "y")));

            Members = new MemberDeclaration[]
            {
                 ShaderRuntime.DeclareMember(this,"x", 0, _scalarType),
                 ShaderRuntime.DeclareMember(this, "y", 1, _scalarType),
            };
            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Vector,
                Columns = 2,
                Name = Name,
                Register = Graphics.RegisterSet.Float4,
                Elements = 1,
                Type = ShaderRuntime.GetShaderType(scalar)
            };
        }

        protected override MemberDeclaration GetMember(char name)
        {
            switch (name)
            {
                case 'r':
                case 'x': return Members[0];
                case 'g':
                case 'y': return Members[1];
            }
            return null;
        }
    }

    public class Vector3Type : VectorType
    {
        public Vector3Type(Scope scope, string scalar)
            : base(scalar + "3")
        {
            Colums = 3;
            _scalarType = scope.GetType(scalar);

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(_scalarType, "x"),
              new ParameterDeclaration(_scalarType, "y"),
              new ParameterDeclaration(_scalarType, "z")));

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(scope.GetType(scalar + "2"), "v"),
              new ParameterDeclaration(_scalarType, "z")));


            Members = new MemberDeclaration[]
            {
                 ShaderRuntime.DeclareMember(this,"x", 0, _scalarType),
                 ShaderRuntime.DeclareMember(this,"y", 1, _scalarType),
                 ShaderRuntime.DeclareMember(this,"z", 2, _scalarType)
            };

            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Vector,
                Columns = 3,
                Name = Name,
                Register = Graphics.RegisterSet.Float4,
                Elements = 1,
                Type = ShaderRuntime.GetShaderType(scalar)
            };
        }

        protected override MemberDeclaration GetMember(char name)
        {
            switch (name)
            {
                case 'r':
                case 'x': return Members[0];
                case 'g':
                case 'y': return Members[1];
                case 'b':
                case 'z': return Members[2];
            }
            return null;
        }
    }

    public class Vector4Type : VectorType
    {
        public Vector4Type(Scope scope, string scalar)
            : base(scalar + "4")
        {
            Colums = 4;
            _scalarType = scope.GetType(scalar);

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(_scalarType, "x"),
              new ParameterDeclaration(_scalarType, "y"),
              new ParameterDeclaration(_scalarType, "z"),
              new ParameterDeclaration(_scalarType, "w")));

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(scope.GetType(scalar + "2"), "v"),
              new ParameterDeclaration(_scalarType, "z"),
              new ParameterDeclaration(_scalarType, "w")));

            scope.AddFunction(new StdFunctionDeclaration(Name, this,
              new ParameterDeclaration(scope.GetType(scalar + "3"), "v"),
              new ParameterDeclaration(_scalarType, "w")));


            Members = new MemberDeclaration[]
            {
                 ShaderRuntime.DeclareMember(this,"x", 0, _scalarType),
                 ShaderRuntime.DeclareMember(this,"y", 1, _scalarType),
                 ShaderRuntime.DeclareMember(this,"z", 2, _scalarType),
                 ShaderRuntime.DeclareMember(this,"w", 3, _scalarType)
            };

            ReflectionType = new Graphics.ShaderReflectionType
            {
                Class = Graphics.TypeClass.Vector,
                Columns = 4,
                Name = Name,
                Register = Graphics.RegisterSet.Float4,
                Elements = 1,
                Type = ShaderRuntime.GetShaderType(scalar)
            };
        }

        protected override MemberDeclaration GetMember(char name)
        {
            switch (name)
            {
                case 'r':
                case 'x': return Members[0];

                case 'g':
                case 'y': return Members[1];

                case 'b':
                case 'z': return Members[2];

                case 'a':
                case 'w': return Members[3];
            }
            return null;
        }
    }
}
