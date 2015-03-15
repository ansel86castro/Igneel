using System;
namespace Igneel.Assets
{
    public interface IAssetContext
    {
        bool ContainsProvider(AssetReference refe);
        bool ContainsReference(IAssetProvider provider);
        Asset GetAsset(int id);
        bool TryGetProvider(AssetReference refe, out IAssetProvider provider);
        bool TryGetReference(IAssetProvider provider, out AssetReference refe);
        bool IsRegister(int assetId);
        void RegisterAsset(Asset asset);
        void RegisterProvider(IAssetProvider provider, AssetReference refe);
        AssetStorage Storage { get; set; }
    }
}
