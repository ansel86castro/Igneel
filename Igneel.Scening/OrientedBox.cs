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
    [Serializable]
   
    [StructLayout(LayoutKind.Sequential)]
    public class OrientedBox : IRotable, ITranslatable
    {
        Vector3 center;
        Vector3 extends;
        Matrix rotation = Matrix.Identity;

        Matrix localPose = Matrix.Identity;
        Matrix globalPose = Matrix.Identity;

        public Vector3 Extends
        {
            get { return extends; }
            set { extends = value; }
        }

        public Matrix LocalPose { get { return localPose; } set { localPose = value; } }

        public Matrix GlobalPose { get { return globalPose; } set { globalPose = value; } }

       
        
        public Matrix LocalRotation { get { return rotation; } set { rotation = value; } }

       
        
        public Vector3 LocalPosition { get { return center; } set { center = value; } }

        public Vector3 GlobalTraslation { get { return globalPose.GetAxis(3); } }

        public OrientedBox() { }

        public OrientedBox(Vector3 center, Vector3 extends, Matrix rotation)
        {
            this.center = center;
            this.extends = extends;
            this.rotation = rotation;
        }

        public OrientedBox(Vector3 dimention, Matrix rotation)
        {
            this.extends = dimention;
            this.rotation = localPose;
        }

        public unsafe OrientedBox(byte* positions, int vertexCount, int stride)
        {
            GetOrientedBox(positions, vertexCount, stride);
        }

        public unsafe void GetOrientedBox(byte* positions, int vertexCount, int stride)
        {
            var MinValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var MaxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);


            var R = eigenVectors[0];
            var S = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                t = new Vector3(Vector3.Dot(*pter, R), Vector3.Dot(*pter, S), Vector3.Dot(*pter, T));
                MinValues = Vector3.Min(MinValues, t);
                MaxValues = Vector3.Max(MaxValues, t);
            }

            t = 0.5f * (MinValues + MaxValues);
            Extends = 0.5f * (MaxValues - MinValues);
            center = t.X * R + t.Y * S + t.Z * T;
            rotation = new Matrix(R, S, T, Vector3.Zero);
            Update(Matrix.Identity);

        }

        public OrientedBox(Vector3[] positions)
        {
            unsafe
            {
                fixed (Vector3* pPosition = positions)
                {
                    GetOrientedBox((byte*)pPosition, positions.Length, sizeof(Vector3));
                }
            }
        }

        public OrientedBox(BufferView<Vector3> positions)
        {
            unsafe
            {
                GetOrientedBox((byte*)positions.BasePter, positions.Count, positions.Stride);
            }
        }

        public void Update(Matrix pose)
        {
            localPose = rotation;
            localPose.Translation = center;
            globalPose = localPose * pose;
        }

        public static OrientedBox Create(Vector3[] positions)
        {
            unsafe
            {
                OrientedBox box;
                fixed (Vector3* pPosition = positions)
                {
                    box = Create((byte*)pPosition, positions.Length, sizeof(Vector3));
                }

                return box;
            }
        }

        public unsafe static OrientedBox Create(byte* positions, int vertexCount, int stride)
        {
            OrientedBox box = new OrientedBox();

            var MinValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var MaxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);

            var R = eigenVectors[0];
            var S = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                t = new Vector3(Vector3.Dot(*pter, R), Vector3.Dot(*pter, S), Vector3.Dot(*pter, T));
                MinValues = Vector3.Min(MinValues, t);
                MaxValues = Vector3.Max(MaxValues, t);
            }

            t = 0.5f * (MinValues + MaxValues);
            box.extends = 0.5f * (MaxValues - MinValues);
            box.center = t.X * R + t.Y * S + t.Z * T;
            box.rotation = new Matrix(R, S, T, new Vector3());
            box.Update(Matrix.Identity);

            return box;
        }

        public static OrientedBox Create(IEnumerable<Vector3> positions)
        {
            OrientedBox box = new OrientedBox();

            var MinValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var MaxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);

            var R = eigenVectors[0];
            var S = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            foreach (var v in positions)
            {
                t = new Vector3(Vector3.Dot(v, R), Vector3.Dot(v, S), Vector3.Dot(v, T));
                MinValues = Vector3.Min(MinValues, t);
                MaxValues = Vector3.Max(MaxValues, t);
            }

            t = 0.5f * (MinValues + MaxValues);
            box.extends = 0.5f * (MaxValues - MinValues);
            box.center = t.X * R + t.Y * S + t.Z * T;
            box.rotation = new Matrix(R, S, T, new Vector3());
            box.Update(Matrix.Identity);

            return box;
        }

        public void CommitChanges()
        {
            localPose = rotation;
            localPose.SetAxis(3, center);
        }

        public OrientedBox Clone()
        {
            return (OrientedBox)MemberwiseClone();
        }

        public Box ToBox()
        {
            return new Box(center, extends, rotation);
        }

        public BoxBuilder GetGeometry()
        {
            BoxBuilder box = new BoxBuilder(2, 2, 2);

            for (int i = 0; i < box.Vertices.Length; i++)
            {
                var mat = Matrix.Scale(extends) * globalPose;
                box.Vertices[i].Position = Vector3.Transform(box.Vertices[i].Position, mat);
            }
            return box;
        }

        public AABB GetAxisAlignedBoundingBox()
        {
            unsafe
            {
                Vector3* corners = stackalloc Vector3[8];
                corners[0] = new Vector3(-1.0f, -1.0f, 0.0f); // xyz 
                corners[1] = new Vector3(1.0f, -1.0f, 0.0f);  // Xyz 
                corners[2] = new Vector3(-1.0f, 1.0f, 0.0f);  // xYz 
                corners[3] = new Vector3(1.0f, 1.0f, 0.0f);   // XYz 
                corners[4] = new Vector3(-1.0f, -1.0f, 1.0f); // xyZ
                corners[5] = new Vector3(1.0f, -1.0f, 1.0f);  // XyZ 
                corners[6] = new Vector3(-1.0f, 1.0f, 1.0f);  // xYZ 
                corners[7] = new Vector3(1.0f, 1.0f, 1.0f);   // XYZ 

                var matrix = Matrix.Scale(extends) * rotation * Matrix.Translate(center);
                AABB aab = new AABB();

                aab.Maximum = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                aab.Minimum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                for (int i = 0; i < 8; i++)
                {
                    var pos = Vector3.Transform(corners[i], matrix);
                    aab.Minimum = Vector3.Min(pos, aab.Minimum);
                    aab.Maximum = Vector3.Max(pos, aab.Maximum);
                }

                return aab;
            }
        }

    }
}
