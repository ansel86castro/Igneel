using Igneel.Rendering;

namespace Igneel.SceneComponents
{

    public interface IGraphicObject : IDrawable
    {
        bool IsTransparent { get; }

        bool Visible { get; set; }

        GraphicMaterial Material { get; set; }

        Render GetRender();
    }   
}
