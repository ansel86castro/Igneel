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
    public class UIVector3TypeEditor:UITypeEditor
    {
        PropertyDescriptor pd;
        object component;        

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
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
            using (Vector3Editor editor = new Vector3Editor(v))
            {
                editor.VectorChanged += editor_VectorChanged;
                edSvc.DropDownControl(editor);
                return editor.Value;
            }
        }

        void editor_VectorChanged(object arg1, Vector3 arg2)
        {
            pd.SetValue(component, arg2);
        }
    }
}
