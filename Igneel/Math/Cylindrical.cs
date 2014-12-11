
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public struct CyCoord
    {
        public float Theta;
        public float Y;
        public float Radius;

        public CyCoord(float theta, float y, float radius)
        {
            this.Theta = theta;
            this.Y = y;
            this.Radius = radius;
        }

        public Vector3 ToCartesian()
        {
            return new Vector3(Radius * (float)Math.Cos(Theta), Y, Radius * (float)Math.Sin(Theta));
        }

        public static CyCoord FromCartesian(Vector3 v)
        {
            float radius = (float)Math.Sqrt(v.X * v.X + v.Z * v.Z);
            if (radius != 0)
            {
                float theta = (float)Math.Acos(v.X / radius);
                return new CyCoord(theta, v.Y, radius);
            }
            return new CyCoord(0, v.Y, 0);
        }

        public override string ToString()
        {
            return Theta + "  ," + Y + " ," + Radius;
        }
    }
}
