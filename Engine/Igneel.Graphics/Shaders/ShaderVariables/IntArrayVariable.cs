using System;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    public sealed class IntArrayVariable : ShaderVariableArray<int>
    {                
        public sealed override unsafe void SetValue()
        {
            if (Value == null) return;
            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();

            Binder.SetIntArray((int*)pter, Math.Min(Elements, Value.Length));

            handle.Free();
        }
                
    }
}