using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Design
{         
   
    public class PropertyDescriptorAttribute : Attribute
    {
        Type descriptorType;

        public Type DescriptorType
        {
            get { return descriptorType; }
            set { descriptorType = value; }
        }

        public PropertyDescriptorAttribute(Type descriptorType)
        {
            this.descriptorType = descriptorType;
        }
    }

   

}
