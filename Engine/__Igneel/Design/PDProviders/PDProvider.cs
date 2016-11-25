using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Igneel.Design.UITypeEditors;
using System.Drawing.Design;

namespace Igneel.Design
{
    public interface IPDProvider
    {        
        PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes);   
    }

    public abstract class PDProviderAttribute : Attribute ,IPDProvider
    {             
       public abstract PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes);   
    }

    public class DeferredAttribute : PDProviderAttribute
    {
        public override PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes)
        {
            return new DeferredPropertyDescriptors(pd);
        }
    }

    public class LockOnSetAttribute : PDProviderAttribute
    {
        public override PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes)
        {
            return new LockeablePropertyDescriptors(pd, DesignTypeConverter.LockFunction);
        }
    }   

    public class DynamicEditableAttribute : PDProviderAttribute
    {
        public override PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes)
        {
            object instance = pd.GetValue(component);
            if (instance != null)
            {
                EditorAttribute attr = instance.GetType().GetCustomAttribute<EditorAttribute>(true);
                if (attr != null)
                    return TypeDescriptor.CreateProperty(component.GetType(), pd, attr);
            }
            return TypeDescriptor.CreateProperty(component.GetType(), pd, new EditorAttribute(typeof(UICreatorEditor), typeof(UITypeEditor)));
        }
    }


    public class PropertyConstrainAttribute : PDProviderAttribute
    {
        string propName;
        public PropertyConstrainAttribute(string propName)
        {
            this.propName = propName;
        }
        public override PropertyDescriptor CreateProperty(object component, PropertyDescriptor pd, params Attribute[] attributes)
        {
            PropertyInfo pi = component.GetType().GetProperty(propName);
            return new ConstrainToPropertyDescriptor(pd, comp=> (bool)pi.GetValue(comp));
        }
    }
    
}
