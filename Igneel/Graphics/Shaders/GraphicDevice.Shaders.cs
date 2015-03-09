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
           
        private ShaderProgram program;        
        private IVertexShaderStage vs;
        private IPixelShaderStage ps;
        private IGeometryShaderStage gs;
        private IHullShaderStage hs;
        private IDomainShaderStage ds;
        private IComputeShaderStage cs;

        private Dictionary<Type, IShaderStage> shaderStages;        

        public ShaderProgram Program 
        { 
            get { return program; }
            set
            {
                if (program != value)
                {
                    SetProgram(value);
                    program = value;
                    InputDefinition = value != null ? value.InputDefinition : null;
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

        protected abstract void SetProgram(ShaderProgram program);

        public abstract ShaderProgram CreateProgram(ShaderProgramDesc desc);              

        public IShaderStage<T> GetShaderStage<T>()
        {
            IShaderStage stage = null;
            shaderStages.TryGetValue(typeof(T), out stage);
            return (IShaderStage<T>)stage;
        }

        public IShaderStage GetShaderStage(Type type)
        {            
            IShaderStage stage = null;
            shaderStages.TryGetValue(type, out stage);
            return stage;
        }

        public IShaderStage GetShaderStage(Shader shader)
        {
            if (shader is VertexShader)
                return vs;            
            else if (shader is PixelShader)
                return ps;
            else if (shader is GeometryShader)
                return gs;
            else if (shader is HullShader)
                return hs;
            else if (shader is DomainShader)
                return ds;
            else if (shader is ComputeShader)
                return cs;
            else
                return null;
        }
     
    }    
   

}
