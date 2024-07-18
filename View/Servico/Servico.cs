using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Controller;
using Model;

namespace Views
{
    public class ViewServico : Form
    {
        private readonly Label LabelTitulo;
        private readonly Button ButtonAdicionar;
        private readonly Button ButtonAlterar;
        private readonly Button ButtonDeletar;
        private readonly DataGridView ListaDeServicos;

        public ViewServico()
        {
            ControllerServico.Sincronizar();

            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;

            LabelTitulo = new Label()
            {
                Text = "LISTA DE SERVIÇOS",
                Location = new Point(280, 150),
                Size = new Size(500, 40),
                Font = new Font("Arial", 20)
            };
            ButtonAdicionar = new Button()
            {
                Text = "ADICIONAR",
                Location = new Point(50, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonAdicionar.Click += ClickAdicionar;

            ButtonAlterar = new Button()
            {
                Text = "ALTERAR",
                Location = new Point(333, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonAlterar.Click += ClickAlterar;

            ButtonDeletar = new Button()
            {
                Text = "DELETAR",
                Location = new Point(633, 550),
                Font = new Font("Arial", 16),
                Size = new Size(200, 70)
            };
            ButtonDeletar.Click += ClickDeletar;

            ListaDeServicos = new DataGridView()
            {
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
            Controls.Add(ListaDeServicos);
            Listar();
        }

        private void ClickAdicionar(object? sender, EventArgs e)
        {
            var viewAdicionarServico = new ViewAdicionarServico(this);
            viewAdicionarServico.ServicoAdicionado += (s, args) => Listar(); // Escutar o evento
            Hide();
            viewAdicionarServico.Show();
        }

        private void Listar()
        {
            List<Servico> servicos = ControllerServico.ListarServico();

            ListaDeServicos.Columns.Clear();
            ListaDeServicos.AutoGenerateColumns = false;
            ListaDeServicos.DataSource = servicos;

            ListaDeServicos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id"
            });
            ListaDeServicos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nome",
                HeaderText = "Nome"
            });
            ListaDeServicos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Preco",
                HeaderText = "Valor"
            });
        }

        private void ClickAlterar(object? sender, EventArgs e)
        {
            var viewAlterarServico = new ViewAlterarServico(this);
            viewAlterarServico.ServicoAlterado += (s, args) => Listar(); // Escutar o evento
            Hide();
            viewAlterarServico.Show();
        }

        private void ClickDeletar(object? sender, EventArgs e)
        {
            int index = ListaDeServicos.SelectedRows[0].Index;
            ControllerServico.DeletarServico(index);
            Listar();
        }
    }
}
