using Repo;
namespace Model
{
    public class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public double CustoTotal { get; set; }
        public string? Descricao { get; set; }
        public double? CustoExtra { get; set; }
        public double? Desconto { get; set; }
        public List<Servico> ServicosRealizados { get; set; }
        public List<Produtos> ProdutosUsados { get; set; }
        public Cliente ClienteAtendido { get; set; }

        public Atendimento() { }
        public Atendimento(DateTime datafim, double custototal, string? descricao, double? custoextra, double? desconto, List<Servico> servicos, List<Produtos> produtos, Cliente cliente)
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
        }
        public static void CriarAtendimento(Atendimento atendimento)
        {
            DB.Criar(atendimento);
        }
        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Atendimento> ListarAtendimento()
        {
            return DB.ListAll<Atendimento>();
        }

        public static void AlterarAtendimento(int indice ,Atendimento atendimento)
        {
            DB.Update(indice, atendimento);
        }
        public static void DeletarAtendimento(int indice)
        {
            DB.Delete("atendimento", indice);
        }
    }
}