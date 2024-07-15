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

            CriarAsyncModel("servico", this);
        }
        public static async void CriarAsyncModel(string tabela, Servico servico)
        {
            await DB.CriarAsync(tabela, servico);
        }

        public static async void Sincronizar()
        {
            await DB.SincronizarAsync();
        }

        public static List<Servico> ListarServico()
        {
            return DB.ListAll<Servico>();
        }

        public static async void AlterarServico(Servico servico)
        {
            await DB.UpdateServicoAsync(servico);
        }

        public static async void DeletarServico(int indice)
        {
            await DB.DeletarAsync("servico", indice);
        }
    }
}