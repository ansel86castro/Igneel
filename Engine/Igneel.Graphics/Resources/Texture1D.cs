using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    //public interface Texture1D : Texture
    //{
    //    Texture1DDesc Description { get; }

    //    int Width { get; }

    //    IntPtr Map(int subResource, MapType map, bool doNotWait);       
    //}
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Texture1DDesc
    {
        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width;

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

        public void SetDefaults()
        {
            MipLevels = 1;
            Format = Graphics.Format.UNKNOWN;
            Usage = ResourceUsage.Default;
            BindFlags = Graphics.BindFlags.ShaderResource;
            CPUAccessFlags = CpuAccessFlags.None;
            Options = ResourceOptionFlags.None;
            Width = 1;
            ArraySize = 1;
        }
    }

    public abstract class Texture1D : Texture
    {      
        protected Texture1DDesc _desc;

        protected Texture1D(GraphicDevice device)
            :base(device,ResourceType.Texture1D)
        { }

        protected Texture1D(GraphicDevice device, Texture1DDesc desc)
            :base(device, ResourceType.Texture1D)
        {            
            Description = desc;
        }

        public Texture1DDesc Description
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

        /// <summary>
        /// Texture width (in texels). 
        /// </summary>
        public int Width { get { return _desc.Width; } }
        
        public abstract IntPtr Map(int subResource, MapType map, bool doNotWait);        
        
        public static implicit operator Sampler<Texture1D>(Texture1D texture)
        {
            return texture.ToSampler();
        }
    }
}
