using Igneel.Design;
using Igneel.Design.UITypeEditors;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    [TypeConverter(typeof(DesignTypeConverter))]
    public struct Spherical
    {

        /// <summary>
        /// Angle respect to the X axis (Pitch)
        /// </summary>
        /// 
        [Browsable(false)]
        public float Theta;

        /// <summary>
        /// Angle respect to the Y axis (Heading)
        /// </summary>
        /// 
        [Browsable(false)]
        public float Phi;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theta">pitch</param>
        /// <param name="phi">heading</param>
        public Spherical(float theta, float phi)
        {
            this.Theta = theta;
            this.Phi = phi;
        }

        [Editor(typeof(UIAngleTypeEditor), typeof(UITypeEditor))]
        [Description("Angle respect to the X axis (Pitch)")]
        public float ThetaAngle
        {
            get
            {
                return Euler.ToAngle(Theta);
            }
            set
            {
                Theta = Euler.ToRadians(value);
            }
        }

        [Editor(typeof(UIAngleTypeEditor), typeof(UITypeEditor))]
        [Description("Angle respect to the Y axis (Heading)")]
        public float PhiAngle
        {
            get
            {
                return Euler.ToAngle(Phi);
            }
            set
            {
                Phi = Euler.ToRadians(value);
            }
        }

        public Vector3 ToCartesian()
        {
            float b = (float)Math.Sin(Theta);
            return new Vector3(b * (float)Math.Sin(Phi), (float)Math.Cos(Theta), b * (float)Math.Cos(Phi));
        }

        public static Spherical FromDirection(Vector3 direction)
        {
            Spherical s = new Spherical();
            s.Theta = (float)Math.Acos(direction.Y);
            if (direction.Z != 0)
                s.Phi = (float)Math.Atan(direction.X / direction.Z);
            //float a = (float)Math.Sin(s.Theta);
            //if (a > 0)
            //    s.Phi = (float)Math.Asin(direction.X / a);            
            return s;
        }

        public static Vector3 ToCartesian(float theta, float phi)
        {
            float b = (float)Math.Sin(theta);
            return new Vector3(b * (float)Math.Sin(phi), (float)Math.Cos(theta), b * (float)Math.Cos(phi));
        }

        public static Spherical FromGrades(float theta, float pi)
        {
            return new Spherical(Euler.ToRadians(theta), Euler.ToRadians(pi));
        }

        public override string ToString()
        {
            return ThetaAngle + " ," + PhiAngle;
        }
    }
}
