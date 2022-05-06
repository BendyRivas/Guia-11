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

   
    public partial class Arco : Form
    {
        public bool control; //variable de control
        public int dato; //valor
        
        public Arco()
        {
            InitializeComponent();
            control = false;
            dato = 0;
        }

        private void btnaceptar_Click(object sender, EventArgs e)
        {
            try
            {
                dato = Convert.ToInt16(txtVertice.Text.Trim());

                if (dato < 0)
                {
                    MessageBox.Show("Debes de ingresar un valor positivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
                }
                else
                {
                    control = true;
                    Hide();
                }                
            }
            catch (Exception ex){
                MessageBox.Show("Debes de ingresar un valor númerico" + ex);

            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            control = false;
            Hide();
        }

        private void Arco_Load(object sender, EventArgs e)
        {
            txtVertice.Focus();
        }

        private void Arco_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Arco_Shown(object sender, EventArgs e)
        {
            txtVertice.Clear();
            txtVertice.Focus();
        }

        private void txtVertice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnaceptar_Click(null,null);
            }
        }
    }
}
