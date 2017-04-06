using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreeRfcViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Text = Application.ProductName;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    string fileName = openFileDialog.FileNames[i];
                    RFCControl rfcControl = new RFCControl();
                    RFCFile file = new RFCFile(fileName);
                    try
                    {
                        file.Load();
                        rfcControl.RFCFile = file;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to process file: " + fileName + "\r\nDetail info:\r\n" + ex.ToString());
                        continue;
                    }
                    TabPage tabPage = new TabPage(System.IO.Path.GetFileName(fileName));
                    rfcControl.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(rfcControl);
                    tabControl.TabPages.Add(tabPage);
                    tabControl.SelectTab(tabPage);
                }
            }
        }

        private void toolStripMenuItem_close_Click(object sender, EventArgs e)
        {
            if (null != tabControl.SelectedTab)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        // TODO: 更改实现为只有header部分显示右键菜单 by SunZhuoshi
        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int tabIndex = -1;
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabIndex = i;
                        break;
                    }
                }
                if (-1 != tabIndex)
                {
                    tabControl.SelectedIndex = tabIndex;
                }
            }
        }

        private void toolStripButton_close_Click(object sender, EventArgs e)
        {
            if (null != tabControl.SelectedTab)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.TabPages.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }
    }
}