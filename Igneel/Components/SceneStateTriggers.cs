using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Components
{
    public class AllwaysOnSceneTriggers : AssetProvider<AllwaysOnSceneTriggers.TriggerAsset>, ISceneStateTrigger
    {
        public static AllwaysOnSceneTriggers Instance = new AllwaysOnSceneTriggers();

        public bool IsActive()
        {
            return true;
        }

        [Serializable]
        public class TriggerAsset: Asset
        {
            public TriggerAsset(IAssetProvider provider)
                : base(provider) { }

            public override IAssetProvider CreateProviderInstance()
            {
                return new AllwaysOnSceneTriggers();
            }
        }
    }    
}
