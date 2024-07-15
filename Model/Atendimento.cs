using Repo;
namespace Model
{
    public class ProdutosUsados
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
    }
    public class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public double CustoTotal { get; set; }
        public string Descricao { get; set; }
        public double? CustoExtra { get; set; }
        public double? Desconto { get; set; }
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


            CriarAsyncModel("atendimento", this);
        }
        public static async void CriarAsyncModel(string tabela, Atendimento atendimento)
        {
            await DB.CriarAsync(tabela, atendimento);
        }

        public static async void Sincronizar()
        {
            await DB.SincronizarAsync();
        }

        public static List<Atendimento> ListarAtendimento()
        {
            return (List<Atendimento>)DB.ListAll("atendimento");
        }

        public static async void AlterarAtendimento(Atendimento atendimento)
        {
            await DB.UpdateAtendimentoAsync(atendimento);
        }
        public static async void DeletarAtendimento(int indice)
        {
            await DB.DeletarAsync("atendimento", indice);
        }
    }
}