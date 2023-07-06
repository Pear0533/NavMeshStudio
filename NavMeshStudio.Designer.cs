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
            toolStrip1 = new ToolStrip();
            splitContainer1 = new SplitContainer();
            sceneGraphGroupBox = new GroupBox();
            sceneGraphTreeView = new TreeView();
            navMeshCollisionEditingGroupBox = new GroupBox();
            navMeshCollisionEditingListView = new ListView();
            splitContainer2 = new SplitContainer();
            splitContainer3 = new SplitContainer();
            splitContainer4 = new SplitContainer();
            ribbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            sceneGraphGroupBox.SuspendLayout();
            navMeshCollisionEditingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.SuspendLayout();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.BackColor = SystemColors.Control;
            ribbon.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            ribbon.Location = new Point(0, 0);
            ribbon.Name = "ribbon";
            ribbon.Size = new Size(1904, 24);
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
            copyrightInfoLabel.Location = new Point(1726, 4);
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
            versionLabel.Location = new Point(1653, 4);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(48, 15);
            versionLabel.TabIndex = 2;
            versionLabel.Text = "Version:";
            // 
            // toolStrip1
            // 
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1904, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(sceneGraphGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(navMeshCollisionEditingGroupBox);
            splitContainer1.Size = new Size(510, 708);
            splitContainer1.SplitterDistance = 354;
            splitContainer1.TabIndex = 4;
            // 
            // sceneGraphGroupBox
            // 
            sceneGraphGroupBox.Controls.Add(sceneGraphTreeView);
            sceneGraphGroupBox.Dock = DockStyle.Fill;
            sceneGraphGroupBox.Location = new Point(0, 0);
            sceneGraphGroupBox.Name = "sceneGraphGroupBox";
            sceneGraphGroupBox.Size = new Size(510, 354);
            sceneGraphGroupBox.TabIndex = 0;
            sceneGraphGroupBox.TabStop = false;
            sceneGraphGroupBox.Text = "Scene Graph";
            // 
            // sceneGraphTreeView
            // 
            sceneGraphTreeView.Dock = DockStyle.Fill;
            sceneGraphTreeView.Location = new Point(3, 19);
            sceneGraphTreeView.Name = "sceneGraphTreeView";
            sceneGraphTreeView.Size = new Size(504, 332);
            sceneGraphTreeView.TabIndex = 0;
            // 
            // navMeshCollisionEditingGroupBox
            // 
            navMeshCollisionEditingGroupBox.Controls.Add(navMeshCollisionEditingListView);
            navMeshCollisionEditingGroupBox.Dock = DockStyle.Fill;
            navMeshCollisionEditingGroupBox.Location = new Point(0, 0);
            navMeshCollisionEditingGroupBox.Name = "navMeshCollisionEditingGroupBox";
            navMeshCollisionEditingGroupBox.Size = new Size(510, 350);
            navMeshCollisionEditingGroupBox.TabIndex = 5;
            navMeshCollisionEditingGroupBox.TabStop = false;
            navMeshCollisionEditingGroupBox.Text = "NavMesh/Collision Editing";
            // 
            // navMeshCollisionEditingListView
            // 
            navMeshCollisionEditingListView.Dock = DockStyle.Fill;
            navMeshCollisionEditingListView.Location = new Point(3, 19);
            navMeshCollisionEditingListView.Name = "navMeshCollisionEditingListView";
            navMeshCollisionEditingListView.Size = new Size(504, 328);
            navMeshCollisionEditingListView.TabIndex = 0;
            navMeshCollisionEditingListView.UseCompatibleStateImageBehavior = false;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            splitContainer2.Size = new Size(1530, 708);
            splitContainer2.SplitterDistance = 510;
            splitContainer2.TabIndex = 5;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(splitContainer2);
            splitContainer3.Size = new Size(1891, 708);
            splitContainer3.SplitterDistance = 1530;
            splitContainer3.TabIndex = 6;
            // 
            // splitContainer4
            // 
            splitContainer4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer4.Location = new Point(6, 46);
            splitContainer4.Name = "splitContainer4";
            splitContainer4.Orientation = Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(splitContainer3);
            splitContainer4.Size = new Size(1891, 988);
            splitContainer4.SplitterDistance = 708;
            splitContainer4.TabIndex = 7;
            // 
            // NavMeshStudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1904, 1041);
            Controls.Add(splitContainer4);
            Controls.Add(toolStrip1);
            Controls.Add(versionLabel);
            Controls.Add(copyrightInfoLabel);
            Controls.Add(ribbon);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = ribbon;
            MinimumSize = new Size(570, 290);
            Name = "NavMeshStudio";
            Text = "NavMesh Studio";
            ribbon.ResumeLayout(false);
            ribbon.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            sceneGraphGroupBox.ResumeLayout(false);
            navMeshCollisionEditingGroupBox.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip ribbon;
        private ToolStripMenuItem fileToolStripMenuItem;
        public ToolStripMenuItem openToolStripMenuItem;
        private Label copyrightInfoLabel;
        public Label versionLabel;
        private ToolStrip toolStrip1;
        private SplitContainer splitContainer1;
        private GroupBox sceneGraphGroupBox;
        private GroupBox navMeshCollisionEditingGroupBox;
        private TreeView sceneGraphTreeView;
        private ListView navMeshCollisionEditingListView;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        private SplitContainer splitContainer4;
    }
}