using System.Collections.Generic;
using Igneel.Graphics;

namespace Igneel.Rendering
{
    public class TechniqueDesc
    {
        protected GraphicDevice Device;

        public TechniqueDesc(GraphicDevice device)
        {
            this.Device = device;
        }

        public string Name { get; set; }
        public List<EffectPassDesc> Passes = new List<EffectPassDesc>();

        public virtual TechniqueDesc Pass(RasterizerState rasterizer = null,
            BlendState blend = null,
            DepthStencilState zbuffer = null,
            params string[] shaders)
        {
            Passes.Add(new EffectPassDesc(Device,rasterizer, blend, zbuffer, shaders));
            return this;
        }

        public TechniqueDesc Pass<TVert>(string vertexShader, params string[] shaders)
            where TVert : struct
        {
            EffectPassDesc desc = new EffectPassDesc(Device);
            desc.WithVertexShader<TVert>(vertexShader);
            for (int i = 0; i < shaders.Length; i++)
            {
                desc.WithShader(shaders[i]);
            }
            Passes.Add(desc);
            return this;
        }

        public TechniqueDesc Pass<TVert, TOut>(string vertexShader, string geometryShader, bool rasterizedStream0 , params string[] shaders)
            where TVert : struct
            where TOut :struct
        {
            EffectPassDesc desc = new EffectPassDesc(Device);
            desc.WithVertexShader<TVert>(vertexShader);
            desc.WithGeometryShader<TOut>(geometryShader, rasterizedStream0);

            for (int i = 0; i < shaders.Length; i++)
            {
                desc.WithShader(shaders[i]);
            }
            Passes.Add(desc);
            
            return this;
        }
        public TechniqueDesc Pass<TVert, TOut>(string vertexShader, string geometryShader, params string[] shaders)
            where TVert : struct
            where TOut : struct
        {
            return Pass<TVert, TOut>(vertexShader, geometryShader, false, shaders);
        }
    }
}