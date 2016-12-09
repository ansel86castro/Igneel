using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Igneel.Graphics;

namespace Igneel.Design.UITypeEditors
{
    public class UIColorTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            Color color = Color.Empty;
            if (e.Value is Vector3)
                color = Vector3.Saturate(((Vector3)e.Value)).ToColor();
            else if (e.Value is Vector4)
                color = Vector4.Saturate(((Vector4)e.Value)).ToColor();
            else if (e.Value is Color4)
                color = Color.FromArgb(((Color4)e.Value).ToArgb());

            e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc =
               (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
            {
                return value;
            }

            using (ColorDialog dialog = new ColorDialog())
            {                
                dialog.AllowFullOpen = true;

                if (value is Vector3)
                    dialog.Color = Vector3.Saturate((Vector3)value).ToColor();
                else if (value is Vector4)
                    dialog.Color = Vector4.Saturate((Vector4)value).ToColor();
                else if (value is Color4)
                    dialog.Color = Color.FromArgb(((Color4)value).ToArgb());

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (value is Vector3)
                        value = new Vector3(dialog.Color);
                    else if (value is Vector4)
                        value = new Vector4(dialog.Color);
                    else if (value is Color4)
                        value = new Color4(dialog.Color);
                        
                }
                return value;
            }


        }
    }
}
