using Igneel.Design.Controls;
using Igneel.Design.Forms;

using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Igneel.Design.UITypeEditors
{
    public class UIRotationMatrixEditor : UITypeEditor
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
            Matrix rot = (Matrix)value;
            Euler euler = Euler.FromMatrix(rot);

            using (EulerEditor ctrol = new EulerEditor(euler))
            {
                ctrol.EulerChanged += ctrol_EulerChanged;
                edSvc.DropDownControl(ctrol);
                return ctrol.Orientation.ToMatrix();    
            }            
        }

        void ctrol_EulerChanged(object arg1, Euler arg2)
        {
            pd.SetValue(component, arg2.ToMatrix());
        }

        void dialog_OrientationChanged(object sender, EventArgs e)
        {
            AttitudePickerForm dialog = sender as AttitudePickerForm;
            pd.SetValue(component, dialog.Orientation.ToMatrix());
        }
    }
}
