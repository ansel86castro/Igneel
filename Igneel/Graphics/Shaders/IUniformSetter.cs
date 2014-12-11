using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
 
    public interface IUniformSetter
    {        
        void SetInt(int value);

        void SetBool(bool value);

        void SetFloat(float value);

        void SetMatrix(Matrix value);

        void SetVector(Vector4 value);

        unsafe void SetValue(void* value, int size);

        unsafe void SetIntArray(int* value, int count);        

        unsafe void SetBoolArray(bool* value, int count);

        unsafe void SetFloatArray(float* value, int count);

        unsafe void SetMatrixArray(Matrix* value, int count);

        unsafe void SetVectorArray(Vector4* value, int count);
    }

    /// <summary>
    /// Each uniform in a effect belongs to several programs 
    /// and each program has a IUniformSetter for that uniform variable
    /// </summary>
    //public sealed class EffectUniformSetter:IUniformSetter
    //{
    //    IUniformSetter[] setters;

    //    internal EffectUniformSetter(IUniformSetter[] binders)
    //    {
    //        if (binders == null) throw new ArgumentNullException("binders");
    //        this.setters = binders;
    //    }


    //    public unsafe void SetValue(void* value, int size)
    //    {
    //        for (int i = 0; i < setters.Length; i++)
    //        {
    //            setters[i].SetValue(value, size);
    //        }
    //    }

    //    public void SetInt(int value)
    //    {
    //        for (int i = 0; i < setters.Length; i++)
    //        {
    //            setters[i].SetInt(value);
    //        }
    //    }

    //    public unsafe void SetIntArray(int* value, int count)
    //    {
    //        for (int i = 0; i < setters.Length; i++)
    //        {
    //            setters[i].SetIntArray(value,count);
    //        }
    //    }

    //    public void SetBool(bool value)
    //    {
    //        for (int i = 0; i < setters.Length; i++)
    //        {
    //            setters[i].SetBool(value);
    //        }
    //    }

    //    public unsafe void SetBoolArray(bool* value, int count)
    //    {
    //        for (int i = 0; i < setters.Length; i++)
    //        {
    //            setters[i].SetBoolArray(value , count);
    //        }
    //    }
    //}

    

}
