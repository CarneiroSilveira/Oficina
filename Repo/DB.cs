using MySqlConnector;
using Model;
namespace Repo
{
    public class DB
    {
        private static MySqlConnection conexao;

        public static List<Servico> servicos { get; set; } = [];
        public static List<Produtos> produtos { get; set; } = [];
        public static List<Atendimento> atendimentos { get; set; } = [];
        public static List<Cliente> clientes { get; set; } = [];

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
                        Nome = readerClientes["Nome"].ToString() ?? "",
                        CPF = readerClientes["CPF"] == DBNull.Value ? null : readerClientes["CPF"].ToString(),
                        Numero = readerClientes["Numero"].ToString() ?? "",
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
                        Nome = readerProdutos["Nome"].ToString() ?? "",
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
                        Nome = readerServicos["Nome"].ToString() ?? "",
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

            reader.Close();
        }

        public static void Criar(object schema)
        {
            InitConexao();
            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = conexao.BeginTransaction();
                command.Transaction = transaction;

                switch (schema)
                {
                    case Servico:
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

                    case Produtos:
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

                    case Cliente:
                        Cliente cliente = (Cliente)schema;
                        if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Numero))
                        {
                            MessageBox.Show("Nome e número do cliente são obrigatórios.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Cliente (Nome, CPF, Numero, Email) VALUES (@Nome, @CPF, @Numero, @Email)";
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@CPF", cliente.CPF);
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

                    case Atendimento:
                        Atendimento atendimento = (Atendimento)schema;
                        if (atendimento.ClienteAtendido == null || atendimento.ServicosRealizados == null || atendimento.ServicosRealizados.Count == 0)
                        {
                            MessageBox.Show("O atendimento deve ter um cliente e pelo menos um serviço.");
                            break;
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Atendimento (IdCliente, DataInicio, DataFim, CustoTotal, Descricao, CustoExtra, Desconto) VALUES (@IdCliente, @DataInicio, @DataFim, @CustoTotal, @Descricao, @CustoExtra, @Desconto)";
                            command.Parameters.AddWithValue("@IdCliente", atendimento.ClienteAtendido.Id);
                            command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                            command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                            command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                            command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                            command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                            command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);

                            int rowsaffected = command.ExecuteNonQuery();
                            atendimento.Id = Convert.ToInt32(command.LastInsertedId);

                            if (rowsaffected > 0)
                            {
                                foreach (Servico servic0 in atendimento.ServicosRealizados)
                                {
                                    MySqlCommand commandServico = new MySqlCommand("INSERT INTO ServicoAtendimento (idAtendimento, idServico) VALUES (@idAtendimento, @idServico)", conexao, transaction);
                                    commandServico.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                    commandServico.Parameters.AddWithValue("@idServico", servic0.Id);
                                    commandServico.ExecuteNonQuery();
                                }

                                foreach (Produtos produt0 in atendimento.ProdutosUsados)
                                {
                                    MySqlCommand commandProduto = new MySqlCommand("INSERT INTO AtendimentoProdutos (idAtendimento, idProdutos, Quantidade) VALUES (@idAtendimento, @idProdutos, @Quantidade)", conexao, transaction);
                                    commandProduto.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                    commandProduto.Parameters.AddWithValue("@idProdutos", produt0.Id);
                                    commandProduto.Parameters.AddWithValue("@Quantidade", produt0.Quantidade);
                                    commandProduto.ExecuteNonQuery();
                                }

                                MessageBox.Show("Atendimento cadastrado com sucesso.");
                                atendimentos.Add(atendimento);
                            }
                            else
                            {
                                MessageBox.Show("Falha ao adicionar o atendimento.");
                            }
                            break;
                        }

                    default:
                        MessageBox.Show("Tabela não reconhecida.");
                        break;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Ignorar erros de rollback
                }

                MessageBox.Show("Erro ao adicionar registro: " + ex.Message);
            }
            finally
            {
                CloseConexao();
            }
        }

        public static void Update(object schema)
        {
            InitConexao();
            MySqlCommand command = conexao.CreateCommand();
            MySqlTransaction transaction = null;

            try
            {
                transaction = conexao.BeginTransaction();
                command.Transaction = transaction;

                switch (schema)
                {
                    case Servico:
                        Servico servico = (Servico)schema;
                        command.CommandText = "UPDATE Servico SET Nome = @Nome, Preco = @Preco WHERE id = @id";
                        command.Parameters.AddWithValue("@Nome", servico.Nome);
                        command.Parameters.AddWithValue("@Preco", servico.Preco);
                        command.Parameters.AddWithValue("@id", servico.Id);

                        int rowsaffectedServico = command.ExecuteNonQuery();
                        if (rowsaffectedServico > 0)
                        {
                            MessageBox.Show("Serviço atualizado com sucesso.");
                            Servico servicoExistente = servicos.FirstOrDefault(s => s.Id == servico.Id);
                            if (servicoExistente != null)
                            {
                                servicoExistente.Nome = servico.Nome;
                                servicoExistente.Preco = servico.Preco;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao atualizar o serviço.");
                        }
                        break;

                    case Produtos:
                        Produtos produto = (Produtos)schema;
                        command.CommandText = "UPDATE Produtos SET Nome = @Nome, Preco = @Preco WHERE id = @id";
                        command.Parameters.AddWithValue("@Nome", produto.Nome);
                        command.Parameters.AddWithValue("@Preco", produto.Preco);
                        command.Parameters.AddWithValue("@id", produto.Id);

                        int rowsaffectedProduto = command.ExecuteNonQuery();
                        if (rowsaffectedProduto > 0)
                        {
                            MessageBox.Show("Produto atualizado com sucesso.");
                            Produtos produtoExistente = produtos.FirstOrDefault(p => p.Id == produto.Id);
                            if (produtoExistente != null)
                            {
                                produtoExistente.Nome = produto.Nome;
                                produtoExistente.Preco = produto.Preco;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao atualizar o produto.");
                        }
                        break;

                    case Cliente:
                        Cliente cliente = (Cliente)schema;
                        command.CommandText = "UPDATE Cliente SET Nome = @Nome, CPF = @CPF, Numero = @Numero, Email = @Email WHERE id = @id";
                        command.Parameters.AddWithValue("@Nome", cliente.Nome);
                        command.Parameters.AddWithValue("@CPF", cliente.CPF);
                        command.Parameters.AddWithValue("@Numero", cliente.Numero);
                        command.Parameters.AddWithValue("@Email", cliente.Email);
                        command.Parameters.AddWithValue("@id", cliente.Id);

                        int rowsaffectedCliente = command.ExecuteNonQuery();
                        if (rowsaffectedCliente > 0)
                        {
                            MessageBox.Show("Cliente atualizado com sucesso.");
                            Cliente clienteExistente = clientes.FirstOrDefault(c => c.Id == cliente.Id);
                            if (clienteExistente != null)
                            {
                                clienteExistente.Nome = cliente.Nome;
                                clienteExistente.CPF = cliente.CPF;
                                clienteExistente.Numero = cliente.Numero;
                                clienteExistente.Email = cliente.Email;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao atualizar o cliente.");
                        }
                        break;

                    case Atendimento:
                        Atendimento atendimento = (Atendimento)schema;
                        command.CommandText = "UPDATE Atendimento SET IdCliente = @IdCliente, DataInicio = @DataInicio, DataFim = @DataFim, CustoTotal = @CustoTotal, Descricao = @Descricao, CustoExtra = @CustoExtra, Desconto = @Desconto WHERE id = @id";
                        command.Parameters.AddWithValue("@IdCliente", atendimento.ClienteAtendido.Id);
                        command.Parameters.AddWithValue("@DataInicio", atendimento.DataInicio);
                        command.Parameters.AddWithValue("@DataFim", atendimento.DataFim);
                        command.Parameters.AddWithValue("@CustoTotal", atendimento.CustoTotal);
                        command.Parameters.AddWithValue("@Descricao", atendimento.Descricao);
                        command.Parameters.AddWithValue("@CustoExtra", atendimento.CustoExtra);
                        command.Parameters.AddWithValue("@Desconto", atendimento.Desconto);
                        command.Parameters.AddWithValue("@id", atendimento.Id);

                        int rowsaffectedAtendimento = command.ExecuteNonQuery();
                        if (rowsaffectedAtendimento > 0)
                        {
                            MySqlCommand commandDeleteServicos = new MySqlCommand("DELETE FROM ServicoAtendimento WHERE idAtendimento = @idAtendimento", conexao, transaction);
                            commandDeleteServicos.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                            commandDeleteServicos.ExecuteNonQuery();

                            foreach (Servico servic0 in atendimento.ServicosRealizados)
                            {
                                MySqlCommand commandServico = new MySqlCommand("INSERT INTO ServicoAtendimento (idAtendimento, idServico) VALUES (@idAtendimento, @idServico)", conexao, transaction);
                                commandServico.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                commandServico.Parameters.AddWithValue("@idServico", servic0.Id);
                                commandServico.ExecuteNonQuery();
                            }

                            MySqlCommand commandDeleteProdutos = new MySqlCommand("DELETE FROM AtendimentoProdutos WHERE idAtendimento = @idAtendimento", conexao, transaction);
                            commandDeleteProdutos.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                            commandDeleteProdutos.ExecuteNonQuery();

                            foreach (Produtos produt0 in atendimento.ProdutosUsados)
                            {
                                MySqlCommand commandProduto = new MySqlCommand("INSERT INTO AtendimentoProdutos (idAtendimento, idProdutos, Quantidade) VALUES (@idAtendimento, @idProdutos, @Quantidade)", conexao, transaction);
                                commandProduto.Parameters.AddWithValue("@idAtendimento", atendimento.Id);
                                commandProduto.Parameters.AddWithValue("@idProdutos", produt0.Id);
                                commandProduto.Parameters.AddWithValue("@Quantidade", produt0.Quantidade);
                                commandProduto.ExecuteNonQuery();
                            }

                            MessageBox.Show("Atendimento atualizado com sucesso.");
                            Atendimento atendimentoExistente = atendimentos.FirstOrDefault(a => a.Id == atendimento.Id);
                            if (atendimentoExistente != null)
                            {
                                atendimentoExistente.ClienteAtendido = atendimento.ClienteAtendido;
                                atendimentoExistente.DataInicio = atendimento.DataInicio;
                                atendimentoExistente.DataFim = atendimento.DataFim;
                                atendimentoExistente.CustoTotal = atendimento.CustoTotal;
                                atendimentoExistente.Descricao = atendimento.Descricao;
                                atendimentoExistente.CustoExtra = atendimento.CustoExtra;
                                atendimentoExistente.Desconto = atendimento.Desconto;
                                atendimentoExistente.ServicosRealizados = atendimento.ServicosRealizados;
                                atendimentoExistente.ProdutosUsados = atendimento.ProdutosUsados;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao atualizar o atendimento.");
                        }
                        break;

                    default:
                        MessageBox.Show("Tabela não reconhecida.");
                        break;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Ignorar erros de rollback
                }

                MessageBox.Show("Erro ao atualizar registro: " + ex.Message);
            }
            finally
            {
                CloseConexao();
            }
        }

        public static void Delete(string table, int id)
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
                        // Excluir referências em ServicoAtendimento
                        MySqlCommand commandDeleteServicoAtendimento = new MySqlCommand("DELETE FROM ServicoAtendimento WHERE idServico = @idServico", conexao, transaction);
                        commandDeleteServicoAtendimento.Parameters.AddWithValue("@idServico", servicos[id].Id);
                        commandDeleteServicoAtendimento.ExecuteNonQuery();

                        // Agora exclua o Servico
                        command.CommandText = "DELETE FROM Servico WHERE id = @id";
                        command.Parameters.AddWithValue("@id", servicos[id].Id);

                        int rowsaffectedServico = command.ExecuteNonQuery();
                        if (rowsaffectedServico > 0)
                        {
                            MessageBox.Show("Serviço excluído com sucesso.");
                            Servico servicoExistente = servicos.FirstOrDefault(s => s.Id == id);
                            if (servicoExistente != null)
                            {
                                servicos.Remove(servicoExistente);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o serviço.");
                        }
                        break;

                    case "produto":
                        // Excluir referências em AtendimentoProdutos
                        MySqlCommand commandDeleteAtendimentoProdutos = new MySqlCommand("DELETE FROM AtendimentoProdutos WHERE idProdutos = @idProduto", conexao, transaction);
                        commandDeleteAtendimentoProdutos.Parameters.AddWithValue("@idProduto", produtos[id].Id);
                        commandDeleteAtendimentoProdutos.ExecuteNonQuery();

                        // Agora exclua o Produto
                        command.CommandText = "DELETE FROM Produtos WHERE id = @id";
                        command.Parameters.AddWithValue("@id", produtos[id].Id);

                        int rowsaffectedProduto = command.ExecuteNonQuery();
                        if (rowsaffectedProduto > 0)
                        {
                            MessageBox.Show("Produto excluído com sucesso.");
                            Produtos produtoExistente = produtos.FirstOrDefault(p => p.Id == id);
                            if (produtoExistente != null)
                            {
                                produtos.Remove(produtoExistente);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o produto.");
                        }
                        break;

                    case "cliente":
                        // Excluir referências em Atendimento antes de excluir Cliente
                        MySqlCommand commandDeleteAtendimentos = new MySqlCommand("DELETE FROM Atendimento WHERE IdCliente = @idCliente", conexao, transaction);
                        commandDeleteAtendimentos.Parameters.AddWithValue("@idCliente", clientes[id].Id);
                        commandDeleteAtendimentos.ExecuteNonQuery();

                        MySqlCommand commandDeleteServicos1 = new MySqlCommand("DELETE FROM ServicoAtendimento WHERE IdCliente = @idCliente", conexao, transaction);
                        commandDeleteServicos1.Parameters.AddWithValue("@idCliente", clientes[id].Id);
                        commandDeleteServicos1.ExecuteNonQuery();

                        // Excluir referências em AtendimentoProdutos
                        MySqlCommand commandDeleteProdutos1 = new MySqlCommand("DELETE FROM AtendimentoProdutos WHERE IdCliente = @idCliente", conexao, transaction);
                        commandDeleteProdutos1.Parameters.AddWithValue("@idCliente", clientes[id].Id);
                        commandDeleteProdutos1.ExecuteNonQuery();

                        // Agora exclua o Cliente
                        command.CommandText = "DELETE FROM Cliente WHERE id = @id";
                        command.Parameters.AddWithValue("@id", clientes[id].Id);

                        int rowsaffectedCliente = command.ExecuteNonQuery();
                        if (rowsaffectedCliente > 0)
                        {
                            MessageBox.Show("Cliente excluído com sucesso.");
                            Cliente clienteExistente = clientes.FirstOrDefault(c => c.Id == id);
                            if (clienteExistente != null)
                            {
                                clientes.Remove(clienteExistente);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o cliente.");
                        }
                        break;

                    case "atendimento":
                        // Excluir referências em ServicoAtendimento
                        MySqlCommand commandDeleteServicos2 = new MySqlCommand("DELETE FROM ServicoAtendimento WHERE idAtendimento = @idAtendimento", conexao, transaction);
                        commandDeleteServicos2.Parameters.AddWithValue("@idAtendimento", atendimentos[id].Id);
                        commandDeleteServicos2.ExecuteNonQuery();

                        // Excluir referências em AtendimentoProdutos
                        MySqlCommand commandDeleteProdutos2 = new MySqlCommand("DELETE FROM AtendimentoProdutos WHERE idAtendimento = @idAtendimento", conexao, transaction);
                        commandDeleteProdutos2.Parameters.AddWithValue("@idAtendimento", atendimentos[id].Id);
                        commandDeleteProdutos2.ExecuteNonQuery();

                        // Agora exclua o Atendimento
                        command.CommandText = "DELETE FROM Atendimento WHERE id = @id";
                        command.Parameters.AddWithValue("@id", atendimentos[id].Id);

                        int rowsaffectedAtendimento = command.ExecuteNonQuery();
                        if (rowsaffectedAtendimento > 0)
                        {
                            MessageBox.Show("Atendimento excluído com sucesso.");
                            Atendimento atendimentoExistente = atendimentos.FirstOrDefault(a => a.Id == id);
                            if (atendimentoExistente != null)
                            {
                                atendimentos.Remove(atendimentoExistente);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir o atendimento.");
                        }
                        break;

                    default:
                        MessageBox.Show("Tabela não reconhecida.");
                        break;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    MessageBox.Show("Erro ao fazer rollback: " + rollbackEx.Message);
                }

                MessageBox.Show("Erro ao excluir registro: " + ex.Message);
            }
            finally
            {
                CloseConexao();
            }
        }
    }
}