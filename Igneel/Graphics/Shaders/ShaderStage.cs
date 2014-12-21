using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract class ShaderStage
    {
        public struct ShaderStageInitialization
        {
            public int NbSamples { get; set; }
            public int NbShaderResources { get; set; }
        }

        private  SamplerState[] _samplers;
        private ShaderResource[] _resources;

        public ShaderStage()
        {
            var ini = GetStageInitialization();

            _samplers = new SamplerState[ini.NbSamples];
            _resources = new ShaderResource[ini.NbShaderResources];
        }

        protected abstract ShaderStageInitialization GetStageInitialization();

        public SamplerState GetSampler(int index)
        {
            return _samplers[index];
        }

        public void SetSampler(int index, SamplerState state)
        {           
            if (_samplers[index] != state)
            {
                _samplers[index] = state;
                OnSetSampler(index, state);
            }
        }

        public void SetSamplers(int index, SamplerState[] states)
        {         
            OnSetSamplers(index, states);
            for (int i = 0; i < states.Length && (index + i) < _samplers.Length; i++)
            {
                _samplers[index + i] = states[i];
            }  
        }

        public ShaderResource GetResource(int index)
        {
            return _resources[index];
        }

        public void GetResources(int index , ShaderResource[]resources)
        {            
            for (int i = 0; i < resources.Length && (index + i )< _resources.Length; i++)
            {
                resources[i] = _resources[index + i];
            }            
        }

        public void SetResource(int index, ShaderResource resource)
        {
            OnSetResource(index, resource);
            _resources[index] = resource;
        }

        public void SetResources(int index, ShaderResource[] resources)
        {
            SetResources(index, resources.Length, resources);
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

        protected abstract void OnSetSamplers(int slot, SamplerState[] state);

        protected abstract void OnSetResource(int index, ShaderResource resource);

        protected abstract void OnSetResources(int index, int nbResources ,ShaderResource[] resources);

        internal protected virtual void Dispose() { }
    }    

}
