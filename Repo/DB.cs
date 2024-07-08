using MySqlConnector;
using Model;

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
            string info = "server=localhost;database=oficina;user id=root;password=''";
            conexao = new MySqlConnection(info);
            try
            {
                conexao.Open();
            }
            catch
            {
                MessageBox.Show("Não foi possível conectar ao banco de dados.");
            }
        }

        public static void CloseConexao()
        {
            conexao.Close();
        }

        public static void Sincronizar()
        {
            InitConexao();

            try
            {
                // Sincronizar Atendimentos
                string queryAtendimentos = "SELECT * FROM Atendimento";
                MySqlCommand commandAtendimentos = new MySqlCommand(queryAtendimentos, conexao);
                MySqlDataReader readerAtendimentos = commandAtendimentos.ExecuteReader();
                while (readerAtendimentos.Read())
                {
                    Atendimento atendimento = new Atendimento
                    {
                        Id = Convert.ToInt32(readerAtendimentos["id"]),
                        DataInicio = Convert.ToDateTime(readerAtendimentos["DataInicio"]),
                        DataFim = Convert.ToDateTime(readerAtendimentos["DataFim"]),
                        IdCliente = Convert.ToInt32(readerAtendimentos["IdCliente"]),
                        CustoTotal = Convert.ToDouble(readerAtendimentos["CustoTotal"]),
                        Descricao = readerAtendimentos["Descricao"].ToString(),
                        CustoExtra = readerAtendimentos["CustoExtra"] == DBNull.Value ? null : (double?)Convert.ToDouble(readerAtendimentos["CustoExtra"]),
                        Desconto = readerAtendimentos["Desconto"] == DBNull.Value ? null : (double?)Convert.ToDouble(readerAtendimentos["Desconto"]),
                        IdServico = Convert.ToInt32(readerAtendimentos["IdServico"])
                    };

                    // Sincronizar Produtos Usados no Atendimento
                    atendimento.ProdutosUsados = ObterProdutosUsadosAtendimento(atendimento.Id);

                    atendimentos.Add(atendimento);
                }
                readerAtendimentos.Close();

                // Sincronizar Clientes
                string queryClientes = "SELECT * FROM Cliente";
                MySqlCommand commandClientes = new MySqlCommand(queryClientes, conexao);
                MySqlDataReader readerClientes = commandClientes.ExecuteReader();
                while (readerClientes.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        Id = Convert.ToInt32(readerClientes["id"]),
                        Nome = readerClientes["Nome"].ToString(),
                        ClienteNovo = Convert.ToBoolean(readerClientes["ClienteNovo"]),
                        Numero = readerClientes["Numero"].ToString(),
                        Email = readerClientes["Email"] == DBNull.Value ? null : readerClientes["Email"].ToString()
                    };

                    clientes.Add(cliente);
                }
                readerClientes.Close();

                // Sincronizar Produtos
                string queryProdutos = "SELECT * FROM Produtos";
                MySqlCommand commandProdutos = new MySqlCommand(queryProdutos, conexao);
                MySqlDataReader readerProdutos = commandProdutos.ExecuteReader();
                while (readerProdutos.Read())
                {
                    Produtos produto = new Produtos
                    {
                        Id = Convert.ToInt32(readerProdutos["id"]),
                        Nome = readerProdutos["Nome"].ToString(),
                        Preco = Convert.ToDouble(readerProdutos["Preco"])
                    };

                    produtos.Add(produto);
                }
                readerProdutos.Close();

                // Sincronizar Serviços
                string queryServicos = "SELECT * FROM Servico";
                MySqlCommand commandServicos = new MySqlCommand(queryServicos, conexao);
                MySqlDataReader readerServicos = commandServicos.ExecuteReader();
                while (readerServicos.Read())
                {
                    Servico servico = new Servico
                    {
                        Id = Convert.ToInt32(readerServicos["id"]),
                        Nome = readerServicos["Nome"].ToString(),
                        Preco = Convert.ToDouble(readerServicos["Preco"])
                    };

                    servicos.Add(servico);
                }
                readerServicos.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao sincronizar dados: " + ex.Message);
            }
            finally
            {
                CloseConexao();
            }
        }

        private static List<ProdutoUsado> ObterProdutosUsadosAtendimento(int idAtendimento)
        {
            List<ProdutoUsado> produtosUsados = new List<ProdutoUsado>();

            InitConexao();

            string queryProdutosUsados = "SELECT * FROM ServicoProdutos WHERE idAtendimento = @idAtendimento";
            MySqlCommand commandProdutosUsados = new MySqlCommand(queryProdutosUsados, conexao);
            commandProdutosUsados.Parameters.AddWithValue("@idAtendimento", idAtendimento);
            MySqlDataReader readerProdutosUsados = commandProdutosUsados.ExecuteReader();
            while (readerProdutosUsados.Read())
            {
                ProdutoUsado produtoUsado = new ProdutoUsado
                {
                    ProdutoId = Convert.ToInt32(readerProdutosUsados["idProdutos"]),
                    Quantidade = Convert.ToInt32(readerProdutosUsados["Quantidade"])
                };

                produtosUsados.Add(produtoUsado);
            }
            readerProdutosUsados.Close();
            CloseConexao();

            return produtosUsados;
        }

        public static void Criar(string table, object schema)
        {
            InitConexao();
            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = conexao.BeginTransaction();
                command.Transaction = transaction;

                switch (table.ToLower())
                {
                    case "servico":
                        Servico servico = (Servico)schema;
                        if (servico.Nome == null || servico.Preco <= 0)
                        {
                            MessageBox.Show("Nome e preço do serviço são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Servico (Nome, Preco) VALUES (@Nome, @Preco)";
                            command.Parameters.AddWithValue("@Nome", servico.Nome);
                            command.Parameters.AddWithValue("@Preco", servico.Preco);

                            int rowsaffected = command.ExecuteNonQuery();
                            servico.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                MessageBox.Show("Serviço cadastrado com sucesso.");
                                servicos.Add(servico);
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o serviço.");
                            }
                            break;
                        }

                    case "produtos":
                        Produtos produto = (Produtos)schema;
                        if (produto.Nome == null || produto.Preco < 0)
                        {
                            MessageBox.Show("Nome e preço do produto são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Produtos (Nome, Preco) VALUES (@Nome, @Preco)";
                            command.Parameters.AddWithValue("@Nome", produto.Nome);
                            command.Parameters.AddWithValue("@Preco", produto.Preco);

                            int rowsaffected = command.ExecuteNonQuery();
                            produto.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                MessageBox.Show("Produto cadastrado com sucesso.");
                                produtos.Add(produto);
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o produto.");
                            }
                            break;
                        }

                    case "atendimento":
                        Atendimento atendimento = (Atendimento)schema;
                        if (atendimento.Descricao == null || atendimento.CustoExtra < 0 || atendimento.CustoTotal < 0 || atendimento.Desconto < 0)
                        {
                            MessageBox.Show("Descrição, custo total, custo extra e desconto do atendimento são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Atendimento (DataInicio, DataFim, IdCliente, CustoTotal, Descricao, CustoExtra, Desconto) VALUES (@DataInicio, @DataFim, @IdCliente, @CustoTotal, @Descricao, @CustoExtra, @Desconto)";
                            command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                            command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                            command.Parameters.AddWithValue("@IdCliente", atendimento.IdCliente);
                            command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                            command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                            command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                            command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);

                            int rowsaffected = command.ExecuteNonQuery();
                            atendimento.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                command.CommandText = "INSERT INTO ServicoAtendimento (idServico, idAtendimento) VALUES (@idServico, @idAtendimento)";
                                command.Parameters.AddWithValue("@idServico", atendimento.IdServico);
                                command.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                rowsaffected = command.ExecuteNonQuery();

                                if (rowsaffected > 0)
                                {
                                    foreach (var produtoUsado in atendimento.ProdutosUsados)
                                    {
                                        command.CommandText = "INSERT INTO ServicoProdutos (idProdutos, idAtendimento, Quantidade) VALUES (@idProdutos, @idAtendimento, @Quantidade)";
                                        command.Parameters.AddWithValue("@idProdutos", produtoUsado.ProdutoId);
                                        command.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                        command.Parameters.AddWithValue("@Quantidade", produtoUsado.Quantidade);
                                        command.ExecuteNonQuery();
                                    }
                                    atendimentos.Add(atendimento);
                                    MessageBox.Show("Atendimento cadastrado com sucesso.");
                                }
                                else
                                {
                                    MessageBox.Show("Falha ao adicionar o serviço ao atendimento.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o atendimento.");
                            }
                            break;
                        }

                    case "cliente":
                        Cliente cliente = (Cliente)schema;
                        if (cliente.Nome == null || cliente.Numero == null)
                        {
                            MessageBox.Show("Nome e número do cliente são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Cliente (Nome, ClienteNovo, Numero, Email) VALUES (@Nome, @ClienteNovo, @Numero, @Email)";
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@ClienteNovo", cliente.ClienteNovo);
                            command.Parameters.AddWithValue("@Numero", cliente.Numero);
                            command.Parameters.AddWithValue("@Email", cliente.Email);

                            int rowsaffected = command.ExecuteNonQuery();
                            cliente.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                MessageBox.Show("Cliente cadastrado com sucesso.");
                                clientes.Add(cliente);
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o cliente.");
                            }
                            break;
                        }

                    default:
                        throw new ArgumentException("Tabela inválida");
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                MessageBox.Show("Erro: " + e.Message);
            }
            finally
            {
                CloseConexao();
            }
        }
    }
}