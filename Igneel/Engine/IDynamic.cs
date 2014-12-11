using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Igneel.Components;


namespace Igneel
{
    public delegate void UpdateEventHandler(IDynamic sender, float deltaT);

    /// <summary>
    /// Represents an object that may move or change his state within the time
    /// </summary>
    public interface IDynamic
    {
        /// <summary>
        /// Update  the state of the object based on the elapsed time
        /// </summary>
        /// <param name="DeltaT">elapsed time since the last update expresed in seconds</param>
        void Update(float elapsedTime);
        
    }

    public interface IDynamicNotificable:IDynamic
    {
        event UpdateEventHandler UpdateEvent;
    }

    public static class DynamicUtils
    {
        public static T SetUpdate<T>(this T d, UpdateEventHandler callback) where T : IDynamicNotificable
        {            
            d.UpdateEvent += callback;            
            return d;
        }
    }

    public class Dynamic:IDynamic
    {
        Action<float> updateMethod;      

        public Dynamic(Action<float> updateMethod)
        {            
            this.updateMethod = updateMethod;
        }        

        public void Update(float elapsedTime)
        {
            updateMethod(elapsedTime);
        }

        public static implicit operator Dynamic(Action<float> action)
        {
            return new Dynamic(action);
        }
        
    }

}
