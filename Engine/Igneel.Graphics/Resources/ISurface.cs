using System;
namespace Igneel.Graphics
{
    public interface ISurface
    {
        int Height { get; }
        Igneel.Graphics.Multisampling Sampling { get; }
        Igneel.Graphics.Format SurfaceFormat { get; }
        int Width { get; }
    }

}
