using Model;

namespace Controller {
    public class ControllerServico {
    
        public static void Sincronizar() {
            Servico.Sincronizar();
        }

        public static void CriarServico(string nome, double preco) {
            new Servico(nome, preco);
        }

        public static List<Servico> ListarServico() {
            return Servico.ListarServico();
        }

        public static void AlterarServico(int indice, string nome, double preco) {
            List<Servico> servicos = ListarServico();
            if(indice > 0 && indice < servicos.Count) {
                Servico.AlterarServico(indice, nome, preco);
                Console.WriteLine("Serviço alterado");
            } else {
                Console.WriteLine("Indice inválido");
            }
        } 

        public static void DeletarServico(int indice) {
            List<Servico> servicos = ListarServico();

            if (indice >= 0 && indice < servicos.Count) {{
                Servico.DeletarServico(indice);
            }} else {
                Console.WriteLine("Indice inválido");
            }
        } 
    }
}