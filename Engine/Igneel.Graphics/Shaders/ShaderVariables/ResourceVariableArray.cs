using System;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public sealed class ResourceVariableArray : ShaderVariableArray
    {
        public IShaderResource[] Value;

        public override void SetValue()
        {
            if (Value == null)
                Binder.SetResource(null, Elements);
            else
                Binder.SetResource(Value, Value.Length);
        }

        public override Type ValueType
        {
            get { return typeof(IShaderResource[]); }
        }
    }
}