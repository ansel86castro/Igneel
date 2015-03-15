using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    [Serializable]
    public abstract class AssetStorage: Asset
    {      
        Dictionary<int,Asset> assets = new Dictionary<int,Asset>();      

        public AssetStorage(IAssetProvider provider) : base(provider)
        {
            Manager.SaveContext.Storage = this;
        }

        public AssetStorage(IAssetProvider provider, string name):base(provider,name) 
        {
            Manager.SaveContext.Storage = this;
        }

        public bool ContainsAsset(int id)
        {
            return assets.ContainsKey(id);
        }

        public bool ContainsAsset(InternalARef aref)
        {
            return ContainsAsset(aref.Id);
        }

        public void AddAsset(Asset asset)
        {
            assets.Add(asset.Id, asset);
        }

        public bool RemoveAsset(int id)
        {
            return assets.Remove(id);
        }

        public bool RemoveAsset(InternalARef aref)
        {
            return assets.Remove(aref.Id);
        }

        public void ClearAssets()
        {
            assets.Clear();
        }

        public bool IsEmpty { get { return assets.Count == 0; } }

        public Asset GetAsset(int id)
        {
            return assets[id];
        }

        public Asset GetAsset(InternalARef aref)
        {
            return assets[aref.Id];
        }

        public override void Delete()
        {
            foreach (var value in assets.Values)
            {
                value.Delete();
            }
        }
        
    }
}
