using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    //public interface Texture : ShaderResource, INameable
    //{     
    //    int MipLevels { get; }

    //    Format Format { get; }

    //    ResourceUsage Usage { get; }

    //    BindFlags DeviceBind { get; }

    //    CpuAccessFlags CpuAccess { get; }

    //    void UnMap(int subResource);
    //}

    public abstract class Texture : Resource, IGraphicResource, IShaderResource, INameable
    {
        private  ResourceType _type;
        private GraphicDevice _device;

        protected Texture(GraphicDevice device)
        {
            _device = device;
        }

        protected Texture(GraphicDevice device, ResourceType type)
            : this(device)
        {
            _type = type;
        }

        protected Texture(string name, ResourceType type)       
        {
            _type = type;
            Name = name;
        }

        public GraphicDevice Device { get { return _device; } }

        public ResourceType Type{get { return _type; }}

        public int MipLevels { get; protected set; }

        public Format Format { get; protected set; }

        public ResourceUsage Usage { get; protected set; }

        public BindFlags DeviceBind { get; protected set; }

        public CpuAccessFlags CpuAccess { get; protected set; }
     
        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public static int CalcSubresource(int mipSlice, int arraySlice, int mipLevels)
        {
            return mipSlice + (arraySlice * mipLevels);
        }

        public static void DecoupleSubresource(int subResource, int mipLevels ,out int arrayIndex, out int mipLevel )
        {
            arrayIndex = subResource / mipLevels;
            mipLevel = subResource % mipLevels;
        }

        public abstract void UnMap(int subResource);
    
    }
}
