using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Igneel.Design.Controls
{
    public partial class EulerEditor : UserControl
    {
        public event Action<object, Euler> EulerChanged;

        public EulerEditor(Euler euler)
        {
            InitializeComponent();

            angleControl1.Angle = euler.HeadingAngle;
            angleControl2.Angle = euler.PitchAngle;
            angleControl3.Angle = euler.RollAngle;
        }

        public Euler Orientation
        {
            get
            {
                return new Euler(Euler.ToRadians((float)angleControl1.Angle),
                                            Euler.ToRadians((float)angleControl2.Angle),
                                            Euler.ToRadians((float)angleControl3.Angle));
            }
        }

        private void angleControl1_AngleChanged(object sender, EventArgs e)
        {
            if (EulerChanged != null)
                EulerChanged(this, new Euler(Euler.ToRadians((float)angleControl1.Angle),
                                            Euler.ToRadians((float)angleControl2.Angle),
                                            Euler.ToRadians((float)angleControl3.Angle)));
        }

        private void angleControl2_AngleChanged(object sender, EventArgs e)
        {
            if (EulerChanged != null)
                EulerChanged(this, new Euler(Euler.ToRadians((float)angleControl1.Angle),
                                            Euler.ToRadians((float)angleControl2.Angle),
                                            Euler.ToRadians((float)angleControl3.Angle)));
        }

        private void angleControl3_AngleChanged(object sender, EventArgs e)
        {
            if (EulerChanged != null)
                EulerChanged(this, new Euler(Euler.ToRadians((float)angleControl1.Angle),
                                            Euler.ToRadians((float)angleControl2.Angle),
                                            Euler.ToRadians((float)angleControl3.Angle)));
        }
    }
}
