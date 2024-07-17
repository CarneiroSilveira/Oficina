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

            DB.Criar(this);
        }
        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Servico> ListarServico()
        {
            return DB.ListAll<Servico>();
        }

        public static void AlterarServico(Servico servico)
        {
            DB.Update(servico);
        }

        public static void DeletarServico(int indice)
        {
            DB.Delete("servico", indice);
        }
    }
}