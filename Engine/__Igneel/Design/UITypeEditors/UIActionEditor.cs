using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace Igneel.Design.UITypeEditors
{
    public class UIActionEditor:UITypeEditor
    {
        Action action;

        public Action Action { get { return action; } set { action = value; } }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (action != null)
            {
                //if (context.PropertyDescriptor.Attributes.Contains(LockOnSetAttribute.Yes))
                //{
                //    Engine.Lock(action);
                //}
                //else
                    action();
            }

            return value;
        }
    }
}
