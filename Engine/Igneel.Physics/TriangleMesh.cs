using Igneel.Assets;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Igneel.Physics
{
   

    public abstract class TriangleMesh: Resource
    {
        internal protected object graphicMesh;

        public string Name { get; set; }

        public object GraphicMesh { get { return graphicMesh; } set { graphicMesh = value; } }

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
      

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                PhysicManager.Sigleton.meshes.Remove(Name);
            }         
        }
    }
}
