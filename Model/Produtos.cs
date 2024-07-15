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

            CriarAsyncModel("produtos", this);
        }
        public static async void CriarAsyncModel(string tabela, Produtos produto)
        {
            await DB.CriarAsync(tabela, produto);
        }
        public static async void Sincronizar()
        {
            await DB.SincronizarAsync();
        }

        public static List<Produtos> ListarProdutos()
        {
            return (List<Produtos>)DB.ListAll("produtos");
        }

        public static async void AlterarProdutos(Produtos produto)
        {
            await DB.UpdateProdutoAsync(produto);
        }

        public static async void DeletarProdutos(int indice)
        {
            await DB.DeletarAsync("produtos", indice);
        }
    }
}