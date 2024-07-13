using Repo;
namespace Model
{
    public class Servico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }


        public Servico() { }
        public Servico(string nome, double preco)
        {
            Nome = nome;
            Preco = preco;

            DB.Criar("servico", this);
        }

        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Servico> ListarServico()
        {
            return (List<Servico>)DB.ListAll("servico");
        }

        public static void AlterarServico(
            int indice,
            string nome,
            double preco
        )
        {
            //DB.Update("servico", indice, nome, preco);
        }

        public static void DeletarServico(int indice)
        {
            DB.Delete("servico", indice);
        }
    }
}