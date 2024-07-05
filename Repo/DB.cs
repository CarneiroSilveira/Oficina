using Model;
using MySqlConnector;

namespace Repo
{
    public class DB
    {
        private static MySqlConnection conexao;


        public static List<Servico> servicos { get; set; }
        public static List<Produtos> produtos { get; set; }
        public static List<Atendimento> atendimentos { get; set; }
        public static List<Cliente> clientes { get; set; }

        public static object ListAll(string table)
        {
            switch (table.ToLower())
            {
                case "servicos":
                    return servicos;
                case "produtos":
                    return produtos;
                case "atendimentos":
                    return atendimentos;
                case "clientes":
                    return clientes;
                default:
                    throw new ArgumentException("Tabela inválida");
            }
        }
        public static void InitConexao()
        {
            string info = "server=localhost;database=projetointegrador;user id=root;password=''";
            conexao = new MySqlConnection(info);
            try
            {
                conexao.Open();
            }
            catch
            {
                MessageBox.Show("Não deu, foi mal");
            }
        }
        public static void CloseConexao()
        {
            conexao.Close();
        }

        public static void Criar(string table, object schema)
        {
            InitConexao();
            string insert = "";
            MySqlCommand command = new MySqlCommand();

            try
            {
                switch (table.ToLower())
                {
                    case "servico":
                        Servico servico = (Servico)schema;
                        if (servico.Nome == null || servico.Preco <= 0)
                        {
                            MessageBox.Show("Deu ruim, revise novamente o que foi enviado");
                            break;
                        }
                        else
                        {
                            insert = "INSERT INTO Servico (Nome, Preco, idAtendimento) VALUES (@Nome, @Preco, @idAtendimento)";
                            command.CommandText = insert;
                            command.Parameters.AddWithValue("@Nome", servico.Nome);
                            command.Parameters.AddWithValue("@Preco", servico.Preco);
                            command.Parameters.AddWithValue("@idAtendimento", servico.IdAtendimento);
                            break;
                        }
                    case "produtos":
                        Produtos produto = (Produtos)schema;
                        if (produto.Nome == null || produto.Preco < 0 || produto.Quantidade < 0)
                        {
                            MessageBox.Show("Deu ruim, revise novamente o que foi enviado");
                            break;
                        }
                        else
                        {
                            insert = "INSERT INTO Produtos (Nome, Preco, idAtendimento, Quantidade) VALUES (@Nome, @Preco, @idAtendimento, @Quantidade)";
                            command.CommandText = insert;
                            command.Parameters.AddWithValue("@Nome", produto.Nome);
                            command.Parameters.AddWithValue("@Preco", produto.Preco);
                            command.Parameters.AddWithValue("@idAtendimento", produto.IdAtendimento);
                            command.Parameters.AddWithValue("@Quantidade", produto.Quantidade);
                            break;
                        }


                    case "atendimento":
                        Atendimento atendimento = (Atendimento)schema;
                        if (atendimento.Descricao == null || atendimento.CustoExtra < 0 || atendimento.CustoTotal < 0 || atendimento.Desconto < 0)
                        {
                            MessageBox.Show("Deu ruim, revise novamente o que foi enviado");
                            break;
                        }
                        else
                        {
                            insert = "INSERT INTO Atendimento (DataInicio, DataFim, IdCliente, CustoTotal, Descricao, CustoExtra, Desconto) VALUES (@DataInicio, @DataFim, @IdCliente, @CustoTotal, @Descricao, @CustoExtra, @Desconto)";
                            command.CommandText = insert;
                            command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                            command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                            command.Parameters.AddWithValue("@IdCliente", atendimento.IdCliente);
                            command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                            command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                            command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                            command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);
                        }

                        break;

                    case "cliente":
                        Cliente cliente = (Cliente)schema;
                        if (cliente.Nome == null || cliente.Numero == null)
                        {
                            MessageBox.Show("Deu ruim, revise novamente o que foi enviado");
                            break;
                        }
                        else
                        {
                            insert = "INSERT INTO Cliente (Nome, ClienteNovo, numero, email) VALUES (@Nome, @ClienteNovo, @numero, @email)";
                            command.CommandText = insert;
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@ClienteNovo", cliente.ClienteNovo);
                            command.Parameters.AddWithValue("@numero", cliente.Numero);
                            command.Parameters.AddWithValue("@email", cliente.Email);
                            break;
                        }

                    default:
                        throw new ArgumentException("Tabela inválida");
                }

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show($"{table} cadastrado com sucesso");
                }
                else
                {
                    MessageBox.Show("Deu ruim, não deu pra adicionar");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Deu ruim: " + e.Message);
            }
            finally
            {
                CloseConexao();
            }
        }
    }

}