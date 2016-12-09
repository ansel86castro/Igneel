using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Igneel.Design.Forms;
using System.Windows.Forms;
using Igneel.Design.Controls;

namespace Igneel.Design.UITypeEditors
{
    public class UIAttitudeTypeEditor: UITypeEditor
    {
        private System.ComponentModel.PropertyDescriptor pd;
        private object component;

        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }

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

            using (EulerEditor ctrol = new EulerEditor((Euler)value))
            {
                ctrol.EulerChanged += ctrol_EulerChanged;
                edSvc.DropDownControl(ctrol);
                return ctrol.Orientation;                
            }         
        }

        void ctrol_EulerChanged(object arg1, Euler arg2)
        {
            pd.SetValue(component, arg2);
        }

        void dialog_OrientationChanged(object sender, EventArgs e)
        {
            AttitudePickerForm dialog = sender as AttitudePickerForm;
            pd.SetValue(component, dialog.Orientation);
        }
    }    
}
