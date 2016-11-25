
namespace Igneel.Graphics
{
    public interface IShaderFactory<T>       
    {        
        T CreateShader(ShaderCode bytecode);

        ShaderCompilationUnit<T> CreateShader(string filename, string functionName="main", ShaderMacro[] defines = null);

        ShaderCode CompileFromMemory(string shaderCode, string functionName = "main", ShaderMacro[] defines = null);

        ShaderCode CompileFromFile(string filename, string functionName = "main", ShaderMacro[] defines = null);
    }   
    
}