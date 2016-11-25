namespace Igneel.Graphics
{
    public sealed class MatrixVariable : ShaderVariable<Matrix>
    {
        public override unsafe void SetValue()
        {
            Binder.SetMatrix(Value);
        }
    }
}