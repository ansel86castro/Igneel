using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract class ShaderStage : IShaderStage
    {
      
        public struct ShaderStageInitialization
        {
            public int NbSamples { get; set; }
            public int NbShaderResources { get; set; }
        }

        private  SamplerState[] _samplers;
        private IShaderResource[] _resources;
        private ResourceCollecion<SamplerStateStack> samplerStacks;
        private ResourceCollecion<SamplerState> samplersCollection;
        private ResourceCollecion<IShaderResource> resourcesCollection;

        public ShaderStage()
        {
            var ini = GetStageInitialization();

            _samplers = new SamplerState[ini.NbSamples];
            _resources = new IShaderResource[ini.NbShaderResources];

            SamplerStateStack[] stacks = new SamplerStateStack[ini.NbSamples];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = new SamplerStateStack(i, this);
            }

            samplerStacks = new ResourceCollecion<SamplerStateStack>(stacks);
            samplersCollection = new ResourceCollecion<SamplerState>(_samplers);
            resourcesCollection = new ResourceCollecion<IShaderResource>(_resources);
        }

        protected abstract ShaderStageInitialization GetStageInitialization();

        public ResourceCollecion<SamplerStateStack> SamplerStacks { get { return samplerStacks; } }

        public ResourceCollecion<SamplerState> Samplers { get { return samplersCollection; } }

        public ResourceCollecion<IShaderResource> Resources { get { return resourcesCollection; } }      

        public void SetSampler(int index, SamplerState state)
        {           
            if (_samplers[index] != state)
            {
                _samplers[index] = state;
                OnSetSampler(index, state);
            }
        }        

        public void SetSamplers(int index, int numSamplers, SamplerState[] states)
        {         
            OnSetSamplers(index, numSamplers, states);
            for (int i = 0; i < numSamplers && (index + i) < _samplers.Length; i++)
            {
                _samplers[index + i] = states[i];
            }  
        }       

        public void SetResource(int index, IShaderResource resource)
        {
            OnSetResource(index, resource);
            _resources[index] = resource;
        }      

        public void SetResources(int index, int numResources, IShaderResource[] resources)
        {
            OnSetResources(index, numResources, resources);
            if (resources == null)
            {
                Array.Clear(_resources, index, numResources);
            }
            else
            {
                Array.ConstrainedCopy(resources, 0, _resources, index, numResources);
            }
            //for (int i = 0; i < resources.Length && (index + i) < _resources.Length; i++)
            //{
            //    _resources[index + i] = resources[i];
            //}  
        }
        
        protected abstract void OnSetSampler(int slot, SamplerState state);

        protected abstract void OnSetSamplers(int slot, int numSamplers, SamplerState[] state);

        protected abstract void OnSetResource(int index, IShaderResource resource);

        protected abstract void OnSetResources(int index, int nbResources ,IShaderResource[] resources);

        internal protected virtual void Dispose() { }       
    }

    public abstract class ShaderStage<T> : ShaderStage, IShaderStage<T>        
    {
        protected struct ShaderCacheEntry
        {
            public ShaderCode Code;
            public T Shader;
        }

        private Dictionary<string, ShaderCacheEntry> shaderCache = new Dictionary<string, ShaderCacheEntry>();

        public ShaderCompilationUnit<T> CreateShader(string filename, string functionName = "main", ShaderMacro[] defines = null)
        {
            ShaderCacheEntry entry;
            if (!shaderCache.TryGetValue(filename, out entry))
            {
                var locator = Service.Get<IShaderRepository>();
                string srcFile = locator.Locate(filename);
                string ext = Path.GetExtension(srcFile);
                ShaderCode code;

                if (ext == ".cso")
                {
                    code = new ShaderCode(File.ReadAllBytes(srcFile), true);
                }
                else
                {
                    code = CompileFromFile(srcFile, functionName, defines);
                }

                entry = new ShaderCacheEntry { Shader = CreateShader(code), Code = code };
                shaderCache[filename] = entry;
            }
            return new ShaderCompilationUnit<T>(entry.Code, (T)entry.Shader);
        }

        public abstract T CreateShader(ShaderCode bytecode);                

        public abstract ShaderCode CompileFromMemory(string shaderCode, string functionName = "main", ShaderMacro[] defines = null);

        public abstract ShaderCode CompileFromFile(string filename, string functionName="main", ShaderMacro[] defines = null);

        //public ShaderCode CompileFile(string filename, string functionName = "main", ShaderMacro[] defines = null)
        //{
        //    var compiler = new HLSLCompiler();
        //    compiler.Sources = new string[] { tbSource.Text };
        //    compiler.IncludePath = new string[] { @"E:\Projects\Igneel\ShaderCompiler\Shaders.D3D10\Shaders.D3D10" };
        //    compiler.Compile();

        //    if (_compiler.Errors.Count > 0)
        //    {
        //        var sb = new StringBuilder();
        //        foreach (var item in _compiler.Errors)
        //        {
        //            sb.AppendLine(item);
        //        }
        //        MessageBox.Show(sb.ToString(), "Errors");
        //    }
        //    else
        //    {
        //        tbCompiled.Text = _compiler.GenerateCode();
        //    }
        //}

     
    }

    public static class ShaderStageEx
    {
        public static void SetSamplers(this IShaderStage stage, int index, SamplerState[] states)
        {
            stage.SetSamplers(index, states.Length, states);
        }

        public static void SetResources(this IShaderStage stage, int index, IShaderResource[] resources)
        {
            stage.SetResources(index, resources.Length, resources);
        }
    }

}
