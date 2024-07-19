using Controller;

namespace Views
{
    public class ViewAlterarServico : Form
    {
        private readonly Form ParentFormAlterarServico;
        private readonly Label LabelIndice;
        private readonly Label LabelNomeServico;
        private readonly Label LabelPreco;
        private readonly TextBox InputIndice;
        private readonly TextBox InputNomeServico;
        private readonly TextBox InputPreco;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;
        public event EventHandler ServicoAlterado; // Evento para notificar edição de serviço
        public ViewAlterarServico(Form parent)
        {
            ParentFormAlterarServico = parent;

            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 320);

            LabelIndice = new Label()
            {
                Text = "ÍNDICE:",
                Location = new Point(100, 25),
                Font = new Font("Arial", 16),
                Size = new Size(115, 30)
            };
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
                Location = new Point(100, 115),
                Font = new Font("Arial", 16),
                Size = new Size(120, 30)
            };
            InputIndice = new TextBox()
            {
                Name = "Indice",
                Location = new Point(220, 27),
                Size = new Size(350, 40)
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
                Location = new Point(220, 117),
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

            Controls.Add(LabelIndice);
            Controls.Add(LabelNomeServico);
            Controls.Add(LabelPreco);
            Controls.Add(InputIndice);
            Controls.Add(InputNomeServico);
            Controls.Add(InputPreco);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);
        }
        private void ClickFechar(object? sender, EventArgs e)
        {
            Close();
            ParentFormAlterarServico.Show();
        }
        private void ClickConfirmar(object? sender, EventArgs e)
        {
            if (InputIndice.Text == "")
            {
                MessageBox.Show("O ÍNDICE ESTÁ VAZIO, COLOQUE O ÍNDICE DA TABELA");
                return;
            }
            if (InputNomeServico.Text == "")
            {
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO SERVIÇO");
                return;
            }
            if (InputPreco.Text == "")
            {
                MessageBox.Show("O TELEFONE ESTÁ VAZIO, COLOQUE O PREÇO DO SERVIÇO");
                return;
            }
            ControllerServico.AlterarServico(int.Parse(InputIndice.Text), InputNomeServico.Text, Convert.ToDouble(InputPreco.Text));
            ServicoAlterado?.Invoke(this, EventArgs.Empty); // Disparar evento de serviço alterado
            Close();
            ParentFormAlterarServico.Show();

        }
    }
}