using System;
using Igneel;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
namespace ForgeEditor.Components.CoordinateSystems
{
    public interface IDecalGlyp:IGraphicObject
    {
        GlypComponent[] Components { get; }
        RenderTexture2D RenderTarget { get; }
        Igneel.Rectangle ScreenRectangle { get; set; }
        GlypComponent DoHitTest(Igneel.Vector2 location);
        void DrawDecal();       
        void Translate(int x, int y);
    }

    
}
