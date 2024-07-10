namespace Views{
    public class ViewDeletarServico : Form{
        private readonly Form ParentFormDeletarServico;
        private readonly Label LabelIndice;
        private readonly TextBox InputIndice;
        private readonly Button ButtonFechar;
        private readonly Button ButtonConfirmar;

        public ViewDeletarServico(Form parent){
            ParentFormDeletarServico = parent;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(700, 250);

            LabelIndice = new Label(){
                Text = "ÍNDICE:",
                Location = new Point(100, 50),
                Font = new Font("Arial", 16),
                Size = new Size(115, 30)
            };
            InputIndice = new TextBox(){
                Name = "Indice",
                Location = new Point(220, 52),
                Size = new Size(350, 40)
            };
            ButtonFechar = new Button(){
                Text = "FECHAR",
                Location = new Point(60, 120),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonFechar.Click += ClickFechar;

            ButtonConfirmar = new Button(){
                Text = "CONFIRMAR",
                Location = new Point(360, 120),
                Font = new Font("Arial", 20),
                Size = new Size(250, 60)
            };
            ButtonConfirmar.Click += ClickConfirmar;

            Controls.Add(LabelIndice);
            Controls.Add(InputIndice);
            Controls.Add(ButtonFechar);
            Controls.Add(ButtonConfirmar);
        }
        private void ClickFechar(object? sender, EventArgs e){
            Close();
            ParentFormDeletarServico.Show();
        }
        private void ClickConfirmar(object? sender, EventArgs e){
            if (InputIndice.Text == ""){
                MessageBox.Show("O ÍNDICE ESTÁ VAZIO, COLOQUE O ÍNDICE DA TABELA");
                return;
            }
        }
    }
}