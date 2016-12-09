using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public class TriangleMeshDesc
    {
        public string Name;

        public MeshFlag Flags;

        public int NumVertices;

        public int NumTriangles;

        public int PointStrideBytes;

        public int TriangleStrideBytes;

        public IntPtr Points;

        public IntPtr Triangles;
    }
}
