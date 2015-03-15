using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using System.Diagnostics.Contracts;

namespace Igneel.Assets
{
    public enum AssetType
    {
        None = -1,
        Mesh = 0,
        Scene = 1,
        Shader = 2,
        Texture = 3,
        Template = 4,
        Settings = 5,
        Animation = 6,        
    }
  
    public class AssetManager
    {       
        Dictionary<IAssetProvider, AssetReference> assetRefLookup = new Dictionary<IAssetProvider, AssetReference>();     
        Dictionary<AssetReference, IAssetProvider> invAssetRefLookup = new Dictionary<AssetReference, IAssetProvider>();       
        Stack<AssetContext> loadingContexts = new Stack<AssetContext>();        
        Stack<AssetContext> savingContexts = new Stack<AssetContext>();
        private Action<AssetContext> contextInitialize;      

        internal protected AssetManager()
        {           
                       
        }

        public virtual void Initialize()
        {
            Clear();                   
        }

        public Action<AssetContext> ContextInitializer
        {
            get { return contextInitialize; }
            set { contextInitialize = value; }
        }   

        public static AssetManager Instance
        {
            get
            {
                return Service.Get<AssetManager>();
            }
        }     

        public AssetContext LoadContext
        {
            get { return loadingContexts.Count > 0 ? loadingContexts.Peek() : null; }
        }

        public AssetContext SaveContext
        {
            get { return savingContexts.Count > 0 ? savingContexts.Peek() : null; }
        }                           

        /// <summary>
        /// Remove the reference to the asset and inform to the provider that a reference to the asset was removed
        /// from the system. Also it means that the Asset was removed from the Store
        /// </summary>
        /// <param name="assetReference"></param>
        private void _DestroyAsset(AssetReference assetReference)
        {
            if (invAssetRefLookup.ContainsKey(assetReference))
            {
                var obj = invAssetRefLookup[assetReference];
                invAssetRefLookup.Remove(assetReference);
                assetRefLookup.Remove(obj);               
            }
        }       
     
        #region Saving

        private bool _ContainReference(IAssetProvider provider)
        {
            return assetRefLookup.ContainsKey(provider);
        }

        private bool _ContainReference(AssetReference refe)
        {
            return invAssetRefLookup.ContainsKey(refe);
        }

        /// <summary>
        /// Add a AssetReference for the corresponding provider
        /// </summary>
        /// <param name="aRef"></param>
        /// <param name="provider"></param>
        private void _AddAssetReference(AssetReference aRef, IAssetProvider provider)
        {
            try
            {
                assetRefLookup.Add(provider, aRef);
                invAssetRefLookup.Add(aRef, provider);
            }
            catch (ArgumentException)
            {

            }
        }

        private void _UpdateAssetReference(AssetReference aRef, IAssetProvider provider)
        {
            assetRefLookup[provider] = aRef;
            invAssetRefLookup[aRef] = provider;
        }

        /// <summary>
        /// Create the Reference to the Asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        internal void _CreateAssetReference(Asset asset, IAssetProvider provider, string path = null, bool register = true)
        {
            AssetReference refe;
            var context = savingContexts.Peek();

            if (asset.AssetType == AssetType.None)
            {
                //this is a internal or contextReference
                if (!context.TryGetReference(provider, out refe))
                {
                    refe = new InternalARef();
                    asset.Id = ((InternalARef)refe).Id;
                    context.RegisterProvider(provider, refe);
                }
            }
            else
            {
                //this is an external asset reference
                if (!assetRefLookup.TryGetValue(provider, out refe))
                {
                    if (string.IsNullOrWhiteSpace(asset.Name) || string.IsNullOrWhiteSpace(asset.Extension))
                        throw new InvalidOperationException("Invalid Asset");
                    
                    refe = Service.Require<IAssetRepository>().CreateReference(asset);
                    _AddAssetReference(refe, provider);
                }

                if (asset is AssetStorage)
                    context.Storage = (AssetStorage)asset;

                refe.IsDirt = true;
            }

            if (refe == null)
                Log(string.Format("Created reference :{0} for {1}", refe.ToString(), provider.ToString()));
        }

        /// <summary>
        /// Get the reference to the corresponding asset of the provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public AssetReference GetAssetReference(IAssetProvider provider)
        {
            if (provider == null)
                return null;

            AssetReference aRef = null;
            var context = SaveContext;
            if (context == null)
            {
                var asset = CreateAsset(provider);
                return assetRefLookup[provider];
            }

            //search in the current context for an internal reference;
            if (context.TryGetReference(provider, out aRef)) 
                return aRef;

            if (!assetRefLookup.TryGetValue(provider, out aRef) || aRef.IsDirt)
            {                                
                if (provider is IAssetStoreProvider)
                {
                    //creates an assets with a new context
                    var asset = CreateAsset(provider);
                    if (aRef == null)
                        assetRefLookup.TryGetValue(provider, out aRef);
                }
                else
                {
                    //creates the asset in the same context
                    Asset asset = provider.CreateAsset();
                    if (aRef == null)
                    {
                        //if it doesn't contains and external reference, pick up the internal reference in
                        //the context. The reference is created when the Assets is created and call the _CreateAssetReference
                        //method fron its contructor

                        if (!assetRefLookup.TryGetValue(provider, out aRef))
                            aRef = context.GetReference(provider);
                    }

                    if (aRef is InternalARef)
                    {
                        if (context.Storage == null) throw new NullReferenceException("There is any Storage for saving the asset in the current SavingContext");

                        //if the reference is an internal reference then save it in the Storage
                        context.Storage.AddAsset(asset);
                    }
                    else if (aRef is ExternalARef)
                    {
                        _SaveAsset(asset, (ExternalARef)aRef);
                    }
                }

                return aRef;
            }
            else if (aRef == null)
                throw new InvalidOperationException("The asset cant not be created");

            return aRef;                
        }

        /// <summary>
        /// Creates and saves and asset for the provider with a new context
        /// </summary>
        /// <param name="provider"></param>
        public Asset CreateAsset(IAssetProvider provider)
        {
            var context = CreateContext();
            savingContexts.Push(context);

            //create the asset to store the data and automatic create the AssetReference
            var asset = provider.CreateAsset();
            _SaveAsset(asset);

            AssetReference refe;
            if (assetRefLookup.TryGetValue(provider, out refe))
                refe.IsDirt = false;

            savingContexts.Pop();
            return asset;
        }

        private void _SaveAsset(Asset asset)
        {
            var rep = Service.Require<IAssetRepository>();
            rep.SaveAsset(asset);          
        }

        /// <summary>
        /// Save the asset in the corresponding location
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        private void _SaveAsset(Asset asset, ExternalARef refe)
        {
            var rep = Service.Require<IAssetRepository>();
            rep.SaveAsset(asset);
            refe.IsDirt = false;                       
        }               

        protected void CreateSavingContext()
        {
            savingContexts.Push(CreateContext());
        }

        protected void ClearSavingContext()
        {
            savingContexts.Pop();
        }

        public virtual void SaveAppState()
        {
            var stateFact = Service.GetFactory<ApplicationStateAsset>();
          
            var context = CreateContext();
            savingContexts.Push(context);  

            var asset = stateFact.CreateInstance();
            _SaveAsset(asset);

            savingContexts.Pop();

        }

        #endregion

        #region Load 
      
        public void RegisterProvider(IAssetProvider provider, AssetReference refe)
        {
            if (provider == null || refe == null) throw new NullReferenceException();

            if (refe is InternalARef && !LoadContext.ContainsProvider(refe))
                LoadContext.RegisterProvider(provider, refe);

            else if (refe is ExternalARef && !_ContainReference(provider))
                _AddAssetReference(refe, provider);
        }

        public Asset GetAsset(AssetReference refe)
        {
            if (refe is InternalARef)
                return LoadContext._GetAsset(refe);
            else
                return _LoadAsset((ExternalARef)refe);
        }        

        /// <summary>
        /// Return the AssetProvider for the Asset of this AssetReference if it already exist.
        /// or craete a new Provider if it doesn`t
        /// </summary>
        /// <param name="aRef"></param>
        /// <returns></returns>
        public IAssetProvider GetAssetProvider(AssetReference aRef)
        {
            if (aRef == null) return null;

            IAssetProvider provider;
            var context = loadingContexts.Peek();

            if (!invAssetRefLookup.TryGetValue(aRef, out provider) && !context.TryGetProvider(aRef, out provider))
            {
                if (aRef is ExternalARef)
                    provider = LoadProvider((ExternalARef)aRef);
                else
                {
                    var asset = context._GetAsset(aRef);
                    provider = asset.CreateProviderInstance();

                    if (provider != null)
                        Log(string.Format("Creating Provider: {0}", provider));

                    if (provider != null && !context.ContainsProvider(aRef))
                        context.RegisterProvider(provider, aRef);                    
                }
                return provider;
            }

            return provider;
        }

        public IAssetProvider GetAssetProvider(Asset asset)
        {
            if (asset == null) throw new ArgumentNullException("asset");

            var provider = asset.CreateProviderInstance();
            AssetReference refe = asset.Id;

            if (provider != null && !LoadContext.ContainsProvider(refe))
                LoadContext.RegisterProvider(provider, refe);

            return provider;
        }

        protected IAssetProvider _CreateProvider(Asset asset, ExternalARef refe = null)
        {
            var provider = asset.CreateProviderInstance();

            if (provider != null && refe != null)
            {
                if (!_ContainReference(provider))
                    _AddAssetReference(refe, provider);
                else
                    _UpdateAssetReference(refe, provider);

            }

            if (provider != null)
                Log(string.Format("Creating Provider: {0}", provider));
            return provider;
        }       

        protected Asset _LoadAsset(ExternalARef refe, bool kepContext = false)
        {
            var rep = Service.Require<IAssetRepository>();

            var context = CreateContext();
            loadingContexts.Push(context);

            var asset = rep.LoadAsset(refe);

            if (!kepContext)
               loadingContexts.Pop();

            return asset;

            //var cd = Environment.CurrentDirectory;
            //Environment.CurrentDirectory = ContentRoot.FullName;                     

            //Asset asset = null;
            //if (File.Exists(file))
            //{
            //    var context = AssetContext.CreateContext();
            //    loadingContexts.Push(context);


            //    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            //    {
            //        BinaryFormatter formatter = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.File, null));
            //        asset = (Asset)formatter.Deserialize(fs);
            //    }
            //    Log(string.Format("Loading Asset: {0}", file));

            //    if (!kepContext)
            //        loadingContexts.Pop();                
            //}

            //Environment.CurrentDirectory = cd;

            //return asset;
        }        

        /// <summary>
        /// Load the Asset from the store
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IAssetProvider LoadProvider(ExternalARef fileRef)
        {           
            var asset = _LoadAsset(fileRef, true);
            Contract.Requires(asset != null);

            var provider = _CreateProvider(asset, fileRef);

            loadingContexts.Pop();

            return provider;
        }

        public IAssetProvider LoadProvider(ExternalARef fileRef, Action<AssetContext> contextInitializer)
        {
            this.contextInitialize = contextInitializer;

            var asset = _LoadAsset(fileRef, true);                        

            Contract.Requires(asset != null);

            var provider = _CreateProvider(asset, fileRef);

            loadingContexts.Pop();

            this.contextInitialize = null;

            return provider;
        }

        public void LoadAppState()
        {
            //var appFile = Path.Combine(GetFullStoreDirectory(AssetType.Settings), "default.app");
            //if (File.Exists(appFile))
            //    LoadProvider(appFile);
            var rep = Service.Require<IAssetRepository>();

            var refe = (ExternalARef)rep.GetReferences(AssetType.Settings).FirstOrDefault();
            if (refe != null)            
            {
                LoadProvider(refe);
            }
        }

        #endregion      

        #region Delete

        public void DeleteFromContent(AssetReference refe)
        {
            if (refe != null && refe is ExternalARef)
            {
                DeleteFromContent((ExternalARef)refe);
            }
        }

        public void DeleteFromContent(ExternalARef refe)
        {
            if (refe != null)
            {
                var asset = _LoadAsset(refe);
                if (asset != null)
                {
                    asset.Delete();
                    Service.Require<IAssetRepository>().DeleteAsset(refe);                    
                }
                RemoveAssetReference(refe);
            }
        }        

        #endregion        

        public bool RemoveAssetReference(IAssetProvider provider)
        {
            AssetReference refe;
            if (assetRefLookup.TryGetValue(provider, out refe))
            {
                assetRefLookup.Remove(provider);
                invAssetRefLookup.Remove(refe);
                return true;
            }
            return false;
        }

        public bool RemoveAssetReference(AssetReference refe)
        {
            IAssetProvider provider;
            if (invAssetRefLookup.TryGetValue(refe, out provider))
            {
                invAssetRefLookup.Remove(refe);
                assetRefLookup.Remove(provider);                
                return true;
            }
            return false;
        }     

        //#region Content

        //private void CreateContentStructure()
        //{
        //    if (ContentRoot != null && !ContentRoot.Exists)
        //    {
        //        ContentRoot.Create();
        //        foreach (var name in ContentFolderNames)
        //        {
        //            var di = ContentRoot.CreateSubdirectory(name);
        //            if (!di.Exists) di.Create();
        //        }
        //    }
        //}

        //public void DeleteContentStructure()
        //{
        //    if (ContentRoot != null && ContentRoot.Exists)
        //        ContentRoot.Delete(true);
        //}

        //#endregion

        public void Log(string text)
        {
            var logService = Service.Get<ILogService>();
            if (logService != null)
                logService.Write(text);
        }

        public virtual void Clear()
        {
            ClearProviderCache();         
        }

        public void ClearProviderCache()
        {
            assetRefLookup.Clear();
            invAssetRefLookup.Clear();
        }

        public virtual void OnApplicationStart()
        {
           
        }

        public AssetContext CreateContext()
        {
            var context = new AssetContext();

            if(contextInitialize!=null)
                contextInitialize(context);

            return context;
        }
    }

    public class AssetManagerFactory : IFactory<AssetManager>
    {
        public AssetManager CreateInstance()
        {
            if(AssetManager.Instance == null)
                return new AssetManager();
            return AssetManager.Instance;
        }
    }
    
}
