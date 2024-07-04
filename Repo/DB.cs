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
            switch (table.ToLower())
            {
                case "servicos":
                    Servico servico = Servico();
                    InitConexao();
                    string insert = "INSERT INTO servicos (Nome, Preco) VALUES (@Nome, @Preco)";
                    MySqlCommand command = new MySqlCommand(insert, conexao);
                    try
                    {
                        if (schema.Nome == null || pessoa.Idade < 0 || pessoa.Cpf == null)
                        {
                            MessageBox.Show("Deu ruim, favor preencher a pessoa");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nome", pessoa.Nome);
                            command.Parameters.AddWithValue("@Idade", pessoa.Idade);
                            command.Parameters.AddWithValue("@Cpf", pessoa.Cpf);

                            int rowsAffected = command.ExecuteNonQuery();
                            pessoa.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pessoa cadastrada com sucesso");
                                pessoas.Add(pessoa);
                            }
                            else
                            {
                                MessageBox.Show("Deu ruim, não deu pra adicionar");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Deu ruim: " + e.Message);
                    }

                    CloseConexao();
                case "produtos":
                    InitConexao();
                    string insert = "INSERT INTO servicos (Nome, Preco) VALUES (@Nome, @Idade, @Cpf)";
                    MySqlCommand command = new MySqlCommand(insert, conexao);
                    try
                    {
                        if (pessoa.Nome == null || pessoa.Idade < 0 || pessoa.Cpf == null)
                        {
                            MessageBox.Show("Deu ruim, favor preencher a pessoa");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nome", pessoa.Nome);
                            command.Parameters.AddWithValue("@Idade", pessoa.Idade);
                            command.Parameters.AddWithValue("@Cpf", pessoa.Cpf);

                            int rowsAffected = command.ExecuteNonQuery();
                            pessoa.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pessoa cadastrada com sucesso");
                                pessoas.Add(pessoa);
                            }
                            else
                            {
                                MessageBox.Show("Deu ruim, não deu pra adicionar");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Deu ruim: " + e.Message);
                    }

                    CloseConexao();
                case "atendimentos":
                    InitConexao();
                    string insert = "INSERT INTO servicos (Nome, Preco) VALUES (@Nome, @Idade, @Cpf)";
                    MySqlCommand command = new MySqlCommand(insert, conexao);
                    try
                    {
                        if (pessoa.Nome == null || pessoa.Idade < 0 || pessoa.Cpf == null)
                        {
                            MessageBox.Show("Deu ruim, favor preencher a pessoa");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nome", pessoa.Nome);
                            command.Parameters.AddWithValue("@Idade", pessoa.Idade);
                            command.Parameters.AddWithValue("@Cpf", pessoa.Cpf);

                            int rowsAffected = command.ExecuteNonQuery();
                            pessoa.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pessoa cadastrada com sucesso");
                                pessoas.Add(pessoa);
                            }
                            else
                            {
                                MessageBox.Show("Deu ruim, não deu pra adicionar");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Deu ruim: " + e.Message);
                    }

                    CloseConexao();
                case "clientes":
                    InitConexao();
                    string insert = "INSERT INTO servicos (Nome, Preco) VALUES (@Nome, @Idade, @Cpf)";
                    MySqlCommand command = new MySqlCommand(insert, conexao);
                    try
                    {
                        if (pessoa.Nome == null || pessoa.Idade < 0 || pessoa.Cpf == null)
                        {
                            MessageBox.Show("Deu ruim, favor preencher a pessoa");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nome", pessoa.Nome);
                            command.Parameters.AddWithValue("@Idade", pessoa.Idade);
                            command.Parameters.AddWithValue("@Cpf", pessoa.Cpf);

                            int rowsAffected = command.ExecuteNonQuery();
                            pessoa.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pessoa cadastrada com sucesso");
                                pessoas.Add(pessoa);
                            }
                            else
                            {
                                MessageBox.Show("Deu ruim, não deu pra adicionar");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Deu ruim: " + e.Message);
                    }

                    CloseConexao();
                default:
                    throw new ArgumentException("Tabela invalida");
            }
        }
    }
}