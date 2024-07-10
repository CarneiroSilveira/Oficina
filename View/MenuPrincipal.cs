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

            ButtonClientes = new Button(){
                Text = "Clientes",
                Location = new Point(225, 252),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20),
            };
            ButtonClientes.Click += ClickEntrarClientes;
            ButtonServico = new Button(){
                Text = "Serviços",
                Location = new Point(225, 322),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20)
            };
            ButtonServico.Click += ClickEntrarServicos;
            
            ButtonProdutos = new Button(){
                Text = "Produtos",
                Location = new Point(225, 392),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20)
            };

            ButtonAtendimentos = new Button(){
                Text = "Atendimentos",
                Location = new Point(225, 462),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20)
            };

            ButtonSair = new Button(){
                Text = "Sair",
                Location = new Point(225, 532),
                Size = new Size(350, 60),
                Font = new Font("Arial", 20)
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
            new ViewClientes().Show();
        }
        private void ClickEntrarServicos(object? sender, EventArgs e){
            Hide();
            new ViewServico().Show();
        }
        private void ClickSair(object? sender, EventArgs e){
            Close();
        }
    }
}