using System;
using Igneel.Graphics;

namespace Igneel.Rendering
{
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

        public EffectPassDesc WithGeometryShader<TOutput>(string filename, bool rasterizedStream0 = false)
            where TOutput : struct
        {
            Program.LinkGeometryShader<TOutput>(filename, rasterizedStream0);
            return this;
        }
    }
}