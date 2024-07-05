using Repo;
namespace Model
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }
        public int? IdAtendimento { get; set; }

        public Produtos() { }
        public Produtos(string nome, double preco, int quantidade, int? idatendimento)
        {
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
            IdAtendimento = idatendimento;

            DB.Criar("produtos", this);
        }

        public static List<Produtos> Sincronizar()
        {
            return DB.Sincronizar();
        }

        public static List<Produtos> ListarProdutos()
        {
            return (List<Produtos>)DB.ListAll("produtos");
        }

        public static void AlterarProdutos(
            int indice,
            string nome,
            double preco,
            int quantidade
        )
        {
            DB.Update("produtos", indice, nome, preco, quantidade);
        }

        public static void DeletarProdutos(int indice)
        {
            DB.Delete("produtos", indice);
        }
    }
}