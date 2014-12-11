using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShaderProgramDesc
    {
        private List<ShaderHandler> shaders = new List<ShaderHandler>();
        private Dictionary<Type, Shader> shaderLookup = new Dictionary<Type, Shader>();

       
        public void SetShader<T>(T shader)
           where T : Shader
        {
            var setter = ShaderProgram.ShaderStore<T>.Setter;
            if (setter == null)
                throw new NotSupportedException("This implementation doesn't support shader of type \"" + typeof(T).Name + "\"");

            shaders.RemoveAll(x => x.GetType() == typeof(T));
            var handle = setter.GetHandler(shader);
            shaders.Add(handle);
            shaderLookup[typeof(T)] = shader;
        }

        public void SetShader<TInput>(ShaderCompilationUnit<VertexShader>cunit)
            where TInput:struct
        {
            SetShader(cunit.Shader);
            Input = Engine.Graphics.CreateInputLayout<TInput>(cunit.Code);
        }

        public void SetShader(string filename)
        {
            if(filename.Length < 2)throw new ArgumentException();

            string type = filename.Substring(filename.Length - 2, 2).ToLower();
            switch (type)
            {
                case "vs":
                   SetShader(ShaderProgram.CreateShader<VertexShader>(filename).Shader);
                    break;
                case "ps":
                    SetShader(ShaderProgram.CreateShader<PixelShader>(filename).Shader);
                    break;
                case "gs":
                    SetShader(ShaderProgram.CreateShader<GeometryShader>(filename).Shader);
                    break;
                case "hs":
                    SetShader(ShaderProgram.CreateShader<HullShader>(filename).Shader);
                    break;
                case "ds":
                    SetShader(ShaderProgram.CreateShader<DomainShader>(filename).Shader);
                    break;
                case "cs":
                    SetShader(ShaderProgram.CreateShader<ComputeShader>(filename).Shader);
                    break;
                default:
                    throw new ShaderCompileException("Invalid Shader Filename");                    
            }
        }

        public void SetShader<TInput>(string filename)
            where TInput :struct
        {
            if (filename.Length < 2) throw new ArgumentException();

            string type = filename.Substring(filename.Length - 2, 2).ToLower();
            switch (type)
            {
                case "vs":
                    ShaderCompilationUnit<VertexShader> cunit = ShaderProgram.CreateShader<VertexShader>(filename);
                    SetShader<TInput>(cunit);                    
                    break;
                case "ps":
                    SetShader(ShaderProgram.CreateShader<PixelShader>(filename).Shader);
                    break;
                case "gs":
                    SetShader(ShaderProgram.CreateShader<GeometryShader>(filename).Shader);
                    break;
                case "hs":
                    SetShader(ShaderProgram.CreateShader<HullShader>(filename).Shader);
                    break;
                case "ds":
                    SetShader(ShaderProgram.CreateShader<DomainShader>(filename).Shader);
                    break;
                case "cs":
                    SetShader(ShaderProgram.CreateShader<ComputeShader>(filename).Shader);
                    break;
                default:
                    throw new ShaderCompileException("Invalid Shader Filename");
            }
        }

        public InputLayout Input { get; set; }

        public T GetShader<T>() where T : Shader
        {
            return (T)shaderLookup[typeof(T)];
        }

        public ShaderHandler[] GetHandles()
        {
            return shaders.ToArray();
        }
       
      
    }
}
