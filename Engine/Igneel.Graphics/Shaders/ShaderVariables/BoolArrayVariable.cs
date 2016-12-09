using System;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    public class BoolArrayVariable : ShaderVariableArray<bool>
    {                
        public override unsafe void SetValue()
        {
            if (Value == null) return;
            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();

            Binder.SetBoolArray((bool*)pter, Math.Min(Elements, Value.Length));

            handle.Free();

        }      
    }
}