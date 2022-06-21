using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TextEditor_N3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void DictCheck()
        {

            char[] separators = new char[]
            {
                ' ', '\r', '\n', ','
            };

            string dictPath = "dict.txt";
            string dictContent = File.ReadAllText(dictPath);
            string[] dictWords = dictContent.ToLower().Split(separators);
            string[] editorWords = rtbEditor.Text.Split(separators);

            for (int i = 0; i < editorWords.Length; i++)
            {
                if (dictWords.Contains(editorWords[i])) { }
                else
                {
                    int startIndex = 0;
                    while (startIndex < rtbEditor.TextLength)
                    {
                        int wordStartIndex = rtbEditor.Find(editorWords[i], startIndex, RichTextBoxFinds.None);

                        if (wordStartIndex != -1)
                        {
                            rtbEditor.SelectionStart = wordStartIndex;
                            rtbEditor.SelectionLength = editorWords[i].Length;
                            rtbEditor.SelectionBackColor = Color.Red;
                        }
                        else
                        {
                            break;
                        }
                        startIndex += wordStartIndex + editorWords[i].Length;
                    }
                }
            }

            rtbEditor.DeselectAll();

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                rtbEditor.Text = File.ReadAllText(dlg.FileName);
            }
        }

        private void adicionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dictPath = "dict.txt";

            if (rtbEditor.Text.Length == 0)
            {
                MessageBox.Show("Texto vazio. Favor preencher e selecionar a palavra que deseja adicionar.", "Erro");
            }
            else if (rtbEditor.SelectionLength == 0)
            {
                MessageBox.Show("Favor selecionar a palavra que deseja adicionar.", "Erro");
            }
            else
            {
                rtbEditor.Select(rtbEditor.SelectionStart, rtbEditor.SelectionLength);
                string wordDict = rtbEditor.SelectedText;
                File.AppendAllText(dictPath, wordDict + Environment.NewLine);
                MessageBox.Show($"Palavra {wordDict} adicionada ao dicionário.", "Sucesso");
            }
        }

        private void rtbEditor_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void verificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DictCheck();
        }
    }
}
