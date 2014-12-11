using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum RegisterSet
    {
        BOOL,
        INT4,
        FLOAT4,
        SAMPLER,
    }

    public enum ParameterClass
    {
        SCALAR,
        VECTOR,
        MATRIX,
        OBJECT,
        STRUCT
    }

    public enum ParameterType
    {
        VOID,
        BOOL,
        INT,
        FLOAT,
        STRING,
        TEXTURE,
        TEXTURE1D,
        TEXTURE2D,
        TEXTURE3D,
        TEXTURECUBE,
        SAMPLER,
        SAMPLER1D,
        SAMPLER2D,
        SAMPLER3D,
        SAMPLERCUBE,
        PIXELSHADER,
        VERTEXSHADER,
        PIXELFRAGMENT,
        VERTEXFRAGMENT,
        UNSUPPORTED,
    }

    public class UniformDesc
    {
        //public int Index;
        public string Name;
        public RegisterSet Register;
        public ParameterClass Class;
        public ParameterType Type;
        public int Elements;
        public int Columns;
        public int Rows;
        public int Bytes;
        public UniformDesc[] Members;
    }    
}
