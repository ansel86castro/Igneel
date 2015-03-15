using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    public class LocalAssetRepository:IAssetRepository
    {
        string contentDir = "../Content";
        bool dirCreated;

        public LocalAssetRepository()
        {

        }

        public string ContentDir 
        {
            get
            {
                if (!dirCreated)
                {
                    Directory.CreateDirectory(contentDir);
                    dirCreated = true;
                }
                return contentDir;
            }
            set 
            {
                if (contentDir != value)
                {
                    contentDir = value;
                    _OnContentDirChanged();
                }
            }
        }

        private void _OnContentDirChanged()
        {
            if (!Directory.Exists(contentDir))
            {
                dirCreated = true;
                Directory.CreateDirectory(contentDir);
            }
        }

        private string _GetRelativeContentFilename(Asset asset)
        {
            return Path.Combine(Enum.GetName(typeof(AssetType), asset.AssetType), asset.Name + asset.Extension);
        }

        private string _GetRelativeDirectory(Asset asset)
        {
            return Enum.GetName(typeof(AssetType), asset.AssetType);
        }        

        public void SaveAsset(Asset asset)
        {
            if (string.IsNullOrWhiteSpace(asset.Name) || string.IsNullOrWhiteSpace(asset.Extension))
                throw new InvalidOperationException("Invalid Asset");
            string filename = _GetRelativeContentFilename(asset);
            var rel = _GetRelativeDirectory(asset);
            var currenDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = ContentDir;

            try
            {
                if (!Directory.Exists(rel))                
                    Directory.CreateDirectory(rel);                

                using (FileStream fs = File.Create(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.File, null));
                    formatter.Serialize(fs, asset);
                }
                //Log(string.Format("writing asset to:{0}", filename));
            }
            finally
            {
                Environment.CurrentDirectory = currenDir;
            }
        }

        public Asset LoadAsset(ExternalARef refe)
        {
            var file = refe.Location;
            var cd = Environment.CurrentDirectory;
            Environment.CurrentDirectory = ContentDir;
            Asset asset;
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.File, null));
                    asset = (Asset)formatter.Deserialize(fs);
                }
            }
            finally
            {
                Environment.CurrentDirectory = cd;                
            }

            return asset;
        }

        public void DeleteAsset(ExternalARef refe)
        {
            var cd = Environment.CurrentDirectory;
            Environment.CurrentDirectory = ContentDir;
            try
            {
                File.Delete(refe.Location);
            }
            finally
            {
                Environment.CurrentDirectory = cd;  
            }
        }

        public ICollection<Asset> GetAssets(AssetType type)
        {
            List<Asset> assets = new List<Asset>();
            var dir = Path.Combine(ContentDir, Enum.GetName(typeof(AssetType), type));
            if (!Directory.Exists(dir)) return new List<Asset>(0);

            var assetManager = Service.Require<AssetManager>();

            foreach (var file in Directory.EnumerateFiles(dir))
            {
                assets.Add(assetManager.GetAsset(file.Substring(contentDir.Length + 1, file.Length - contentDir.Length - 1)));
            }

            return assets;
        }

        public ICollection<T> GetAssets<T>() where T : Asset
        {
            var attr = (AssetAttribute[])typeof(T).GetCustomAttributes(typeof(AssetAttribute), true);
            if (attr.Length == 0)
                return new List<T>(0);

            List<T> assets = new List<T>();
            var type = attr[0].AssetType;          
            var dir = Path.Combine(ContentDir, Enum.GetName(typeof(AssetType), type));
            if (!Directory.Exists(dir)) return new List<T>(0);


            var assetManager = Service.Require<AssetManager>();

            foreach (var file in Directory.EnumerateFiles(dir))
            {
                assets.Add((T)assetManager.GetAsset(file.Substring(contentDir.Length + 1, file.Length - contentDir.Length - 1)));
            }

            return assets;
        }

        public ICollection<AssetReference> GetReferences(AssetType type)
        {
            List<AssetReference> references = new List<AssetReference>();           
            var relative = Enum.GetName(typeof(AssetType), type);
            var dir = Path.Combine(ContentDir, relative);
            if (!Directory.Exists(dir)) return new List<AssetReference>(0);

            foreach (var file in Directory.EnumerateFiles(dir))
            {
                references.Add(file.Substring(contentDir.Length + 1, file.Length - contentDir.Length - 1));
            }

            return references;
        }

        public ICollection<AssetReference> GetReferences<T>() where T : Asset
        {
            var attr = (AssetAttribute[])typeof(T).GetCustomAttributes(typeof(AssetAttribute), true);
            if (attr.Length == 0)
                return new List<AssetReference>(0);

            var type = attr[0].AssetType;      
            List<AssetReference> references = new List<AssetReference>();
            var relative = Enum.GetName(typeof(AssetType), type);
            var dir = Path.Combine(ContentDir, relative);

            if (!Directory.Exists(dir)) return new List<AssetReference>(0);

            foreach (var file in Directory.EnumerateFiles(dir))
            {
                references.Add(file.Substring(contentDir.Length + 1, file.Length - contentDir.Length - 1));
            }

            return references;
        }

        public ExternalARef CreateReference(Asset asset)
        {
           return new ExternalARef(Path.Combine(Enum.GetName(typeof(AssetType), asset.AssetType), asset.Name + asset.Extension));
        }
    }
}
