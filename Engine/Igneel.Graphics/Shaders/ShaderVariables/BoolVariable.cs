using System;

namespace Igneel.Graphics
{
    public sealed class BoolVariable : ShaderVariable
    {
        public bool Value;

        public sealed override void SetValue()
        {
            Binder.SetBool(Value);
        }

        public override Type ValueType
        {
            get { return typeof(bool); }
        }
    }
}