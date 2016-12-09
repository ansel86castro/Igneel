using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Assets
{
    //[Serializable]
    //public abstract class AssetReference : SerializableBase
    //{      
    //    [NonSerialized]
    //    bool isDirt;      
      
    //    /// <summary>
    //    /// Indicates the asset needs to be saved to the store
    //    /// </summary>
    //    public bool IsDirt { get { return isDirt; } set { isDirt = value; } }

    //    public static AssetReference CreateReference(int id)
    //    {
    //        return new InternalARef(id);
    //    }

    //    public static AssetReference CreateReference()
    //    {
    //        return new InternalARef();
    //    }

    //    public static AssetReference CreateReference(string filename)
    //    {
    //        return new ExternalARef(filename);
    //    }

    //    public static explicit operator AssetReference(Asset asset)
    //    {
    //        return asset.Id;
    //    }

    //    public static implicit operator AssetReference(int id)
    //    {
    //        return new InternalARef(id);
    //    }

    //    public static implicit operator AssetReference(string file)
    //    {
    //        return new ExternalARef(file);
    //    }
    //}

    //[Serializable]
    //public sealed class InternalARef : AssetReference, IEquatable<InternalARef>
    //{
    //    static int idCounter;        

    //    int id;

    //    public int Id { get { return id; } }

    //    internal InternalARef()
    //    {
    //        id = ++idCounter;
    //    }

    //    internal InternalARef(int id)
    //    {
    //        this.id = id;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return id.GetHashCode();
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        var other = obj as InternalARef;
    //        if (other != null)
    //            return id == other.id;
    //        return false;

    //    }

    //    public override string ToString()
    //    {
    //        return id.ToString();
    //    }

    //    public bool Equals(InternalARef other)
    //    {
    //        return other != null ? id == other.id : false;
    //    }
    //}

    //[Serializable]
    //public sealed class ExternalARef : AssetReference,IEquatable<ExternalARef>
    //{
    //    string location;

    //    public string Location { get { return location; } }

    //    public ExternalARef(string location)
    //    {
    //        if (location == null)
    //            throw new ArgumentNullException("location");

    //        this.location = location;
    //    }

    //    public override int GetHashCode()
    //    {            
    //         return location.GetHashCode();
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        ExternalARef other = obj as ExternalARef;
    //        if (other != null)
    //        {
    //            return other.location.Equals(location);
    //        }
    //        return false;
    //    }

    //    public override string ToString()
    //    {
    //        return location;
    //    }

    //    public bool Equals(ExternalARef other)
    //    {
    //        if (other == null)
    //            return false;

    //        return location == other.location;
    //    }
    //}
}
