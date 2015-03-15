using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public enum FrustumPlane { Front, Back, Left, Right, Top, Bottom }

    public class Frustum
    {
        const int TestPlanes = 4;

        private static Vector3[] corners = new Vector3[8];

        internal Vector3[] cornersWorld = new Vector3[8];
        internal Plane[] planes = new Plane[6];        

        public Frustum()
        {
            planes = new Plane[6];                   
            cornersWorld = new Vector3[8];          
        }

        static Frustum()
        {           
            corners[0] = new Vector3(-1.0f, -1.0f, 0.0f); // xyz 
            corners[1] = new Vector3(1.0f, -1.0f, 0.0f);  // Xyz 
            corners[2] = new Vector3(-1.0f, 1.0f, 0.0f);  // xYz 
            corners[3] = new Vector3(1.0f, 1.0f, 0.0f);   // XYz 
            corners[4] = new Vector3(-1.0f, -1.0f, 1.0f); // xyZ
            corners[5] = new Vector3(1.0f, -1.0f, 1.0f);  // XyZ 
            corners[6] = new Vector3(-1.0f, 1.0f, 1.0f);  // xYZ 
            corners[7] = new Vector3(1.0f, 1.0f, 1.0f);   // XYZ 
        }

        public Plane this[FrustumPlane plane]
        {
            get { return planes[IndexOfPlane(plane)]; }
        }

        public Plane[] Planes { get { return planes; } }

        public Vector3[] Corners { get { return cornersWorld; } }

        public int IndexOfPlane(FrustumPlane plane)
        {
            switch (plane)
            {
                case FrustumPlane.Front: return 3;
                case FrustumPlane.Back: return 2;
                case FrustumPlane.Left: return 1;
                case FrustumPlane.Right: return 0;
                case FrustumPlane.Top: return 4;
                case FrustumPlane.Bottom: return 5;
            }

            return -1;
        }

        public void Transform(Matrix invViewProj)
        {
            unsafe
            {               
                for (int i = 0; i < 8; i++)
                    Vector3.TransformCoordinates(ref corners[i], ref invViewProj, out cornersWorld[i]);

                planes[0] = new Plane(cornersWorld[7], cornersWorld[3], cornersWorld[5]); // Right
                planes[1] = new Plane(cornersWorld[2], cornersWorld[6], cornersWorld[4]); // Left
                planes[2] = new Plane(cornersWorld[6], cornersWorld[7], cornersWorld[5]); // Far
                planes[3] = new Plane(cornersWorld[0], cornersWorld[1], cornersWorld[2]); // Near
                planes[4] = new Plane(cornersWorld[2], cornersWorld[3], cornersWorld[6]); // Top
                planes[5] = new Plane(cornersWorld[1], cornersWorld[0], cornersWorld[4]); // Bottom
            }
        }

        public static void CreatePlanes(Plane[] planes, Vector3[] corners)
        {
            planes[0] = new Plane(corners[7], corners[3], corners[5]); // Right
            planes[1] = new Plane(corners[2], corners[6], corners[4]); // Left
            planes[2] = new Plane(corners[6], corners[7], corners[5]); // Far
            planes[3] = new Plane(corners[0], corners[1], corners[2]); // Near
            planes[4] = new Plane(corners[2], corners[3], corners[6]); // Top
            planes[5] = new Plane(corners[1], corners[0], corners[4]); // Bottom
        }

        public CullTestResult GetCullResult(Sphere sphere)
        {
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;

            float distance;
            int count = 0;
            // Don’t check against top and bottom. 
            for (int i = 0; i < TestPlanes; i++)
            {
                //distancia del punto al plano , positiva si el punto esta en la direccion de la normal y negativa en otro caso
                distance = planes[i].DotCoordinate(center);

                //if distance > radio and distance < 0 then the sphere is on the negative side of the plane
                if (distance <= -radius)
                    return CullTestResult.Outside;

                //if the sphere is on the posisive side of the plane 
                if (distance >= radius)
                    count++;
            }

            return count == TestPlanes ? CullTestResult.Inside : CullTestResult.Partial;
        }

        public static CullTestResult GetContainment(Plane[]planes, Sphere sphere)
        {
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;

            float distance;
            int count = 0;
            // Don’t check against top and bottom. 
            for (int i = 0; i < planes.Length; i++)
            {
                //distancia del punto al plano , positiva si el punto esta en la direccion de la normal y negativa en otro caso
                distance = planes[i].DotCoordinate(center);

                //if distance > radio and distance < 0 then the sphere is on the negative side of the plane
                if (distance <= -radius)
                    return CullTestResult.Outside;

                //if the sphere is on the posisive side of the plane 
                if (distance >= radius)
                    count++;
            }

            return count == planes.Length ? CullTestResult.Inside : CullTestResult.Partial;
        }

        public bool Contains(Sphere sphere)
        {
            var cont = GetCullResult(sphere);
            return cont == CullTestResult.Inside || cont == CullTestResult.Partial;
        }
    }
}
