using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{  
    public abstract class ShaderVariable
    {      
        public IUniformSetter binder;   
   
        public string Name { get; set; }
       
        public abstract void SetValue();      

        //public void SetSetter(IUniformSetter binder)
        //{
        //    this.binder = binder;
        //    if (binder == null)
        //        throw new ArgumentNullException();
        //    SetValue();           
        //}

        //public void Commit()
        //{
        //    if (binder != null)
        //    {
        //        SetValue();
        //    }
        //}

        public abstract Type ValueType { get; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }

    public abstract class ShaderVariableArray : ShaderVariable
    {
        public int Elements;      
    }

    public class ShaderVariable<T> : ShaderVariable
        where T:struct
    {                
       public static int Size = ClrRuntime.Runtime.SizeOf<T>();               

        public T Value;

        public override unsafe void SetValue()
        {
            void* pter = ClrRuntime.Runtime.GetPointer(ref Value);
            binder.SetValue(pter, Size);
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
            binder.SetValue(pter, Size *  Math.Min(Elements , Value.Length));
            handle.Free();
        }
           
        public override Type ValueType
        {
            get { return typeof(T[]); }
        }
    }

    public sealed class IntVariable : ShaderVariable<int>
    {       
        public sealed override void SetValue()
        {
            binder.SetInt(Value);
        }        
    }

    public sealed class IntArrayVariable : ShaderVariableArray<int>
    {                
        public sealed override unsafe void SetValue()
        {
            if (Value == null) return;
            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();

            binder.SetIntArray((int*)pter, Math.Min(Elements, Value.Length));

            handle.Free();
        }
                
    }

    public sealed class BoolVariable : ShaderVariable
    {
        public bool Value;

        public sealed override void SetValue()
        {
            binder.SetBool(Value);
        }

        public override Type ValueType
        {
            get { return typeof(bool); }
        }
    }

    public class BoolArrayVariable : ShaderVariableArray<bool>
    {                
        public override unsafe void SetValue()
        {
            if (Value == null) return;
            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();

            binder.SetBoolArray((bool*)pter, Math.Min(Elements, Value.Length));

            handle.Free();

        }      
    }

    public sealed class FloatVariable : ShaderVariable<float>
    {
        public override unsafe void SetValue()
        {
            binder.SetFloat(Value);
        }
    }

    public sealed class MatrixVariable : ShaderVariable<Matrix>
    {
        public override unsafe void SetValue()
        {
            binder.SetMatrix(Value);
        }
    }

    public sealed class Vector4Variable : ShaderVariable<Vector4>
    {
        public override unsafe void SetValue()
        {
            binder.SetVector(Value);
        }
    }


    public sealed class FloatArrayVariable : ShaderVariableArray<float>
    {
        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            try
            {              
                void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();
                binder.SetFloatArray((float*)pter, Math.Min(Elements, Value.Length));

            }
            finally
            {
                handle.Free();
            }
        }
    }

    public sealed class MatrixArrayVariable : ShaderVariableArray<Matrix>
    {
        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            try
            {
                void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();
                binder.SetMatrixArray((Matrix*)pter, Math.Min(Elements, Value.Length));
            }
            finally
            {
                handle.Free();
            }
        }
    }

    public sealed class Vector4ArrayVariable : ShaderVariableArray<Vector4>
    {
        public override unsafe void SetValue()
        {
            if (Value == null) return;

            GCHandle handle = GCHandle.Alloc(Value, GCHandleType.Pinned);
            try
            {
                void* pter = Marshal.UnsafeAddrOfPinnedArrayElement(Value, 0).ToPointer();
                binder.SetVectorArray((Vector4*)pter, Math.Min(Elements, Value.Length));
            }
            finally
            {
                handle.Free();
            }
        }
    }

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
                binder.SetValue(pter + Value.Offset, Size * Value.Count);
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

    public sealed class ResourceVariable : ShaderVariable
    {
        public ShaderResource Value;

        public override void SetValue()
        {
            binder.SetResource(Value);
        }

        public override Type ValueType
        {
            get { return typeof(ShaderResource); }
        }
    }

    public sealed class ResourceVariableArray : ShaderVariableArray
    {
        public ShaderResource[] Value;

        public override void SetValue()
        {
            if (Value == null)
                binder.SetResource(null, Elements);
            else
                binder.SetResource(Value, Value.Length);
        }

        public override Type ValueType
        {
            get { return typeof(ShaderResource[]); }
        }
    }

    public sealed class SamplerStateVariable : ShaderVariable
    {
        public SamplerState Value;

        public override void SetValue()
        {
            binder.SetSampler(Value);
        }

        public override Type ValueType
        {
            get { return typeof(SamplerState); }
        }
    }

    public sealed class SamplerStateVariableArray : ShaderVariableArray
    {
        public SamplerState[] Value;

        public override void SetValue()
        {
            if (Value == null)
                binder.SetSampler(null, Elements);
            else
                binder.SetSampler(Value, Value.Length);
        }

        public override Type ValueType
        {
            get { return typeof(SamplerState[]); }
        }
    }

    public sealed class SamplerVariable<T> : ShaderVariable<Sampler<T>>
        where T:Texture
    {
        public override sealed unsafe void SetValue()
        {
            binder.SetResource(Value.Texture);
            binder.SetSampler(Value.State);
        }
    }

    public sealed class SamplerVariableArray<T> : ShaderVariable<SamplerArray<T>>
        where T:Texture
    {
        public int Elements;

        public override unsafe void SetValue()
        {
            if (Value.Textures == null)
            {
                binder.SetResource(null, Elements);
                binder.SetSampler(null, Elements);
            }
            else
            {
                binder.SetResource(ClrRuntime.Runtime.StaticCast<ShaderResource[]>(Value.Textures), Elements);
                binder.SetSampler(Value.States, Elements);
            }
        }
    }
}
