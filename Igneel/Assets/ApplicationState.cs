using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    [Serializable]
    [Asset(Assets.AssetType.Settings, ".app")]
    public abstract class ApplicationStateAsset:Asset
    {
        public ApplicationStateAsset()
            : base(null, "default")
        {

        }
    }
}
