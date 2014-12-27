using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct ShadingInitialization
    {
        public IVertexShaderStage VS { get; set; }
        public IPixelShaderStage PS { get; set; }
        public IGeometryShaderStage GS { get; set; }
        public IHullShaderStage HS { get; set; }
        public IDomainShaderStage DS { get; set; }
        public IComputeShaderStage CS { get; set; }
    }

    public abstract partial class GraphicDevice
    {
        struct ShaderCacheEntry
        {
            public ShaderCode Code;
            public Shader Shader;
        }
           
        private ShaderProgram program;        
        private IVertexShaderStage vs;
        private IPixelShaderStage ps;
        private IGeometryShaderStage gs;
        private IHullShaderStage hs;
        private IDomainShaderStage ds;
        private IComputeShaderStage cs;

        private Dictionary<Type, IShaderStage> shaderStages;
        private Dictionary<string, ShaderCacheEntry> shaderCache = new Dictionary<string, ShaderCacheEntry>();
      

        public ShaderProgram Program 
        { 
            get { return program; }
            set
            {
                if (program != value)
                {
                    SetProgram(value);
                    program = value;
                    IAInputLayout = value.InputDefinition;
                }
            }
        }

        public IVertexShaderStage VS { get { return vs; } }

        public IPixelShaderStage PS { get { return ps; } }

        public IGeometryShaderStage GS { get { return gs; } }

        public IHullShaderStage HS { get { return hs; } }

        public IDomainShaderStage DS { get { return ds; } }

        public IComputeShaderStage CS { get { return cs; } }      

        protected void InitShading()
        {
            var ini = GetShadingInitialization();
            vs = ini.VS;
            ps = ini.PS;
            gs = ini.GS;
            hs = ini.HS;
            ds = ini.DS;
            cs = ini.CS;

            shaderStages = new Dictionary<Type, IShaderStage>(6)
            {
                {typeof(VertexShader), vs},
                {typeof(PixelShader), ps},
                {typeof(GeometryShader), gs},
                {typeof(HullShader), hs},
                {typeof(DomainShader), ds},
                {typeof(ComputeShader), cs},
            };         
        }

        protected abstract ShadingInitialization GetShadingInitialization();        

        public abstract ShaderProgram CreateProgram(ShaderProgramDesc desc);              

        protected abstract void SetProgram(ShaderProgram program);                    

        public IShaderStage<T> GetShaderStage<T>() where T : Shader
        {
            IShaderStage stage = null;
            shaderStages.TryGetValue(typeof(T), out stage);
            return (IShaderStage<T>)stage;
        }        

        public T CreateShader<T>(ShaderCode bytecode)
              where T : Shader
        {
            var stage = GetShaderStage<T>();
            if (stage == null)
                throw new NotSupportedException(GetNoSuportedString<T>());
            return stage.CreateShader(bytecode);
        }

        private static string GetNoSuportedString<T>() where T : Shader
        {
            return "The implementation of GraphicDevice does not support the creation of shaders of type \"" + typeof(T).Name + "\"";
        }

        public  ShaderCompilationUnit<T> CreateShader<T>(string filename, string functionName, ShaderMacro[] defines = null)
            where T : Shader
        {            
            ShaderCacheEntry entry;
            var stage = GetShaderStage<T>();
            if(stage == null)
                throw new NotSupportedException(GetNoSuportedString<T>());

            if (!shaderCache.TryGetValue(filename + ":" + functionName, out entry))
            {                
                var bytecode = stage.CompileFromFile(filename, functionName, defines);
                entry = new ShaderCacheEntry
                {
                    Code = bytecode,
                    Shader = CreateShader<T>(bytecode)
                };
                shaderCache[filename] = entry;
            }
            return new ShaderCompilationUnit<T>(entry.Code, (T)entry.Shader);
        }

        public ShaderCompilationUnit<T> CreateShader<T>(string filename, ShaderMacro[] defines = null)
            where T : Shader
        {
            ShaderCacheEntry entry;
            var stage = GetShaderStage<T>();
            if (stage == null)
                throw new NotSupportedException(GetNoSuportedString<T>());

            if (!shaderCache.TryGetValue(filename, out entry))
            {
                var locator = Service.Require<IShaderRepository>();
                string srcFile = locator.Locate(filename);
                string ext = Path.GetExtension(srcFile);
                ShaderCode code;

                if (ext == ".cso")
                {
                    code = new ShaderCode(File.ReadAllBytes(srcFile), true);
                }
                else
                {
                    code = stage.CompileFromFile(srcFile, "main", defines);
                }

                entry = new  ShaderCacheEntry { Shader = CreateShader<T>(code), Code = code };
                shaderCache[filename] = entry;
            }
            return new ShaderCompilationUnit<T>(entry.Code, (T)entry.Shader);
        }       
    }    
   

}
