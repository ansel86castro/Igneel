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
    public partial class DirectionPicker : UserControl
    {
        Vector3 direction;

        public DirectionPicker()
        {
            InitializeComponent();
            direction = new Euler(0,0,0).ToDirection();
            tbAttitude.Text = StringConverter.GetString(direction);
        }

        public event EventHandler DirectionChanged;
         
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if (direction != value)
                {
                    direction = value;
                    tbAttitude.Text = StringConverter.GetString(direction);
                    OnDirectionChanged();
                    Refresh();
                }
            }
        }

        protected void OnDirectionChanged()
        {
            if (DirectionChanged != null)
            {
                DirectionChanged(this, EventArgs.Empty);
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
                    var newAtt = (Vector3)StringConverter.GetValue(typeof(Vector3), tbAttitude.Text);
                    if (direction != newAtt)
                    {
                        direction = newAtt;
                        OnDirectionChanged();
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
            using (AttitudePickerForm form = new AttitudePickerForm(Euler.FromDirection(direction)))
            {
                Vector3 oldDir = direction;
                form.OrientationChanged += new EventHandler(form_OrientationChanged);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Vector3 dir = form.Orientation.ToDirection();
                    direction = oldDir;
                    if (direction != dir )
                    {
                        direction = dir;
                        tbAttitude.Text = StringConverter.GetString(direction);
                        OnDirectionChanged();
                    }
                }
            }
        }

        void form_OrientationChanged(object sender, EventArgs e)
        {
            var form = sender as AttitudePickerForm;
            Vector3 dir = form.Orientation.ToDirection();
            if (direction != dir)
            {
                direction = dir;
                tbAttitude.Text = StringConverter.GetString(direction);
                OnDirectionChanged();
            }
        }
    }
}
