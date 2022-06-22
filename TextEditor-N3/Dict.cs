using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextEditor_N3
{
    public partial class Dict : Form
    {
        string dictPath = "dict.txt";

        char[] separators = new char[]
        {
            ' ', '\r', '\n', ',', '\t'
        };

        public Dict()
        {
            InitializeComponent();
        }

        public void RenewGrid(string[] dados)
        {
            for (int i = 0; i < dados.Length; i++)
            {
                dataGridView1.Rows.Add(dados[i]);
            }
        }

        public void ShowGrid(string[] dados)
        {
            dataGridView1.Columns.Add("word", "Palavras");

            RenewGrid(dados);
        }

        private void removerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                string cellValue = Convert.ToString(selectedRow.Cells["word"].Value);

                if (cellValue != "" && cellValue != null)
                {
                    string dictContent = File.ReadAllText(dictPath);
                    string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    var newLines = dictWords.Select(line => Regex.Replace(line, cellValue, string.Empty, RegexOptions.IgnoreCase));
                    File.WriteAllLines(dictPath, newLines);

                    string newDictContent = File.ReadAllText(dictPath);
                    string[] newDictWords = newDictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    dataGridView1.Rows.Clear();
                    RenewGrid(newDictWords);

                    MessageBox.Show($"Palavra {cellValue} removida com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Selecione um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Selecione um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adicionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                string wordDict = Convert.ToString(selectedRow.Cells["word"].Value);

                if (wordDict != "" && wordDict != null)
                {
                    File.AppendAllText(dictPath, wordDict + Environment.NewLine);
                    MessageBox.Show($"Palavra {wordDict} adicionada ao dicionário.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Selecione um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show($"Selecione um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
