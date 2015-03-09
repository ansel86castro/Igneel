

using Igneel.Graphics;

using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public enum CullTestResult { Inside, Partial, Outside }

    public enum InsideTestResult
    {
        Inside,
        Outside,
        PartialInside,
        Contained
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere :IEquatable<Sphere>
    {        
        public Vector3 Center;
        public float Radius;

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Sphere(Vector3[] positions)
        {
            Center = new Vector3();
            Radius = 0;
            unsafe
            {
                fixed (Vector3* pPosition = positions)
                {
                    CreateBoundingSphere((byte*)pPosition, positions.Length, sizeof(Vector3));
                }
            }
        }     

        public unsafe Sphere(byte* positions, int vertexCount, int stride)
        {
            Center = new Vector3();
            Radius = 0;
            CreateBoundingSphere(positions, vertexCount, stride);
        }

        unsafe private void CreateBoundingSphere(byte* positions, int vertexCount, int stride)
        {
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);
            Vector3 R = eigenVectors[0];
            float min = float.MaxValue, max = float.MinValue;
            Vector3 minPoint = Vector3.Zero;
            Vector3 maxPoint = Vector3.Zero;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                float t = Vector3.Dot(*pter, R);
                if (t < min)
                {
                    min = t;
                    minPoint = *pter;
                }
                if (t > max)
                {
                    max = t;
                    maxPoint = *pter;
                }
            }

            Center = 0.5f * (maxPoint + minPoint);
            Radius = Vector3.Distance(maxPoint, Center);
            float r2 = Radius * Radius;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 pos = *(Vector3*)(positions + i * stride);
                Vector3 d = pos - Center;
                if (d.LengthSquared() > r2)
                {
                    d.Normalize();
                    Vector3 g = Center - Radius * d;
                    Center = 0.5f * (g + pos);
                    Radius = Vector3.Distance(pos, Center);
                    r2 = Radius * Radius;
                }
            }
        }      

        public unsafe Sphere GetTranformed(Matrix matrix)
        {
            Sphere sphere = new Sphere();
            Vector3 center = Center;
            Vector3.TransformCoordinates(ref center, ref matrix, out sphere.Center);
            if (Radius > 0)
            {
                sphere.Radius = Math.Max(new Vector3(Radius * matrix.M11, Radius * matrix.M12, Radius * matrix.M13).Length(),
                                         Math.Max(new Vector3(Radius * matrix.M21, Radius * matrix.M22, Radius * matrix.M23).Length(),
                                                  new Vector3(Radius * matrix.M31, Radius * matrix.M32, Radius * matrix.M33).Length()));
            }
            return sphere;
        }

        public unsafe void GetTranformed(Matrix matrix, out Vector3 center, out float radius)
        {
            radius = 0;
            Vector3 _center = Center;
            Vector3.Transform(ref _center, ref matrix, out center);
            if (Radius > 0)
            {
                radius = Math.Max(new Vector3(Radius * matrix.M11, Radius * matrix.M12, Radius * matrix.M13).Length(),
                                         Math.Max(new Vector3(Radius * matrix.M21, Radius * matrix.M22, Radius * matrix.M23).Length(),
                                                  new Vector3(Radius * matrix.M31, Radius * matrix.M32, Radius * matrix.M33).Length()));
            }
        }

        public SphereBuilder GetGeometry(int stack = 16, int slices = 16)
        {
            SphereBuilder sphere = new SphereBuilder(stack, slices, Radius);
            for (int i = 0; i < sphere.Vertices.Length; i++)
            {
                var mat = Matrix.Translate(Center);
                sphere.Vertices[i].Position = Vector3.Transform(sphere.Vertices[i].Position, mat);
            }

            return sphere;
        }

        public static CullTestResult GetCullTest(Sphere sphere, Plane[] planes)
        {
            if (sphere.Radius == 0) return CullTestResult.Inside;

            var globalPosition = sphere.Center;
            var radius = sphere.Radius;
            float distance;
            int count = 0;
            for (int i = 0; i < planes.Length; i++)
            {
                distance = Plane.DotCoordinate(planes[i], globalPosition);
                if (distance <= -radius)
                    return CullTestResult.Outside;
                if (distance >= radius)
                    count++;
            }
            return count == planes.Length ? CullTestResult.Inside : CullTestResult.Partial;
        }

        public static bool IsInsideFrustum(Sphere sphere, Plane[] planes)
        {
            var cullState = GetCullTest(sphere, planes);
            return cullState == CullTestResult.Inside || cullState == CullTestResult.Partial;
        }

        public static bool IsInsideRect(Sphere sphere, RectangleF rect)
        {
            var globalPosition = sphere.Center;
            var radius = sphere.Radius;
            // Check to see if the bounding circle around the model 
            // intersects this rectangle. 
            float center_x = rect.X + rect.Width * 0.5f;
            float center_z = rect.Y - rect.Height * 0.5f;

            float delta_x = center_x - globalPosition.X;
            float delta_z = center_z - globalPosition.Z;

            float distance_squared = delta_x * delta_x + delta_z * delta_z;

            float combined_radius = (radius * radius) + (rect.Width * rect.Width);

            return distance_squared < combined_radius;
        }

        public bool IsInsideRect(RectangleF rect)
        {
           var radius = Radius;
            // Check to see if the bounding circle around the model 
            // intersects this rectangle. 
            float rec_center_x = rect.X + rect.Width * 0.5f;
            float rec_center_z = rect.Y - rect.Height * 0.5f;

            float delta_x = rec_center_x - Center.X;
            float delta_z = rec_center_z - Center.Z;

            float distance_squared = delta_x * delta_x + delta_z * delta_z;

            float combined_radius = (radius * radius) + (rect.Width * rect.Width);

            return distance_squared < combined_radius;
        }        

        public bool IsInsideFrustum(Plane[] planes)
        {
            var cullState = GetCullTest(new Sphere(Center, Radius), planes);
            return cullState == CullTestResult.Inside || cullState == CullTestResult.Partial;
        }

        public bool Equals(Sphere other)
        {
            return Center == other.Center && Radius == other.Radius;
        }

        public override bool Equals(object obj)
        {
            if (obj is Sphere)
                return Equals((Sphere)obj);
            return false;
        }
       
        public override int GetHashCode()
        {
            return Center.GetHashCode() + Radius.GetHashCode();
        }

        public override string ToString()
        {
            return "Center:" + Center + " Radius:" + Radius;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box:IEquatable<Box>
    {
        public Vector3 Translation;
        public Vector3 Extends;
        public Matrix Rotation;

        public Box(Vector3 translation, Vector3 extends, Matrix rotation)
        {
            this.Translation = translation;
            this.Extends = extends;
            this.Rotation = rotation;
        }

        public OrientedBox ToVolume()
        {
            return new OrientedBox(Translation, Extends, Rotation);
        }

        public BoxBuilder GetGeometry()
        {
            BoxBuilder box = new BoxBuilder(2, 2, 2);

            for (int i = 0; i < box.Vertices.Length; i++)
            {
                var mat = Matrix.Scale(Extends) * Rotation;
                box.Vertices[i].Position= Vector3.Transform(box.Vertices[i].Position, mat);
            }
            return box;
        }

        public bool Equals(Box other)
        {
            return Translation == other.Translation && Extends == other.Extends && Rotation == other.Rotation;
        }

        public override string ToString()
        {
            return "T:" + Translation + ", E:" + Extends;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment
    {
       public Vector3 P0;		//!< Start of segment
       public Vector3 P1;		//!< End of segment

       public override string ToString()
       {
           return "P0:" + P0 + " P1:" + P1;
       }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule
    {
        public Segment Segment;
        public float Radius;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct AABB
    {
        public Vector3 Minimum;
        public Vector3 Maximum;

        public AABB(Vector3 minumun, Vector3 maximun)
        {
            this.Minimum = minumun;
            this.Maximum = maximun;
        }
        
        //public CullTestResult CullTest(Matrix viewProj)
        //{
        //     //transform the 8 points of the box to clip space and clip these point
        //     //if the test fail then transform the 6 faces of the box to clip space and clip these faces
        //    unsafe
        //    {
        //        Vector4* corners = stackalloc Vector4[8];
        //        corners[0]
        //    }
        //}

        public override string ToString()
        {
            return "Min:" + Minimum + " Max:" + Maximum;
        }
    }
}
