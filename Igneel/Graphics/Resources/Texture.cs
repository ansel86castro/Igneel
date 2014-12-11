using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    public interface Texture : ShaderResource
    {
        string Location { get; }

        int MipLevels { get; }

        Format Format { get; }

        ResourceUsage Usage { get; }

        BindFlags DeviceBind { get; }

        CpuAccessFlags CpuAccess { get; }

        void UnMap(int subResource);
    }

    public abstract class TextureBase :GraphicResource,  Texture
    {
        protected string _location;

        public TextureBase() { }

        public TextureBase(ResourceType type)
            :base(type)
        {            
        }

        public TextureBase(string location, ResourceType type)
            :base(type)
        {
            _location = location;
        }

        public abstract int MipLevels { get; }       

        public abstract Format Format { get; }

        public abstract ResourceUsage Usage { get; }

        public abstract BindFlags DeviceBind { get; }

        public abstract CpuAccessFlags CpuAccess { get; }

        public string Location
        {
            get { return _location; }
        }

        public override string ToString()
        {
            return _location ?? base.ToString();
        }

        public static int CalcSubresource(int mipSlice, int arraySlice, int mipLevels)
        {
            return mipSlice + (arraySlice * mipLevels);
        }


        public abstract void UnMap(int subResource);      
    }
}
