using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Igneel;
using Igneel.SceneComponents;

namespace Igneel.Windows.Wpf
{
    public class VisualDecal:GraphicObject<VisualDecal>
    {
        Visual _visual;
        RenderTargetBitmap _bmp;
        DrawingVisual _drawing;
        VisualBrush _brush;

        public VisualDecal()
        {
            
        }
    }
}
