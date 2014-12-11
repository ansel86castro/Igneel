using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum DrawFontFormat
    {
        TOP                  =    0x00000000,
        LEFT                 =    0x00000000,
        CENTER               =    0x00000001,
        RIGHT                =    0x00000002,
        VCENTER               =   0x00000004,
        BOTTOM               =    0x00000008,
        WORDBREAK             =   0x00000010,
        SINGLELINE            =   0x00000020,
        EXPANDTABS            =   0x00000040,
        //TABSTOP               =   0x00000080,
        NOCLIP                =   0x00000100,
        //EXTERNALLEADING       =   0x00000200,
        CALCRECT              =   0x00000400,
       // NOPREFIX              =   0x00000800,
       // INTERNAL              =   0x00001000
    }

    public abstract class GraphicFont
    {
        protected Font font;

        GraphicFont(Font font)
        {
            this.font = font;
        }

        public Font Font { get { return font; } }

        public abstract void DrawText(string text, Rectangle rec, Color4 color, DrawFontFormat format);
    }
}
