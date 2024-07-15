using MySqlConnector;
using Model;
namespace Repo
{
    public class DB
    {
        private static MySqlConnection conexao;

        public static List<Servico> servicos { get; set; } = new List<Servico>();
        public static List<Produtos> produtos { get; set; } = new List<Produtos>();
        public static List<Atendimento> atendimentos { get; set; } = new List<Atendimento>();
        public static List<Cliente> clientes { get; set; } = new List<Cliente>();

        public static List<T> ListAll<T>()
        {
            if (typeof(T) == typeof(Servico))
            {
                return servicos as List<T>;
            }
            else if (typeof(T) == typeof(Produtos))
            {
                return produtos as List<T>;
            }
            else if (typeof(T) == typeof(Atendimento))
            {
                return atendimentos as List<T>;
            }
            else if (typeof(T) == typeof(Cliente))
            {
                return clientes as List<T>;
            }
            else
            {
                throw new ArgumentException("Tipo inválido. Erro em ListAll.");
            }
        }

        public static async Task InitConexaoAsync()
        {
            string info = "server=localhost;database=oficina;user id=root;password=''";
            conexao = new MySqlConnection(info);
            try
            {
                await conexao.OpenAsync();
            }
            catch
            {
                MessageBox.Show("Não foi possível conectar ao banco de dados.");
            }
        }

        public static async Task CloseConexaoAsync()
        {
            await conexao.CloseAsync();
        }

        public static async Task SincronizarAsync()
        {
            await InitConexaoAsync();

            try
            {
                // Limpar dados anteriores dos modelos
                // servicos.Clear();
                // produtos.Clear();
                // atendimentos.Clear();
                // clientes.Clear();

                // Sincronizar Atendimentos 
                await ObterAtendimentosComDetalhesAsync();

                // Sincronizar Clientes
                string queryClientes = "SELECT * FROM Cliente";
                MySqlCommand commandClientes = new MySqlCommand(queryClientes, conexao);
                MySqlDataReader readerClientes = await commandClientes.ExecuteReaderAsync();
                while (await readerClientes.ReadAsync())
                {
                    Cliente cliente = new Cliente
                    {
                        Id = Convert.ToInt32(readerClientes["id"]),
                        Nome = readerClientes["Nome"].ToString() ?? "",
                        CPF = readerClientes["CPF"].ToString(),
                        Numero = readerClientes["Numero"].ToString() ?? "",
                        Email = readerClientes["Email"] == DBNull.Value ? null : readerClientes["Email"].ToString()
                    };

                    clientes.Add(cliente);
                }
                await readerClientes.CloseAsync();

                // Sincronizar Produtos
                string queryProdutos = "SELECT * FROM Produtos";
                MySqlCommand commandProdutos = new MySqlCommand(queryProdutos, conexao);
                MySqlDataReader readerProdutos = await commandProdutos.ExecuteReaderAsync();
                while (await readerProdutos.ReadAsync())
                {
                    Produtos produto = new Produtos
                    {
                        Id = Convert.ToInt32(readerProdutos["id"]),
                        Nome = readerProdutos["Nome"].ToString() ?? "",
                        Preco = Convert.ToDouble(readerProdutos["Preco"])
                    };

                    produtos.Add(produto);
                }
                await readerProdutos.CloseAsync();

                // Sincronizar Serviços
                string queryServicos = "SELECT * FROM Servico";
                MySqlCommand commandServicos = new MySqlCommand(queryServicos, conexao);
                MySqlDataReader readerServicos = await commandServicos.ExecuteReaderAsync();
                while (await readerServicos.ReadAsync())
                {
                    Servico servico = new Servico
                    {
                        Id = Convert.ToInt32(readerServicos["id"]),
                        Nome = readerServicos["Nome"].ToString() ?? "",
                        Preco = Convert.ToDouble(readerServicos["Preco"])
                    };

                    servicos.Add(servico);
                }
                await readerServicos.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao sincronizar dados: " + ex.Message);
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }

        private static async Task ObterAtendimentosComDetalhesAsync()
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
                    Cliente.CPF AS CPFCliente,
                    Servico.id AS ServicoID,
                    Servico.Nome AS NomeServico,
                    Servico.Preco AS PrecoServico,
                    Produtos.id AS ProdutoID,
                    Produtos.Nome AS NomeProduto,
                    Produtos.Preco AS PrecoProduto,
                    AtendimentoProdutos.Quantidade AS QuantidadeProduto
                FROM
                    Atendimento
                JOIN
                    Cliente ON Atendimento.IdCliente = Cliente.id
                JOIN
                    ServicoAtendimento ON Atendimento.id = ServicoAtendimento.idAtendimento
                JOIN
                    Servico ON ServicoAtendimento.idServico = Servico.id
                JOIN
                    AtendimentoProdutos ON Atendimento.id = AtendimentoProdutos.idAtendimento
                JOIN
                    Produtos ON AtendimentoProdutos.idProdutos = Produtos.id;";

            MySqlCommand command = new MySqlCommand(query, conexao);
            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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
                        Descricao = reader["Descricao"].ToString() ?? "",
                        CustoExtra = reader["CustoExtra"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["CustoExtra"]),
                        Desconto = reader["Desconto"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["Desconto"]),
                        ClienteAtendido = new Cliente
                        {
                            Id = Convert.ToInt32(reader["ClienteID"]),
                            Nome = reader["NomeCliente"].ToString() ?? "",
                            Numero = reader["NumeroCliente"].ToString() ?? "",
                            Email = reader["EmailCliente"] == DBNull.Value ? null : reader["EmailCliente"].ToString(),
                            CPF = reader["CPFCliente"] == DBNull.Value ? null : reader["CPFCliente"].ToString()
                        },
                        ServicosRealizados = new List<Servico>(),
                        ProdutosUsados = new List<Produtos>()
                    };

                    atendimentos.Add(atendimento);
                }

                Servico servico = new Servico
                {
                    Id = Convert.ToInt32(reader["ServicoID"]),
                    Nome = reader["NomeServico"].ToString() ?? "",
                    Preco = Convert.ToDouble(reader["PrecoServico"])
                };

                if (!atendimento.ServicosRealizados.Exists(s => s.Id == servico.Id))
                {
                    atendimento.ServicosRealizados.Add(servico);
                }

                Produtos produto = new Produtos
                {
                    Id = Convert.ToInt32(reader["ProdutoID"]),
                    Nome = reader["NomeProduto"].ToString() ?? "",
                    Preco = Convert.ToDouble(reader["PrecoProduto"]),
                    Quantidade = int.Parse(reader["QuantidadeProduto"].ToString() ?? "")
                };

                if (!atendimento.ProdutosUsados.Exists(p => p.Id == produto.Id))
                {
                    atendimento.ProdutosUsados.Add(produto);
                }
            }

            await reader.CloseAsync();
        }

        public static async Task CriarAsync(string table, object schema)
        {
            await InitConexaoAsync();
            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
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

                            int rowsaffected = await command.ExecuteNonQueryAsync();
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

                            int rowsaffected = await command.ExecuteNonQueryAsync();
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

                    case "clientes":
                        Cliente cliente = (Cliente)schema;
                        if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.CPF) || string.IsNullOrEmpty(cliente.Numero))
                        {
                            MessageBox.Show("Nome, CPF e número de contato do cliente são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Cliente (Nome, CPF, Numero, Email) VALUES (@Nome, @CPF, @Numero, @Email)";
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@CPF", cliente.CPF);
                            command.Parameters.AddWithValue("@Numero", cliente.Numero);
                            command.Parameters.AddWithValue("@Email", cliente.Email);

                            int rowsaffected = await command.ExecuteNonQueryAsync();
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

                    case "atendimento":
                        Atendimento atendimento = (Atendimento)schema;
                        if (atendimento.CustoTotal < 0 || atendimento.ClienteAtendido == null)
                        {
                            MessageBox.Show("Data de início, data de fim, custo total e cliente atendido são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = @"INSERT INTO Atendimento (DataInicio, DataFim, CustoTotal, Descricao, CustoExtra, Desconto, IdCliente) 
                                                    VALUES (@DataInicio, @DataFim, @CustoTotal, @Descricao, @CustoExtra, @Desconto, @IdCliente)";
                            command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                            command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                            command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                            command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                            command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                            command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);
                            command.Parameters.AddWithValue("@IdCliente", atendimento.ClienteAtendido.Id);

                            int rowsaffected = await command.ExecuteNonQueryAsync();
                            atendimento.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                MessageBox.Show("Atendimento cadastrado com sucesso.");
                                atendimentos.Add(atendimento);

                                // Adicionar serviços e produtos relacionados ao atendimento
                                foreach (var servicoRel in atendimento.ServicosRealizados)
                                {
                                    command.CommandText = "INSERT INTO ServicoAtendimento (IdServico, IdAtendimento) VALUES (@IdServico, @IdAtendimento)";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@IdServico", servicoRel.Id);
                                    command.Parameters.AddWithValue("@IdAtendimento", atendimento.Id);

                                    await command.ExecuteNonQueryAsync();
                                }

                                foreach (var produtoRel in atendimento.ProdutosUsados)
                                {
                                    command.CommandText = "INSERT INTO AtendimentoProdutos (IdProdutos, IdAtendimento, Quantidade) VALUES (@IdProdutos, @IdAtendimento, @Quantidade)";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@IdProdutos", produtoRel.Id);
                                    command.Parameters.AddWithValue("@IdAtendimento", atendimento.Id);
                                    command.Parameters.AddWithValue("@Quantidade", produtoRel.Quantidade);

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o atendimento.");
                            }
                            break;
                        }

                    default:
                        throw new ArgumentException("Tabela inválida Erro em CriarAsync");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir dados: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }

        public static async Task DeletarAsync(string table, int id)
        {
            await InitConexaoAsync();

            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
                command.Transaction = transaction;

                switch (table.ToLower())
                {
                    case "servico":
                        command.CommandText = "DELETE FROM Servico WHERE id = @Id";
                        command.Parameters.AddWithValue("@Id", id);
                        int servicoRows = await command.ExecuteNonQueryAsync();
                        if (servicoRows > 0)
                        {
                            servicos.RemoveAll(s => s.Id == id);
                            MessageBox.Show("Serviço excluído com sucesso.");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o serviço.");
                        }
                        break;

                    case "produtos":
                        command.CommandText = "DELETE FROM Produtos WHERE id = @Id";
                        command.Parameters.AddWithValue("@Id", id);
                        int produtoRows = await command.ExecuteNonQueryAsync();
                        if (produtoRows > 0)
                        {
                            produtos.RemoveAll(p => p.Id == id);
                            MessageBox.Show("Produto excluído com sucesso.");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o produto.");
                        }
                        break;

                    case "clientes":
                        command.CommandText = "DELETE FROM Cliente WHERE id = @Id";
                        command.Parameters.AddWithValue("@Id", id);
                        int clienteRows = await command.ExecuteNonQueryAsync();
                        if (clienteRows > 0)
                        {
                            clientes.RemoveAll(c => c.Id == id);
                            MessageBox.Show("Cliente excluído com sucesso.");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o cliente.");
                        }
                        break;

                    case "atendimento":
                        command.CommandText = "DELETE FROM Atendimento WHERE id = @Id";
                        command.Parameters.AddWithValue("@Id", id);
                        int atendimentoRows = await command.ExecuteNonQueryAsync();
                        if (atendimentoRows > 0)
                        {
                            atendimentos.RemoveAll(a => a.Id == id);
                            MessageBox.Show("Atendimento excluído com sucesso.");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o atendimento.");
                        }
                        break;

                    default:
                        throw new ArgumentException("Tabela inválida Erro em Deletar Async");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao deletar dados: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }

        public static async Task UpdateAtendimentoAsync(Atendimento atendimento)
        {
            await InitConexaoAsync();

            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
                command.Transaction = transaction;

                command.CommandText = @"UPDATE Atendimento SET DataInicio = @DataInicio, DataFim = @DataFim, CustoTotal = @CustoTotal, Descricao = @Descricao, CustoExtra = @CustoExtra, Desconto = @Desconto, IdCliente = @IdCliente 
                                        WHERE id = @Id";
                command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);
                command.Parameters.AddWithValue("@IdCliente", atendimento.ClienteAtendido.Id);
                command.Parameters.AddWithValue("@Id", atendimento.Id);

                int rowsaffected = await command.ExecuteNonQueryAsync();

                if (rowsaffected > 0)
                {
                    // Atualizar serviços e produtos relacionados ao atendimento
                    command.CommandText = "DELETE FROM ServicoAtendimento WHERE IdAtendimento = @Id";
                    await command.ExecuteNonQueryAsync();

                    foreach (var servico in atendimento.ServicosRealizados)
                    {
                        command.CommandText = "INSERT INTO ServicoAtendimento (IdServico, IdAtendimento) VALUES (@IdServico, @IdAtendimento)";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@IdServico", servico.Id);
                        command.Parameters.AddWithValue("@IdAtendimento", atendimento.Id);

                        await command.ExecuteNonQueryAsync();
                    }

                    command.CommandText = "DELETE FROM AtendimentoProdutos WHERE IdAtendimento = @Id";
                    await command.ExecuteNonQueryAsync();

                    foreach (var produto in atendimento.ProdutosUsados)
                    {
                        command.CommandText = "INSERT INTO AtendimentoProdutos (IdProdutos, IdAtendimento, Quantidade) VALUES (@IdProdutos, @IdAtendimento, @Quantidade)";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@IdProdutos", produto.Id);
                        command.Parameters.AddWithValue("@IdAtendimento", atendimento.Id);
                        command.Parameters.AddWithValue("@Quantidade", produto.Quantidade);

                        await command.ExecuteNonQueryAsync();
                    }

                    MessageBox.Show("Atendimento atualizado com sucesso.");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o atendimento.");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar atendimento: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }
        public static async Task UpdateProdutoAsync(Produtos produto)
        {
            await InitConexaoAsync();

            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
                command.Transaction = transaction;

                command.CommandText = @"UPDATE Produtos SET Nome = @Nome, Preco = @Preco WHERE id = @Id";
                command.Parameters.AddWithValue("@Nome", produto.Nome);
                command.Parameters.AddWithValue("@Preco", produto.Preco);
                command.Parameters.AddWithValue("@Id", produto.Id);

                int rowsaffected = await command.ExecuteNonQueryAsync();

                if (rowsaffected > 0)
                {
                    MessageBox.Show("Produto atualizado com sucesso.");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o produto.");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar produto: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }
        public static async Task UpdateClienteAsync(Cliente cliente)
        {
            await InitConexaoAsync();

            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
                command.Transaction = transaction;

                command.CommandText = @"UPDATE Cliente SET Nome = @Nome, CPF = @CPF, Numero = @Numero, Email = @Email WHERE id = @Id";
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@CPF", cliente.CPF);
                command.Parameters.AddWithValue("@Numero", cliente.Numero);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Id", cliente.Id);

                int rowsaffected = await command.ExecuteNonQueryAsync();

                if (rowsaffected > 0)
                {
                    MessageBox.Show("Cliente atualizado com sucesso.");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o cliente.");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar cliente: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }
        public static async Task UpdateServicoAsync(Servico servico)
        {
            await InitConexaoAsync();

            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = await conexao.BeginTransactionAsync();
                command.Transaction = transaction;

                command.CommandText = @"UPDATE Servico SET Nome = @Nome, Preco = @Preco WHERE id = @Id";
                command.Parameters.AddWithValue("@Nome", servico.Nome);
                command.Parameters.AddWithValue("@Preco", servico.Preco);
                command.Parameters.AddWithValue("@Id", servico.Id);

                int rowsaffected = await command.ExecuteNonQueryAsync();

                if (rowsaffected > 0)
                {
                    MessageBox.Show("Serviço atualizado com sucesso.");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar o serviço.");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar serviço: " + ex.Message);
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            finally
            {
                await CloseConexaoAsync();
            }
        }
    }
}