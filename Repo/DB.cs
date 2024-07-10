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
                // Limpar dados anteriores dos modelos
                servicos.Clear();
                produtos.Clear();
                atendimentos.Clear();
                clientes.Clear();

                // Sincronizar Atendimentos 
                ObterAtendimentosComDetalhes();

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

        private static void ObterAtendimentosComDetalhes()
        {
            string query = @"
                SELECT
                    Atendimento.id AS AtendimentoID,
                    Atendimento.DataInicio,
                    Atendimento.DataFim,
                    Atendimento.CustoTotal,
                    Atendimento.Descricao,
                    Atendimento.CustoExtra,
                    Atendimento.Desconto,
                    Cliente.id AS ClienteID,
                    Cliente.Nome AS NomeCliente,
                    Cliente.Numero AS NumeroCliente,
                    Cliente.Email AS EmailCliente,
                    Servico.id AS ServicoID,
                    Servico.Nome AS NomeServico,
                    Servico.Preco AS PrecoServico,
                    Produtos.id AS ProdutoID,
                    Produtos.Nome AS NomeProduto,
                    Produtos.Preco AS PrecoProduto,
                    ServicoProdutos.Quantidade AS QuantidadeProduto
                FROM
                    Atendimento
                JOIN
                    Cliente ON Atendimento.IdCliente = Cliente.id
                JOIN
                    ServicoAtendimento ON Atendimento.id = ServicoAtendimento.idAtendimento
                JOIN
                    Servico ON ServicoAtendimento.idServico = Servico.id
                JOIN
                    ServicoProdutos ON Atendimento.id = ServicoProdutos.idAtendimento
                JOIN
                    Produtos ON ServicoProdutos.idProdutos = Produtos.id;";

            MySqlCommand command = new MySqlCommand(query, conexao);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int atendimentoId = Convert.ToInt32(reader["AtendimentoID"]);

                Atendimento atendimento = atendimentos.Find(a => a.Id == atendimentoId);

                if (atendimento == null)
                {
                    atendimento = new Atendimento
                    {
                        Id = atendimentoId,
                        DataInicio = Convert.ToDateTime(reader["DataInicio"]),
                        DataFim = Convert.ToDateTime(reader["DataFim"]),
                        CustoTotal = Convert.ToDouble(reader["CustoTotal"]),
                        Descricao = reader["Descricao"].ToString(),
                        CustoExtra = reader["CustoExtra"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["CustoExtra"]),
                        Desconto = reader["Desconto"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["Desconto"]),
                        QuantidadeProduto = Convert.ToInt32(reader["QuantidadeProduto"]),
                        ClienteAtendido = new Cliente
                        {
                            Id = Convert.ToInt32(reader["ClienteID"]),
                            Nome = reader["NomeCliente"].ToString(),
                            Numero = reader["NumeroCliente"].ToString(),
                            Email = reader["EmailCliente"] == DBNull.Value ? null : reader["EmailCliente"].ToString()
                        },
                        ServicosRealizados = new List<Servico>(),
                        ProdutosUsados = new List<Produtos>()
                    };

                    atendimentos.Add(atendimento);
                }

                Servico servico = new Servico
                {
                    Id = Convert.ToInt32(reader["ServicoID"]),
                    Nome = reader["NomeServico"].ToString(),
                    Preco = Convert.ToDouble(reader["PrecoServico"])
                };

                if (!atendimento.ServicosRealizados.Exists(s => s.Id == servico.Id))
                {
                    atendimento.ServicosRealizados.Add(servico);
                }

                Produtos produto = new Produtos
                {
                    Id = Convert.ToInt32(reader["ProdutoID"]),
                    Nome = reader["NomeProduto"].ToString(),
                    Preco = Convert.ToDouble(reader["PrecoProduto"]),
                };

                if (!atendimento.ProdutosUsados.Exists(p => p.Id == produto.Id))
                {
                    atendimento.ProdutosUsados.Add(produto);
                }
            }

            reader.Close();
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
                        if (string.IsNullOrEmpty(servico.Nome) || servico.Preco <= 0)
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
                        if (string.IsNullOrEmpty(produto.Nome) || produto.Preco < 0)
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
                        if (string.IsNullOrEmpty(atendimento.Descricao) || atendimento.CustoTotal < 0)
                        {
                            MessageBox.Show("Descrição e custo total do atendimento são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Atendimento (DataInicio, DataFim, IdCliente, CustoTotal, Descricao, CustoExtra, Desconto) VALUES (@DataInicio, @DataFim, @IdCliente, @CustoTotal, @Descricao, @CustoExtra, @Desconto)";
                            command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                            command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                            command.Parameters.AddWithValue("@IdCliente", atendimento.ClienteAtendido?.Id);
                            command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                            command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                            command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                            command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);

                            int rowsaffected = command.ExecuteNonQuery();
                            atendimento.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                // Inserir os serviços realizados no atendimento
                                foreach (var servicoRealizado in atendimento.ServicosRealizados)
                                {
                                    command.CommandText = "INSERT INTO ServicoAtendimento (idServico, idAtendimento) VALUES (@idServico, @idAtendimento)";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@idServico", servicoRealizado.Id);
                                    command.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                    command.ExecuteNonQuery();
                                }

                                // Inserir os produtos usados no atendimento
                                foreach (var produtoUsado in atendimento.ProdutosUsados)
                                {
                                    command.CommandText = "INSERT INTO ServicoProdutos (idProdutos, idAtendimento, Quantidade) VALUES (@idProdutos, @idAtendimento, @Quantidade)";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@idProdutos", produtoUsado.Id);
                                    command.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                    command.Parameters.AddWithValue("@Quantidade", atendimento.QuantidadeProduto);
                                    command.ExecuteNonQuery();
                                }

                                atendimentos.Add(atendimento);
                                MessageBox.Show("Atendimento cadastrado com sucesso.");
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o atendimento.");
                            }
                            break;
                        }

                    case "cliente":
                        Cliente cliente = (Cliente)schema;
                        if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Numero))
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
