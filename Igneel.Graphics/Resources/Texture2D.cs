using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    public interface Texture2D : Texture
    {
        Texture2DDesc Description { get; }

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        int Width { get; }

        int Height { get; }

        Multisampling MSAA { get; }     

        MappedTexture2D Map(int subResource, MapType map, bool doNotWait = false);
    }

    public abstract class Texture2DBase : TextureBase, Texture2D
    {        
        protected Texture2DDesc _desc;

        public Texture2DBase() : base(ResourceType.Texture2D) { }

        public Texture2DBase(Texture2DDesc desc)
            :base(ResourceType.Texture2D)
        {
            _desc = desc;
        }

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width { get { return _desc.Width; } }

        public int Height { get { return _desc.Height; } }

        public Multisampling MSAA { get { return _desc.SamplerDesc; } }

        public override int MipLevels { get { return _desc.MipLevels; } }

        public int ArraySize { get { return _desc.ArraySize; } }

        public override Format Format { get { return _desc.Format; } }

        public override ResourceUsage Usage { get { return _desc.Usage; } }

        public override BindFlags DeviceBind { get { return _desc.BindFlags; } }

        public override CpuAccessFlags CpuAccess { get { return _desc.CPUAccessFlags; } }

        public Texture2DDesc Description { get { return _desc; } protected set { _desc = value; } }

        public abstract MappedTexture2D Map(int subResource, MapType map, bool doNotWait = false);
      
    
    }
}
