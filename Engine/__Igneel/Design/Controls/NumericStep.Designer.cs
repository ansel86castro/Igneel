namespace Igneel.Design.Controls
{
    partial class NumericStep
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
            this.components = new System.ComponentModel.Container();
            this.ntbValue = new System.Windows.Forms.NumericUpDown();
            this.ntbStep = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ntbValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntbStep)).BeginInit();
            this.SuspendLayout();
            // 
            // ntbValue
            // 
            this.ntbValue.DecimalPlaces = 5;
            this.ntbValue.Location = new System.Drawing.Point(3, 3);
            this.ntbValue.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ntbValue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.ntbValue.Name = "ntbValue";
            this.ntbValue.Size = new System.Drawing.Size(120, 20);
            this.ntbValue.TabIndex = 0;
            this.toolTip1.SetToolTip(this.ntbValue, "Value");
            this.ntbValue.ValueChanged += new System.EventHandler(this.ntbValue_ValueChanged);
            // 
            // ntbStep
            // 
            this.ntbStep.DecimalPlaces = 5;
            this.ntbStep.Location = new System.Drawing.Point(3, 26);
            this.ntbStep.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.ntbStep.Name = "ntbStep";
            this.ntbStep.Size = new System.Drawing.Size(119, 20);
            this.ntbStep.TabIndex = 1;
            this.toolTip1.SetToolTip(this.ntbStep, "Step");
            this.ntbStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbStep.ValueChanged += new System.EventHandler(this.ntbStep_ValueChanged);
            // 
            // NumericStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.ntbStep);
            this.Controls.Add(this.ntbValue);
            this.Name = "NumericStep";
            this.Size = new System.Drawing.Size(126, 49);
            ((System.ComponentModel.ISupportInitialize)(this.ntbValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntbStep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown ntbValue;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown ntbStep;
    }
}
