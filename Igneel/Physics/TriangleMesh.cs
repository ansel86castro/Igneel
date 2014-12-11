using Igneel.Assets;
using Igneel.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

    public abstract class TriangleMesh:ResourceAllocator ,IAssetProvider,INameable
    {
        internal protected Mesh graphicMesh;

        public string Name { get; set; }

        public Mesh GraphicMesh { get { return graphicMesh; } }

        public object UserData { get; set; }

        public abstract int PagesCount { get; }

        public abstract int SubMeshCount { get; }

        public abstract int ReferenceCount{get;}        

        public abstract int GetCount(InternalArray arrayType);

        public abstract InternalFormat GetFormat(InternalArray arrayType);

        public abstract IntPtr GetBase(InternalArray arrayType);

        public abstract int GetStride(InternalArray arrayType);

        public abstract int GetTriangleMaterial(int triangleIndex);

        public abstract void Load(Stream stream);

        public Asset CreateAsset()
        {
            return Asset.Create(this, Name);
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                PhysicManager.Sigleton.meshes.Remove(Name);
            }
            base.OnDispose(disposing);
        }
    }
}
