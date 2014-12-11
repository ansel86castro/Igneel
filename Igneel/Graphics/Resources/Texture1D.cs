using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public interface Texture1D : Texture
    {
        Texture1DDesc Description { get; }

        int Width { get; }

        IntPtr Map(int subResource, MapType map, bool doNotWait);       
    }

    public abstract class Texture1DBase : TextureBase,  Texture1D
    {      
        protected Texture1DDesc _desc;

        public Texture1DBase()
            :base(ResourceType.Texture1D)
        { }

        public Texture1DBase(Texture1DDesc desc)
            :base(ResourceType.Texture1D)
        {            
            _desc = desc;
        }

        public Texture1DDesc Description
        {
            get { return _desc; }
            protected set
            {
                _desc = value;              
            }
        }

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width { get { return _desc.Width; } }

        public override int MipLevels { get { return _desc.MipLevels; } }

        public int ArraySize { get { return _desc.ArraySize; } }

        public override Format Format { get { return _desc.Format; } }

        public override ResourceUsage Usage { get { return _desc.Usage; } }

        public override BindFlags DeviceBind { get { return _desc.BindFlags; } }

        public override CpuAccessFlags CpuAccess { get { return _desc.CPUAccessFlags; } }

        public abstract IntPtr Map(int subResource, MapType map, bool doNotWait);        
        
    }
}
