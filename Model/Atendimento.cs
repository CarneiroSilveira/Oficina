using Repo;
namespace Model
{
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

        public Atendimento() { }
        public Atendimento(DateTime datafim, double custototal, string descricao, double? custoextra, double? desconto, int? idcliente)
        {
            DataInicio = DateTime.Now;
            DataFim = datafim;
            CustoTotal = custototal;
            Descricao = descricao;
            CustoExtra = custoextra;
            Desconto = desconto;
            IdCliente = idcliente;

            DB.Criar("atendimento", this);
        }

        public static List<Atendimento> Sincronizar()
        {
            return DB.Sincronizar();
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
            double? desconto

        )
        {
            DB.Update("atendimento", indice, datafim, custototal, descricao, custoextra, desconto);
        }

        public static void DeletarAtendimento(int indice)
        {
            DB.Delete("atendimento", indice);
        }
    }
}