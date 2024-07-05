using Model;

namespace Controller {
    public class ControllerCliente {
    
        public static List<Cliente> Sincronizar() {
            return Cliente.Sincronizar();
        }

        public static void CriarCliente(string nome, bool? clientenovo, string contato, string? email) {
            new Cliente(nome, clientenovo, contato, email);
        }

        public static List<Cliente> ListarCliente() {
            return Cliente.ListarCliente();
        }

        public static void AlterarCliente(int indice, string nome, bool? clientenovo, string contato, string? email) {
            List<Cliente> clientes = ListarCliente();
            if(indice > 0 && indice < clientes.Count) {
                Cliente.AlterarCliente(indice, nome, clientenovo, contato, email);
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