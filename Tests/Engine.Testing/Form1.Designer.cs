namespace D3D9Testing
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.testSets = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableShadowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enablePhysicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tests = new System.Windows.Forms.ListBox();
            this.fpsCounter = new System.Windows.Forms.Label();
            this.screen = new Igneel.Windows.Forms.Canvas3D();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpsCounter);
            this.splitContainer1.Panel2.Controls.Add(this.screen);
            this.splitContainer1.Size = new System.Drawing.Size(839, 483);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.testSets);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tests);
            this.splitContainer2.Size = new System.Drawing.Size(188, 483);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.TabIndex = 0;
            // 
            // testSets
            // 
            this.testSets.ContextMenuStrip = this.contextMenuStrip1;
            this.testSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testSets.FormattingEnabled = true;
            this.testSets.Location = new System.Drawing.Point(0, 0);
            this.testSets.Name = "testSets";
            this.testSets.Size = new System.Drawing.Size(188, 213);
            this.testSets.TabIndex = 0;
            this.testSets.SelectedIndexChanged += new System.EventHandler(this.testSets_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableShadowsToolStripMenuItem,
            this.enablePhysicsToolStripMenuItem,
            this.wireframeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 70);
            // 
            // enableShadowsToolStripMenuItem
            // 
            this.enableShadowsToolStripMenuItem.Name = "enableShadowsToolStripMenuItem";
            this.enableShadowsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.enableShadowsToolStripMenuItem.Text = "Enable Shadows";
            this.enableShadowsToolStripMenuItem.Click += new System.EventHandler(this.enableShadowsToolStripMenuItem_Click);
            // 
            // enablePhysicsToolStripMenuItem
            // 
            this.enablePhysicsToolStripMenuItem.Name = "enablePhysicsToolStripMenuItem";
            this.enablePhysicsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.enablePhysicsToolStripMenuItem.Text = "Enable Physics";
            this.enablePhysicsToolStripMenuItem.Click += new System.EventHandler(this.enablePhysicsToolStripMenuItem_Click);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            // 
            // tests
            // 
            this.tests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tests.FormattingEnabled = true;
            this.tests.Location = new System.Drawing.Point(0, 0);
            this.tests.Name = "tests";
            this.tests.Size = new System.Drawing.Size(188, 266);
            this.tests.TabIndex = 0;
            this.tests.DoubleClick += new System.EventHandler(this.tests_DoubleClick);
            // 
            // fpsCounter
            // 
            this.fpsCounter.AutoSize = true;
            this.fpsCounter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.fpsCounter.ForeColor = System.Drawing.Color.Cyan;
            this.fpsCounter.Location = new System.Drawing.Point(3, 0);
            this.fpsCounter.Name = "fpsCounter";
            this.fpsCounter.Size = new System.Drawing.Size(58, 13);
            this.fpsCounter.TabIndex = 0;
            this.fpsCounter.Text = "fpsCounter";
            // 
            // screen
            // 
            this.screen.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.screen.Camera = null;
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.IsDefault = false;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Presenter = null;
            this.screen.Scene = null;
            this.screen.Size = new System.Drawing.Size(647, 483);
            this.screen.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 483);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox testSets;
        private System.Windows.Forms.ListBox tests;
        internal System.Windows.Forms.Label fpsCounter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem enableShadowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enablePhysicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
        private Igneel.Windows.Forms.Canvas3D screen;
    }
}

