namespace Igneel.Rendering
{
    public abstract class BindableTechnique<T> : Technique
    {
        T _binding;

        public BindableTechnique() { }      

        public T Binding { get { return _binding; } set { _binding = value; } }

        public override void Bind(Render render)
        {
            render.Bind(_binding);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(_binding);
        }
    }
}