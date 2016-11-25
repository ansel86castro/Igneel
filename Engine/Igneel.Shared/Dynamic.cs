using System;

namespace Igneel
{
    public class Dynamic:IDynamic
    {
        readonly Action<float> _updateMethod;      

        public Dynamic(Action<float> updateMethod)
        {            
            this._updateMethod = updateMethod;
        }        

        public void Update(float elapsedTime)
        {
            _updateMethod(elapsedTime);
        }

        public static implicit operator Dynamic(Action<float> action)
        {
            return new Dynamic(action);
        }
        
    }
}