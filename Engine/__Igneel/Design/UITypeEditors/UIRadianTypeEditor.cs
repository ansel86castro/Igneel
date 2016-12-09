using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Igneel.Design.Controls;

namespace Igneel.Design.UITypeEditors
{
    public class UIRadianTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc =
               (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
            {
                return value;
            }
            
            using (AngleControl control = new AngleControl(Euler.ToAngle((float)value)))
            {
                edSvc.DropDownControl(control);
                return Euler.ToRadians((float)control.Angle);
            }

        }
    }
    public class UIAngleTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc =
               (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc == null)
            {
                return value;
            }

            using (AngleControl control = new AngleControl((float)value))
            {
                edSvc.DropDownControl(control);
                return (float)control.Angle;
            }
        }
    }
}
