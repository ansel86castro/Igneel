using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum RegisterSet
    {
        Undefined,
        Bool4,
        Int4,
        Float4,
        Sampler,
        Texture
    }

    public enum TypeClass
    {
        Undefined,
        Scalar,
        Vector,
        Matrix,
        Object,
        Struct
    }

    public enum ShaderType
    {
        Unsupported,
        UserDefined,
        Bool,
        Int,
        Float,
        String,
        Texture,
        Texture1D,
        Texture2D,
        Texture3D,
        TextureCube,
        Sampler,
        Sampler1D,
        Sampler2D,
        Sampler3D,
        SamplerCube,              
    }

    public enum BufferType
    {
        Undefined, CBuffer, TBuffer
    }

    public interface IShaderMemberReflection
    {

    }

    public class ShaderReflectionVariable : INameable, IShaderMemberReflection
    {
        /// <summary>
        /// Variable name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Semantic 
        /// </summary>
        public string Semantic { get ;set ;}
        /// <summary>
        /// Register index or offset in bytes from the constant buffer
        /// </summary>
        public int Location { get; set; }
        /// <summary>
        /// Size in Bytes
        /// </summary>
        public int Size { get; set; }

        public ShaderReflectionType Type { get; set; }
    }

    public class ShaderReflectionType : INameable, IShaderMemberReflection
    {
        /// <summary>
        /// Type name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Hardware register set
        /// </summary>
        public RegisterSet Register { get; set; }
        /// <summary>
        /// Hardware class of the type
        /// </summary>
        public TypeClass Class { get; set; }
        /// <summary>
        /// Hardware type 
        /// </summary>
        public ShaderType Type { get; set; }
        /// <summary>
        /// Number of columns in the case of a matrix or vector
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// Number of rows in the case of matrix else 0
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// Number of elements in the array
        /// </summary>
        public int Elements { get; set; }

        public ShaderReflectionVariable[] Members { get; set; }
    }

    public class BufferReflection : INameable, IShaderMemberReflection
    {

        private NamedCollection<ShaderReflectionVariable> _constants = new NamedCollection<ShaderReflectionVariable>();
        private BufferType _type;
        private string _name;

        public NamedCollection<ShaderReflectionVariable> Constants { get { return _constants; } }

        public BufferType Type { get { return _type; } set { _type = value; } }

        public string Name { get { return _name; } set { _name = value; } }
    }

    public class ShaderReflection:IShaderMemberReflection
    {
        NamedCollection<BufferReflection> _buffers = new NamedCollection<BufferReflection>();
        NamedCollection<ShaderReflectionVariable> _variables = new NamedCollection<ShaderReflectionVariable>();

        public NamedCollection<BufferReflection> Buffers { get { return _buffers; } }     
        public NamedCollection<ShaderReflectionVariable> Variables { get { return _variables; } }

        public ShaderReflectionVariable GetGlobal(string name)
        {
            if(_variables.Contains(name))
                return _variables[name];
            foreach (var cb in _buffers)
            {
                if (cb.Constants.Contains(name))
                    return cb.Constants[name];
            }
            return null;
        }
    }
   
}
