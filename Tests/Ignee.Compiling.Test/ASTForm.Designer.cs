namespace Ignee.Compiling.Test
{
    partial class ASTForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvAST = new System.Windows.Forms.TreeView();
            this.tbNodeString = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvAST);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbNodeString);
            this.splitContainer1.Size = new System.Drawing.Size(545, 364);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvAST
            // 
            this.tvAST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAST.Location = new System.Drawing.Point(0, 0);
            this.tvAST.Name = "tvAST";
            this.tvAST.Size = new System.Drawing.Size(183, 364);
            this.tvAST.TabIndex = 0;
            this.tvAST.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAST_AfterSelect);
            // 
            // tbNodeString
            // 
            this.tbNodeString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNodeString.Location = new System.Drawing.Point(0, 0);
            this.tbNodeString.Multiline = true;
            this.tbNodeString.Name = "tbNodeString";
            this.tbNodeString.Size = new System.Drawing.Size(358, 364);
            this.tbNodeString.TabIndex = 0;
            // 
            // ASTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 364);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ASTForm";
            this.Text = "ASTForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvAST;
        private System.Windows.Forms.TextBox tbNodeString;
    }
}