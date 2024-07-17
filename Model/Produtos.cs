using Repo;
namespace Model
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int? Quantidade { get; set; }

        public Produtos() { }
        public Produtos(string nome, double preco)
        {
            Nome = nome;
            Preco = preco;

            DB.Criar(this);
        }
        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Produtos> ListarProdutos()
        {
            return DB.ListAll<Produtos>();
        }

        public static void AlterarProdutos(Produtos produto)
        {
            DB.Update(produto);
        }

        public static void DeletarProdutos(int indice)
        {
            DB.Delete("produto", indice);
        }
    }
}