using System;
using Igneel.Graphics;

namespace Igneel.Graphics
{
    public abstract class ShaderVariable
    {      
        public IUniformSetter Binder;   
   
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
}