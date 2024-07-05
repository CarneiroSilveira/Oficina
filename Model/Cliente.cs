using Repo;
namespace Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool? ClienteNovo { get; set; } = true;
        public string Numero { get; set; }
        public string? Email { get; set; }

        public Cliente() { }
        public Cliente(string nome, string numero, string? email, bool? clientenovo)
        {
            Nome = nome;
            ClienteNovo = clientenovo;
            Numero = numero;
            Email = email;

            DB.Criar("cliente", this);
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
            string numero,
            string? email,
            bool clientenovo
        )
        {
            DB.Update("cliente", indice, nome, numero, email, clientenovo);
        }

        public static void DeletarCliente(int indice)
        {
            DB.Delete("cliente", indice);
        }
    }
}