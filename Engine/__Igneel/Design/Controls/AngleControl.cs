using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Igneel.Design.Controls
{
    // Provides a user interface for adjusting an angle value.
    public class AngleControl : System.Windows.Forms.UserControl
    {
        // Stores the angle.
        private double angle;
        // Stores the rotation offset.
        private int rotation = 0;
        // Control state tracking variables.
        private int dbx = -10;
        private int dby = -10;
        private int overButton = -1;

        SolidBrush white, darkBlue, slateGray, darkGray, red, gray, yellow;

        public event EventHandler AngleChanged;

        public AngleControl()
            : this(0)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public AngleControl(double initial_angle)
        {
            this.angle = initial_angle;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);

            Font = new Font("Arial", 8);

            darkGray = new SolidBrush(Color.DarkGray);            
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // Set angle origin point at center of control.
            int originX = (this.Width / 2);
            int originY = (this.Height / 2);

            // Fill background and ellipse and center point.
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), 0, 0, this.Width, this.Height);
            e.Graphics.FillEllipse(new SolidBrush(Color.White), 1, 1, this.Width - 3, this.Height - 3);
            e.Graphics.FillEllipse(new SolidBrush(Color.SlateGray), originX - 1, originY - 1, 3, 3);

            // Draw angle markers.
            int startangle = (270 - rotation) % 360;
            e.Graphics.DrawString(startangle.ToString(), Font, darkGray, (this.Width / 2) - 10, 10);
            startangle = (startangle + 90) % 360;
            e.Graphics.DrawString(startangle.ToString(), Font, darkGray, this.Width - 18, (this.Height / 2) - 6);
            startangle = (startangle + 90) % 360;
            e.Graphics.DrawString(startangle.ToString(), Font, darkGray, (this.Width / 2) - 6, this.Height - 18);
            startangle = (startangle + 90) % 360;
            e.Graphics.DrawString(startangle.ToString(), Font, darkGray, 10, (this.Height / 2) - 6);

            // Draw line along the current angle.         
            double radians = ((((angle + rotation) + 360) % 360) * Math.PI) / (double)180;
            e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Red), 1), originX, originY,
                originX + (int)((double)originX * (double)Math.Cos(radians)),
                originY + (int)((double)originY * (double)Math.Sin(radians)));

            // Output angle information.
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), this.Width - 84, 3, 82, 13);
            e.Graphics.DrawString("Angle: " + angle.ToString("F4"), Font, new SolidBrush(Color.Yellow), this.Width - 84, 2);
            // Draw square at mouse position of last angle adjustment.
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 1), dbx - 2, dby - 2, 4, 4);
            // Draw rotation adjustment buttons.
            if (overButton == 1)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), this.Width - 28, this.Height - 14, 12, 12);
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), 2, this.Height - 13, 110, 12);
                e.Graphics.DrawString("Rotate 90 degrees left", Font, new SolidBrush(Color.White), 2, this.Height - 14);
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.DarkGreen), this.Width - 28, this.Height - 14, 12, 12);
            if (overButton == 2)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), this.Width - 14, this.Height - 14, 12, 12);
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), 2, this.Height - 13, 116, 12);
                e.Graphics.DrawString("Rotate 90 degrees right", Font, new SolidBrush(Color.White), 2, this.Height - 14);
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.DarkGreen), this.Width - 14, this.Height - 14, 12, 12);
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White), 1), this.Width - 11, this.Height - 11, 6, 6);
            e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.White), 1), this.Width - 25, this.Height - 11, 6, 6);
            if (overButton == 1)
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), this.Width - 25, this.Height - 6, 4, 4);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.DarkGreen), this.Width - 25, this.Height - 6, 4, 4);
            if (overButton == 2)
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), this.Width - 8, this.Height - 6, 4, 4);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.DarkGreen), this.Width - 8, this.Height - 6, 4, 4);
            e.Graphics.FillPolygon(new SolidBrush(Color.White), new Point[] { new Point(this.Width - 7, this.Height - 8), new Point(this.Width - 3, this.Height - 8), new Point(this.Width - 5, this.Height - 4) });
            e.Graphics.FillPolygon(new SolidBrush(Color.White), new Point[] { new Point(this.Width - 26, this.Height - 8), new Point(this.Width - 21, this.Height - 8), new Point(this.Width - 25, this.Height - 4) });
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            // Handle rotation adjustment button clicks.
            if (e.X >= this.Width - 28 && e.X <= this.Width - 2 && e.Y >= this.Height - 14 && e.Y <= this.Height - 2)
            {
                if (e.X <= this.Width - 16)
                    rotation -= 90;
                else if (e.X >= this.Width - 14)
                    rotation += 90;
                if (rotation < 0)
                    rotation += 360;
                rotation = rotation % 360;
                dbx = -10;
                dby = -10;
            }
            else
                UpdateAngle(e.X, e.Y);
            this.Refresh();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {
                UpdateAngle(e.X, e.Y);
                overButton = -1;
            }
            else if (e.X >= this.Width - 28 && e.X <= this.Width - 16 && e.Y >= this.Height - 14 && e.Y <= this.Height - 2)
                overButton = 1;
            else if (e.X >= this.Width - 14 && e.X <= this.Width - 2 && e.Y >= this.Height - 14 && e.Y <= this.Height - 2)
                overButton = 2;
            else
                overButton = -1;
            this.Refresh();
        }

        private void UpdateAngle(int mx, int my)
        {
            // Store mouse coordinates.
            dbx = mx;
            dby = my;

            // Translate y coordinate input to GetAngle function to correct for ellipsoid distortion.
            double widthToHeightRatio = (double)this.Width / (double)this.Height;
            int tmy;
            if (my == 0)
                tmy = my;
            else if (my < this.Height / 2)
                tmy = (this.Height / 2) - (int)(((this.Height / 2) - my) * widthToHeightRatio);
            else
                tmy = (this.Height / 2) + (int)((double)(my - (this.Height / 2)) * widthToHeightRatio);

            // Retrieve updated angle based on rise over run.
            Angle = (GetAngle(this.Width / 2, this.Height / 2, mx, tmy) - rotation) % 360;
        }

        private double GetAngle(int x1, int y1, int x2, int y2)
        {
            double degrees;

            // Avoid divide by zero run values.
            if (x2 - x1 == 0)
            {
                if (y2 > y1)
                    degrees = 90;
                else
                    degrees = 270;
            }
            else
            {
                // Calculate angle from offset.
                double riseoverrun = (double)(y2 - y1) / (double)(x2 - x1);
                double radians = Math.Atan(riseoverrun);
                degrees = radians * ((double)180 / Math.PI);

                // Handle quadrant specific transformations.       
                if ((x2 - x1) < 0 || (y2 - y1) < 0)
                    degrees += 180;
                if ((x2 - x1) > 0 && (y2 - y1) < 0)
                    degrees -= 180;
                if (degrees < 0)
                    degrees += 360;
            }
            return degrees;
        }

        protected void OnAngleValueChanged()
        {
            if (AngleChanged != null)
                AngleChanged(this, EventArgs.Empty);
        }

        public double Angle 
        {
            get 
            {
                return angle;
            }
            set 
            {
                if (angle != value)
                {
                    angle = value;
                    OnAngleValueChanged();                   
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AngleControl
            // 
            this.DoubleBuffered = true;
            this.Name = "AngleControl";
            this.ResumeLayout(false);

        }
    }
}
