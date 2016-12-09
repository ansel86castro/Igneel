using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Igneel.Design.Forms;
using Igneel;

namespace Igneel.Design.Controls
{
    public partial class AttitudePicker : UserControl
    {
        Euler orientation;

        public AttitudePicker()
        {
            InitializeComponent();
            tbAttitude.Text = StringConverter.GetString(orientation);
        }

        public event EventHandler OrientationChanged;

        [Browsable(false)]          
        internal Euler Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    tbAttitude.Text = StringConverter.GetString(orientation);
                    OnOrientationChanged();
                    Refresh();
                }
            }
        }

        protected void OnOrientationChanged()
        {
            if (OrientationChanged != null)
            {
                OrientationChanged(this, EventArgs.Empty);
            }
        }

        private void tbAttitude_Enter(object sender, EventArgs e)
        {
            tbAttitude.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void tbAttitude_Leave(object sender, EventArgs e)
        {
            try
            {
                tbAttitude.BackColor = Color.White;
                if (!string.IsNullOrEmpty(tbAttitude.Text))
                {
                    var newAtt = (Euler)StringConverter.GetValue(typeof(Euler), tbAttitude.Text);
                    if (orientation != newAtt)
                    {
                        orientation = newAtt;
                        OnOrientationChanged();
                    }
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid Format");
            }
        }

        private void btPick_Click(object sender, EventArgs e)
        {
            using (AttitudePickerForm form = new AttitudePickerForm(orientation))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (orientation != form.Orientation)
                    {
                        orientation = form.Orientation;
                        tbAttitude.Text = StringConverter.GetString(orientation);
                        OnOrientationChanged();
                    }
                }
            }
        }
    }
}
