using Controller;

namespace Views
{
    public class ViewAdicionarProdutos : Form
    {
        private readonly Form ParentFormAdicionarProduto;
        private readonly Label LabelNomeProduto;
        private readonly Label LabelPreco;
        private readonly TextBox InputNomeProduto;
        private readonly TextBox InputPreco;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;
        public event EventHandler produtoAdicionado; // Evento para notificar adição de produtos
        public ViewAdicionarProdutos(Form parent)
        {
            ParentFormAdicionarProduto = parent;

            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 320);

            LabelNomeProduto = new Label()
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
            InputNomeProduto = new TextBox()
            {
                Name = "Nome Produto",
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

            Controls.Add(LabelNomeProduto);
            Controls.Add(LabelPreco);
            Controls.Add(InputNomeProduto);
            Controls.Add(InputPreco);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);
        }

        private void ClickFechar(object? sender, EventArgs e)
        {
            Close();
            ParentFormAdicionarProduto.Show();
        }

        private void ClickConfirmar(object? sender, EventArgs e)
        {
            if (InputNomeProduto.Text == "")
            {
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO PRODUTO");
                return;
            }
            if (InputPreco.Text == "")
            {
                MessageBox.Show("O PREÇO ESTÁ VAZIO, COLOQUE O PREÇO DO PRODUTO");
                return;
            }
            ControllerProdutos.CriarProduto(InputNomeProduto.Text, Convert.ToDouble(InputPreco.Text));
            produtoAdicionado?.Invoke(this, EventArgs.Empty); // Disparar evento de produto adicionado
            Close();
            ParentFormAdicionarProduto.Show();
        }
    }
}
