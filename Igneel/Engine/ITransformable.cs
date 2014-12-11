using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Drawing.Design;
using Igneel.Design;
using Igneel.Design.UITypeEditors;

namespace Igneel
{  
    
    public interface ITranslatable : IDeferreable
    {        

        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]
        Vector3 LocalPosition { get; set; }
    }

    public interface IPositionable
    {
        /// <summary>
        /// Get or set the global position
        /// </summary>
        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]
        Vector3 GlobalPosition { get; }
    }

    public interface IScalable : IDeferreable
    {
        [TypeConverter(typeof(DesignTypeConverter))]
        [Editor(typeof(UIVector3TypeEditor), typeof(UITypeEditor))]
        Vector3 LocalScale { get; set; }
    }

    public interface IRotable : IDeferreable
    {      
        [TypeConverter(typeof(DesignTypeConverter))]
        [EditorAttribute(typeof(UIRotationMatrixEditor), typeof(UITypeEditor))]
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
