using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Windows
{
    public class WindowFont:GraphicFont
    {
        Bitmap bitmap;
        Font font;

        public WindowFont(GraphicDevice device, Font font)
            : base(device)
        {
            this.font = font;
            Family = font.FontFamily.Name;
            FontHeight = font.Height;
            
        }

        protected override void OnSizeChanged()
        {
            throw new NotImplementedException();
        }

        protected override void DrawFontToBitmap()
        {
            throw new NotImplementedException();
        }

        protected override void DisposeBitmap()
        {
            throw new NotImplementedException();
        }

        protected override MappedTexture2D LockFontBitmap(Format format)
        {
            throw new NotImplementedException();
        }

        protected override void UnlockFontBitmap()
        {
            throw new NotImplementedException();
        }
    }
}
