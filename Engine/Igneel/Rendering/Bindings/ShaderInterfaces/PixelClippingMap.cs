namespace Igneel.Rendering.Bindings
{
    public interface PixelClippingMap
    {
        bool NoRenderTransparency { get; set; }
        bool NoRenderOpaque { get; set; }
    }
}