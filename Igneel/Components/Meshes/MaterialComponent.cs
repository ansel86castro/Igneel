using Igneel.Assets;
using Igneel.Collections;
using Igneel.Graphics;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public interface IMaterialContainer : IGraphicObject
    {
        MeshMaterial[] Materials { get; set; }

        int[] TransparentMaterials { get; }
    }

    public abstract class MaterialContainer<T> : ExclusiveGraphicObject<T>, IMaterialContainer      
        where T:class,IGraphicObject        
    {        
        internal MeshMaterial[] materials = new MeshMaterial[0];
        internal int[] transparents;
        
        int nodeCount;             
              
        public MaterialContainer()
        {
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        public MaterialContainer(MeshMaterial[] materials)            
        {
            if (materials != null)
                Materials = materials;           
        }

        [AssetMember(typeof(ArrayStoreConverter<MeshMaterial>))]
        public MeshMaterial[] Materials 
        { 
            get { return materials; }
            set 
            {
                if (value == null)
                    this.materials = new MeshMaterial[0];

                else if (materials != value)
                {
                    this.materials = value;
                    ComputeRenderLayerIndex();
                }
            }
        }

        public int[] TransparentMaterials { get { return transparents; } }

        protected override void OnDispose(bool d)
        {
            if (!d)
            {
                var srv = Service.Get<INotificationService>();
                if (srv != null)
                    srv.OnObjectDestroyed(this);
            }

            base.OnDispose(d);
        }

        private void ComputeRenderLayerIndex()
        {
            List<int> transparents = new List<int>(materials.Length);         

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].CheckForTransparency())
                    transparents.Add(i);                
            }

            IsTransparent = transparents.Count > 0;
            this.transparents = transparents.ToArray();
        }

        public override void OnNodeAttach(SceneNode node)
        {
            nodeCount++;

            base.OnNodeAttach(node);
        }

        public override void OnNodeDetach(SceneNode node)
        {
            nodeCount--;

            base.OnNodeDetach(node);
        }
      

        struct ComparerStuff:IComparable<ComparerStuff>
        {
            public int RenderLayer;
            public int MaterialIndex;

            public int CompareTo(ComparerStuff other)
            {
                return RenderLayer.CompareTo(other.RenderLayer);
            }
        }
        
    }

    class _MaterialsConverter : IStoreConverter
    {
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            var materials = (MeshMaterial[])propValue;
            var materialsRef = new AssetReference[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materialsRef[i] = AssetManager.Instance.GetAssetReference(materials[i]);
            }
            return materialsRef;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            var materialContainer = (IMaterialContainer)provider;
            AssetReference[] materialsRef = (AssetReference[])storeValue;
            MeshMaterial[] materials = new MeshMaterial[materialsRef.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = (MeshMaterial) AssetManager.Instance.GetAssetProvider(materialsRef[i]);
            }
           
            materialContainer.Materials = materials;
        }
    }

  
}
