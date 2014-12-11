using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    public interface IAssetRepository
    {
        ExternalARef CreateReference(Asset asset);
        
        void SaveAsset(Asset asset);      

        Asset LoadAsset(ExternalARef refe);

        void DeleteAsset(ExternalARef refe);

        ICollection<Asset> GetAssets(AssetType type);

        ICollection<T> GetAssets<T>() where T : Asset;

        ICollection<AssetReference> GetReferences(AssetType type);

        ICollection<AssetReference> GetReferences<T>() where T : Asset;
    }
}
