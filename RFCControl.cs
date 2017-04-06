using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreeRfcViewer
{
    public partial class RFCControl : UserControl
    {
        public string RFCFilePath
        {
            get
            {
                if (null != rfcFile)
                {
                    return rfcFile.FilePath;
                }
                else
                {
                    return "";
                }
            }
        }

        public RFCFile RFCFile 
        {
            get
            {
                return rfcFile;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("rfcFile");
                }
                this.rfcFile = value;
                richTextBox.Text = rfcFile.Text;
                for (int i = 0; i < rfcFile.StructureTree.Nodes.Count; i++)
                {
                    TreeNode Node = rfcFile.StructureTree.Nodes[i];
                    int lineNumber = int.Parse(Node.Tag.ToString());
                    // 生成文档结构图
                    treeView_structure.Nodes.Add(Node);
                    // 设置节点字体
                    richTextBox.Select(richTextBox.GetFirstCharIndexFromLine(lineNumber), Node.Text.Length);
                    richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, richTextBox.SelectionFont.Style | FontStyle.Bold);
                }
                for (int i = 0; i < rfcFile.Pages.Count; i++)
                {
                    // 生成页导航
                    TreeNode node = new TreeNode("Page " + (i + 1).ToString());
                    RFCFile.Page page = rfcFile.Pages[i];
                    node.Tag = page;
                    treeView_page.Nodes.Add(node);
                    // 设置页脚字体
                    richTextBox.Select(richTextBox.GetFirstCharIndexFromLine(page.lastLineNumber), richTextBox.Lines[page.lastLineNumber].Length);
                    richTextBox.SelectionColor = Color.Gray;
                    // 设置页眉字体
                    if (0 != i)
                    {
                        richTextBox.Select(richTextBox.GetFirstCharIndexFromLine(page.firstLineNumber), richTextBox.Lines[page.firstLineNumber].Length);
                        richTextBox.SelectionColor = Color.Gray;
                    }
                }
                richTextBox.ZoomFactor = (richTextBox.ClientSize.Width-richTextBox.Margin.Horizontal) / Graphics.FromHwnd(richTextBox.Handle).MeasureString(richTextBox.Text, richTextBox.Font).Width;
            }
        }
        private RFCFile rfcFile;

        public RFCControl()
        {
            InitializeComponent();
            richTextBox.BackColor = Color.White;
        }

        private void treeView_structure_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ScrollToLine(int.Parse(e.Node.Tag.ToString()));
            }
            treeView_structure.SelectedNode = e.Node;
        }

        private void ScrollToLine(int lineNumber)
        {
            // TODO: 恢复原来的光标位置 by SunZhuoshi
            if (0 < lineNumber && richTextBox.Lines.Length > lineNumber)
            {
                richTextBox.Select(richTextBox.GetFirstCharIndexFromLine(lineNumber), 0);
                richTextBox.ScrollToCaret();
            }
        }

        private void treeView_page_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RFCFile.Page page = (RFCFile.Page)e.Node.Tag;
                ScrollToLine(page.firstLineNumber);
            }
            treeView_page.SelectedNode = e.Node;
        }

        private void gotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (0 == tabControl.SelectedIndex)
            {
                ScrollToLine(int.Parse(treeView_structure.SelectedNode.Tag.ToString()));
            }
            else if (1 == tabControl.SelectedIndex)
            {
                RFCFile.Page page = (RFCFile.Page)treeView_page.SelectedNode.Tag;
                ScrollToLine(page.firstLineNumber);
            }
        }

        private void richTextBox_ClientSizeChanged(object sender, EventArgs e)
        {
            if (0 < richTextBox.Text.Length)
            {
                richTextBox.ZoomFactor = (richTextBox.ClientSize.Width - richTextBox.Margin.Horizontal) * 0.9f / Graphics.FromHwnd(richTextBox.Handle).MeasureString(richTextBox.Text, richTextBox.Font).Width;
            }
        }
    }
}
