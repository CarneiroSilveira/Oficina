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

            DB.Criar(this);
        }

        public static void Sincronizar()
        {
            DB.Sincronizar();
        }

        public static List<Cliente> ListarCliente()
        {
            return DB.ListAll<Cliente>();
        }

        public static void AlterarCliente(Cliente cliente)
        {
            DB.Update(cliente);
        }

        public static void DeletarCliente(int indice)
        {
            DB.Delete("cliente", indice);
        }
    }
}