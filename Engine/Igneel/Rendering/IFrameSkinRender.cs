using System;
namespace Igneel.Rendering
{
    public interface IFrameSkinRender:IGraphicRender
    {
        void Draw(Igneel.SceneComponents.FrameSkin component);
        int MaxNbBones { get; }
        int MaxPaletteMatrices { get; }
    }
}
