using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShaderProgramDesc
    {
        private List<Shader> shaders = new List<Shader>();
        private ReadOnlyCollection<Shader>shaderR;
        private Dictionary<Type, Shader> shaderLookup = new Dictionary<Type, Shader>();
        private GraphicDevice device;

        public GraphicDevice Device { get { return device; } }

        public ShaderProgramDesc(GraphicDevice device)
        {
            this.device = device;
            shaderR = shaders.AsReadOnly();            
        }
       
        public ReadOnlyCollection<Shader> Shaders { get{return shaderR;} }

        public void LinkShader<T>(T shader) where T : Shader
        {           
            shaders.RemoveAll(x => x.GetType() == typeof(T));         
            shaders.Add(shader);
            shaderLookup[typeof(T)] = shader;
        }

        public void LinkShader<TInput>(ShaderCompilationUnit<VertexShader>cunit)
            where TInput:struct
        {
            LinkShader(cunit.Shader);
            Input = Engine.Graphics.CreateInputLayout<TInput>(cunit.Code);
        }

        public void LinkShader(string filename)
        {
            if(filename.Length < 2)throw new ArgumentException();          

            string type = filename.Substring(filename.Length - 2, 2).ToLower();
            switch (type)
            {
                case "vs":
                    LinkShader(device.CreateShader<VertexShader>(filename).Shader);
                    break;
                case "ps":
                    LinkShader(device.CreateShader<PixelShader>(filename).Shader);
                    break;
                case "gs":
                    LinkShader(device.CreateShader<GeometryShader>(filename).Shader);
                    break;
                case "hs":
                    LinkShader(device.CreateShader<HullShader>(filename).Shader);
                    break;
                case "ds":
                    LinkShader(device.CreateShader<DomainShader>(filename).Shader);
                    break;
                case "cs":
                    LinkShader(device.CreateShader<ComputeShader>(filename).Shader);
                    break;
                default:
                    throw new ShaderCompileException("Invalid Shader Filename");                    
            }
        }

        public void LinkVertexShader<TInput>(string filename) where TInput :struct
        {
            if (filename.Length < 2) throw new ArgumentException();
           
            string type = filename.Substring(filename.Length - 2, 2).ToLower();
            switch (type)
            {
                case "vs":
                    ShaderCompilationUnit<VertexShader> cunit = device.CreateShader<VertexShader>(filename);
                    LinkShader<TInput>(cunit);                    
                    break;
                case "ps":
                    LinkShader(device.CreateShader<PixelShader>(filename).Shader);
                    break;
                case "gs":
                    LinkShader(device.CreateShader<GeometryShader>(filename).Shader);
                    break;
                case "hs":
                    LinkShader(device.CreateShader<HullShader>(filename).Shader);
                    break;
                case "ds":
                    LinkShader(device.CreateShader<DomainShader>(filename).Shader);
                    break;
                case "cs":
                    LinkShader(device.CreateShader<ComputeShader>(filename).Shader);
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
    }
}
