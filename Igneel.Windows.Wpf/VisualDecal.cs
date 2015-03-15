using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Igneel.Windows.Wpf
{
    public class VisualDecal:GraphicObject<VisualDecal>
    {
        Visual visual;
        RenderTargetBitmap bmp;
        DrawingVisual drawing;
        VisualBrush brush;

        public VisualDecal()
        {
            
        }
    }
}
