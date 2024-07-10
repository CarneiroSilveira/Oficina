namespace Views{
    public class ViewServico : Form{
        private readonly Label LabelTitulo;
        private readonly Button ButtonAdicionar;
        private readonly Button ButtonAlterar;
        private readonly Button ButtonDeletar;
        private readonly DataGridView ListaDeServicos;

        public ViewServico(){
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;
            
            LabelTitulo = new Label(){
                Text = "LISTA DE SERVIÇOS",
                Location = new Point(280, 150),
                Size =  new Size(500, 40),
                Font = new Font("Arial", 20)
            };
            ButtonAdicionar = new Button(){
                Text = "ADICIONAR",
                Location = new Point(50, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonAdicionar.Click += ClickAdicionar;

            ButtonAlterar = new Button(){
                Text = "ALTERAR",
                Location = new Point(333, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonAlterar.Click += ClickAlterar;

            ButtonDeletar = new Button(){
                Text = "DELETAR",
                Location = new Point(633, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonDeletar.Click += ClickDeletar;

            ListaDeServicos = new DataGridView(){
                Location = new Point(50, 200),
                Size = new Size(780, 320)
            };


            Controls.Add(LabelTitulo);
            Controls.Add(ButtonAdicionar);
            Controls.Add(ButtonAlterar);
            Controls.Add(ButtonDeletar);
            Controls.Add(ListaDeServicos);
        }
        private void ClickAdicionar(object? sender, EventArgs e){
            Hide();
            new ViewAdicionarServico(this).Show();
        }
        private void ClickAlterar(object? sender, EventArgs e){
            Hide();
            new ViewAlterarServico(this).Show();
        }
        private void ClickDeletar(object? sender, EventArgs e){
            Hide();
            new ViewDeletarServico(this).Show();
        }   
    }
}