using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TextEditor_N3
{
    public partial class Form1 : Form
    {
        string dictPath = "dict.txt";
        int startIndex = 0;
        char[] separators = new char[]
        {
            ' ', '\r', '\n', ',', '\t'
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void DictCheck()
        {
            string dictContent = File.ReadAllText(dictPath);
            string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string[] editorWords = rtbEditor.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < editorWords.Length; i++)
            {
                if (dictWords.Contains(editorWords[i])) { }
                else
                {
                    
                    while (startIndex < rtbEditor.TextLength)
                    {
                        int wordStartIndex = rtbEditor.Find(editorWords[i], startIndex, RichTextBoxFinds.WholeWord);

                        if (wordStartIndex != -1)
                        {
                            rtbEditor.SelectionStart = wordStartIndex;
                            rtbEditor.SelectionLength = editorWords[i].Length;
                            rtbEditor.SelectionBackColor = Color.Red;
                            startIndex = wordStartIndex + editorWords[i].Length;
                            break;
                        }
                        else
                        {
                            break;
                        }


                    }
                }
            }

            rtbEditor.DeselectAll();
            startIndex = 0;
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

        private void adicionarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

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
                MessageBox.Show($"Palavra {wordDict} adicionada ao dicionário.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void removerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
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
                string dictContent = File.ReadAllText(dictPath);
                string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                rtbEditor.Select(rtbEditor.SelectionStart, rtbEditor.SelectionLength);
                string wordDict = rtbEditor.SelectedText;
                var newLines = dictWords.Select(line => Regex.Replace(line, wordDict, string.Empty, RegexOptions.IgnoreCase));
                File.WriteAllLines(dictPath, newLines);
            }
        }

        private void verificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DictCheck();
        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string dictContent = File.ReadAllText(dictPath);
            string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Dict d1 = new Dict();
            d1.ShowGrid(dictWords);
            d1.Show();
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            sfd.Title = "Salve seu arquivo.";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(rtbEditor.Text);
                sw.Close();
                MessageBox.Show($"Arquivo salvo com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
