using Igneel.Design.UITypeEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Igneel.Design
{
    public class DesignTypeConverter: TypeConverter
    {
        public static Action<Action> LockFunction { get; set; }

        protected class FieldDescriptor : TypeConverter.SimplePropertyDescriptor
        {
            FieldInfo fi;

            public FieldDescriptor(FieldInfo field)
                : base(field.DeclaringType, field.Name, field.FieldType)
            {
                this.fi = field;
            }

            public FieldDescriptor(FieldInfo field, Attribute[] attributes)
                : base(field.DeclaringType, field.Name, field.FieldType, attributes)
            {
                this.fi = field;
            }

            public FieldInfo Field
            {
                get
                {
                    return fi;
                }
                set
                {
                    fi = value;
                }
            }

            public override object GetValue(object component)
            {
                return fi.GetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                fi.SetValue(component, value);
            }
        }

        //protected class EditablePropertyDescriptor : TypeConverter.SimplePropertyDescriptor
        //{
        //    PropertyDescriptor pd;            
        //    public EditablePropertyDescriptor(PropertyDescriptor pd, params Attribute[] attributes)
        //        : base(pd.ComponentType, pd.Name, pd.PropertyType, attributes)
        //    {
        //        this.pd = pd;                
        //    }

        //    public override object GetValue(object component)
        //    {                
        //        return pd.GetValue(component);
        //    }

        //    public override void SetValue(object component, object value)
        //    {
        //        if (!pd.IsReadOnly)
        //            pd.SetValue(component, value);
        //    }         
        //}       

        
        PropertyDescriptorCollection properties;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return false;
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string))
                return true;

            return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return "None";
            try
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    //Vector2 v =context.PropertyDescriptor.PropertyType (Vector2)value;
                    //var ctor = typeof(Vector2).GetConstructor(new Type[] { typeof(float), typeof(float) });
                    //return new InstanceDescriptor(ctor, new float[] { v.X, v.Y });
                    return base.ConvertTo(context, culture, value, destinationType);
                }
                else if (context != null && context.PropertyDescriptor.PropertyType.IsArray)
                {
                    Array array = (Array)value;
                    StringBuilder sb = new StringBuilder("[");
                    bool first = true;
                    foreach (var item in array)
                    {
                        if (first)
                            first = false;
                        else
                            sb.Append(" ,");
                        sb.Append(StringConverter.GetString(item));
                    }
                    sb.Append("]");
                    return sb.ToString();
                }
                else if (destinationType == typeof(string))
                    return StringConverter.GetString(value);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            if (value is string)
            {
                if ((string)value == "None")
                    return null;
                return StringConverter.GetValue(context.PropertyDescriptor.PropertyType, (string)value);
            }

            return null;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        { 
            if (properties == null)
            {
                List<PropertyDescriptor> props = new List<PropertyDescriptor>();
                
                Type type = value.GetType();
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                for (int i = 0; i < fields.Length; i++)
                {
                    List<Attribute> attrs = new List<Attribute>();
                    attrs.AddRange(fields[i].FieldType.GetCustomAttributes(true) as Attribute[] ?? new Attribute[0]);
                    foreach (Attribute item in fields[i].GetCustomAttributes(true))
                    {
                        int index = attrs.IndexOf(item);
                        if (index >= 0)
                            attrs.RemoveAt(index);
                        attrs.Add(item);
                    }                    
                    var fd = new FieldDescriptor(fields[i], attrs.ToArray());                    
                    if (fd.IsBrowsable)
                    {                        
                        props.Add(fd);
                    }
                }

                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(value, attributes))
                {
                    if (prop.IsBrowsable)
                    {
                        PropertyDescriptor pd = prop;
                        for (int i = 0; i < prop.Attributes.Count; i++)
                        {
                            if (prop.Attributes[i] is IPDProvider)
                            {
                                pd = ((IPDProvider)prop.Attributes[i]).CreateProperty(value, pd, attributes);
                            }
                        }

                        //if (prop.Attributes.Contains(DeferredAttribute.Yes))
                        //    pd = new DeferredPropertyDescriptors(pd, LockFunction);
                        //if (prop.Attributes.Contains(LockOnSetAttribute.Yes))
                        //    pd = new LockeablePropertyDescriptors(pd, LockFunction);
                        //if (prop.Attributes.Contains(UIEditableAttribute.Yes))
                        //    pd = new EditablePropertyDescriptor(pd, ReadOnlyAttribute.No);
                        //if (prop.Attributes.Contains(DynamicEditableAttribute.Yes))
                        //{
                        //    object instance = prop.GetValue(value);
                        //    if (instance != null)
                        //    {
                        //        EditorAttribute attr = instance.GetType().GetCustomAttribute<EditorAttribute>(true);
                        //        if (attr != null)
                        //        {
                        //            pd = TypeDescriptor.CreateProperty(type, pd, attr);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        pd = TypeDescriptor.CreateProperty(type, pd, new EditorAttribute(typeof(UICreatorEditor),typeof(UITypeEditor)));
                        //    }
                        //}
                        //if (prop.Attributes.OfType<PropertyDescriptorAttribute>().Count() > 0)
                        //{
                        //    var attr = prop.Attributes.OfType<PropertyDescriptorAttribute>().First();
                        //    pd = (PropertyDescriptor)Activator.CreateInstance(attr.DescriptorType, pd);
                        //}

                        props.Add(pd);
                    }
                }


                properties = new PropertyDescriptorCollection(props.ToArray());
            }
            return properties;
        }
    }
 
    public class ObjectPropertyTab : PropertyTab
    {
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public override string TabName
        {
            get { throw new NotImplementedException(); }
        }
    }
}
