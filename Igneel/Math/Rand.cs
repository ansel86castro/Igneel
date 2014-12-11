using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Igneel
{
    
    public class Rand
    {
        static Random r = new Random();

        public static float Uniform01()
        {
            return (float)r.NextDouble();
        }

        public static float Uniform(float a, float b)
        {
            return a + (float)r.NextDouble() * (b - a);
        }

        public static Vector3 RandomUnitVector()
        {
            return Vector3.Normalize(new Vector3(Uniform(-1, 1), Uniform(-1, 1), Uniform(-1, 1)));
        }
    }              

    
}
