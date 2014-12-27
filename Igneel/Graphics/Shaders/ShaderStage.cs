using Igneel.Collections;
using System;
using System.Collections.Generic;
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
        private ShaderResource[] _resources;
        private ResourceCollecion<SamplerStateStack> samplerStacks;
        private ResourceCollecion<SamplerState> samplersCollection;
        private ResourceCollecion<ShaderResource> resourcesCollection;

        public ShaderStage()
        {
            var ini = GetStageInitialization();

            _samplers = new SamplerState[ini.NbSamples];
            _resources = new ShaderResource[ini.NbShaderResources];

            SamplerStateStack[] stacks = new SamplerStateStack[ini.NbSamples];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = new SamplerStateStack(i, this);
            }

            samplerStacks = new ResourceCollecion<SamplerStateStack>(stacks);
            samplersCollection = new ResourceCollecion<SamplerState>(_samplers);
            resourcesCollection = new ResourceCollecion<ShaderResource>(_resources);
        }

        protected abstract ShaderStageInitialization GetStageInitialization();

        public ResourceCollecion<SamplerStateStack> SamplerStacks { get { return samplerStacks; } }

        public ResourceCollecion<SamplerState> Samplers { get { return samplersCollection; } }

        public ResourceCollecion<ShaderResource> Resources { get { return resourcesCollection; } }      

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

        public void SetResource(int index, ShaderResource resource)
        {
            OnSetResource(index, resource);
            _resources[index] = resource;
        }      

        public void SetResources(int index, int numResources, ShaderResource[] resources)
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

        protected abstract void OnSetResource(int index, ShaderResource resource);

        protected abstract void OnSetResources(int index, int nbResources ,ShaderResource[] resources);

        internal protected virtual void Dispose() { }
    }

    public static class ShaderStageEx
    {
        public static void SetSamplers(this IShaderStage stage, int index, SamplerState[] states)
        {
            stage.SetSamplers(index, states.Length, states);
        }

        public static void SetResources(this IShaderStage stage, int index, ShaderResource[] resources)
        {
            stage.SetResources(index, resources.Length, resources);
        }
    }

}
