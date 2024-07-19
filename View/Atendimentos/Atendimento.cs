using Controller;
using Model;
namespace Views{
    public class ViewAtendimentos : Form {
        private readonly Label LabelTitulo;
        private readonly Label LabelDataInicio;
        private readonly Label LabelDataTermino;
        private readonly Label LabelCliente;
        private readonly Label LabelSvcRealizado;
        private readonly Label LabelPdtUtilizado;
        private readonly Label LabelDesconto;
        private readonly Label LabelAdicional;
        private readonly TextBox InputDataInicio;
        private readonly TextBox InputDataTermino;
        private readonly ComboBox ComboBoxCliente;
        private readonly ComboBox ComboBoxSvcRealizado;
        private readonly ComboBox ComboBoxPdtUtilizado;
        private readonly TextBox InputDesconto;
        private readonly TextBox InputAdicional;
        private readonly Button ButtonCriar;
        private readonly Button ButtonAlterar;
        private readonly Button ButtonDeletar;
        private readonly DataGridView ListaDeAtendimentos;
        public ViewAtendimentos(){
            Size = new Size(900, 850);
            StartPosition = FormStartPosition.CenterScreen;
            LabelTitulo = new Label(){
                Text = "MENU DE ATENDIMENTO",
                Location = new Point(190, 100),
                Size = new Size(600, 44),
                Font = new Font("Arial", 26)
            };
            LabelDataInicio = new Label(){
                Text = "Data de início:",
                Location = new Point(95, 165),
                Size = new Size(205, 30),
                Font = new Font("Arial", 18)
            };
            LabelDataTermino = new Label(){
                Text = "Data do término:",
                Location = new Point(69, 205),
                Size = new Size(235, 40),
                Font = new Font("Arial", 18)
            };
            LabelCliente = new Label(){
                Text = "Cliente:",
                Location = new Point(163, 248),
                Size = new Size(145, 40),
                Font = new Font("Arial", 18)
            };
            LabelSvcRealizado = new Label(){
                Text = "Serviço realizado:",
                Location = new Point(57, 288),
                Size = new Size(250, 40),
                Font = new Font("Arial", 18)
            };
            LabelPdtUtilizado = new Label(){
                Text = "Produto utilizado:",
                Location = new Point(61, 331),
                Size = new Size(250, 40),
                Font = new Font("Arial", 18)
            };
            LabelDesconto = new Label(){
                Text = "Desconto:",
                Location = new Point(140, 373),
                Size = new Size(165, 40),
                Font = new Font("Arial", 18)
            };
            LabelAdicional = new Label(){
                Text = "Valor adicional:",
                Location = new Point(86, 411),
                Size = new Size(215, 40),
                Font = new Font("Arial", 18)
            };
            InputDataInicio = new TextBox(){
                Location = new Point(325, 165),
                Size = new Size(250, 40),
            };
            InputDataTermino = new TextBox(){
                Location = new Point(325, 205),
                Size = new Size(250, 40),
            };
            ComboBoxCliente = new ComboBox(){
                Location = new Point(325, 248),
                Size = new Size(250, 40),
            };
            ComboBoxSvcRealizado = new ComboBox(){
                Location = new Point(325, 288),
                Size = new Size(250, 40),
            };
            ComboBoxPdtUtilizado = new ComboBox(){
                Location = new Point(325, 331),
                Size = new Size(250, 40),
            };
            InputDesconto = new TextBox(){
                Location = new Point(325, 373),
                Size = new Size(250, 40),
            };
            InputAdicional = new TextBox(){
                Location = new Point(325, 411),
                Size = new Size(250, 40),
            };
            ButtonCriar = new Button()
            {
                Text = "ADICIONAR",
                Location = new Point(50, 700),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };

            ButtonAlterar = new Button()
            {
                Text = "ALTERAR",
                Location = new Point(333, 700),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonDeletar = new Button()
            {
                Text = "DELETAR",
                Location = new Point(633, 700),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ListaDeAtendimentos = new DataGridView(){
                Location = new Point (50, 460),
                Size = new Size(789, 230)
            };
            Controls.Add(LabelTitulo);
            Controls.Add(LabelDataInicio);
            Controls.Add(LabelDataTermino);
            Controls.Add(LabelCliente);
            Controls.Add(LabelSvcRealizado);
            Controls.Add(LabelPdtUtilizado);
            Controls.Add(LabelDesconto);
            Controls.Add(LabelAdicional);
            Controls.Add(InputDataInicio);
            Controls.Add(InputDataTermino);
            Controls.Add(ComboBoxCliente);
            Controls.Add(ComboBoxSvcRealizado);
            Controls.Add(ComboBoxPdtUtilizado);
            Controls.Add(InputDesconto);
            Controls.Add(InputAdicional);
            Controls.Add(ButtonCriar);
            Controls.Add(ButtonAlterar);
            Controls.Add(ButtonDeletar);
            Controls.Add(ListaDeAtendimentos);
        }
    }
}