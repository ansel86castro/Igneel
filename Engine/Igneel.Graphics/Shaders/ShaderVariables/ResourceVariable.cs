using System;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class ResourceVariable : ShaderVariable
    {
        public IShaderResource Value;

        public override void SetValue()
        {
            Binder.SetResource(Value);
        }

        public override Type ValueType
        {
            get { return typeof(IShaderResource); }
        }
    }
}