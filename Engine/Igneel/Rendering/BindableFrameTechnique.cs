namespace Igneel.Rendering
{
    public abstract class BindableFrameTechnique<T> : FrameTechnique
        where T : FrameTechnique
    {

        public override void Bind(Render render)
        {
            render.Bind(ClrRuntime.Runtime.StaticCast<T>(this));
        }

        public override void UnBind(Render render)
        {
            render.UnBind(ClrRuntime.Runtime.StaticCast<T>(this));
        }
    }
}