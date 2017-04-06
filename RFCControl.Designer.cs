namespace FreeRfcViewer
{
    partial class RFCControl
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_structure = new System.Windows.Forms.TabPage();
            this.treeView_structure = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_TreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage_page = new System.Windows.Forms.TabPage();
            this.treeView_page = new System.Windows.Forms.TreeView();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage_structure.SuspendLayout();
            this.contextMenuStrip_TreeNode.SuspendLayout();
            this.tabPage_page.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.richTextBox);
            this.splitContainer.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.SplitterDistance = 200;
            this.splitContainer.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_structure);
            this.tabControl.Controls.Add(this.tabPage_page);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(200, 600);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage_structure
            // 
            this.tabPage_structure.Controls.Add(this.treeView_structure);
            this.tabPage_structure.Location = new System.Drawing.Point(4, 21);
            this.tabPage_structure.Name = "tabPage_structure";
            this.tabPage_structure.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_structure.Size = new System.Drawing.Size(192, 575);
            this.tabPage_structure.TabIndex = 0;
            this.tabPage_structure.Text = "Structure";
            this.tabPage_structure.UseVisualStyleBackColor = true;
            // 
            // treeView_structure
            // 
            this.treeView_structure.ContextMenuStrip = this.contextMenuStrip_TreeNode;
            this.treeView_structure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_structure.Location = new System.Drawing.Point(3, 3);
            this.treeView_structure.Name = "treeView_structure";
            this.treeView_structure.Size = new System.Drawing.Size(186, 569);
            this.treeView_structure.TabIndex = 0;
            this.treeView_structure.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_structure_NodeMouseClick);
            // 
            // contextMenuStrip_TreeNode
            // 
            this.contextMenuStrip_TreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoToolStripMenuItem});
            this.contextMenuStrip_TreeNode.Name = "contextMenuStrip_TreeNode";
            this.contextMenuStrip_TreeNode.Size = new System.Drawing.Size(95, 26);
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.gotoToolStripMenuItem.Text = "&Goto";
            this.gotoToolStripMenuItem.Click += new System.EventHandler(this.gotoToolStripMenuItem_Click);
            // 
            // tabPage_page
            // 
            this.tabPage_page.Controls.Add(this.treeView_page);
            this.tabPage_page.Location = new System.Drawing.Point(4, 21);
            this.tabPage_page.Name = "tabPage_page";
            this.tabPage_page.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_page.Size = new System.Drawing.Size(192, 575);
            this.tabPage_page.TabIndex = 1;
            this.tabPage_page.Text = "Page";
            this.tabPage_page.UseVisualStyleBackColor = true;
            // 
            // treeView_page
            // 
            this.treeView_page.ContextMenuStrip = this.contextMenuStrip_TreeNode;
            this.treeView_page.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_page.Location = new System.Drawing.Point(3, 3);
            this.treeView_page.Name = "treeView_page";
            this.treeView_page.Size = new System.Drawing.Size(186, 569);
            this.treeView_page.TabIndex = 0;
            this.treeView_page.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_page_NodeMouseClick);
            // 
            // richTextBox
            // 
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.ShowSelectionMargin = true;
            this.richTextBox.Size = new System.Drawing.Size(596, 600);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            this.richTextBox.WordWrap = false;
            this.richTextBox.ClientSizeChanged += new System.EventHandler(this.richTextBox_ClientSizeChanged);
            // 
            // RFCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "RFCControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage_structure.ResumeLayout(false);
            this.contextMenuStrip_TreeNode.ResumeLayout(false);
            this.tabPage_page.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_structure;
        private System.Windows.Forms.TabPage tabPage_page;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TreeView treeView_structure;
        private System.Windows.Forms.TreeView treeView_page;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_TreeNode;
        private System.Windows.Forms.ToolStripMenuItem gotoToolStripMenuItem;

    }
}
