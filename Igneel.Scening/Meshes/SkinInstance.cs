using Igneel.Animations;
using Igneel.Assets;
using Igneel.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering
{  
    [ProviderActivator(typeof(SkinInstance.Activator))]
    public class SkinInstance : MeshContainer<SkinInstance>
    {
        SkinDeformer skin;

        public SkinInstance(MeshMaterial[] materials = null, SkinDeformer skin = null) :
            base(materials, skin != null ? skin.Mesh : null)
        {
            this.skin = skin;
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public SkinDeformer Skin
        {
            get { return skin; }
            set
            {
                skin = value;
                if (skin != null)
                {
                    Mesh = skin.Mesh;
                }
            }
        }

        protected override void OnMeshChanged(Mesh newMesh)
        {
            base.OnMeshChanged(newMesh);

            BoundingBox = null;
            BoundingSphere = new Sphere();

            if (skin != null)
                skin.Mesh = newMesh;            
        }              

        public override void OnNodeDetach(SceneNode node)
        {           
            node.ComputeBoundingsShapes();

            base.OnNodeDetach(node);
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {               
                var srv = Service.Get<INotificationService>();
                if (srv != null)
                    srv.OnObjectDestroyed(this);
            }
            base.OnDispose(d);
        }

        [Serializable]
        class Activator : IProviderActivator
        {            
            public void Initialize(IAssetProvider provider)
            {
             
            }

            public IAssetProvider CreateInstance()
            {             
                return new SkinInstance();
            }
            
        }

        public override void OnPoseUpdated(SceneNode node)
        {
           
        }
    }
}
