using Controller;
using Model;

namespace Views{
    public class ViewClientes : Form{
        private readonly Form ParentVoltarClienet;
        private readonly Label LabelTitulo;
        private readonly Button ButtonAdicionar;
        private readonly Button ButtonAlterar;
        private readonly Button ButtonDeletar;
        private readonly Button ButtonVoltar;
        private readonly DataGridView ListaDeClientes;

        public ViewClientes(Form parent){
            ControllerCliente.Sincronizar();

            ParentVoltarClienet = parent;
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.DimGray;
            
            LabelTitulo = new Label(){
                Text = "LISTA DE CLIENTES",
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

            ListaDeClientes = new DataGridView(){
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
            Controls.Add(ListaDeClientes);
            Listar();
        }
        private void ClickAdicionar(object? sender, EventArgs e){
            var viewAdicioanarCliente = new ViewAdicionarClientes(this);
            viewAdicioanarCliente.clienteAdicionado += (c, args) => Listar(); // Escutar o evento
            Hide();
            viewAdicioanarCliente.Show();
        }
        private void Listar(){
            List<Cliente> clientes = ControllerCliente.ListarCliente();
            ListaDeClientes.Columns.Clear();
            ListaDeClientes.AutoGenerateColumns = false;
            ListaDeClientes.DataSource = clientes;

            ListaDeClientes.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Id",
            HeaderText = "Id"
            });
            ListaDeClientes.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Nome",
            HeaderText = "Nome"
            });
            ListaDeClientes.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Numero",
            HeaderText = "Número"
            });
            ListaDeClientes.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "CPF",
            HeaderText = "CPF"
            });
            ListaDeClientes.Columns.Add(new DataGridViewTextBoxColumn {
            DataPropertyName = "Email",
            HeaderText = "Email"
            });
        }
        private void ClickAlterar(object? sender, EventArgs e){
            var viewAlterarCliente = new ViewAlterarClientes(this);
            viewAlterarCliente.clienteAlterado += (c, args) => Listar(); // Escutar o evento
            Hide();
            viewAlterarCliente.Show();
        }
        private void ClickDeletar(object? sender, EventArgs e){
            int index = ListaDeClientes.SelectedRows[0].Index;
            ControllerCliente.DeletarCliente(index);
            Listar();

        }
        private void ClickVoltar(object? sender, EventArgs e)
        {
            Close();
            ParentVoltarClienet.Show();
        }
    }
}