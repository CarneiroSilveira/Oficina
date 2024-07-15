using Repo;
namespace Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }

        public Cliente() { }
        public Cliente(string nome, string numero, string? cpf, string? email)
        {
            Nome = nome;
            Numero = numero;
            CPF = cpf;
            Email = email;

            CriarAsyncModel("cliente", this);
        }
        public static async void CriarAsyncModel(string tabela, Cliente cliente)
        {
            await DB.CriarAsync(tabela, cliente);
        }

        public static async void Sincronizar()
        {
            await DB.SincronizarAsync();
        }

        public static List<Cliente> ListarCliente()
        {
            return DB.ListAll<Cliente>();
        }

        public static async void AlterarCliente(Cliente cliente)
        {
            await DB.UpdateClienteAsync(cliente);
        }

        public static async void DeletarCliente(int indice)
        {
            await DB.DeletarAsync("cliente", indice);
        }
    }
}