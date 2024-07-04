using Repo;
namespace Model
{
    public class Atendimento
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public double CustoTotal { get; set; }
        public string Descricao { get; set; }
        // public double? CustoExtra { get; set; }
        // public double? Desconto { get; set; }

        public Atendimento() { }
        public Atendimento(DateOnly datafim, double custototal, string descricao /*, double? custoextra, double? desconto*/)
        {
            DataInicio = DateTime.Now;
            DataFim = datafim;
            CustoTotal = custototal;
            Descricao = descricao;
            // CustoExtra = custoextra;
            // Desconto = desconto;

            DB.Criar(this);
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
            DateOnly datafim,
            double custototal,
            string descricao
        )
        {
            DB.Update("atendimento", indice, datafim, custototal, descricao);
        }

        public static void DeletarAtendimento(int indice)
        {
            DB.Delete("atendimento", indice);
        }
    }
}