using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
   

   

    /// <summary>
    ///  Structure that specifies multisampling parameters for the texture.
    /// </summary>
    public struct Multisampling
    {
        /// <summary>
        /// The number of multisamples per pixel.
        /// </summary>
        public int Count;

        /// <summary>
        /// The image quality level. The higher the quality, the lower the performance. The valid range is between zero and one less than the level returned by GraphicDevice.CheckMultiSampleQualityLevels
        /// </summary>
        public int Quality;

        public Multisampling(int count, int quality)
        {
            this.Count = count;
            this.Quality = quality;
        }

        public static Multisampling Disable { get { return new Multisampling(1, 0); } }

        public bool IsDisable { get { return Count == 1 && Quality == 0; } }
    }

    public class TextureDesc
    {
        /// <summary>
        /// Number of subtextures (also called mipmap levels). Use 1 for a multisampled texture; or 0 to generate a full set of subtextures. 
        /// </summary>
        public int MipLevels;

        /// <summary>
        /// Texture format 
        /// </summary>
        public Format Format;

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

        public TextureDesc()
        {
            MipLevels = 1;
            Format = Graphics.Format.UNKNOWN;
            SamplerDesc = Multisampling.Disable;
            Usage = ResourceUsage.Default;
            BindFlags = Graphics.BindFlags.ShaderResource;
            CPUAccessFlags = CpuAccessFlags.None;
            Options = ResourceOptionFlags.None;
        }

    }


}
