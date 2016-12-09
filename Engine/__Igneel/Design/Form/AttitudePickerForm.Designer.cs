namespace Igneel.Design.Forms
{
    partial class AttitudePickerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.acHeading = new Igneel.Design.Controls.AngleControl();
            this.acPitch = new Igneel.Design.Controls.AngleControl();
            this.acRoll = new Igneel.Design.Controls.AngleControl();
            this.tbOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Heading";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pitch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Roll";
            // 
            // acHeading
            // 
            this.acHeading.Angle = 0D;
            this.acHeading.Font = new System.Drawing.Font("Arial", 8F);
            this.acHeading.Location = new System.Drawing.Point(5, 20);
            this.acHeading.Name = "acHeading";
            this.acHeading.Size = new System.Drawing.Size(97, 97);
            this.acHeading.TabIndex = 7;
            this.acHeading.AngleChanged += new System.EventHandler(this.acHeading_AngleChanged);
            // 
            // acPitch
            // 
            this.acPitch.Angle = 0D;
            this.acPitch.Font = new System.Drawing.Font("Arial", 8F);
            this.acPitch.Location = new System.Drawing.Point(109, 20);
            this.acPitch.Name = "acPitch";
            this.acPitch.Size = new System.Drawing.Size(97, 97);
            this.acPitch.TabIndex = 8;
            this.acPitch.AngleChanged += new System.EventHandler(this.acPitch_AngleChanged);
            // 
            // acRoll
            // 
            this.acRoll.Angle = 0D;
            this.acRoll.Font = new System.Drawing.Font("Arial", 8F);
            this.acRoll.Location = new System.Drawing.Point(212, 20);
            this.acRoll.Name = "acRoll";
            this.acRoll.Size = new System.Drawing.Size(97, 97);
            this.acRoll.TabIndex = 9;
            this.acRoll.AngleChanged += new System.EventHandler(this.acRoll_AngleChanged);
            // 
            // tbOk
            // 
            this.tbOk.Location = new System.Drawing.Point(152, 127);
            this.tbOk.Name = "tbOk";
            this.tbOk.Size = new System.Drawing.Size(75, 23);
            this.tbOk.TabIndex = 10;
            this.tbOk.Text = "OK";
            this.tbOk.UseVisualStyleBackColor = true;
            this.tbOk.Click += new System.EventHandler(this.tbOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(233, 127);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 11;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // AttitudePickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 162);
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.tbOk);
            this.Controls.Add(this.acRoll);
            this.Controls.Add(this.acPitch);
            this.Controls.Add(this.acHeading);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttitudePickerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Orientation";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Controls.AngleControl acHeading;
        private Controls.AngleControl acPitch;
        private Controls.AngleControl acRoll;
        private System.Windows.Forms.Button tbOk;
        private System.Windows.Forms.Button btCancel;
    }
}