using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Assets
{    
    public class AssetContext : IAssetContext
    {
        class Globals<T>
        {
           public static T Instance;
        }

        Dictionary<Type, object> objects = new Dictionary<Type, object>();
        Dictionary<int, Asset> assetsLookup = new Dictionary<int, Asset>();
        Dictionary<IAssetProvider, AssetReference> refLookup = new Dictionary<IAssetProvider, AssetReference>();
        Dictionary<AssetReference, IAssetProvider> providerLookup = new Dictionary<AssetReference, IAssetProvider>();
        
        public AssetStorage Storage { get; set; }

        public void RegisterProvider(IAssetProvider provider, AssetReference refe)
        {
            refLookup.Add(provider, refe);
            providerLookup.Add(refe, provider);
        }

        public bool ContainsReference(IAssetProvider provider)
        {
            return refLookup.ContainsKey(provider);
        }

        public bool ContainsProvider(AssetReference refe)
        {
            return providerLookup.ContainsKey(refe);
        }

        public bool ContainsProvider(InternalARef refe)
        {
            return providerLookup.ContainsKey(refe);
        }

        public AssetReference GetReference(IAssetProvider provider)
        {
            return refLookup[provider];
        }

        public bool TryGetReference(IAssetProvider provider, out AssetReference refe)
        {
            return refLookup.TryGetValue(provider, out refe);
        }

        public IAssetProvider GetProvider(AssetReference refe)
        {
            return providerLookup[refe];
        }

        public bool TryGetProvider(AssetReference refe, out IAssetProvider provider)
        {
            return providerLookup.TryGetValue(refe, out provider);
        }

        public void RegisterAsset(Asset asset)
        {
            if (!assetsLookup.ContainsKey(asset.Id))
                assetsLookup.Add(asset.Id, asset);
        }

        public bool IsRegister(int assetId)
        {
            return assetsLookup.ContainsKey(assetId);
        }

        public Asset GetAsset(int id)
        {
             Asset asset = null;
             if (!assetsLookup.TryGetValue(id, out asset) && Storage != null)
                 return Storage.GetAsset(id);
            return asset;
        }

        internal Asset _GetAsset(AssetReference refe)
        {
            if (refe is InternalARef)
                return GetAsset(((InternalARef)refe).Id);
            return null;
        }

        public ICollection<int> AssetIds { get { return assetsLookup.Keys; } }

        public ICollection<Asset> Assets { get { return assetsLookup.Values; } }

        public Dictionary<Type, object> Objects { get { return objects; } }

        public static T GetGlobal<T>()
        {
            return Globals<T>.Instance;
        }

        public static void SetGlobal<T>(T instance)
        {
            Globals<T>.Instance = instance;
        }

    }    
}
