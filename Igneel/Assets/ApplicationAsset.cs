using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{  
    

    [Serializable]    
    public class EngineAsset:ApplicationStateAsset
    {
        StaticStoreAsset storage;
        
        public EngineAsset()            
        {
            storage = new StaticStoreAsset(typeof(Engine));            
        }     
             

        public override IAssetProvider CreateProviderInstance()
        {
            storage.CreateProviderInstance();
            return null;
        }
    }
}
