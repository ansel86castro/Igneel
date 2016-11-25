namespace Igneel.Design.Forms
{
    partial class TypeProviderForm
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
            this.lbAssemblys = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.clbTypes = new System.Windows.Forms.CheckedListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Types";
            // 
            // lbAssemblys
            // 
            this.lbAssemblys.FormattingEnabled = true;
            this.lbAssemblys.Location = new System.Drawing.Point(292, 25);
            this.lbAssemblys.Name = "lbAssemblys";
            this.lbAssemblys.Size = new System.Drawing.Size(130, 173);
            this.lbAssemblys.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(338, 206);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Import DLL";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(95, 206);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(13, 206);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 7;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // clbTypes
            // 
            this.clbTypes.FormattingEnabled = true;
            this.clbTypes.Location = new System.Drawing.Point(12, 27);
            this.clbTypes.Name = "clbTypes";
            this.clbTypes.Size = new System.Drawing.Size(274, 169);
            this.clbTypes.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // TypeProviderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 247);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAssemblys);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.clbTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TypeProviderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TypeProviderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbAssemblys;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.CheckedListBox clbTypes;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}