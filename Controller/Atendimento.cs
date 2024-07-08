using Model;

namespace Controller {
    public class ControllerAtendimento {
    
        public static List<Atendimento> Sincronizar() {
            return Atendimento.Sincronizar();
        }

        public static void CriarAtendimento(DateTime datafim, double custototal, string descricao, double? custoExtra, double? desconto, int? idCliente, List<Servico> servicos, List<ProdutoUsado> produtos) {
            new Atendimento(datafim, custototal, descricao, custoExtra, desconto, idCliente, servicos, produtos);
        }

        public static List<Atendimento> ListarAtendimento() {
            return Atendimento.ListarAtendimento();
        }

        public static void AlterarAtendimento(int indice, DateTime datafim, double custototal, string descricao, double? custoExtra, double? desconto, int? idCliente, List<Servico> servicos, List<ProdutoUsado> produtos) {
            List<Atendimento> atendimentos = ListarAtendimento();
            if(indice > 0 && indice < atendimentos.Count) {
                Atendimento.AlterarAtendimento(indice, datafim, custototal, descricao, custoExtra, desconto, idCliente, servicos, produtos);
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