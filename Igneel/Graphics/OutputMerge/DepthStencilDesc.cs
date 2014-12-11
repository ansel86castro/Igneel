using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum DepthStencilDimension
    {
        TEXTURE2D,
        TEXTURE2DARRAY,
        TEXTURECUBE
    }
    public class DepthStencilDesc
    {
        public int Width;
        public int Height;
        public Format Format = Format.D32_FLOAT;
        public Multisampling Sampling = new Multisampling(1, 0);
        public bool Readable = false;
        public int ArraySize = 1;
        public DepthStencilDimension Dimension = DepthStencilDimension.TEXTURE2D;

        public DepthStencilDesc() { }

        public DepthStencilDesc(int width, int height, Format format, Multisampling sampling, bool readable)
        {
            Width = width;
            Height = height;
            this.Format = format;
            Sampling = sampling;
            Readable = readable;
        }
    }
}
