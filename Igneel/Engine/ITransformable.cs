using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;





namespace Igneel
{  
    
    public interface ITranslatable : IDeferreable
    {        

       
        
        Vector3 LocalPosition { get; set; }
    }

    public interface IPositionable
    {
        /// <summary>
        /// Get or set the global position
        /// </summary>
       
        
        Vector3 GlobalPosition { get; }
    }

    public interface IScalable : IDeferreable
    {
       
        
        Vector3 LocalScale { get; set; }
    }

    public interface IRotable : IDeferreable
    {      
       
        
        Matrix LocalRotation { get; set; }

    }    

    public interface ITransformable : IPositionable, ITranslatable, IRotable, IScalable
    {

    }

    public static class Orientable
    {
        public static Euler GetEulerAngles(this IRotable o)
        {
            return Euler.FromMatrix(o.LocalRotation);
        }

        public static void SetEulerAngles(this IRotable o, Euler orientation)
        {
            o.LocalRotation = orientation.ToMatrix();
        }
    }

}
