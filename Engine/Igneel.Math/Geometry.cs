
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    //public static class Geometry
    //{
    //    //public static Vector2[] SphereProjectionZ(Vector3 position, float radius)
    //    //{
    //    //    Vector2[] points = new Vector2[]
    //    //    {   new Vector2(position.X, position.Z + radius), //norh
    //    //        new Vector2(position.X, position.Z - radius), //south
    //    //        new Vector2(position.X - radius, position.Z), //west
    //    //        new Vector2(position.X + radius, position.Z), //east
    //    //    };
    //    //    return points;
    //    //}          

    //    /// <summary>
    //    /// Compute the transformation matrix in the following order
    //    ///  matrix = scaling * rotation * translation
    //    /// </summary>
    //    /// <param name="pivot">Rotation Pivot</param>
    //    /// <param name="scaling">Scale</param>
    //    /// <param name="orientation">Rotation in Euler Angles</param>
    //    /// <param name="translation">Translation from pivot</param>
    //    /// <returns></returns>       

    //    //public static Matrix RotationPivot(Vector3 pivot, Euler orientation)
    //    //{
    //    //    var transformMatrix = Matrix.Translation(-pivot);
    //    //    transformMatrix *= orientation.ToMatrix();
    //    //    transformMatrix *= Matrix.Translation(pivot);
    //    //    return transformMatrix;
    //    //}
        
    //    //public static Matrix ScalePivot(Vector3 pivot, Vector3 scale)
    //    //{
    //    //    var transformMatrix = Matrix.Translation(-pivot);
    //    //    transformMatrix *= Matrix.Scaling(scale);
    //    //    transformMatrix *= Matrix.Translation(pivot);
    //    //    return transformMatrix;
    //    //}           

    //    //public static Vector3 CylindricalToCartesian(float theta, float y, float radius)
    //    //{
    //    //    return new Vector3(radius * (float)Math.Cos(theta), y, radius * (float)Math.Sin(theta));
    //    //}

    //    //public static Vector3 CylindricalToCartesian(Vector3 p)
    //    //{
    //    //    return new Vector3(p.Z * (float)Math.Cos(p.X), p.Y, p.Z * (float)Math.Sin(p.X));
    //    //}

    //    ///// <summary>
    //    ///// 
    //    ///// </summary>
    //    ///// <param name="phi">Rotation angle respect to the Xaxis </param>
    //    ///// <param name="theta">Rotation Angle respect to the Yaxis</param>
    //    ///// <param name="radius">lenght of the vector</param>
    //    ///// <returns></returns>
    //    //public static Vector3 SphericalToCartesian(float phi, float theta, float radius)
    //    //{
    //    //    float b = radius * (float)Math.Sin(phi);
    //    //    return new Vector3(b * (float)Math.Sin(theta), radius * (float)Math.Cos(phi), b * (float)Math.Cos(theta));
    //    //}

    //    //public static Vector3 SphericalToCartesian(Vector3 p)
    //    //{
    //    //    float b = p.Z * (float)Math.Sin(p.X);
    //    //    return new Vector3(b * (float)Math.Sin(p.Y), p.Z * (float)Math.Cos(p.X), b * (float)Math.Cos(p.Y));
    //    //}

    //    ///// <summary>
    //    ///// Converte a (x,y,z) = > (phi, theta ,radius)
    //    ///// phi : Rotation angle respect to the Zaxis ,
    //    ///// theta : Rotation Angle respect to the Yaxis , 
    //    ///// radius : lenght of the vector
    //    ///// </summary>
    //    ///// <param name="v">direction</param>
    //    ///// <returns>phi - Rotation angle respect to the Zaxis , theta - Rotation Angle respect to the Yaxis , radius - lenght of the vector</returns>
    //    //public static Vector3 CartesianToSpherical(Vector3 p)
    //    //{
    //    //    float radius = p.Length();
    //    //    float phi = (float)Math.Acos(p.Y / radius);
    //    //    float theta = (float)Math.Asin(p.X / (radius * (float)Math.Sin(phi)));
    //    //    return new Vector3(phi, theta, radius);
    //    //}
    //    ///// <summary>
    //    ///// Converte a (x,y,z) = > (phi, theta ,radius)
    //    ///// </summary>
    //    ///// <param name="v"></param>
    //    ///// <returns>phi - angle in the XY plane starting 0 at top, theta - angle in the ZX plane starting 0 at front radius - lenght of the vector</returns>
    //    //public static Vector3 CartesianToCylindrical(float x, float y, float z)
    //    //{
    //    //    float radius = (float)Math.Sqrt(x * x + z * z);
    //    //    if (radius != 0)
    //    //    {
    //    //        float theta = (float)Math.Acos(x / radius);
    //    //        return new Vector3(theta, y, radius);
    //    //    }
    //    //    return new Vector3(0, y, 0);
    //    //}

    //    //public static Vector3 CartesianToCylindrical(Vector3 v)
    //    //{
    //    //    float radius = (float)Math.Sqrt(v.X * v.X + v.Z * v.Z);
    //    //    if (radius != 0)
    //    //    {
    //    //        float theta = (float)Math.Acos(v.X / radius);
    //    //        return new Vector3(theta, v.Y, radius);
    //    //    }
    //    //    return new Vector3(0, v.Y, 0);
    //    //}

    //    //public static Vector3 Reflect(Vector3 dir, Vector3 normal)
    //    //{
    //    //    //v = i - 2 * n * dot(i•n) 
    //    //    Vector3 result = dir - 2 * normal * Vector3.Dot(dir, normal);
    //    //    return result;
    //    //}

    //    //public static OrientedBox GetBoundingBox(Vector3[] positions)
    //    //{
    //    //    unsafe
    //    //    {
    //    //        OrientedBox box;
    //    //        fixed (Vector3* pPosition = positions)
    //    //        {
    //    //            box = GetBoundingBox((byte*)pPosition, positions.Length, sizeof(Vector3));
    //    //        }

    //    //        return box;
    //    //    }
    //    //}

    //    //public unsafe static OrientedBox GetBoundingBox(byte* positions, int vertexCount, int stride)
    //    //{
    //    //    OrientedBox box = new OrientedBox();

    //    //    var MinValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    //    //    var MaxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    //    //    //Compute Covariance Matrix
    //    //    Matrix c = Numerics.ComputeCorrelationMatrix(positions, vertexCount, stride);
    //    //    Vector3[] eigenVectors;
    //    //    Vector3 eigenValues;
    //    //    Numerics.ComputeEigenVectors(c, out eigenValues, out eigenVectors, 1.0e-10f);


    //    //    var R = eigenVectors[0];
    //    //    var S = eigenVectors[1];
    //    //    var T = eigenVectors[2];

    //    //    Vector3 t;

    //    //    for (int i = 0; i < vertexCount; i++)
    //    //    {
    //    //        Vector3* pter = (Vector3*)(positions + i * stride);
    //    //        t = new Vector3(Vector3.Dot(*pter, R), Vector3.Dot(*pter, S), Vector3.Dot(*pter, T));
    //    //        MinValues = Vector3.Minimize(MinValues, t);
    //    //        MaxValues = Vector3.Maximize(MaxValues, t);
    //    //    }

    //    //    t = 0.5f * (MinValues + MaxValues);
    //    //    box.Extends = 0.5f * (MaxValues - MinValues);
    //    //    box.LocalTranslation = t.X * R + t.Y * S + t.Z * T;
    //    //    box.LocalRotation = Geometry.RowMajorMatrix(R, S, T);
    //    //    box.Update(Matrix.Identity);

    //    //    return box;
    //    //}

    //    //public static OrientedBox GetBoundingBox(IEnumerable<Vector3> positions)
    //    //{
    //    //    OrientedBox box = new OrientedBox();

    //    //    var MinValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    //    //    var MaxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    //    //    //Compute Covariance Matrix
    //    //    Matrix c = Numerics.ComputeCorrelationMatrix(positions);
    //    //    Vector3[] eigenVectors;
    //    //    Vector3 eigenValues;
    //    //    Numerics.ComputeEigenVectors(c, out eigenValues, out eigenVectors, 1.0e-10f);


    //    //    var R = eigenVectors[0];
    //    //    var S = eigenVectors[1];
    //    //    var T = eigenVectors[2];

    //    //    Vector3 t;

    //    //    foreach (var v in positions)
    //    //    {
    //    //        t = new Vector3(Vector3.Dot(v, R), Vector3.Dot(v, S), Vector3.Dot(v, T));
    //    //        MinValues = Vector3.Minimize(MinValues, t);
    //    //        MaxValues = Vector3.Maximize(MaxValues, t);
    //    //    }

    //    //    t = 0.5f * (MinValues + MaxValues);
    //    //    var center = t.X * R + t.Y * S + t.Z * T;
    //    //    box.Extends = 0.5f * (MaxValues - MinValues);
    //    //    box.LocalRotation = Geometry.RowMajorMatrix(R, S, T);
    //    //    box.LocalTranslation = center;
    //    //    box.Update(Matrix.Identity);

    //    //    return box;
    //    //}      

    //    //public static unsafe void GetBoundingSphere(byte* positions, int vertexCount, int stride, out Vector3 center, out float radius)
    //    //{
    //    //    Matrix c = Numerics.ComputeCorrelationMatrix(positions, vertexCount, stride);
    //    //    Vector3[] eigenVectors;
    //    //    Vector3 eigenValues;
    //    //    Numerics.ComputeEigenVectors(c, out eigenValues, out eigenVectors, 1.0e-10f);
    //    //    Vector3 R = eigenVectors[0];
    //    //    float min = float.MaxValue, max = float.MinValue;
    //    //    Vector3 minPoint = Vector3.Zero;
    //    //    Vector3 maxPoint = Vector3.Zero;
    //    //    for (int i = 0; i < vertexCount; i++)
    //    //    {
    //    //        Vector3* pter = (Vector3*)(positions + i * stride);
    //    //        float t = Vector3.Dot(*pter, R);
    //    //        if (t < min)
    //    //        {
    //    //            min = t;
    //    //            minPoint = *pter;
    //    //        }
    //    //        if (t > max)
    //    //        {
    //    //            max = t;
    //    //            maxPoint = *pter;
    //    //        }
    //    //    }

    //    //    center = 0.5f * (maxPoint + minPoint);
    //    //    radius = Vector3.Distance(maxPoint, center);
    //    //    float r2 = radius * radius;

    //    //    for (int i = 0; i < vertexCount; i++)
    //    //    {
    //    //        Vector3 pos = *(Vector3*)(positions + i * stride);
    //    //        Vector3 d = pos - center;
    //    //        if (d.LengthSquared() > r2)
    //    //        {
    //    //            d.Normalize();
    //    //            Vector3 g = center - radius * d;
    //    //            center = 0.5f * (g + pos);
    //    //            radius = Vector3.Distance(pos, center);
    //    //            r2 = radius * radius;
    //    //        }
    //    //    }
    //    //}

    //    //public static void GetBoundingSphere(Vector3[] positions, out Vector3 center, out float radius)
    //    //{
    //    //    unsafe
    //    //    {
    //    //        fixed (Vector3* pPosition = positions)
    //    //        {
    //    //            GetBoundingSphere((byte*)pPosition, positions.Length, sizeof(Vector3), out center, out radius);
    //    //        }
    //    //    }
    //    //}
       
    //    //public static void GetBoundingSphere(IEnumerable<Vector3> positions, out Vector3 center, out float radius)
    //    //{
    //    //    Matrix c = Numerics.ComputeCorrelationMatrix(positions);
    //    //    Vector3[] eigenVectors;
    //    //    Vector3 eigenValues;
    //    //    Numerics.ComputeEigenVectors(c, out eigenValues, out eigenVectors, 1.0e-10f);
    //    //    Vector3 R = eigenVectors[0];
    //    //    float min = float.MaxValue, max = float.MinValue;
    //    //    Vector3 minPoint = Vector3.Zero;
    //    //    Vector3 maxPoint = Vector3.Zero;

    //    //    foreach (var p in positions)
    //    //    {
    //    //        float t = Vector3.Dot(p, R);
    //    //        if (t < min)
    //    //        {
    //    //            min = t;
    //    //            minPoint = p;
    //    //        }
    //    //        if (t > max)
    //    //        {
    //    //            max = t;
    //    //            maxPoint = p;
    //    //        }
    //    //    }

    //    //    center = 0.5f * (maxPoint + minPoint);
    //    //    radius = Vector3.Distance(maxPoint, center);
    //    //    float r2 = radius * radius;

    //    //    foreach (var p in positions)
    //    //    {
    //    //        Vector3 d = p - center;
    //    //        if (d.LengthSquared() > r2)
    //    //        {
    //    //            d.Normalize();
    //    //            Vector3 g = center - radius * d;
    //    //            center = 0.5f * (g + p);
    //    //            radius = Vector3.Distance(p, center);
    //    //            r2 = radius * radius;
    //    //        }
    //    //    }
    //    //}

    //    //public static IEnumerable<Vector3> TransformCoordinate(IEnumerable<Vector3> stream, Matrix transform)
    //    //{
    //    //    foreach (var v in stream)
    //    //    {
    //    //        yield return Vector3.TransformCoordinate(v, transform);
    //    //    }
    //    //}

    //    //public static IEnumerable<Vector3> TransformNormal(IEnumerable<Vector3> stream, Matrix transform)
    //    //{
    //    //    foreach (var v in stream)
    //    //    {
    //    //        yield return Vector3.TransformNormal(v, transform);
    //    //    }
    //    //}

    //    //public static Vector3 GetAnglesFromRotation(Matrix rotation)
    //    //{
    //    //    Vector3 angles;
    //    //    angles.Y = (float)Math.Asin(-rotation.M13);
    //    //    float cosY = (float)Math.Cos(angles.Y);
    //    //    angles.X = (float)Math.Acos(rotation.M33 / cosY);
    //    //    angles.Z = (float)Math.Acos(rotation.M11 / cosY);
    //    //    return angles;
    //    //}

    //    //public static Matrix GetMatrixFromAngles(Vector3 angles)
    //    //{
    //    //    Matrix m = Matrix.RotationX(angles.X);
    //    //    m *= Matrix.RotationY(angles.Y);
    //    //    m *= Matrix.RotationZ(angles.Z);
    //    //    return m;
    //    //}
    //}
}
