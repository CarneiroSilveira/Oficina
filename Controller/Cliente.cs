using Model;

namespace Controller {
    public class ControllerCliente {
    
        public static void Sincronizar() {
            Cliente.Sincronizar();
        }

        public static void CriarCliente(string nome, string numero, string? cpf, string? email) {
            new Cliente(nome, numero, cpf, email);
        }

        public static List<Cliente> ListarCliente() {
            return Cliente.ListarCliente();
        }

        public static void AlterarCliente(Cliente cliente) {
            List<Cliente> clientes = ListarCliente();
            if(cliente.Id > 0) {
                Cliente.AlterarCliente(cliente);
                Console.WriteLine("Cliente alterado");
            } else {
                Console.WriteLine("Indice inválido");
            }
        } 

        public static void DeletarCliente(int indice) {
            List<Cliente> clientes = ListarCliente();

            if (indice >= 0 && indice < clientes.Count) {{
                Cliente.DeletarCliente(indice);
            }} else {
                Console.WriteLine("Indice inválido");
            }
        } 
    }
}