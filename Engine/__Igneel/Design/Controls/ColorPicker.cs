using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Igneel.Design.Controls
{     
    public partial class ColorPicker : UserControl
    {
        //public delegate void ColorChangeEventHandler(object sender, EventArgs arg);       

        Vector3 color;
        public event EventHandler ColorChanged;

        public ColorPicker()
        {
            InitializeComponent();

            tbColor.Text = StringConverter.GetString(color);
            pbColor.BackColor =Vector3.Saturate(color).ToColor();
        }

        public Vector3 ColorValue 
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    tbColor.Text = StringConverter.GetString(color);
                    pbColor.BackColor = Vector3.Saturate(color).ToColor();
                    OnColorChanged();
                    Refresh();
                }
                
            }
        }

        protected virtual void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, EventArgs.Empty);
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Vector3.Saturate(color).ToColor();
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (pbColor.BackColor != colorDialog1.Color)
                {
                    pbColor.BackColor = colorDialog1.Color;
                    color = new Vector3(colorDialog1.Color);
                    tbColor.Text = StringConverter.GetString(color);
                    OnColorChanged();
                }
            }
        }

        private void tbColor_Enter(object sender, EventArgs e)
        {
            tbColor.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void tbColor_Leave(object sender, EventArgs e)
        {
            try
            {
                tbColor.BackColor = Color.White;

                var newColor = (Vector3)StringConverter.GetValue(typeof(Vector3), tbColor.Text);
                if (color != newColor)
                {
                    color = newColor;
                    pbColor.BackColor = Vector3.Saturate(color).ToColor();
                    OnColorChanged();
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid Format");
            }
        }
    }
}
