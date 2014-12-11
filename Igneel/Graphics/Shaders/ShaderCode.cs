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

        public ShaderCode(IntPtr ptrCode,  int sizeInBytes, bool isCompiled)
        {
            code = new byte[sizeInBytes];
            unsafe
            {
                fixed (byte* pCode = code)
                {
                    ClrPlatform.Crl.CopyMemory(ptrCode.ToPointer(), pCode, sizeInBytes);
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
    }

    public struct ShaderCompilationUnit<T>
        where T:Shader
    {
        public ShaderCode Code;
        public T Shader;
    }
}
