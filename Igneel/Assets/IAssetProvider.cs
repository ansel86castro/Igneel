using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    public interface IAssetProvider
    {
        Asset CreateAsset();
    }

    public interface IAssetStoreProvider
    {

    }

    public class AssetProvider:IAssetProvider,INameable
    {
        private string name;

        public AssetProvider(string name)
        {
            this.name = name;
        }

        public Asset CreateAsset()
        {
            return new AutoAssetStore(this, name);
        }

        public string Name
        {
            get { return name; }
        }
    }

    public class AssetProvider<T> : SerializableBase, IAssetProvider
        where T : Asset
    {
        public Asset CreateAsset()
        {
            return (Asset)Activator.CreateInstance(typeof(T), this);
        }

        public virtual void OnAssetDestroyed(AssetReference assetRef) { }
    }
    public interface IAssetProviderNotificator
    {
        void OnSavingBegin();

        void OnSavingEnd();
    }
}
