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
        //Variáveis globais declaradas para serem utilizadas em quantos processos forem necessários.
        string dictPath = "dict.txt";

        char[] separators = new char[]
        {
            ' ', '\r', '\n', ',', '\t', ';'
        };

        public Dict()
        {
            InitializeComponent();
        }

        public void RenewGrid(string[] dados)
        {
            //Popular dados - dinâmico para atualizar de acordo com as atuações de adicionar/remover.
            for (int i = 0; i < dados.Length; i++)
            {
                dataGridView1.Rows.Add(dados[i]);
            }
        }

        public void ShowGrid(string[] dados)
        {
            //Criar coluna Palavras do grid.
            dataGridView1.Columns.Add("word", "Palavras");
            RenewGrid(dados);
        }

        private void removerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedCells.Count > 0) //Linha selecionada
            {
                if (dataGridView1.SelectedCells.Count == 1) // Execução única.
                {
                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex; //Definindo indice da linha selecionada
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex]; // Definindo linha a ser selecionada
                    string cellValue = Convert.ToString(selectedRow.Cells["word"].Value).ToLower(); //Obtendo valor da linha selecionada

                    if (cellValue != "" && cellValue != null) //Verificando se o valor da linha não é nulo.
                    {
                        string dictContent = File.ReadAllText(dictPath); //Conteúdo do dicionário
                        string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Splitando conteúdo do dicionário para verificar a nível de palavra.

                        string formCell = $@"\b{cellValue}\b"; //Formatação para obter palavra exata.

                        var newLines = dictWords.Select(line => Regex.Replace(line, formCell, string.Empty, RegexOptions.None)); //Determinando novas linhas do arquivo, retirando a selecionada.
                        File.WriteAllLines(dictPath, newLines); //Gravando novo conteúdo do dicionário.

                        string newDictContent = File.ReadAllText(dictPath); //Lendo novo conteúdo do dicionário.
                        string[] newDictWords = newDictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Splitando novo conteúdo do dicionário para atualizar na tela.

                        dataGridView1.Rows.Clear(); //Limpando linhas do grid.
                        RenewGrid(newDictWords); //Populando grid.

                        MessageBox.Show($"Palavra {cellValue} removida com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Selecione um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // Execução múltipla.
                {
                    int selectedRows = dataGridView1.SelectedCells.Count;

                    for (int i = 0; i <= selectedRows; i++)
                    {
                        DataGridViewRow selectedRow = dataGridView1.Rows[0]; // Definindo linha a ser selecionada
                        string cellValue = Convert.ToString(selectedRow.Cells["word"].Value).ToLower(); //Obtendo valor da linha selecionada

                        if (cellValue != "" && cellValue != null) //Verificando se o valor da linha não é nulo.
                        {
                            string dictContent = File.ReadAllText(dictPath); //Conteúdo do dicionário
                            string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Splitando conteúdo do dicionário para verificar a nível de palavra.

                            string formCell = $@"\b{cellValue}\b"; //Formatação para obter palavra exata.

                            var newLines = dictWords.Select(line => Regex.Replace(line, formCell, string.Empty, RegexOptions.None)); //Determinando novas linhas do arquivo, retirando a selecionada.
                            File.WriteAllLines(dictPath, newLines); //Gravando novo conteúdo do dicionário.

                            string newDictContent = File.ReadAllText(dictPath); //Lendo novo conteúdo do dicionário.
                            string[] newDictWords = newDictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Splitando novo conteúdo do dicionário para atualizar na tela.

                            dataGridView1.Rows.Clear(); //Limpando linhas do grid.
                            RenewGrid(newDictWords); //Populando grid.
                        }
                    }
                    MessageBox.Show("Todas as palavras selecionadas foram removidas com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex; //Definindo indice da linha selecionada
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex]; // Definindo linha a ser selecionada
                string wordDict = Convert.ToString(selectedRow.Cells["word"].Value).ToLower(); //Obtendo valor da linha selecionada

                if (wordDict != "" && wordDict != null) //Verificando se o valor da linha não é nulo.
                {
                    File.AppendAllText(dictPath, wordDict + Environment.NewLine); //Adicionando nova linha no arquivo do dicionário
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
