using Model;

namespace Controller {
    public class ControllerAtendimento {
    
        public static List<Atendimento> Sincronizar() {
            return Atendimento.Sincronizar();
        }

        public static void CriarAtendimento(DateOnly datafim, double custototal, string descricao) {
            new Atendimento(datafim, custototal, descricao);
        }

        public static List<Atendimento> ListarAtendimento() {
            return Atendimento.ListarAtendimento();
        }

        public static void AlterarAtendimento(int indice, DateOnly datafim, double custototal, string descricao) {
            List<Atendimento> atendimentos = ListarAtendimento();
            if(indice > 0 && indice < atendimentos.Count) {
                Atendimento.AlterarAtendimento(indice, datafim, custototal, descricao);
                Console.WriteLine("Atendimento alterado");
            } else {
                Console.WriteLine("Indice inválido");
            }
        } 

        public static void DeletarAtendimento(int indice) {
            List<Atendimento> atendimentos = ListarAtendimento();

            if (indice >= 0 && indice < atendimentos.Count) {{
                Atendimento.DeletarAtendimento(indice);
            }} else {
                Console.WriteLine("Indice inválido");
            }
        } 
    }
}