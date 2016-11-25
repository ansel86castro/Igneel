using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using Igneel.Design.Controls;

namespace Igneel.Design.UITypeEditors
{
    public class UIInmediateNumericEditor:UITypeEditor
    {
        PropertyDescriptor pd;
        object component;

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc =
              (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
            {
                return value;
            }
            pd = context.PropertyDescriptor;
            component = context.Instance;

            if (pd == null) return value;

            using (NumericStep numericCrtl = new NumericStep())
            {
                //numericCrtl.Maximum = float.MaxValue;
                //numericCrtl.Minimum = float.MinValue;            
                
                //IncrementAttribute increment = (IncrementAttribute)pd.Attributes[typeof(IncrementAttribute)];
                //if (increment != null)
                //    numericCrtl.Increment = increment.Increment;

                numericCrtl.Value = (float)value;
                numericCrtl.ValueChanged += new EventHandler(numericCrtl_ValueChanged);
                edSvc.DropDownControl(numericCrtl);
                return numericCrtl.Value;
            }
        }

        void numericCrtl_ValueChanged(object sender, EventArgs e)
        {
            NumericStep numericCrtl = sender as NumericStep;
            pd.SetValue(component, (float)numericCrtl.Value);
        }
    }
}
