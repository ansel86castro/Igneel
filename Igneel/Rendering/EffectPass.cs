using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{   
    public sealed class EffectPass
    {
        struct VariableBinding
        {
            public ShaderVariable Variable;
            public IUniformSetter Setter;

            public override string ToString()
            {
                return Variable.ToString();
            }
        }

        struct SamplerBind
        {
            public int Index;
            public IShaderStage Stage;
            public SamplerState Sampler;
        }

        internal int index;
        internal ShaderProgram program;
        List<VariableBinding> variables = new List<VariableBinding>();

        RasterizerState rState, oldrsState;
        BlendState blendState, oldBlendState;
        DepthStencilState zBufferState, oldzBufferState;
        //List<SamplerBind> samplerBinds = new List<SamplerBind>(0);

        public RasterizerState RState
        {
            get { return rState; }
            set { rState = value; }
        }       

        public BlendState BlendState
        {
            get { return blendState; }
            set { blendState = value; }
        }
       
        public DepthStencilState ZBufferState
        {
            get { return zBufferState; }
            set { zBufferState = value; }
        }

        public ShaderProgram Program { get { return program; } }

        public EffectPass(ShaderProgram pipeline)
        {
            this.program = pipeline;
        }

        public bool Apply(int technique, GraphicDevice device)
        {          
            if (rState != null)
            {
                oldrsState = device.Rasterizer;
                device.Rasterizer = rState;
            }
            if (blendState != null)
            {
                oldBlendState = device.Blend;
                device.Blend = blendState;
            }
            if (zBufferState != null)
            {
                oldzBufferState = device.DepthTest;
                device.DepthTest = zBufferState;
            }

            if (device.Program != program)
            {
                //bind variables to the pipeline
                foreach (var v in variables)
                {
                    v.Variable.SetSetter(v.Setter);
                }
                //setup Input Assembler Layout
                //device.IAInputLayout = program.InputDefinition;

                //Setup Shaders
                device.Program = program;                             

                return true;
            }

            return false;
        }

        public void Clear(Effect effect, GraphicDevice device)
        {
            if (oldrsState != null)
                device.Rasterizer = oldrsState;
            if (oldBlendState != null)
                device.Blend = oldBlendState;
            if (oldzBufferState != null)
                device.DepthTest = oldzBufferState;
        }

        //internal void OnClear(int tech, int pass)
        //{
        //    if (Clearded != null)
        //        Clearded(tech, pass);
        //    //foreach (var v in variables)
        //    //{
        //    //    v.Variable.Deferred = true;
        //    //}
        //}

        internal void AddVariable(string name, ShaderVariable v)
        {
            VariableBinding binding = new VariableBinding
            {
                Variable = v,                   
                Setter = program.CreateUniformSetter(name)
            };
            variables.Add(binding);
        }

        internal bool ContainsVariable(string name)
        {
            return variables.FindIndex(x => x.Variable.Name == name) >= 0;
        }
    }


    public class TechniqueDesc
    {
        protected GraphicDevice device;

        public TechniqueDesc(GraphicDevice device)
        {
            this.device = device;
        }

        public string Name { get; set; }
        public List<EffectPassDesc> Passes = new List<EffectPassDesc>();

        public virtual TechniqueDesc Pass(RasterizerState rasterizer = null,
                             BlendState blend = null,
                             DepthStencilState zbuffer = null,
                             params string[] shaders)
        {
            Passes.Add(new EffectPassDesc(device,rasterizer, blend, zbuffer, shaders));
            return this;
        }

        public TechniqueDesc Pass<TVert>(params string[] shaders)
             where TVert : struct
        {
            EffectPassDesc desc = new EffectPassDesc(device);
            desc.WithVertexShader<TVert>(shaders[0]);
            for (int i = 1; i < shaders.Length; i++)
            {
                desc.WithShader(shaders[i]);
            }
            Passes.Add(desc);
            return this;
        }
    }

    //public class TechniqueDesc<TVert> : TechniqueDesc
    //       where TVert : struct
    //{
    //    public TechniqueDesc(GraphicDevice device) : base(device) { }

    //    public TechniqueDesc Pass(params string[] shaders)       
    //    {
    //        EffectPassDesc desc = new EffectPassDesc(device);
    //        desc.WithVertexShader<TVert>(shaders[0]);
    //        for (int i = 1; i < shaders.Length; i++)
    //        {
    //            desc.WithShader(shaders[i]);
    //        }
    //        Passes.Add(desc);
    //        return this;
    //    }
    //}

   public class EffectPassDesc
    {
       public EffectPassDesc(GraphicDevice device)
       {
           Program = new ShaderProgramDesc(device);
       }
       public EffectPassDesc(GraphicDevice device,
                             RasterizerState rasterizer = null,
                             BlendState blend = null,
                             DepthStencilState zbuffer = null,
                             params string[] shaders)           
       {
           if (shaders == null) throw new ArgumentNullException("shaders");

           RState = rasterizer;
           BlendState = blend;
           ZBufferState = zbuffer;

           Program = new ShaderProgramDesc(device);
           foreach (var item in shaders)
           {
               Program.LinkShader(item);
           }
       }

        public ShaderProgramDesc Program { get; set; }
        public RasterizerState RState { get; set; }
        public BlendState BlendState { get; set; }
        public DepthStencilState ZBufferState { get; set; }

        public EffectPassDesc WithRasterizer(RasterizerState state)
        {
            RState = state;
            return this;
        }
        public EffectPassDesc WithBlending(BlendState state)
        {
            BlendState = state;
            return this;
        }
        public EffectPassDesc WithDepthTest(DepthStencilState state)
        {
            ZBufferState = state;
            return this;
        }
        public EffectPassDesc WithShader(string filename)            
        {
            Program.LinkShader(filename);
            return this;
        }

        public EffectPassDesc WithVertexShader<TInput>(string filename)
            where TInput : struct
        {
            Program.LinkVertexShader<TInput>(filename);
            return this;
        }
    }
}
