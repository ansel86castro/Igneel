using System;
using System.Collections.Generic;
using Igneel.Assets;
using Igneel.Assets.StorageConverters;
using Igneel.SceneComponents;
using Igneel.Components;

namespace Igneel.SceneComponents
{    
    public abstract class MaterialCollection<TRenderStack> : GraphicObject<TRenderStack>, IMaterialContainer           
        where TRenderStack :class, IGraphicObject
    {        
        internal BasicMaterial[] materials = new BasicMaterial[0];
        internal int[] Transparents;               
              
        public MaterialCollection()
        {
          
        }

        public MaterialCollection(BasicMaterial[] materials)            
        {
            if (materials != null)
                Materials = materials;           
        }

        [AssetMember(typeof(ArrayStoreConverter<BasicMaterial>))]
        public BasicMaterial[] Materials 
        { 
            get { return materials; }
            set 
            {
                if (value == null)
                    this.materials = new BasicMaterial[0];

                else if (materials != value)
                {
                    this.materials = value;
                    FindTransparentMaterials();
                }
            }
        }

        public int[] TransparentMaterials { get { return Transparents; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var mat in materials)
                {
                    mat.Dispose();
                }
                materials = new BasicMaterial[0];
            }            
        }

        private void FindTransparentMaterials()
        {
            List<int> transparents = new List<int>(materials.Length);         

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].CheckForTransparency())
                    transparents.Add(i);                
            }

            IsTransparent = transparents.Count > 0;
            this.Transparents = transparents.ToArray();
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

    //class MaterialsConverter : IStoreConverter
    //{
    //    public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
    //    {
    //        var materials = (BasicMaterial[])propValue;
    //        var materialsRef = new AssetReference[materials.Length];
    //        for (int i = 0; i < materials.Length; i++)
    //        {
    //            materialsRef[i] = AssetManager.Instance.GetAssetReference(materials[i]);
    //        }
    //        return materialsRef;
    //    }

    //    public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
    //    {
    //        var materialContainer = (IMaterialContainer)provider;
    //        AssetReference[] materialsRef = (AssetReference[])storeValue;
    //        BasicMaterial[] materials = new BasicMaterial[materialsRef.Length];

    //        for (int i = 0; i < materials.Length; i++)
    //        {
    //            materials[i] = (BasicMaterial) AssetManager.Instance.GetAssetProvider(materialsRef[i]);
    //        }
           
    //        materialContainer.Materials = materials;
    //    }
    //}

  
}
