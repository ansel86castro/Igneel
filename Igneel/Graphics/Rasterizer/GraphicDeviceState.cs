using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract class GraphicDeviceState<T> :ResourceAllocator
        where T : struct
    {
        protected T _state;

        public GraphicDeviceState() { }

        public GraphicDeviceState(T state)
        {
            this._state = state;
        }

        public T State { get { return _state; } }        

        public GraphicDeviceState<T> Clone()
        {
            return (GraphicDeviceState<T>)MemberwiseClone();
        }
       
    }
   
}
