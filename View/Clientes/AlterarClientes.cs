namespace Views{
    public class ViewAlterarClientes : Form{
        private readonly Form ParentFormAlterarClientes;
        private readonly Label LabelIndice;
        private readonly Label LabelNomeCliente;
        private readonly Label LabelTelefone;
        private readonly Label LabelCPF;
        private readonly Label LabelEmail;
        private readonly TextBox InputIndice;
        private readonly TextBox InputNomeCliente;
        private readonly TextBox InputTelefone;
        private readonly TextBox InputCPF;
        private readonly TextBox InputEmail;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;

        public ViewAlterarClientes(Form parent){
            ParentFormAlterarClientes = parent;
            
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 500);

            LabelIndice = new Label(){
                Text = "ÍNDICE:",
                Location = new Point(100, 25),
                Font = new Font("Arial", 16),
                Size = new Size(115, 30)
            };
            LabelNomeCliente = new Label(){
                Text = "NOME:",
                Location = new Point(100, 85),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelTelefone = new Label(){
                Text = "FONE:",
                Location = new Point(100, 145),
                Font = new Font("Arial", 16),
                Size = new Size(105, 30)
            };
            LabelCPF = new Label(){
                Text = "CPF:",
                Location = new Point(100, 205),
                Font = new Font("Arial", 16),
                Size = new Size(95, 30) 
            };
            LabelEmail = new Label(){
                Text = "EMAIL:",
                Location = new Point(100, 265),
                Font = new Font("Arial", 16),
                Size = new Size(110, 30)
            };
            InputIndice = new TextBox(){
                Name = "Indice",
                Location = new Point(220, 27),
                Size = new Size(350, 40)
            };
            InputNomeCliente = new TextBox(){
                Name = "Nome cliente",
                Location = new Point(220, 87),
                Size = new Size(350, 40)
            };
            InputTelefone = new TextBox(){
                Name = "Fone",
                Location = new Point(220, 147),
                Size = new Size(350, 40)
            };
            InputCPF = new TextBox(){
                Name = "CPF",
                Location = new Point(220, 207),
                Size = new Size(350, 40)
            };
            InputEmail = new TextBox(){
                Name = "Email",
                Location = new Point(220, 267),
                Size = new Size(350, 40)
            };
            ButtonFechar = new Button(){
                Text = "FECHAR",
                Location = new Point(60, 345),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonFechar.Click += ClickFechar;

            ButtonConfirmar = new Button(){
                Text = "CONFIRMAR",
                Location = new Point(360, 345),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonConfirmar.Click += ClickConfirmar;

            Controls.Add(LabelIndice);
            Controls.Add(LabelNomeCliente);
            Controls.Add(LabelTelefone);
            Controls.Add(LabelCPF);
            Controls.Add(LabelEmail);
            Controls.Add(InputIndice);
            Controls.Add(InputNomeCliente);
            Controls.Add(InputTelefone);
            Controls.Add(InputCPF);
            Controls.Add(InputEmail);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);           
        }
        private void ClickFechar(object? sender, EventArgs e){
            Close();
            ParentFormAlterarClientes.Show();
        }
        private void ClickConfirmar(object? sender, EventArgs e){
            if (InputIndice.Text == ""){
                MessageBox.Show("O ÍNDICE ESTÁ VAZIO, COLOQUE O ÍNDICE DA TABELA");
                return;
            }
            if (InputNomeCliente.Text == ""){
                MessageBox.Show("O NOME ESTÁ VAZIO, COLOQUE O NOME DO CLIENTE");
                return;
            }
            if (InputTelefone.Text == ""){
                MessageBox.Show("O TELEFONE ESTÁ VAZIO, COLOQUE O TELEFONE DO CLIENTE");
                return;
            }
            if (InputCPF.Text == ""){
                MessageBox.Show("O CPF ESTÁ VAZIO, COLOQUE O CPF DO CLIENTE");
                return;
            }
            if (InputEmail.Text == ""){
                MessageBox.Show("O EMAIL ESTÁ VAZIO, COLOQUE O EMAIL DO CLIENTE");
                return;
            }
        }
    }
}