﻿namespace NavMeshStudio
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
            menuRibbon = new MenuStrip();
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
            viewerGroupBox = new GroupBox();
            splitContainer3 = new SplitContainer();
            attributesGroupBox = new GroupBox();
            attributesListView = new ListView();
            splitContainer4 = new SplitContainer();
            consoleGroupBox = new GroupBox();
            consoleTextBox = new RichTextBox();
            statusRibbon = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            menuRibbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            sceneGraphGroupBox.SuspendLayout();
            navMeshCollisionEditingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            attributesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            consoleGroupBox.SuspendLayout();
            statusRibbon.SuspendLayout();
            SuspendLayout();
            // 
            // menuRibbon
            // 
            menuRibbon.BackColor = SystemColors.Control;
            menuRibbon.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuRibbon.Location = new Point(0, 0);
            menuRibbon.Name = "menuRibbon";
            menuRibbon.Size = new Size(1904, 24);
            menuRibbon.TabIndex = 0;
            menuRibbon.Text = "menuStrip1";
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
            openToolStripMenuItem.Size = new Size(180, 22);
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
            splitContainer1.Size = new Size(510, 695);
            splitContainer1.SplitterDistance = 347;
            splitContainer1.TabIndex = 4;
            // 
            // sceneGraphGroupBox
            // 
            sceneGraphGroupBox.Controls.Add(sceneGraphTreeView);
            sceneGraphGroupBox.Dock = DockStyle.Fill;
            sceneGraphGroupBox.Location = new Point(0, 0);
            sceneGraphGroupBox.Name = "sceneGraphGroupBox";
            sceneGraphGroupBox.Size = new Size(510, 347);
            sceneGraphGroupBox.TabIndex = 0;
            sceneGraphGroupBox.TabStop = false;
            sceneGraphGroupBox.Text = "Scene Graph";
            // 
            // sceneGraphTreeView
            // 
            sceneGraphTreeView.Dock = DockStyle.Fill;
            sceneGraphTreeView.Location = new Point(3, 19);
            sceneGraphTreeView.Name = "sceneGraphTreeView";
            sceneGraphTreeView.Size = new Size(504, 325);
            sceneGraphTreeView.TabIndex = 0;
            // 
            // navMeshCollisionEditingGroupBox
            // 
            navMeshCollisionEditingGroupBox.Controls.Add(navMeshCollisionEditingListView);
            navMeshCollisionEditingGroupBox.Dock = DockStyle.Fill;
            navMeshCollisionEditingGroupBox.Location = new Point(0, 0);
            navMeshCollisionEditingGroupBox.Name = "navMeshCollisionEditingGroupBox";
            navMeshCollisionEditingGroupBox.Size = new Size(510, 344);
            navMeshCollisionEditingGroupBox.TabIndex = 5;
            navMeshCollisionEditingGroupBox.TabStop = false;
            navMeshCollisionEditingGroupBox.Text = "NavMesh/Collision Editing";
            // 
            // navMeshCollisionEditingListView
            // 
            navMeshCollisionEditingListView.Dock = DockStyle.Fill;
            navMeshCollisionEditingListView.Location = new Point(3, 19);
            navMeshCollisionEditingListView.Name = "navMeshCollisionEditingListView";
            navMeshCollisionEditingListView.Size = new Size(504, 322);
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
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(viewerGroupBox);
            splitContainer2.Size = new Size(1530, 695);
            splitContainer2.SplitterDistance = 510;
            splitContainer2.TabIndex = 5;
            // 
            // viewerGroupBox
            // 
            viewerGroupBox.Dock = DockStyle.Fill;
            viewerGroupBox.Location = new Point(0, 0);
            viewerGroupBox.Name = "viewerGroupBox";
            viewerGroupBox.Size = new Size(1016, 695);
            viewerGroupBox.TabIndex = 0;
            viewerGroupBox.TabStop = false;
            viewerGroupBox.Text = "Viewer";
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
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(attributesGroupBox);
            splitContainer3.Size = new Size(1891, 695);
            splitContainer3.SplitterDistance = 1530;
            splitContainer3.TabIndex = 6;
            // 
            // attributesGroupBox
            // 
            attributesGroupBox.Controls.Add(attributesListView);
            attributesGroupBox.Dock = DockStyle.Fill;
            attributesGroupBox.Location = new Point(0, 0);
            attributesGroupBox.Name = "attributesGroupBox";
            attributesGroupBox.Size = new Size(357, 695);
            attributesGroupBox.TabIndex = 0;
            attributesGroupBox.TabStop = false;
            attributesGroupBox.Text = "Attributes";
            // 
            // attributesListView
            // 
            attributesListView.Dock = DockStyle.Fill;
            attributesListView.Location = new Point(3, 19);
            attributesListView.Name = "attributesListView";
            attributesListView.Size = new Size(351, 673);
            attributesListView.TabIndex = 0;
            attributesListView.UseCompatibleStateImageBehavior = false;
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
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(consoleGroupBox);
            splitContainer4.Size = new Size(1891, 970);
            splitContainer4.SplitterDistance = 695;
            splitContainer4.TabIndex = 7;
            // 
            // consoleGroupBox
            // 
            consoleGroupBox.Controls.Add(consoleTextBox);
            consoleGroupBox.Dock = DockStyle.Fill;
            consoleGroupBox.Location = new Point(0, 0);
            consoleGroupBox.Name = "consoleGroupBox";
            consoleGroupBox.Size = new Size(1891, 271);
            consoleGroupBox.TabIndex = 0;
            consoleGroupBox.TabStop = false;
            consoleGroupBox.Text = "Console";
            // 
            // consoleTextBox
            // 
            consoleTextBox.BorderStyle = BorderStyle.FixedSingle;
            consoleTextBox.Dock = DockStyle.Fill;
            consoleTextBox.Location = new Point(3, 19);
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.Size = new Size(1885, 249);
            consoleTextBox.TabIndex = 0;
            consoleTextBox.Text = "";
            // 
            // statusRibbon
            // 
            statusRibbon.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusRibbon.Location = new Point(0, 1019);
            statusRibbon.Name = "statusRibbon";
            statusRibbon.Size = new Size(1904, 22);
            statusRibbon.TabIndex = 8;
            statusRibbon.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(39, 17);
            toolStripStatusLabel1.Text = "Ready";
            // 
            // NavMeshStudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1904, 1041);
            Controls.Add(statusRibbon);
            Controls.Add(splitContainer4);
            Controls.Add(toolStrip1);
            Controls.Add(versionLabel);
            Controls.Add(copyrightInfoLabel);
            Controls.Add(menuRibbon);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuRibbon;
            MinimumSize = new Size(570, 290);
            Name = "NavMeshStudio";
            Text = "NavMesh Studio";
            menuRibbon.ResumeLayout(false);
            menuRibbon.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            sceneGraphGroupBox.ResumeLayout(false);
            navMeshCollisionEditingGroupBox.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            attributesGroupBox.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            consoleGroupBox.ResumeLayout(false);
            statusRibbon.ResumeLayout(false);
            statusRibbon.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuRibbon;
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
        private GroupBox attributesGroupBox;
        private ListView attributesListView;
        private GroupBox consoleGroupBox;
        private RichTextBox consoleTextBox;
        private GroupBox viewerGroupBox;
        private StatusStrip statusRibbon;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}