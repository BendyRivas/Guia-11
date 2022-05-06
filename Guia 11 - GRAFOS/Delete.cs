using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guia_10___GRAFOS
{
    public partial class Delete : Form
    {
        public bool control;
        public string dato;
        public Delete()
        {
            InitializeComponent();
            control = false;
            dato = "";
        }

        private void btnaceptar_Click(object sender, EventArgs e)
        {
            string valor = txtVertice_d.Text.Trim();

            if ((valor == "") || (valor == " "))
            {
                MessageBox.Show("Debe de ingresar un valor para borrar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                control = true;
                Hide();
            }
        }
    }
}
