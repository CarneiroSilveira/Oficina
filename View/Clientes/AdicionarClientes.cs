using Controller;

namespace Views
{
    public class ViewAdicionarClientes : Form
    {
        private readonly Form ParentFormAdicionarClientes;
        private readonly Label LabelNomeCliente;
        private readonly Label LabelTelefone;
        private readonly Label LabelCPF;
        private readonly Label LabelEmail;
        private readonly TextBox InputNomeCliente;
        private readonly TextBox InputTelefone;
        private readonly TextBox InputCPF;
        private readonly TextBox InputEmail;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;
        public event EventHandler clienteAdicionado; // Evento para notificar adição de cliente
        public ViewAdicionarClientes(Form parent)
        {
            ParentFormAdicionarClientes = parent;

            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 500);

            LabelNomeCliente = new Label()
            {
                Text = "NOME:",
                Location = new Point(100, 70),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelTelefone = new Label()
            {
                Text = "FONE:",
                Location = new Point(100, 130),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelCPF = new Label()
            {
                Text = "CPF:",
                Location = new Point(100, 190),
                Font = new Font("Arial", 16),
                Size = new Size(95, 30)
            };
            LabelEmail = new Label()
            {
                Text = "EMAIL:",
                Location = new Point(100, 250),
                Font = new Font("Arial", 16),
                Size = new Size(110, 30)
            };
            InputNomeCliente = new TextBox()
            {
                Name = "Nome cliente",
                Location = new Point(220, 72),
                Size = new Size(350, 40)
            };
            InputTelefone = new TextBox()
            {
                Name = "Fone",
                Location = new Point(220, 132),
                Size = new Size(350, 40)
            };
            InputCPF = new TextBox()
            {
                Name = "CPF",
                Location = new Point(220, 192),
                Size = new Size(350, 40)
            };
            InputEmail = new TextBox()
            {
                Name = "Email",
                Location = new Point(220, 252),
                Size = new Size(350, 40)
            };
            ButtonFechar = new Button()
            {
                Text = "FECHAR",
                Location = new Point(60, 330),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonFechar.Click += ClickFechar;

            ButtonConfirmar = new Button()
            {
                Text = "CONFIRMAR",
                Location = new Point(360, 330),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonConfirmar.Click += ClickConfirmar;

            Controls.Add(LabelNomeCliente);
            Controls.Add(LabelTelefone);
            Controls.Add(LabelCPF);
            Controls.Add(LabelEmail);
            Controls.Add(InputNomeCliente);
            Controls.Add(InputTelefone);
            Controls.Add(InputCPF);
            Controls.Add(InputEmail);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);
        }
        private void ClickFechar(object? sender, EventArgs e)
        {
            Close();
            ParentFormAdicionarClientes.Show();
        }
        private void ClickConfirmar(object? sender, EventArgs e)
        {
            if (InputNomeCliente.Text == "")
            {
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO CLIENTE");
                return;
            }
            if (InputTelefone.Text == "")
            {
                MessageBox.Show("O TELEFONE ESTÁ VAZIO, COLOQUE O TELEFONE DO CLIENTE");
                return;
            }
            ControllerCliente.CriarCliente(InputNomeCliente.Text, InputTelefone.Text, InputCPF.Text, InputEmail.Text);
            clienteAdicionado?.Invoke(this, EventArgs.Empty); // Disparar evento de cliente adicionado
            Close();
            ParentFormAdicionarClientes.Show();
        }
    }
}