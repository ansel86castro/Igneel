using System;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class SamplerStateVariableArray : ShaderVariableArray
    {
        public SamplerState[] Value;

        public override void SetValue()
        {
            if (Value == null)
                Binder.SetSampler(null, Elements);
            else
                Binder.SetSampler(Value, Value.Length);
        }

        public override Type ValueType
        {
            get { return typeof(SamplerState[]); }
        }
    }
}