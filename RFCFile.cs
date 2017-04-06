using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreeRfcViewer
{
    public class RFCFile
    {
        public struct Page
        {
            public int firstLineNumber;
            public int lastLineNumber;
        }
        public string FilePath
        {
            get
            {
                return filePath;
            }
        }
        public TreeNode StructureTree
        {
            get
            {
                return structureTree;
            }
        }
        public List<Page> Pages
        {
            get
            {
                return pages;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
        }
        public string[] Lines
        {
            get
            {
                return lines;
            }
        }

        private TreeNode structureTree;
        private string filePath;
        private string text;
        private string[] lines;
        private List<Page> pages;
        private static readonly string[] STATIC_CHAPTER_NAMES = { "ABSTRACT", 
            "ACKNOWLEDGEMENT", "ACKNOWLEDGEMENTS", "ACKNOWLEDGMENT", 
            "ACKNOWLEDGMENTS", "ACKNOWLEGEMENTS", "AUTHOR'S ADDRESS", "COPYRIGHT NOTICE", 
            "I. APPENDIX", "REFERENCES", "SECURITY CONSIDERATIONS", "STATUS OF THIS MEMO", "SUMMARY", 
            "TABLE OF CONTENTS" };
        // TODO: 使用正则表达式来实现 by SunZhuoshi
        private static readonly string[] STATIC_CHAPTER_PREFIXS = { 
            "APPENDIX I -", "APPENDIX II -", "APPENDIX III -", "APPENDIX IV -",
            "APPENDIX V -" };
        private static readonly string FF = "\f";

        public RFCFile(string filePath)
        {
            if (null == filePath)
            {
                throw new Exception("filePath");
            }
            this.filePath = filePath;
            structureTree = new TreeNode();
            text = "";
            lines = new string[0];
            pages = new List<Page>();
        }
        public void Load()
        {
            string tmpText = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                tmpText = sr.ReadToEnd();
                //< TODO: try the normal mothod by SunZhuoshi
                RichTextBox rtb = new RichTextBox();
                rtb.Text = tmpText;
                lines = rtb.Lines;
                //>
            }
            TreeNode root_display = new TreeNode("root");
            TreeNode root_calc = new TreeNode("0");
            root_display.Tag = "";
            root_calc.Tag = "";

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                bool isStaticIndex = false;

                if ( -1 != line.IndexOf(FF))
                {
                    // 从prePageLine下一行
                    lines[i] = lines[i].Replace(FF, "");
                    Page page = new Page();
                    if (0 == pages.Count)
                    {
                        page.firstLineNumber = 0;
                    }
                    else 
                    {
                        page.firstLineNumber = pages[pages.Count-1].lastLineNumber + 2;
                    }
                    page.lastLineNumber = i - 1;
                    pages.Add(page);
                }

                // 略过空白行
                if (line.Trim().Equals(""))
                {
                    continue;
                }
                // 解析固定名字的草节
                for (int j = 0; j < STATIC_CHAPTER_NAMES.Length; j++)
                {
                    if (line.Trim().ToUpper().Equals(STATIC_CHAPTER_NAMES[j]))
                    {
                        TreeNode node = new TreeNode(line.Trim());
                        node.Tag = i;
                        root_display.Nodes.Add(node);
                        isStaticIndex = true;
                        break;
                    }
                }
                // 解析固定前缀的章节
                if (!isStaticIndex)
                {
                    for (int j = 0; j < STATIC_CHAPTER_PREFIXS.Length; j++)
                    {
                        if (line.Trim().ToUpper().StartsWith(STATIC_CHAPTER_PREFIXS[j]))
                        {
                            TreeNode node = new TreeNode(line.Trim());
                            node.Tag = i;
                            root_display.Nodes.Add(node);
                            isStaticIndex = true;
                            break;
                        }
                    }
                }
                // 解析带编号的章节
                if (!isStaticIndex)
                {
                    // TODO: 使用正则表达式修正name中包括两个以前连续space的情况 by SunZhuoshi
                    Match match = Regex.Match(line, @"^(?<PreWhite>\s*)(?<Index>([1-9]\d*(\.\d+)*))\.*(\s)+(?<Name>([\w\x20()\-'/\.=\:]+))$");
                    if (match.Success)
                    {
                        TreeNode p_calc = root_calc;
                        TreeNode p_display = root_display;

                        string preWhite = match.Result("${PreWhite}");
                        string index = match.Result("${Index}");
                        string name = match.Result("${Name}");

                        Debug.WriteLine("Parsing " + line);
                        // 目录中没有"."的item的误识别
                        if (-1 != name.IndexOf("   "))
                        {
                            continue; //处理下一行
                        }

                        // Table of content中的行
                        if (-1 != name.IndexOf(@"..") || -1 != name.IndexOf(@". ."))
                        {
                            continue;  //处理下一行
                        }
                        // 目录中多行的item的误识别
                        if (i + 1 < lines.Length)
                        {
                            if (Regex.Match(lines[i + 1], @"^\s+\w+\x20\.*\s+\d+$").Success)
                            {
                                continue; //处理下一行
                            }
                        }

                        // TODO: 重构这里：先在已有树中查找路径，然后再处理 by SunZhuoshi
                        char[] splitters = { '.' };
                        string[] index_splitted = index.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

                        bool ifOk = false;
                        TreeNode nodeToBeRemove = null;
                        try
                        {
                            for (int k = 0; k < index_splitted.Length; k++)
                            {
                                TreeNode[] results = p_calc.Nodes.Find(index_splitted[k], false);
                                if (0 == results.Length)
                                {
                                    if (0 == p_calc.Nodes.Count ||  // 没兄弟结点（父结点无子结点）
                                        int.Parse(p_calc.LastNode.Text) + 1 == int.Parse(index_splitted[k]))    // 有兄弟结点，编号需要比上一个兄弟结点大1
                                    {
                                        if (0 == p_calc.Nodes.Count)
                                        {
                                            if (0 != int.Parse(index_splitted[k]) && 1 != int.Parse(index_splitted[k]))
                                            {
                                                break; //忽略不以0或1开始的新章节行
                                            }
                                        }
                                        if (k == index_splitted.Length - 1) // 添加计算结点为叶子节点时才添加显示结点
                                        {
                                            if (0 == p_calc.Nodes.Count || preWhite.Equals(p_calc.LastNode.Tag))
                                            {
                                                Debug.WriteLine("Added new display node: " + line);
                                                p_display.Nodes.Add(line).Tag = i;
                                                ifOk = true;
                                            }
                                            else
                                            {
                                                ;//忽略前导空白字符与兄弟结点不相同的行
                                            }
                                        }

                                        Debug.WriteLine("Added new calc node(" + index_splitted[k] + ") to calc node(" + p_calc.Text + ")");
                                        p_calc = p_calc.Nodes.Insert(p_calc.Nodes.Count, index_splitted[k], index_splitted[k]);
                                        p_calc.Tag = preWhite;
                                        nodeToBeRemove = p_calc;
                                    }
                                    else
                                    {
                                        ;//忽略误识别结点
                                    }
                                }
                                else if (1 == results.Length) //一定是最后一个节点
                                {
                                    Debug.Assert(null != p_calc.LastNode);
                                    Debug.WriteLine("Tracing calc node from " + p_calc.Text + " to " + p_calc.LastNode.Text);
                                    p_calc = p_calc.LastNode;
                                    Debug.Assert(null != p_display.LastNode);
                                    Debug.WriteLine("Tracing display node from \"" + p_display.Text + "\" to \"" + p_display.LastNode.Text + "\"");
                                    p_display = p_display.LastNode;
                                }
                                else
                                {
                                    throw new Exception("Internal error: duplicated child nodes found");
                                }
                            }
                        }
                        finally
                        {
                            if (!ifOk)
                            {
                                if (null != nodeToBeRemove)
                                {
                                    Debug.WriteLine("Deleting calc node(" + nodeToBeRemove.Text + ")");
                                    nodeToBeRemove.Parent.Nodes.Remove(nodeToBeRemove);
                                }
                            }
                        }
                    }
                }
                structureTree = root_display;
            }
            // 删除仅用于打印机换纸的FF
            text = tmpText.Replace(FF, "");
        }
    }
}
