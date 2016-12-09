using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    //public interface Texture2D : Texture
    //{
    //    Texture2DDesc Description { get; }

    //    /// <summary>
    //    /// Texture width (in texels). 
    //    /// </summary>
    //    int Width { get; }

    //    int Height { get; }

    //    Multisampling MSAA { get; }     

    //    MappedTexture2D Map(int subResource, MapType map, bool doNotWait = false);
    //}

    public struct Texture2DDesc
    {
        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width;

        /// <summary>
        /// Texture height (in texels). 
        /// </summary>
        public int Height;

        /// <summary>
        /// Number of subtextures (also called mipmap levels). Use 1 for a multisampled texture; or 0 to generate a full set of subtextures. 
        /// </summary>
        public int MipLevels;

        /// <summary>
        /// Number of textures in the array.
        /// </summary>
        public int ArraySize;

        /// <summary>
        /// Texture format 
        /// </summary>
        public Format Format;

        /// <summary>
        /// Multisampling
        /// </summary>
        public Multisampling SamplerDesc;

        /// <summary>
        /// Value that identifies how the texture is to be read from and written to. The most common value is ResourceUsage.Default; see ResourceUsage for all possible values
        /// </summary>
        public ResourceUsage Usage;

        /// <summary>
        /// Flags for binding to pipeline stages .The flags can be combined by a logical OR.
        /// </summary>
        public BindFlags BindFlags;

        /// <summary>
        /// Flags to specify the types of CPU access allowed. Use 0 if CPU access is not required. These flags can be combined with a logical OR
        /// </summary>
        public CpuAccessFlags CPUAccessFlags;

        /// <summary>
        /// Flags (see D3D10_RESOURCE_MISC_FLAG) that identifies other, less common resource options. Use 0 if none of these flags apply. These flags can be combined with a logical OR.
        /// </summary>
        public ResourceOptionFlags Options;

    }

    public struct MappedTexture2D
    {
        /// <summary>
        /// The pitch, or width, or physical size (in bytes), of one row of an uncompressed texture. A block-compressed texture is encoded in 4x4 blocks (see virtual size vs physical size) ; therefore, RowPitch is the number of bytes in a block of 4x4 texels. 
        /// </summary>
        public int RowPitch;

        /// <summary>
        /// Pointer to the data. 
        /// </summary>
        public IntPtr DataPointer;
    }

    public abstract class Texture2D : Texture
    {        
        protected Texture2DDesc _desc;

        protected Texture2D(GraphicDevice device) 
            : base(device, ResourceType.Texture2D) 
        {
 
        }

        protected Texture2D(GraphicDevice device, Texture2DDesc desc)
            :base(device, ResourceType.Texture2D)
        {
            _desc = desc;
        }

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width { get { return _desc.Width; } }

        public int Height { get { return _desc.Height; } }

        public Multisampling MSAA { get { return _desc.SamplerDesc; } }       

        public int ArraySize { get { return _desc.ArraySize; } }

        public Texture2DDesc Description
        {
            get { return _desc; }
            protected set
            {
                _desc = value;
                MipLevels = _desc.MipLevels;
                Format = _desc.Format;
                Usage = _desc.Usage;
                DeviceBind = _desc.BindFlags;
                CpuAccess = _desc.CPUAccessFlags;
            }
        }

        public abstract MappedTexture2D Map(int subResource, MapType map, bool doNotWait = false);

        public static implicit operator Sampler<Texture2D>(Texture2D texture)
        {
            return texture.ToSampler();
        }
          
    }
}
