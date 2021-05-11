using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Modelo
{
    public partial class Form1 : Form
    {
        Stack<Pallet> pilha = new Stack<Pallet>(); //global

        public Form1()
        {
            InitializeComponent();
            carregar();
        }

        void salvar()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("pallets.txt", FileMode.Create)))
            {
                writer.Write(pilha.Count());
                foreach (Pallet p in pilha)
                {
                    writer.Write(p.Nome);
                    writer.Write(p.Quantidade);
                }
            }
        }

        void carregar()
        {
            if (File.Exists("pallets.txt"))
            {
                Stack<Pallet> aux = new Stack<Pallet>();
                using (BinaryReader reader = new BinaryReader(File.Open("pallets.txt", FileMode.Open)))
                {
                    int qtd = reader.ReadInt32();
                    for (int i = 0; i < qtd; i++) { 
                    Pallet p = new Pallet();
                    p.Nome = reader.ReadString();
                    p.Quantidade = reader.ReadInt32();
                    aux.Push(p);
                  }// fim for
                }
                foreach (Pallet p in aux)
                {
                    pilha.Push(p);
                }// fim foreach
                mostraPilha();
            }
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            salvar();
            this.Close();
        }

        private void BtnSobre_Click(object sender, EventArgs e)
        {

        }

        void mostraPilha()
        {
            listPallets.Items.Clear();
            foreach (Pallet p in pilha)
            {
                listPallets.Items.Add(p.Nome + ": "+p.Quantidade);
            }
        }
        void limpar()
        {
            txtProd.Clear();
            txtQtd.Clear();
            txtProd.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Pallet p = new Pallet();
            p.Nome = txtProd.Text;
            p.Quantidade = Convert.ToInt32(txtQtd.Text);
            pilha.Push(p);
            mostraPilha();
            limpar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stack<Pallet> aux = new Stack<Pallet>();
            while (pilha.Count > 0)
            {
                Pallet p = new Pallet();
                p = pilha.Peek(); // pegando o pallet que esta no topo
                if (p.Nome.Equals(txtProd.Text))
                {
                    int qtd = Convert.ToInt32(txtQtd.Text);
                    if(p.Quantidade > qtd)
                    {
                        p.Quantidade = p.Quantidade - qtd;
                        MessageBox.Show(p.Nome + ": ouve remoção de " + qtd +" produtos");
                    }
                    else
                    {
                        pilha.Pop();
                        MessageBox.Show(p.Nome + ": ouve remoção de " + p.Quantidade + " produtos");
                    }
                    break;               
                }
                else
                {
                    aux.Push(p);
                    pilha.Pop();
                    MessageBox.Show(p.Nome + " movido para pilha auxiliar");
                }
            }// fim while

            foreach(Pallet p in aux) {
                pilha.Push(p);
            }// fim foreach

            mostraPilha();
            limpar();
        }
    }
}
