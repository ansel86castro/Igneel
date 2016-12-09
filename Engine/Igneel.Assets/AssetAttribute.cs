using System;

namespace Igneel.Assets
{
    public class AssetAttribute : Attribute
    {
        public AssetAttribute(string resourceType)
        {
            this.ResourceType = resourceType;
        }

        public string ResourceType { get; set; }
    }
}