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
    public partial class Vector3Editor : UserControl
    {
        public event Action<object, Vector3> VectorChanged;

        public Vector3Editor(Vector3 value)
        {
            InitializeComponent();

            numericUpDownX.Value = (decimal)value.X;
            numericUpDownY.Value = (decimal)value.Y;
            numericUpDownZ.Value = (decimal)value.Z;
        }

        public Vector3 Value
        {
            get
            {
                return new Vector3((float)numericUpDownX.Value,
                    (float)numericUpDownY.Value,
                    (float)numericUpDownZ.Value);
            }            
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {
            if (VectorChanged != null)
            {
                VectorChanged(this, new Vector3((float)numericUpDownX.Value, 
                    (float)numericUpDownY.Value, 
                    (float)numericUpDownZ.Value));
            }
        }

        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {
            if (VectorChanged != null)
            {
                VectorChanged(this, new Vector3((float)numericUpDownX.Value,
                    (float)numericUpDownY.Value,
                    (float)numericUpDownZ.Value));
            }
        }

        private void numericUpDownZ_ValueChanged(object sender, EventArgs e)
        {
            if (VectorChanged != null)
            {
                VectorChanged(this, new Vector3((float)numericUpDownX.Value,
                    (float)numericUpDownY.Value,
                    (float)numericUpDownZ.Value));
            }
        }

        private void numericUpDownInc_ValueChanged(object sender, EventArgs e)
        {
            var inc = numericUpDownInc.Value;
            numericUpDownX.Increment = inc;
            numericUpDownY.Increment = inc;
            numericUpDownZ.Increment = inc;
        }
    }
}
