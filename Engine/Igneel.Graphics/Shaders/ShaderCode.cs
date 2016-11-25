using ClrRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShaderCode
    {
        byte[] code;
        bool isCompiled;
        ShaderReflection shaderReflection;

        public ShaderCode(IntPtr ptrCode,  int sizeInBytes, bool isCompiled)
        {
            code = new byte[sizeInBytes];
            unsafe
            {
                fixed (byte* pCode = code)
                {
                    Runtime.Copy(ptrCode.ToPointer(), pCode, sizeInBytes);
                }
            }
        }

        public ShaderCode(byte[]code , bool isCompiled)
        {
            this.code = code;
            this.isCompiled = isCompiled;
        }

        public byte[] Code { get { return code; } }

        public bool IsCompiled { get { return isCompiled; } }

        public ShaderReflection Reflection { get { return shaderReflection; } set { shaderReflection = value; } }
    }

    public struct ShaderCompilationUnit<T>      
    {
        public ShaderCompilationUnit(ShaderCode code, T shader)
        {
            Code = code;
            Shader = shader;
        }

        public ShaderCode Code;
        public T Shader;
    }
}
