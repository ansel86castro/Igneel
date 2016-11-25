using System;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    public sealed class FloatArrayVariable : ShaderVariableArray<float>
    {
        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            try
            {              
                void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();
                Binder.SetFloatArray((float*)pter, Math.Min(Elements, Value.Length));

            }
            finally
            {
                handle.Free();
            }
        }
    }
}