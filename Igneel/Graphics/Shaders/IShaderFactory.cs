
namespace Igneel.Graphics
{
    public interface IShaderFactory<T>
        where T:Shader
    {        
        T CreateShader(ShaderCode bytecode);                     

        ShaderCode CompileFromMemory(string shaderCode,string functionName ,ShaderMacro[] defines);

        ShaderCode CompileFromFile(string filename, string functionName, ShaderMacro[] defines);
    }   
    
}