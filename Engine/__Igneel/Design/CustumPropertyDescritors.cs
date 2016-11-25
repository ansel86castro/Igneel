using Igneel.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Design
{
    public class ShallowedPropertyDescriptor : PropertyDescriptor
    {
        PropertyDescriptor pd;
        public ShallowedPropertyDescriptor(PropertyDescriptor pd)
            : base(pd)
        {
            this.pd = pd;
        }

        public PropertyDescriptor Property { get { return pd; } }

        public override bool CanResetValue(object component)
        {
            return pd.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return pd.ComponentType; }
        }

        public override bool IsReadOnly
        {
            get { return pd.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return pd.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            pd.ResetValue(component);
        }

        public override object GetValue(object component)
        {
            return pd.GetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            pd.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return pd.ShouldSerializeValue(component);
        }
    }

    public class DeferredPropertyDescriptors : ShallowedPropertyDescriptor
    {                 
        public DeferredPropertyDescriptors(PropertyDescriptor pd)
            : base(pd)
        {
            
        }        
     
        public override void SetValue(object component, object value)
        {
            base.SetValue(component, value);
            if (component is SceneNode)
            {
                ((SceneNode)component).UpdateLocalPose();
            }
            ((IDeferreable)component).CommitChanges();                
        }
    }

    public class LockeablePropertyDescriptors : ShallowedPropertyDescriptor
    {       
        Action<Action> lockFunction;        
        public LockeablePropertyDescriptors(PropertyDescriptor pd, Action<Action> lockFunction)
            : base(pd)
        {            
            this.lockFunction = lockFunction;            
        }        

        public override void SetValue(object component, object value)
        {
            if (lockFunction != null)
                lockFunction(() => Property.SetValue(component, value));
            else
            {
                Property.SetValue(component, value);
            }
        }
    }

    public class ConstrainToPropertyDescriptor : ShallowedPropertyDescriptor
    {
        Predicate<object> predicated;

        public ConstrainToPropertyDescriptor(PropertyDescriptor pd, Predicate<object> predicated)
            : base(pd)
        {
            this.predicated = predicated;
        }

        public override object GetValue(object component)
        {
            if(predicated(component))
                return base.GetValue(component);

            return "invalid";
        }

        public override void SetValue(object component, object value)
        {
            if (predicated(component))
                base.SetValue(component, value);
        }
    }

}
