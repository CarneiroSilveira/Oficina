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
        }
        public static void CriarProduto(Produtos produto)
        {
            DB.Criar(produto);
        }
        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Produtos> ListarProdutos()
        {
            return DB.ListAll<Produtos>();
        }

        public static void AlterarProdutos(int indice, Produtos produto)
        {
            DB.Update(indice, produto);
        }

        public static void DeletarProdutos(int indice)
        {
            DB.Delete("produto", indice);
        }
    }
}