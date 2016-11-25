namespace Igneel.Rendering
{
    public interface IComponentRender<in T>
    {
        void Draw(T component);
    }
}