namespace Igneel.Graphics
{
    public sealed class Vector4Variable : ShaderVariable<Vector4>
    {
        public override unsafe void SetValue()
        {
            Binder.SetVector(Value);
        }
    }
}