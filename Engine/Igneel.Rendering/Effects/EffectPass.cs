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

        internal int Index;
        internal ShaderProgram program;
        List<VariableBinding> _variables = new List<VariableBinding>();

        RasterizerState _rState, _oldrsState;
        BlendState _blendState, _oldBlendState;
        DepthStencilState _zBufferState, _oldzBufferState;
        //List<SamplerBind> samplerBinds = new List<SamplerBind>(0);

        public RasterizerState RState
        {
            get { return _rState; }
            set { _rState = value; }
        }       

        public BlendState BlendState
        {
            get { return _blendState; }
            set { _blendState = value; }
        }
       
        public DepthStencilState ZBufferState
        {
            get { return _zBufferState; }
            set { _zBufferState = value; }
        }

        public ShaderProgram Program { get { return program; } }

        public EffectPass(ShaderProgram pipeline)
        {
            this.program = pipeline;
        }

        public bool Apply(int technique, GraphicDevice device)
        {          
            if (_rState != null)
            {
                _oldrsState = device.Rasterizer;
                device.Rasterizer = _rState;
            }
            if (_blendState != null)
            {
                _oldBlendState = device.Blend;
                device.Blend = _blendState;
            }
            if (_zBufferState != null)
            {
                _oldzBufferState = device.DepthTest;
                device.DepthTest = _zBufferState;
            }

            if (device.Program != program)
            {
                //bind variables to the pipeline
                foreach (var v in _variables)
                {
                    var variable = v.Variable;
                    variable.Binder = v.Setter;
                    variable.SetValue();                 
                }
              
                device.Program = program;                             

                return true;
            }

            return false;
        }

        public void Clear(Effect effect, GraphicDevice device)
        {
            if (_oldrsState != null)
                device.Rasterizer = _oldrsState;
            if (_oldBlendState != null)
                device.Blend = _oldBlendState;
            if (_oldzBufferState != null)
                device.DepthTest = _oldzBufferState;
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
            if (binding.Setter == null)
                throw new NullReferenceException("The IUniformSetter is null");

            _variables.Add(binding);
        }

        internal bool ContainsVariable(string name)
        {
            return _variables.FindIndex(x => x.Variable.Name == name) >= 0;
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
}
