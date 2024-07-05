using Model;

namespace Controller {
    public class ControllerProdutos {
    
        public static List<Produtos> Sincronizar() {
            return Produtos.Sincronizar();
        }

        public static void CriarProduto(string nome, double preco, int quantidade) {
            new Produtos(nome, preco, quantidade);
        }

        public static List<Produtos> ListarProdutos() {
            return Produtos.ListarProdutos();
        }

        public static void AlterarProdutos(int indice, string nome, double preco, int quantidade) {
            List<Produtos> produtos = ListarProdutos();
            if(indice > 0 && indice < produtos.Count) {
                Produtos.AlterarProdutos(indice, nome, preco, quantidade);
                Console.WriteLine("Produto alterado");
            } else {
                Console.WriteLine("Indice inválido");
            }
        } 

        public static void DeletarProdutos(int indice) {
            List<Produtos> produtos = ListarProdutos();

            if (indice >= 0 && indice < produtos.Count) {{
                Produtos.DeletarProdutos(indice);
            }} else {
                Console.WriteLine("Indice inválido");
            }
        } 
    }
}