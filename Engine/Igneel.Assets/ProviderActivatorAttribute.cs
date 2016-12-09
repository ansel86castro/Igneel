using System;

namespace Igneel.Assets
{
    public class ResourceActivatorAttribute : Attribute
    {               
        public ResourceActivatorAttribute(Type type)
        {
            ActivatorType = type;
        }

        public Type ActivatorType { get; set; }             
       
    }
}