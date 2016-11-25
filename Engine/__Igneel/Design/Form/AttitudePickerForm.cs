using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Igneel.Design.Forms
{
    public partial class AttitudePickerForm : Form
    {
        Euler orientation;
        Euler initialValue;

        public AttitudePickerForm() : this(new Euler()) { }

        public AttitudePickerForm(Euler orientation)
        {
            InitializeComponent();

            this.orientation = orientation;
            this.initialValue = orientation;
            acHeading.Angle = Euler.ToAngle(orientation.Heading);
            acPitch.Angle = Euler.ToAngle(orientation.Pitch);
            acRoll.Angle = Euler.ToAngle(orientation.Roll);

        }

        public event EventHandler OrientationChanged;

        public Euler Orientation
        {
            get
            {
                return orientation;
            }

            set
            {
                orientation = value;
                acHeading.Angle =Euler.ToAngle(orientation.Heading);
                acPitch.Angle = Euler.ToAngle(orientation.Pitch);
                acRoll.Angle = Euler.ToAngle(orientation.Roll);
            }
        }

        protected void OnOrientationChanged()
        {
            if (OrientationChanged != null)
                OrientationChanged(this, EventArgs.Empty);
        }

        private void acHeading_AngleChanged(object sender, EventArgs e)
        {
            orientation.Heading = Euler.ToRadians((float)acHeading.Angle);
            OnOrientationChanged();
        }

        private void acPitch_AngleChanged(object sender, EventArgs e)
        {
            orientation.Pitch = Euler.ToRadians((float)acPitch.Angle);
            OnOrientationChanged();
        }

        private void acRoll_AngleChanged(object sender, EventArgs e)
        {
            orientation.Roll = Euler.ToRadians((float)acRoll.Angle);
            OnOrientationChanged();
        }

        private void tbOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            orientation.Heading = Euler.ToRadians((float)acHeading.Angle);
            orientation.Pitch = Euler.ToRadians((float)acPitch.Angle);
            orientation.Roll = Euler.ToRadians((float)acRoll.Angle);
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            orientation = initialValue;
            if (orientation != initialValue)
                OnOrientationChanged();
            Close();
        }

    }
}
