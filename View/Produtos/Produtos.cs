using Controller;
using Model;

namespace Views{
    public class ViewProdutos : Form{
        private readonly Form ParentVoltarProdutos;
        private readonly Label LabelTitulo;
        private readonly Button ButtonAdicionar;
        private readonly Button ButtonAlterar;
        private readonly Button ButtonDeletar;
        private readonly Button ButtonVoltar;
        private readonly DataGridView ListaDeProdutos;

        public ViewProdutos(Form parent){
            ControllerProdutos.Sincronizar();

            ParentVoltarProdutos = parent;
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.DimGray;
            
            LabelTitulo = new Label(){
                Text = "LISTA DE PRODUTOS",
                Location = new Point(280, 150),
                Size =  new Size(500, 35),
                Font = new Font("Arial", 20)
            };
            ButtonAdicionar = new Button(){
                Text = "ADICIONAR",
                Location = new Point(50, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70),
                BackColor = Color.GhostWhite
            };
            ButtonAdicionar.Click += ClickAdicionar;

            ButtonAlterar = new Button(){
                Text = "ALTERAR",
                Location = new Point(333, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70),
                BackColor = Color.GhostWhite
            };
            ButtonAlterar.Click += ClickAlterar;

            ButtonDeletar = new Button(){
                Text = "DELETAR",
                Location = new Point(633, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70),
                BackColor = Color.GhostWhite
            };
            ButtonDeletar.Click += ClickDeletar;

            ButtonVoltar = new Button(){
                Text = "< VOLTAR",
                Location = new Point(20, 20),
                Font = new Font("Arial", 12),
                Size = new Size(150, 40),
                BackColor = Color.GhostWhite
            };
            ButtonVoltar.Click += ClickVoltar;

            ListaDeProdutos = new DataGridView(){
                Location = new Point(50, 200),
                Size = new Size(780, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };


            Controls.Add(LabelTitulo);
            Controls.Add(ButtonAdicionar);
            Controls.Add(ButtonAlterar);
            Controls.Add(ButtonDeletar);
            Controls.Add(ButtonVoltar);
            Controls.Add(ListaDeProdutos);
            Listar();
        }
        private void ClickAdicionar(object? sender, EventArgs e){
            var viewAdicionarProduto = new ViewAdicionarProdutos(this);
            viewAdicionarProduto.produtoAdicionado += (p, args) => Listar(); // Escutar o evento
            Hide();
            viewAdicionarProduto.Show();
        }
        private void Listar(){
            List<Produtos> produtos = ControllerProdutos.ListarProdutos();
            ListaDeProdutos.Columns.Clear();
            ListaDeProdutos.AutoGenerateColumns = false;
            ListaDeProdutos.DataSource = produtos;

            ListaDeProdutos.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Id",
            HeaderText = "Id"
            });
            ListaDeProdutos.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Nome",
            HeaderText = "Nome"
            });
            ListaDeProdutos.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Preco",
            HeaderText = "Valor"
            });
        }
        private void ClickAlterar(object? sender, EventArgs e){
            var viewAlterarProduto = new ViewAlterarProdutos(this);
            viewAlterarProduto.produtoAlterado += (p, args) => Listar(); // Escutar o evento
            Hide();
            viewAlterarProduto.Show();
        }
        private void ClickDeletar(object? sender, EventArgs e){
            int index = ListaDeProdutos.SelectedRows[0].Index;
            ControllerProdutos.DeletarProdutos(index);
            Listar();

        }
        private void ClickVoltar(object? sender, EventArgs e)
        {
            Close();
            ParentVoltarProdutos.Show(); 
        }
    }
}