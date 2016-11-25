using System;

namespace Igneel.Assets
{
    [Serializable]
    public class ResourceReference:IEquatable<ResourceReference>
    {
        public int? Id { get; set; }

        public string Path { get; set; }        

        public ResourceReference(int id)
        {
            this.Id = id;
        }

        public ResourceReference(string path)
        {
            this.Path = path;
        }

        public bool Equals(ResourceReference other)
        {
            if (other == null)
                return false;
            if (Id != null)
            {
                return other.Id == Id;
            }
            if (Path != null)
            {
                return other.Path == Path;
            }
            return false;
        }

        public override bool Equals(object other)
        {
            return Equals(other as ResourceReference);
        }

        public override int GetHashCode()
        {
            return Id ?? ((Path != null) ? Path.GetHashCode() : base.GetHashCode());
        }
    }
}
