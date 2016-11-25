using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Techniques
{
    public struct HitTestResult
    {
        public object Data { get; set; }
        public IDrawable Drawable { get; set; }
        public Frame Frame { get; set; }
        public int RenderId { get; set; }
    }
}
