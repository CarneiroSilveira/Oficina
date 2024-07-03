using Repo;
namespace Model
{
    public class Servico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public double? CustoExtra { get; set; }
        public double? Desconto { get; set; }

        public Servico() { }
        public Servico(string nome, double preco, double? custoextra, double? desconto)
        {
            Nome = nome;
            Preco = preco;
            CustoExtra = custoextra;
            Desconto = desconto;

            DB.Criar(this);
        }

        public static List<Servico> Sincronizar()
        {
            return DB.Sincronizar();
        }

        public static List<Servico> ListarServico()
        {
            return (List<Servico>)DB.ListAll("servico");
        }

        public static void AlterarServico(
            int indice,
            string nome,
            double preco,
            double? custoextra,
            double? desconto
        )
        {
            DB.Update("servico", indice, nome, preco, custoextra, desconto);
        }

        public static void DeletarServico(int indice)
        {
            DB.Delete("servico", indice);
        }
    }
}