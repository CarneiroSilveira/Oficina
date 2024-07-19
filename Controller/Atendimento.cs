using Model;

namespace Controller
{
    public class ControllerAtendimento
    {

        public static void Sincronizar()
        {
            Atendimento.Sincronizar();
        }

        public static void CriarAtendimento(DateTime datafim, double custototal, string? descricao, double? custoExtra, double? desconto, List<Servico> servicos, List<Produtos> produtos, Cliente cliente)
        {
            Atendimento atendimento = new Atendimento(datafim, custototal, descricao, custoExtra, desconto, servicos, produtos, cliente);
            Atendimento.CriarAtendimento(atendimento);
        }

        public static List<Atendimento> ListarAtendimento()
        {
            return Atendimento.ListarAtendimento();
        }

        public static void AlterarAtendimento(int indice, DateTime datafim, double custototal, string? descricao, double? custoextra, double? desconto, List<Servico> servicos, List<Produtos> produtos, Cliente cliente)
        {
            Atendimento atendimento = new Atendimento(datafim, custototal, descricao, custoextra, desconto, servicos, produtos, cliente)
            {
                Id = indice + 1
            };
            if (atendimento.Id > 0)
            {
                Atendimento.AlterarAtendimento(atendimento);
                Console.WriteLine("Atendimento alterado");
            }
            else
            {
                Console.WriteLine("Indice inválido");
            }
        }

        public static void DeletarAtendimento(int indice)
        {
            List<Atendimento> atendimentos = ListarAtendimento();

            if (indice >= 0 && indice < atendimentos.Count)
            {
                {
                    Atendimento.DeletarAtendimento(indice);
                }
            }
            else
            {
                Console.WriteLine("Indice inválido");
            }
        }
    }
}