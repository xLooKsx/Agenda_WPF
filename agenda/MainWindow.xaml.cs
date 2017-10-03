using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String operacao;

        public String connectionString = "Server=localhost;Database=agendawpf;Uid=root;Pwd=admin;";

        

        public MainWindow()
        {
            InitializeComponent();
        }

        public void operacoes(String op) {
            String connectionString = "Server=localhost;Database=agendawpf;Uid=root;Pwd=admin;";

            //Criando e abrindo a conexão
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rd;

            switch (op)
            {
                case "inserir":
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO contatos(nome, email, telefone) " +
                                           "VALUES(@subjectNome, @subjectemail, @subjectTelefone);";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@subjectNome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@subjectemail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@subjectTelefone", txtTelefone.Text);                    
                    cmd.ExecuteNonQuery();
                    this.ListarContatos();
                    this.AlteraBotoes(1);
                    this.LimpaCampos();
                    break;

                case "alterar":
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE contatos SET nome= @subjectNome, email= @subjectEmail, telefone= @subjectTelefone WHERE id= @subjectId;";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@subjectNome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@subjectEmail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@subjectTelefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@subjectId", txtID.Text);
                    cmd.ExecuteNonQuery();
                    this.ListarContatos();
                    this.AlteraBotoes(1);
                    this.LimpaCampos();
                    break;

                case "deletar":
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM conntatos WHERE id= @subjectId;";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@subjectId", txtID.Text);
                    cmd.ExecuteNonQuery();
                    this.ListarContatos();
                    this.AlteraBotoes(1);
                    this.LimpaCampos();
                    break;
            }
        }

        private void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            //cria um contexto que representa o DB e grava no DB
            if (operacao == "inserir")
            {
                operacoes(operacao);
            }
            if (operacao == "alterar")
            {
                operacoes(operacao);
            }
            this.ListarContatos();
            this.AlteraBotoes(1);
            this.LimpaCampos();
        }

        private void LimpaCampos()
        {
            txtID.IsEnabled = true;
            txtID.Clear();
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
        }

        private void btInserir_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "inserir";            
            this.AlteraBotoes(2);
            this.LimpaCampos();
            txtID.IsEnabled = false;
        }

        //Assim que carrega a window ele já lista os contatos
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListarContatos();
            btInserir.IsEnabled = true;
        }

        private void ListarContatos()
        {

            String connectionString = "Server=localhost;Database=agendawpf;Uid=root;Pwd=admin;";

            //Criando e abrindo a conexão
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //Criando um select
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "SELECT * FROM contatos;";
            cmd.Connection = connection;

            //Executa o comando sql
            MySqlDataReader rd = cmd.ExecuteReader();

            dgDados.ItemsSource = rd;

            btnLimparPesquisa.IsEnabled = false;

        }

        private void AlteraBotoes(int op)
        {
            btAlterar.IsEnabled = false;
            btInserir.IsEnabled = false;
            btExcluir.IsEnabled = false;
            btCancelar.IsEnabled = false;
            btLocalizar.IsEnabled = false;
            btSalvar.IsEnabled = false;
            btnLimparPesquisa.IsEnabled = false;

            switch (op)
            {
                case 1:
                    btInserir.IsEnabled = true;
                    btLocalizar.IsEnabled = true;
                    break;
                case 2:
                    btCancelar.IsEnabled = true;
                    btSalvar.IsEnabled = true;
                    break;
                case 3:
                    btAlterar.IsEnabled = true;
                    btExcluir.IsEnabled = true;
                    break;
            }
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.AlteraBotoes(1);
            this.LimpaCampos();
        }

        private void btLocalizar_Click(object sender, RoutedEventArgs e)
        {
            //buscar pelo nome
            if (txtNome.Text.Trim().Count() > 0)
            {
                String connectionString = "Server=localhost;Database=agendawpf;Uid=root;Pwd=admin;";

                //Criando e abrindo a conexão
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //Criando um select
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM contatos WHERE nome LIKE '%"+txtNome.Text+"%';";
                cmd.Connection = connection;

                //Executa o comando sql
                MySqlDataReader rd = cmd.ExecuteReader();

                dgDados.ItemsSource = rd;

                btnLimparPesquisa.IsEnabled=true;
                btAlterar.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Busca deve ser realizada pelo nome");
            }
           
        }

        private void dgDados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void btAlterar_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "alterar";
            this.AlteraBotoes(2);
            //this.LimpaCampos();
            txtID.IsEnabled = false;
        }

        private void btExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.Text.Trim().Count() > 0)
            {
                operacoes("deletar");
                this.ListarContatos();
                this.AlteraBotoes(1);
                this.LimpaCampos();
            }
            else
            {
                MessageBox.Show("exclusão deve ser realizada pelo id");
            }
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListarContatos();
        }
    }
}

