using Model;

namespace Controller {
    public class ControllerServico {
    
        public static List<Servico> Sincronizar() {
            return Servico.Sincronizar();
        }

        public static void CriarServico(string nome, double preco, double extra, double desconto) {
            new Servico(nome, preco, extra, desconto);
        }

        public static List<Servico> ListarServico() {
            return Servico.ListarServico();
        }

        public static void AlterarServico(int indice, string nome, double preco, double extra, double desconto) {
            List<Servico> servicos = ListarServico();
            if(indice > 0 && indice < servicos.Count) {
                Servico.AlterarServico(indice, nome, preco, extra, desconto);
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