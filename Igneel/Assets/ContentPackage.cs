using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    [Asset(AssetType.Template, ".pk")]
    public class ContentPackage : AssetProvider ,IAssetStoreProvider ,ISceneElement 
    {        
        List<IAssetProvider> providers = new List<IAssetProvider>();
        
        public ContentPackage(string name):base(name)  
        {
            
        }        

        [AssetMember(typeof(CollectionStoreConverter<IAssetProvider>))]
        public List<IAssetProvider> Providers { get { return providers; } }                
       
        public void Add(IAssetProvider provider)
        {
            providers.Add(provider);
        }

        public bool Remove(IAssetProvider provider)
        {
            return providers.Remove(provider);
        }

        public void OnRemoveFromScene(Scene scene)
        {
            foreach (var item in providers)
            {
                if (item is ISceneElement)
                {
                    var se = (ISceneElement)item;
                    se.OnRemoveFromScene(scene);
                }
            }
        }

        public void OnAddToScene(Scene scene)
        {
            foreach (var item in providers)
            {
                if (item is ISceneElement)
                {
                    var se= (ISceneElement)item;
                    se.OnAddToScene(scene);
                }
            }

            foreach (var item in scene.Nodes)
            {
                TagProcesors.TagProcessor.ProcessHeirarchy(scene, item);   
            }            
        }
    }

    //[Serializable]
    //[Asset(AssetType.Template, ".pk")]
    //public abstract class ContentPackageAsset : AssetStorage
    //{
    //    AssetReference[] references;

    //    public ContentPackageAsset(ContentPackage package)
    //        : base(package, package.Name)
    //    {
    //        references = new AssetReference[package.Providers.Count];

    //        for (int i = 0; i < references.Length; i++)
    //        {
    //            references[i] = Manager.GetAssetReference(package.Providers[i]);
    //        }
    //    }

    //    public override IAssetProvider CreateProviderInstance()
    //    {
    //        ContentPackage package = CreateContentInstance();


    //        for (int i = 0; i < references.Length; i++)
    //        {
    //            package.Providers.Add(Manager.GetAssetProvider(references[i]));
    //        }

    //        return package;
    //    }

    //    protected abstract ContentPackage CreateContentInstance();        
    //}

}
