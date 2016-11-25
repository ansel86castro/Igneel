namespace Igneel.Graphics
{
    public sealed class FloatVariable : ShaderVariable<float>
    {
        public override unsafe void SetValue()
        {
            Binder.SetFloat(Value);
        }
    }
}