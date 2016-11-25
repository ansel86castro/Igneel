using Igneel.Design.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Igneel.Design.UITypeEditors
{
    public class UICreatorEditor : UITypeEditor
    {
        Dictionary<Type, UITypeEditor> editors = new Dictionary<Type,UITypeEditor>();

        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value == null)
            {
                using (TypeProviderForm form = new TypeProviderForm(context.PropertyDescriptor.PropertyType))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var type = form.SelectedType;
                        EditorAttribute attr = type.GetCustomAttribute<EditorAttribute>(true);
                        UITypeEditor editor = null;
                        if (attr != null)
                        {
                            editor = (UITypeEditor)Activator.CreateInstance(Type.GetType(attr.EditorTypeName));
                            editors.Add(type, editor);
                        }
                        try
                        {
                            value = Activator.CreateInstance(type);
                            if (editor != null)
                                value = editor.EditValue(context, provider, value);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Unable to Create Instance\r\n " + e.Message);
                        }                        
                    }                    
                }
                return value;
            }
            else
            {
                var editor = editors[value.GetType()];
                return editor.EditValue(context, provider, value);
            }
        }
    
    }
}
