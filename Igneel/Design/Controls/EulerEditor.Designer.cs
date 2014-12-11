namespace Igneel.Design.Controls
{
    partial class EulerEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.angleControl1 = new Igneel.Design.Controls.AngleControl();
            this.angleControl2 = new Igneel.Design.Controls.AngleControl();
            this.angleControl3 = new Igneel.Design.Controls.AngleControl();
            this.SuspendLayout();
            // 
            // angleControl1
            // 
            this.angleControl1.Angle = 0D;
            this.angleControl1.Font = new System.Drawing.Font("Arial", 8F);
            this.angleControl1.Location = new System.Drawing.Point(2, 2);
            this.angleControl1.Name = "angleControl1";
            this.angleControl1.Size = new System.Drawing.Size(93, 96);
            this.angleControl1.TabIndex = 0;
            this.angleControl1.AngleChanged += new System.EventHandler(this.angleControl1_AngleChanged);
            // 
            // angleControl2
            // 
            this.angleControl2.Angle = 0D;
            this.angleControl2.Font = new System.Drawing.Font("Arial", 8F);
            this.angleControl2.Location = new System.Drawing.Point(96, 2);
            this.angleControl2.Name = "angleControl2";
            this.angleControl2.Size = new System.Drawing.Size(93, 96);
            this.angleControl2.TabIndex = 1;
            this.angleControl2.AngleChanged += new System.EventHandler(this.angleControl2_AngleChanged);
            // 
            // angleControl3
            // 
            this.angleControl3.Angle = 0D;
            this.angleControl3.Font = new System.Drawing.Font("Arial", 8F);
            this.angleControl3.Location = new System.Drawing.Point(190, 2);
            this.angleControl3.Name = "angleControl3";
            this.angleControl3.Size = new System.Drawing.Size(93, 96);
            this.angleControl3.TabIndex = 2;
            this.angleControl3.AngleChanged += new System.EventHandler(this.angleControl3_AngleChanged);
            // 
            // EulerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.angleControl3);
            this.Controls.Add(this.angleControl2);
            this.Controls.Add(this.angleControl1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "EulerEditor";
            this.Size = new System.Drawing.Size(286, 101);
            this.ResumeLayout(false);

        }

        #endregion

        private AngleControl angleControl1;
        private AngleControl angleControl2;
        private AngleControl angleControl3;
    }
}
