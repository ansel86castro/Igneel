using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract class StateStack<T>
    {        
        private Stack<T> states = new Stack<T>();                  

        protected abstract void Set(T state);

        public void Push(T state)
        {
            states.Push(state);
            Set(state);
        }

        public T Pop()
        {
            var s = states.Pop();
            var previus = states.Count > 0 ? states.Peek() : default(T);
            Set(previus);
            return s;
        }
    }

    public class RasterizerStateStack : StateStack<RasterizerState>
    {
        private GraphicDevice device;
        public RasterizerStateStack(GraphicDevice device) { this.device = device; }

        public GraphicDevice Device { get { return device; } }

        protected override void Set(RasterizerState state)
        {
            device.RSState = state;
        }
    }

    public class DepthStencilStateStack : StateStack<DepthStencilState>
    {
        private GraphicDevice device;
        public DepthStencilStateStack(GraphicDevice device) { this.device = device; }

        public GraphicDevice Device { get { return device; } }

        protected override void Set(DepthStencilState state)
        {
            device.OMDepthStencilState = state;
        }
    }

    public class BlendStateStack : StateStack<BlendState>
    {
          private GraphicDevice device;
          public BlendStateStack(GraphicDevice device) { this.device = device; }

          public GraphicDevice Device { get { return device; } }

          protected override void Set(BlendState state)
        {
            device.OMBlendState = state;
        }
    }

    public class SamplerStateStack : StateStack<SamplerState>
    {
        private int index;
        private IShaderStage stage;

        public SamplerStateStack(int index, IShaderStage stage)            
        {
            this.index = index;
            this.stage = stage;
        }

        public int Index { get { return index; } }

        public IShaderStage Stage { get { return stage; } }

        protected override void Set(SamplerState state)
        {
            stage.SetSampler(index, state);
        }
    }    

   
}
