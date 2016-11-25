namespace Igneel.Rendering
{
    static class Binding<TBind, TShader>
        where TShader:Effect
    {
        public static IRenderBinding<TBind> Value;
    }
}