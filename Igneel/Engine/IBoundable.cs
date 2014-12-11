using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Igneel.Design;

namespace Igneel
{
    public interface IBoundable
    {
        Sphere BoundingSphere { get; }
        
        OrientedBox BoundingBox { get; }       
    }

    public static class Boundable
    {
        public static CullTestResult GetCullState(this IBoundable obj, Plane[] planes)
        {
            return Sphere.GetCullTest(obj.BoundingSphere, planes);
        }

        public static bool IsInsideFrustum(this IBoundable obj, Plane[] planes)
        {
            return Sphere.IsInsideFrustum(obj.BoundingSphere, planes);
        }

        public static bool IsInsideRect(this IBoundable obj, RectangleF rect)
        {
            return Sphere.IsInsideRect(obj.BoundingSphere, rect);
        }
    }
}
