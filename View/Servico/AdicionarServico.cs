
using Controller;

namespace Views
{
    public class ViewAdicionarServico : Form
    {
        private readonly Form ParentFormAdicionarServico;
        private readonly Label LabelNomeServico;
        private readonly Label LabelPreco;
        private readonly TextBox InputNomeServico;
        private readonly TextBox InputPreco;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;

        public event EventHandler ServicoAdicionado; // Evento para notificar adição de serviço

        public ViewAdicionarServico(Form parent)
        {
            ParentFormAdicionarServico = parent;

            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 320);

            LabelNomeServico = new Label()
            {
                Text = "NOME:",
                Location = new Point(100, 70),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelPreco = new Label()
            {
                Text = "PREÇO:",
                Location = new Point(100, 130),
                Font = new Font("Arial", 16),
                Size = new Size(120, 30)
            };
            InputNomeServico = new TextBox()
            {
                Name = "Nome Servico",
                Location = new Point(220, 72),
                Size = new Size(350, 40)
            };
            InputPreco = new TextBox()
            {
                Name = "Preco",
                Location = new Point(220, 132),
                Size = new Size(350, 40)
            };
            ButtonFechar = new Button()
            {
                Text = "FECHAR",
                Location = new Point(60, 200),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonFechar.Click += ClickFechar;

            ButtonConfirmar = new Button()
            {
                Text = "CONFIRMAR",
                Location = new Point(360, 200),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonConfirmar.Click += ClickConfirmar;

            Controls.Add(LabelNomeServico);
            Controls.Add(LabelPreco);
            Controls.Add(InputNomeServico);
            Controls.Add(InputPreco);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);
        }

        private void ClickFechar(object? sender, EventArgs e)
        {
            Close();
            ParentFormAdicionarServico.Show();
        }

        private void ClickConfirmar(object? sender, EventArgs e)
        {
            if (InputNomeServico.Text == "")
            {
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO SERVIÇO");
                return;
            }
            if (InputPreco.Text == "")
            {
                MessageBox.Show("O PREÇO ESTÁ VAZIO, COLOQUE O PREÇO DO SERVIÇO");
                return;
            }

            ControllerServico.CriarServico(InputNomeServico.Text, Convert.ToDouble(InputPreco.Text));
            ServicoAdicionado?.Invoke(this, EventArgs.Empty); // Disparar evento de serviço adicionado
            Close();
            ParentFormAdicionarServico.Show();
        }
    }
}
