
namespace Igneel.Graphics
{
    public interface IShaderFactory<T>
        where T:Shader
    {        
        T CreateShader(ShaderCode bytecode);

        void SetShader(T shader);

        ShaderHandler GetHandler(T shader);        

        ShaderCode CompileFromMemory(string shaderCode,string functionName ,ShaderMacro[] defines);

        ShaderCode CompileFromFile(string filename, string functionName, ShaderMacro[] defines);
    }

    public abstract class ShaderHandler
    {
        public Shader Function;
        public abstract void Set();        
    }
    
}