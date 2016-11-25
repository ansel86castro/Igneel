using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract class PipelineState<T> :ResourceAllocator
        where T : struct
    {
        protected T _state;

        public PipelineState() { }

        public PipelineState(T state)
        {
            this._state = state;
        }

        public T State { get { return _state; } }        

        public PipelineState<T> Clone()
        {
            return (PipelineState<T>)MemberwiseClone();
        }
       
    }
   
}
