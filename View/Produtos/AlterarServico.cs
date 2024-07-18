using Controller;

namespace Views{
    public class ViewAlterarProdutos : Form{
        private readonly Form ParentFormAlterarProduto;
        private readonly Label LabelIndice;
        private readonly Label LabelNomeProduto;
        private readonly Label LabelPreco;
        private readonly TextBox InputIndice;
        private readonly TextBox InputNomeProduto;
        private readonly TextBox InputPreco;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;
        public event EventHandler produtoAlterado; // Evento para notificar edição produto

        public ViewAlterarProdutos(Form parent){
            ParentFormAlterarProduto = parent;
            
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 320);

            LabelIndice = new Label(){
                Text = "ÍNDICE:",
                Location = new Point(100, 25),
                Font = new Font("Arial", 16),
                Size = new Size(115, 30)
            };
            LabelNomeProduto = new Label(){
                Text = "NOME:",
                Location = new Point(100, 70),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelPreco = new Label(){
                Text = "PREÇO:",
                Location = new Point(100, 115),
                Font = new Font("Arial", 16),
                Size = new Size(120, 30)
            };
            InputIndice = new TextBox(){
                Name = "Indice",
                Location = new Point(220, 27),
                Size = new Size(350, 40)
            };
            InputNomeProduto = new TextBox(){
                Name = "Nome Servico",
                Location = new Point(220, 72),
                Size = new Size(350, 40)
            };
            InputPreco = new TextBox(){
                Name = "Preco",
                Location = new Point(220, 117),
                Size = new Size(350, 40)
            };
            ButtonFechar = new Button(){
                Text = "FECHAR",
                Location = new Point(60, 200),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonFechar.Click += ClickFechar;

            ButtonConfirmar = new Button(){
                Text = "CONFIRMAR",
                Location = new Point(360, 200),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonConfirmar.Click += ClickConfirmar;

            Controls.Add(LabelIndice);
            Controls.Add(LabelNomeProduto);
            Controls.Add(LabelPreco);
            Controls.Add(InputIndice);
            Controls.Add(InputNomeProduto);
            Controls.Add(InputPreco);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);           
        }
        private void ClickFechar(object? sender, EventArgs e){
            Close();
            ParentFormAlterarProduto.Show();
        }
        private void ClickConfirmar(object? sender, EventArgs e){

            if (InputIndice.Text == ""){
                MessageBox.Show("O ÍNDICE ESTÁ VAZIO, COLOQUE O ÍNDICE DA TABELA");
                return;
            }
            if (InputNomeProduto.Text == ""){
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO PRODUTO");
                return;
            }
            if (InputPreco.Text == ""){
                MessageBox.Show("O VALOR ESTÁ VAZIO, COLOQUE O VALOR DO PRODUTO");
                return;
            }
            //ControllerProdutos.AlterarProdutos(InputIndice.Text, InputNomeProduto.Text, Convert.ToDouble(InputPreco.Text));
            produtoAlterado?.Invoke(this, EventArgs.Empty); // Disparar evento de produto alterado
            Close();
            ParentFormAlterarProduto.Show();
        }
    }
}