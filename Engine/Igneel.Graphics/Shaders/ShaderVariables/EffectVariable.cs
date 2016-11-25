using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShaderVariable<T> : ShaderVariable
        where T:struct
    {                
       public static int Size = ClrRuntime.Runtime.SizeOf<T>();               

        public T Value;

        public override unsafe void SetValue()
        {
            void* pter = ClrRuntime.Runtime.GetPointer(ref Value);
            Binder.SetValue(pter, Size);
        }

        public override Type ValueType
        {
            get { return typeof(T); }
        }
    }

    public class ShaderVariableArray<T> : ShaderVariableArray
        where T:struct
    {
        public T[] Value;

        public static int Size = ClrRuntime.Runtime.SizeOf<T>();       

        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            void* pter =  ClrRuntime.Runtime.GetPointer(Value, 0);
            Binder.SetValue(pter, Size *  Math.Min(Elements , Value.Length));
            handle.Free();
        }
           
        public override Type ValueType
        {
            get { return typeof(T[]); }
        }
    }
}
