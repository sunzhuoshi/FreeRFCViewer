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
        // TODO: ʹ���������ʽ��ʵ�� by SunZhuoshi
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
                    // ��prePageLine��һ��
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

                // �Թ��հ���
                if (line.Trim().Equals(""))
                {
                    continue;
                }
                // �����̶����ֵĲݽ�
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
                // �����̶�ǰ׺���½�
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
                // ��������ŵ��½�
                if (!isStaticIndex)
                {
                    // TODO: ʹ���������ʽ����name�а���������ǰ����space����� by SunZhuoshi
                    Match match = Regex.Match(line, @"^(?<PreWhite>\s*)(?<Index>([1-9]\d*(\.\d+)*))\.*(\s)+(?<Name>([\w\x20()\-'/\.=\:]+))$");
                    if (match.Success)
                    {
                        TreeNode p_calc = root_calc;
                        TreeNode p_display = root_display;

                        string preWhite = match.Result("${PreWhite}");
                        string index = match.Result("${Index}");
                        string name = match.Result("${Name}");

                        Debug.WriteLine("Parsing " + line);
                        // Ŀ¼��û��"."��item����ʶ��
                        if (-1 != name.IndexOf("   "))
                        {
                            continue; //������һ��
                        }

                        // Table of content�е���
                        if (-1 != name.IndexOf(@"..") || -1 != name.IndexOf(@". ."))
                        {
                            continue;  //������һ��
                        }
                        // Ŀ¼�ж��е�item����ʶ��
                        if (i + 1 < lines.Length)
                        {
                            if (Regex.Match(lines[i + 1], @"^\s+\w+\x20\.*\s+\d+$").Success)
                            {
                                continue; //������һ��
                            }
                        }

                        // TODO: �ع���������������в���·����Ȼ���ٴ��� by SunZhuoshi
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
                                    if (0 == p_calc.Nodes.Count ||  // û�ֵܽ�㣨��������ӽ�㣩
                                        int.Parse(p_calc.LastNode.Text) + 1 == int.Parse(index_splitted[k]))    // ���ֵܽ�㣬�����Ҫ����һ���ֵܽ���1
                                    {
                                        if (0 == p_calc.Nodes.Count)
                                        {
                                            if (0 != int.Parse(index_splitted[k]) && 1 != int.Parse(index_splitted[k]))
                                            {
                                                break; //���Բ���0��1��ʼ�����½���
                                            }
                                        }
                                        if (k == index_splitted.Length - 1) // ���Ӽ�����ΪҶ�ӽڵ�ʱ��������ʾ���
                                        {
                                            if (0 == p_calc.Nodes.Count || preWhite.Equals(p_calc.LastNode.Tag))
                                            {
                                                Debug.WriteLine("Added new display node: " + line);
                                                p_display.Nodes.Add(line).Tag = i;
                                                ifOk = true;
                                            }
                                            else
                                            {
                                                ;//����ǰ���հ��ַ����ֵܽ�㲻��ͬ����
                                            }
                                        }

                                        Debug.WriteLine("Added new calc node(" + index_splitted[k] + ") to calc node(" + p_calc.Text + ")");
                                        p_calc = p_calc.Nodes.Insert(p_calc.Nodes.Count, index_splitted[k], index_splitted[k]);
                                        p_calc.Tag = preWhite;
                                        nodeToBeRemove = p_calc;
                                    }
                                    else
                                    {
                                        ;//������ʶ����
                                    }
                                }
                                else if (1 == results.Length) //һ�������һ���ڵ�
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
            // ɾ�������ڴ�ӡ����ֽ��FF
            text = tmpText.Replace(FF, "");
        }
    }
}