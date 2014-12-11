using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public interface Texture3D : Texture
    {
        Texture3DDesc Description { get; }

        int Width { get; }

        int Height { get; }

        int Depth { get; }

        MappedTexture3D Map(int subResource, MapType map, bool doNotWait);
    }
    public abstract class Texture3DBase : TextureBase, Texture3D
    {              
        protected Texture3DDesc _desc = new Texture3DDesc();

        public Texture3DBase() : this(new Texture3DDesc()) { }

        public Texture3DBase(Texture3DDesc desc)
            :base(ResourceType.Texture3D)
        {
            _desc = desc;
        }

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width { get { return _desc.Width; } }

        public int Height { get { return _desc.Height; } }

        public int Depth { get { return _desc.Depth; } }

        public override int MipLevels { get { return _desc.MipLevels; } }

        public override Format Format { get { return _desc.Format; } }

        public override ResourceUsage Usage { get { return _desc.Usage; } }

        public override BindFlags DeviceBind { get { return _desc.BindFlags; } }

        public override CpuAccessFlags CpuAccess { get { return _desc.CPUAccessFlags; } }

        public Texture3DDesc Description { get { return _desc; } protected set { _desc = value; } }

        public abstract MappedTexture3D Map(int subResource, MapType map, bool doNotWait);                 
    }
}
