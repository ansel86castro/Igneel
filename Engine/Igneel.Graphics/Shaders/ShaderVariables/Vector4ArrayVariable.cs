using System;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    public sealed class Vector4ArrayVariable : ShaderVariableArray<Vector4>
    {
        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            try
            {
                void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();
                Binder.SetVectorArray((Vector4*)pter, Math.Min(Elements, Value.Length));
            }
            finally
            {
                handle.Free();
            }
        }
    }
}