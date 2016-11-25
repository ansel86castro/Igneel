namespace Igneel.Design.Controls
{
    partial class AttitudePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttitudePicker));
            this.tbAttitude = new System.Windows.Forms.TextBox();
            this.btPick = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbAttitude
            // 
            this.tbAttitude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAttitude.Location = new System.Drawing.Point(0, 0);
            this.tbAttitude.Name = "tbAttitude";
            this.tbAttitude.Size = new System.Drawing.Size(103, 20);
            this.tbAttitude.TabIndex = 0;
            this.tbAttitude.Enter += new System.EventHandler(this.tbAttitude_Enter);
            this.tbAttitude.Leave += new System.EventHandler(this.tbAttitude_Leave);
            // 
            // btPick
            // 
            this.btPick.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btPick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btPick.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btPick.BackgroundImage")));
            this.btPick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btPick.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btPick.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btPick.Location = new System.Drawing.Point(104, 2);
            this.btPick.Name = "btPick";
            this.btPick.Size = new System.Drawing.Size(22, 18);
            this.btPick.TabIndex = 1;
            this.btPick.UseVisualStyleBackColor = false;
            this.btPick.Click += new System.EventHandler(this.btPick_Click);
            // 
            // AttitudePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.btPick);
            this.Controls.Add(this.tbAttitude);
            this.Name = "AttitudePicker";
            this.Size = new System.Drawing.Size(128, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAttitude;
        private System.Windows.Forms.Button btPick;
    }
}
