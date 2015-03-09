using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Igneel.Graphics;

using Igneel.Assets;
using Igneel.Collections;
using Igneel.Services;

namespace Igneel.Components
{      
    public interface IMeshContainer:IGraphicObject
    {
        Mesh Mesh { get; set; }
    }   

    public abstract class MeshContainer<T> : MaterialContainer<T>, IMeshContainer, INameable
        where T : class ,IGraphicObject        
    {
        Mesh mesh;

        public MeshContainer()
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        protected MeshContainer( MeshMaterial[] materials, Mesh mesh)
            : base(materials)
        {         
            if (mesh != null)
                Mesh = mesh;
        }

        [AssetMember(storeAs:StoreType.Reference)]
        public Mesh Mesh
        {
            get
            {
                return mesh;
            }
            set
            {
                if (mesh != value)
                {
                    OnMeshChanged(value);
                    mesh = value;
                }              
            }
        }

        protected virtual void OnMeshChanged(Mesh newMesh) 
        {         
            if (newMesh != null)
            {                              
                BoundingBox = newMesh.BoundingBox;
                BoundingSphere = newMesh.BoundingSphere;

            }
            else
            {                
                BoundingBox = null;
                BoundingSphere = new Sphere();
            }
        }        

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                if (mesh != null)
                    mesh.Dispose();

                var srv = Service.Get<INotificationService>();
                if (srv != null)
                    srv.OnObjectDestroyed(this);
            }
            base.OnDispose(d);
        }

        public override string ToString()
        {
            return Mesh!=null? Mesh.ToString() : base.ToString();
        }

        public string Name
        {
            get
            {
                if (mesh != null) return mesh.Name;
                return null;
            }
            set
            {
                if (mesh != null)
                    mesh.Name = value;
            }
        }

        public override void OnAddToScene(Scene scene)
        {
            scene.Geometries.Add(Node);
            base.OnAddToScene(scene);
        }

        public override void OnRemoveFromScene(Scene scene)
        {
            scene.Geometries.Remove(Node);
            base.OnRemoveFromScene(scene);
        }
    }

    [ProviderActivator(typeof(MeshInstance.Activator))]
    public class MeshInstance : MeshContainer<MeshInstance>
    {
        public MeshInstance(MeshMaterial[] materials = null, Mesh mesh = null)
            : base(materials, mesh)
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }        

        [Serializable]
        class Activator:IProviderActivator
        {           
            public void Initialize(IAssetProvider provider)
            {
             
            }

            public IAssetProvider CreateInstance()
            {               
                return new MeshInstance();
            }
            
        }     
    }
}
