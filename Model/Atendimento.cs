using Repo;
namespace Model
{
    public class ProdutoUsado
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
    public class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int? IdCliente { get; set; }
        public double CustoTotal { get; set; }
        public string Descricao { get; set; }
        public double? CustoExtra { get; set; }
        public double? Desconto { get; set; }
        public int IdProdutos { get; set; }
        public int IdServico { get; set; }
        public List<ProdutoUsado> ProdutosUsados { get; set; }


        public Atendimento() { }
        public Atendimento(DateTime datafim, double custototal, string descricao, double? custoextra, double? desconto, int? idcliente, int idservico, List<ProdutoUsado> produtosusados)
        {
            DataInicio = DateTime.Now;
            DataFim = datafim;
            CustoTotal = custototal;
            Descricao = descricao;
            CustoExtra = custoextra;
            Desconto = desconto;
            IdCliente = idcliente;
            IdServico = idservico;
            ProdutosUsados = produtosusados;

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

        public static void AlterarAtendimento(int id, DateTime dataFim, double custoTotal, string descricao, double? custoExtra, double? desconto, int idServico, List<ProdutoUsado> produtosUsados)
        {
            Atendimento atendimento = new Atendimento
            {
                Id = id,
                DataFim = dataFim,
                CustoTotal = custoTotal,
                Descricao = descricao,
                CustoExtra = custoExtra,
                Desconto = desconto,
                IdServico = idServico,
                ProdutosUsados = produtosUsados
            };

            DB.Update("atendimento", atendimento);
        }
        public static void DeletarAtendimento(int indice)
        {
            DB.Delete("atendimento", indice);
        }
    }
}