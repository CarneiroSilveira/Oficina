namespace Views{
    public class ViewMenu : Form{
        private readonly Button ButtonClientes;
        private readonly Button ButtonServico;
        private readonly Button ButtonProdutos;
        private readonly Button ButtonAtendimentos;
        private readonly Button ButtonSair;

        public ViewMenu(){
            Size = new Size(800, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.DimGray;

            ButtonClientes = new Button(){
                Text = "Clientes",
                Location = new Point(225, 252),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
                BackColor = Color.GhostWhite
            };
            ButtonClientes.Click += ClickEntrarClientes;
            ButtonServico = new Button(){
                Text = "Serviços",
                Location = new Point(225, 322),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
                BackColor = Color.GhostWhite
            };
            ButtonServico.Click += ClickEntrarServicos;
            
            ButtonProdutos = new Button(){
                Text = "Produtos",
                Location = new Point(225, 392),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
                BackColor = Color.GhostWhite
            };
            ButtonProdutos.Click += ClickEntrarProdutos;

            ButtonAtendimentos = new Button(){
                Text = "Atendimentos",
                Location = new Point(225, 462),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
                BackColor = Color.GhostWhite
            };
            ButtonAtendimentos.Click += ClickEntrarAtendimentos;

            ButtonSair = new Button(){
                Text = "Sair",
                Location = new Point(225, 532),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
                BackColor = Color.GhostWhite
            };
            ButtonSair.Click += ClickSair;

            Controls.Add(ButtonClientes);
            Controls.Add(ButtonServico);
            Controls.Add(ButtonProdutos);
            Controls.Add(ButtonAtendimentos);
            Controls.Add(ButtonSair);
        }
        private void ClickEntrarClientes(object? sender, EventArgs e){
            Hide();
            new ViewClientes(this).Show();
        }
        private void ClickEntrarServicos(object? sender, EventArgs e){
            Hide();
            new ViewServico(this).Show();
        }
        private void ClickEntrarProdutos(object? sender, EventArgs e){
            Hide();
            new ViewProdutos(this).Show();
        }
        private void ClickEntrarAtendimentos(object? sender, EventArgs e){
            Hide();
            new ViewAtendimentos().Show();
        }
        private void ClickSair(object? sender, EventArgs e){
            Close();
        }
    }
}