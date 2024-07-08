using Repo;
namespace Model
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }

        public Produtos() { }
        public Produtos(string nome, double preco)
        {
            Nome = nome;
            Preco = preco;

            DB.Criar("produtos", this);
        }

        public static void Sincronizar()
        {
            DB.Sincronizar();
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