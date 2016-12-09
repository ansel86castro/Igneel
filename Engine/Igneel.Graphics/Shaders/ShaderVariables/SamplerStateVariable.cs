using System;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class SamplerStateVariable : ShaderVariable
    {
        public SamplerState Value;

        public override void SetValue()
        {
            Binder.SetSampler(Value);
        }

        public override Type ValueType
        {
            get { return typeof(SamplerState); }
        }
    }
}