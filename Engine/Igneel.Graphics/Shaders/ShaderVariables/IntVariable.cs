namespace Igneel.Graphics
{
    public sealed class IntVariable : ShaderVariable<int>
    {       
        public override void SetValue()
        {
            Binder.SetInt(Value);
        }        
    }
}