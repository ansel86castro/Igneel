using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;
using System.IO;


namespace Igneel.Physics
{
    public abstract class PhysicManager:ResourceAllocator
    {
        internal ReadOnlyDictionary<string, Physic> scenes = new ReadOnlyDictionary<string, Physic>();
        internal ReadOnlyDictionary<string, TriangleMesh> meshes = new ReadOnlyDictionary<string, TriangleMesh>();

        static PhysicManager _instance;

        public PhysicManager()
        {
            Service.Set<PhysicManager>(this);
            _instance = this;
        }

        public static PhysicManager Sigleton { get { return _instance; } }

        public ReadOnlyDictionary<string, TriangleMesh> Meshes { get { return meshes; } }

        public ReadOnlyDictionary<string, Physic> Scenes { get { return scenes; } }

        public Physic CreatePhysic(PhysicDesc desc)
        {
            var scene = _CreatePhysic(desc);
            scenes.Add(scene.Name, scene);
            return scene;
        }

        public TriangleMesh CreateTriangleMesh(TriangleMeshDesc desc)
        {
            var mesh = _CreateTriangleMesh(desc);
            meshes.Add(mesh.Name, mesh);

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(mesh);

            return mesh;
        }

        public TriangleMesh CreateTriangleMeshFromFile(string filename)
        {
            var mesh = _CreateTriangleMeshFromFile(filename);
            mesh.Name = "Mesh" + meshes.Count;
            meshes.Add(mesh.Name, mesh);

            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(mesh);
            return mesh;
        }

        public TriangleMesh CreateTriangleMeshFromStream(Stream stream)
        {
            var mesh = _CreateTriangleMeshFromStream(stream);
            meshes.Add(mesh.Name, mesh);
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(mesh);
            return mesh;
        }

        public abstract CharacterControllerManager CreateControllerManager();

        protected abstract Physic _CreatePhysic(PhysicDesc desc);

        protected abstract TriangleMesh _CreateTriangleMesh(TriangleMeshDesc desc);

        protected abstract TriangleMesh _CreateTriangleMeshFromFile(string filename);

        protected abstract TriangleMesh _CreateTriangleMeshFromStream(Stream stream);

        protected override void OnDispose(bool disposing)
        {          
            foreach (var m in meshes.ToArray())
            {
                m.Value.Dispose();
            }
            foreach (var s in scenes.ToArray())
            {
                s.Value.Dispose();
            }
            base.OnDispose(disposing);
        }
    }
}
