using Repo;
namespace Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool ClienteNovo { get; set; } = true;
        public string Contato { get; set; }
        public string? Email { get; set; }

        public Cliente(string nome, bool clientenovo, string contato, string? email)
        {
            Nome = nome;
            ClienteNovo = clientenovo;
            Contato = contato;
            Email = email;

            DB.Criar(this);
        }

        public static List<Cliente> Sincronizar()
        {
            return DB.Sincronizar();
        }

        public static List<Cliente> ListarCliente()
        {
            return (List<Cliente>)DB.ListAll("cliente");
        }

        public static void AlterarCliente(
            int indice,
            string nome,
            bool clientenovo,
            string contato,
            string? email
        )
        {
            DB.Update("cliente", indice, nome, clientenovo, contato, email);
        }

        public static void DeletarCliente(int indice)
        {
            DB.Delete("cliente", indice);
        }
    }
}