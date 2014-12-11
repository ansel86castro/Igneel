using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{    

    class DefaultContextFactory:IFactory<AssetContext>
    {
        public AssetContext CreateInstance()
        {
            return new AssetContext();
        }
    }
   
}
