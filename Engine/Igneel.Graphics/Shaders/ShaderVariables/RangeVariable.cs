using System;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    public sealed class RangeVariable<T> : ShaderVariable
        where T:struct
    {
        public SArray<T> Value;
      
        public static int Size = ClrRuntime.Runtime.SizeOf<T>();

        public override sealed unsafe void SetValue()
        {
            if (Value.Array == null) return;               
            GCHandle handle = GCHandle.Alloc(Value.Array, GCHandleType.Pinned);
            try
            {
                byte* pter = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(Value.Array, 0).ToPointer();
                Binder.SetValue(pter + Value.Offset, Size * Value.Count);
            }
            finally
            {
                handle.Free();
            }
        }

        public override Type ValueType
        {
            get { return typeof(SArray<T>); }
        }
    }
}