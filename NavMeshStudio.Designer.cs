namespace NavMeshStudio
{
    partial class NavMeshStudio
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavMeshStudio));
            ribbon = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            copyrightInfoLabel = new Label();
            versionLabel = new Label();
            ribbon.SuspendLayout();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.BackColor = SystemColors.Control;
            ribbon.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            ribbon.Location = new Point(0, 0);
            ribbon.Name = "ribbon";
            ribbon.Size = new Size(800, 24);
            ribbon.TabIndex = 0;
            ribbon.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(150, 22);
            openToolStripMenuItem.Text = "Open (Ctrl+O)";
            // 
            // copyrightInfoLabel
            // 
            copyrightInfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyrightInfoLabel.AutoSize = true;
            copyrightInfoLabel.BackColor = SystemColors.Control;
            copyrightInfoLabel.ForeColor = Color.DimGray;
            copyrightInfoLabel.Location = new Point(622, 4);
            copyrightInfoLabel.Name = "copyrightInfoLabel";
            copyrightInfoLabel.Size = new Size(174, 15);
            copyrightInfoLabel.TabIndex = 1;
            copyrightInfoLabel.Text = "© Pear, 2023 All rights reserved.";
            // 
            // versionLabel
            // 
            versionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            versionLabel.AutoSize = true;
            versionLabel.BackColor = SystemColors.Control;
            versionLabel.ForeColor = Color.DimGray;
            versionLabel.Location = new Point(549, 4);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(48, 15);
            versionLabel.TabIndex = 2;
            versionLabel.Text = "Version:";
            // 
            // NavMeshStudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(versionLabel);
            Controls.Add(copyrightInfoLabel);
            Controls.Add(ribbon);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = ribbon;
            Name = "NavMeshStudio";
            Text = "NavMesh Studio";
            ribbon.ResumeLayout(false);
            ribbon.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip ribbon;
        private ToolStripMenuItem fileToolStripMenuItem;
        public ToolStripMenuItem openToolStripMenuItem;
        private Label copyrightInfoLabel;
        public Label versionLabel;
    }
}