using Repo;
namespace Model
{
    public class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public double CustoTotal { get; set; }
        public string Descricao { get; set; }
        public double? CustoExtra { get; set; }
        public double? Desconto { get; set; }
        public int QuantidadeProduto { get; set; }
        public List<Servico> ServicosRealizados { get; set; }
        public List<Produtos> ProdutosUsados { get; set; }
        public Cliente ClienteAtendido { get; set; }

        public Atendimento() { }
        public Atendimento(DateTime datafim, double custototal, string descricao, double? custoextra, double? desconto, List<Servico> servicos, List<Produtos> produtos, Cliente cliente)
        {
            DataInicio = DateTime.Now;
            DataFim = datafim;
            CustoTotal = custototal;
            Descricao = descricao;
            CustoExtra = custoextra;
            Desconto = desconto;
            ServicosRealizados = servicos;
            ProdutosUsados = produtos;
            ClienteAtendido = cliente;


            DB.Criar("atendimento", this);
        }

        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Atendimento> ListarAtendimento()
        {
            return (List<Atendimento>)DB.ListAll("atendimento");
        }

        public static void AlterarAtendimento(
            int indice,
            DateTime datafim,
            double custototal,
            string descricao,
            double? custoextra,
            double? desconto,
            int quantidadeusada,
            List<Servico> servicos,
            List<Produtos> produtos,
            Cliente cliente

        )
        {
            // DB.Update("atendimento", indice, datafim, custototal, descricao, custoextra, desconto, idcliente, servicos, produtos);
        }
        public static void AlterarAtendimento(int id, DateTime dataFim, double custoTotal, string descricao, double? custoExtra, double? desconto, int quantidadeProduto, List<Servico> servicos, List<Produtos> produtos, Cliente cliente)
        {
            Atendimento atendimento = new Atendimento
            {
                Id = id,
                DataFim = dataFim,
                CustoTotal = custoTotal,
                Descricao = descricao,
                CustoExtra = custoExtra,
                Desconto = desconto,
                QuantidadeProduto = quantidadeProduto,
                ServicosRealizados = servicos,
                ProdutosUsados = produtos,
                ClienteAtendido = cliente
            };

            // DB.Update("atendimento", atendimento);
        }
        public static void DeletarAtendimento(int indice)
        {
            // DB.Delete("atendimento", indice);
        }
    }
}