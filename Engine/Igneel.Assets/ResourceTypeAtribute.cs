using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    public class ResourceTypeAtribute:Attribute
    {
        public ResourceTypeAtribute(string type)
        {
            this.ResourceType = type;
        }

        public string ResourceType { get; set; }
    }
}
