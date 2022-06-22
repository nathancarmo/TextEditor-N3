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
        //Variáveis globais declaradas para serem utilizadas em quantos processos forem necessários.
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
            //Tratativa para atualizar uma palavra adicionada no dicionário.
            rtbEditor.SelectAll();
            rtbEditor.SelectionBackColor = Color.Transparent;
            rtbEditor.DeselectAll();

            string dictContent = File.ReadAllText(dictPath); // Ler conteúdo do dicionário
            string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); // Obter palavras do dicionário
            string[] editorWords = rtbEditor.Text.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Obter palavras do editor de texto.

            for (int i = 0; i < editorWords.Length; i++)
            {
                if (dictWords.Contains(editorWords[i])) { } // Se a palavra estiver contida no dicionário, não faça nada.
                else
                {
                    while (startIndex < rtbEditor.TextLength) //Tratativa para verificar sempre do começo do texto
                    {
                        int wordStartIndex = rtbEditor.Find(editorWords[i], startIndex, RichTextBoxFinds.WholeWord); //Index que a palavra se inicia

                        if (wordStartIndex != -1) // Se = -1, significa que a palavra naõ está no dicionário. Marcação será feita.
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
            //Tratativa para melhorar a experiência do usuário e remover a seleção feita na marcação.
            rtbEditor.DeselectAll();
            startIndex = 0;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog(); //Usando dialog de arquivos
            dlg.Filter = "Text File (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"; //Filtro txt e rtf
            if (dlg.ShowDialog() == DialogResult.OK) // Se o diálogo ocorrer com sucesso
            {
                rtbEditor.Text = File.ReadAllText(dlg.FileName); // Editor de texto recebe todo o texto do arquivo aberto.
            }
        }

        private void adicionarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (rtbEditor.Text.Length == 0) //Se o editor não tiver nada escrito/nenhum arquivo aberto.
            {
                MessageBox.Show("Texto vazio. Favor preencher e selecionar a palavra que deseja adicionar.", "Erro");
            }
            else if (rtbEditor.SelectionLength == 0) //Se nada foi selecionado
            {
                MessageBox.Show("Favor selecionar a palavra que deseja adicionar.", "Erro");
            }
            else
            {
                //Obtendo palavra selecionada pelo usuário e adicionando no dicionário.
                rtbEditor.Select(rtbEditor.SelectionStart, rtbEditor.SelectionLength);
                string wordDict = rtbEditor.SelectedText;
                File.AppendAllText(dictPath, wordDict + Environment.NewLine);
                MessageBox.Show($"Palavra {wordDict} adicionada ao dicionário.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void removerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (rtbEditor.Text.Length == 0) //Se o editor não tiver nada escrito/nenhum arquivo aberto.
            {
                MessageBox.Show("Texto vazio. Favor preencher e selecionar a palavra que deseja adicionar.", "Erro");
            }
            else if (rtbEditor.SelectionLength == 0) //Se nada foi selecionado
            {
                MessageBox.Show("Favor selecionar a palavra que deseja adicionar.", "Erro");
            }
            else
            {
                //Obtendo conteúdo do dicionário
                string dictContent = File.ReadAllText(dictPath);
                string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

                //Obtendo palavra selecionada pelo usuário e removendo do dicionário.
                rtbEditor.Select(rtbEditor.SelectionStart, rtbEditor.SelectionLength);
                string wordDict = rtbEditor.SelectedText;
                string formWord = $@"\b{wordDict}\b";
                var newLines = dictWords.Select(line => Regex.Replace(line, formWord, string.Empty, RegexOptions.IgnoreCase));
                File.WriteAllLines(dictPath, newLines);
                MessageBox.Show($"Palavra {wordDict} removida do dicionário.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void verificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Verificação do dicionário
            DictCheck();
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog(); //Abrindo diálogo para escolhar local do arquivo
            sfd.Filter = "Text File (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"; //Definindo filtro de extensões.
            sfd.Title = "Salve seu arquivo."; //Título do diálogo.

            if (sfd.ShowDialog() == DialogResult.OK) // Se o diálogo ocorrer com sucesso
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create); //Acessar o caminho em modo de criação
                StreamWriter sw = new StreamWriter(fs); //Abrir um escritor para determinar o conteúdo do arquivo
                sw.Write(rtbEditor.Text); //Escrever os dados
                sw.Close(); //Fechar arquivo
                MessageBox.Show($"Arquivo salvo com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void abrirDictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dictContent = File.ReadAllText(dictPath); //Obter conteúdo do dicionário
            string[] dictWords = dictContent.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries); //Obter palavras do dicionário

            Dict d1 = new Dict(); //Instanciando novo form (grid do dicionário)
            d1.ShowGrid(dictWords); //Chamando função ShowGrid com parâmetro de palavras
            d1.Show(); //Chamando novo form.
        }

        private void desfazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.Undo(); //Desfazer
        }

        private void refazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.Redo(); //Refazer
        }

        private void recortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.Cut(); //Recortar
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.Copy(); //Copiar
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.Paste(); //Colar
        }

        private void selecionarTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbEditor.SelectAll(); //Selecionar todo o texto.
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Sobre o aplicativo.
            MessageBox.Show("Editor de texto desenvolvido em C# que utiliza dicionário de palavras.", "Desenvolvido por: Nathan / Marcos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
