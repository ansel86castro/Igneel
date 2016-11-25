using Igneel.Design.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Igneel.Design.UITypeEditors
{
    public class UIDirectionEditor:UITypeEditor
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

            Vector3 v = (Vector3)value;

            using (EulerEditor ctrol = new EulerEditor(Euler.FromDirection(v)))
            {
                ctrol.EulerChanged += ctrol_EulerChanged;
                edSvc.DropDownControl(ctrol);
                return ctrol.Orientation.ToDirection();
            }
        }

        void ctrol_EulerChanged(object arg1, Euler arg2)
        {
            pd.SetValue(component,arg2.ToDirection());
        }
    }
}
