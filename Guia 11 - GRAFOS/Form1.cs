using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace Guia_10___GRAFOS
{
    public partial class Form1 : Form
    {
        private CGrafo grafo; // instanciamos la clase CGrafo
        private CVertice nuevoNodo; // instanciamos la clase Cvertice para crear el nodo "nuevoNodo"
        private CVertice NodoOrigen; // instanciamos la clase Cvertice para crear el nodo "NodoOrigen"
        private CVertice NodoDestino;// instanciamos la clase Cvertice para crear el nodo "NodoDestino"
        private int var_control = 0;// la utilizaremos para determinar el estado en la pizarra: 
                                    // 0: Sin acción, 1: Dibujando arco, 2: Nuevo vértice


        //Nuevos
        //private Recorrido ventanaRecorrido;
        private Vertice ventanaVertice;
        private Arco ventanaArco;
        List<CVertice> nodosRuta;
        List<CVertice> nodosOrdenados;
        bool buscarRuta = false, nuevoVertice = false, nuevoArco = false;
        private int numeronodos = 0;
        bool profundidad = false, anchura = false, nodoEncontrado = false;
        Queue cola = new Queue(); //para recorrido en anchura
        private string destino = "", origen = "";
        private int distancia = 0;

       

       
        public Form1()
        {
            InitializeComponent();
            grafo = new CGrafo();
            nuevoNodo = null;
            var_control = 0;
            ventanaVertice = new Vertice();
            ventanaArco = new Arco();
            nodosRuta = new List<CVertice>();
            nodosOrdenados = new List<CVertice>();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer, true);
        }

        private void Pizarra_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;                    
                grafo.DibujarGrafo(e.Graphics);
                if (nuevoVertice)
                {
                    CBVertice.Items.Clear();
                    CBVertice.SelectedIndex = -1;
                    CBNodoPartida.Items.Clear();
                    CBNodoPartida.SelectedIndex = -1;
                    foreach (CVertice nodo in grafo.nodos)
                    {
                        CBVertice.Items.Add(nodo.Valor);
                        CBNodoPartida.Items.Add(nodo.Valor);
                    }
                    nuevoVertice = false;
                }
                if (nuevoArco)
                {
                    CBArco.Items.Clear();
                    CBArco.SelectedIndex = -1;
                    foreach (CVertice nodo in grafo.nodos)
                    {
                        foreach (CArco arco in nodo.ListaAdyacencia)                        
                            CBArco.Items.Add("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso:" + arco.peso);                        
                    }
                    nuevoArco = false;
                }
                if (buscarRuta)
                {
                    foreach (CVertice nodo in nodosRuta)
                    {
                        nodo.colorear(e.Graphics);
                        Thread.Sleep(1000);
                        nodo.DibujarVertice(e.Graphics);
                    }
                    buscarRuta = false;
                }

                if (profundidad)
                {
                    ordenarNodos();
                    foreach (CVertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado)
                            recorridoProfundidad(nodo, e.Graphics);
                    }
                    profundidad = false;
                    //se reestrablecen los valores
                    foreach (CVertice nodo in grafo.nodos)
                    {
                        nodo.Visitado=false;
                    }
                }

                if (anchura)
                {
                    distancia = 0;
                    //ordenando los nodos desde el que indica el usuario
                    cola = new Queue();
                    ordenarNodos();
                    foreach (CVertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado && !nodoEncontrado)
                            recorridoAnchura(nodo, e.Graphics, destino);
                    }
                    anchura = false;
                    nodoEncontrado = false;
                    //restablecer los valores
                    foreach (CVertice nodo in grafo.nodos)

                        nodo.Visitado = false;
                    
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void ordenarNodos()
        {
            nodosOrdenados = new List<CVertice>();
            bool est = false;
            foreach (CVertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    nodosOrdenados.Add(nodo);
                    est = true;
                }
                else if (est)
                {
                    nodosOrdenados.Add(nodo);
                }
            }

            foreach (CVertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    est = false;
                    break;
                }
                else if(est)
                {
                    nodosOrdenados.Add(nodo);
                }

            }
        }


        private void Pizarra_MouseLeave(object sender, EventArgs e)
        {
            Pizarra.Refresh();
        }

        private void BtnEliminarVer_Click(object sender, EventArgs e)
        {
            if (CBVertice.SelectedIndex > -1)
            {
                foreach (CVertice nodo in grafo.nodos)
                {
                    if (nodo.Valor == CBVertice.SelectedItem.ToString())
                    {
                        grafo.nodos.Remove(nodo);
                        //Borrando arcos que posea el nodo eliminado
                        nodo.ListaAdyacencia = new List<CArco>();
                        break;
                    }

                }

                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco arco in nodo.ListaAdyacencia)
                    {
                        if(arco.nDestino.Valor==CBVertice.SelectedItem.ToString())
                        {
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }
                    }
                }


               
                nuevoArco = true;
                nuevoVertice = true;
                CBVertice.SelectedIndex = -1;
                Pizarra.Refresh();
            }
            else
            {
                MessageBox.Show("Seleccione un nodo");
            }
        }

        private void BtnElArc_Click(object sender, EventArgs e)
        {
            if (CBArco.SelectedIndex > -1)
            {
                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco arco in nodo.ListaAdyacencia)
                    {
                        if("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso: "+ arco.peso ==
                            CBArco.SelectedItem.ToString())
                        {
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }

                    }

                }
                nuevoArco = true;
                nuevoVertice = true;
                CBVertice.SelectedIndex = -1;
                Pizarra.Refresh();
            }
            else
            {
                MessageBox.Show("Seleccione un arco");

            }
        }

        private void CMSCrearVertice_Click(object sender, EventArgs e)
        {
           //no sirve
        }

        private void Pizarra_MouseUp(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 1:
                    // Dibujando arco
                    if ((NodoDestino = grafo.DetectarPunto(e.Location)) != null && NodoOrigen != NodoDestino)
                    {
                        ventanaArco.Visible = false;
                        ventanaArco.control = false;
                        ventanaArco.ShowDialog();
                        if (ventanaArco.control)
                        {
                            if (grafo.AgregarArco(NodoOrigen, NodoDestino,ventanaArco.dato))
                            //Se procede a crear la arista
                            {
                                int distancia = ventanaArco.dato;
                                NodoOrigen.ListaAdyacencia.Find(v => v.nDestino == NodoDestino).peso = distancia;
                            }
                            nuevoArco = true;
                        }
                        
                    }
                    var_control = 0;
                    NodoOrigen = null;
                    NodoDestino = null;

                    Pizarra.Refresh();
                    break;
            }
        }

        private void BtnProf_Click(object sender, EventArgs e)
        {
            if (CBNodoPartida.SelectedIndex > -1)
            {
                profundidad = true;
                origen = CBNodoPartida.SelectedItem.ToString();
                Pizarra.Refresh();
                CBNodoPartida.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Seleccione un nodo de partida");
            }
        }

        private void BtnAnch_Click(object sender, EventArgs e)
        {
            if (CBNodoPartida.SelectedIndex > -1)
            {
                origen = CBNodoPartida.SelectedItem.ToString();
                anchura = true;
                Pizarra.Refresh();
                CBNodoPartida.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Seleccione un nodo de partida");
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Trim() != "")
            {
                if (grafo.BuscarVertice(txtBuscar.Text) != null)
                {
                    MessageBox.Show("Si se encuentra el vertice " + txtBuscar.Text);
                }
                else
                {
                    MessageBox.Show("No se encuentra el vertice " +txtBuscar.Text );
                }
            }
        }

        private void Pizarra_MouseMove(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 2:
                    if (nuevoNodo != null)
                    {                        //Creando nuevo nodo
                        int posX = e.Location.X;
                        int posY = e.Location.Y;

                        if (posX < nuevoNodo.Dimensiones.Width / 2)
                            posX = nuevoNodo.Dimensiones.Width / 2;

                        else if (posX > Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2)
                            posX = Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2;

                        if (posY < nuevoNodo.Dimensiones.Height / 2)
                            posY = nuevoNodo.Dimensiones.Height / 2;
                        else if (posY > Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2)
                            posY = Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2;

                        nuevoNodo.Posicion = new Point(posX, posY);
                        Pizarra.Refresh();
                        nuevoNodo.DibujarVertice(Pizarra.CreateGraphics());
                    }
                    break;
                case 1:
                    // Dibujar arco
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4, true);
                    bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle;

                    Pizarra.Refresh();
                    Pizarra.CreateGraphics().DrawLine(new Pen(Brushes.Black, 2) {CustomEndCap = bigArrow},
                    NodoOrigen.Posicion, e.Location);
                    break;

                
               

            }
        }

        private void Pizarra_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) // Si se ha presionado el botón
                                                                    // izquierdo del mouse
            {
                if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                {
                    var_control = 1;
                    // recordemos que es usado para indicar el estado en la pizarra:
                    // 0: Sin acción, 1: Dibujando arco, 2: Nuevo vértice
                }
               
                
            }
            if (nuevoNodo != null && NodoOrigen == null)
            {
                ventanaVertice.Visible = false;
                ventanaVertice.control = false;
                ventanaVertice.ShowDialog();
                grafo.AgregarVertice(nuevoNodo);
                numeronodos = grafo.nodos.Count();//CUENTA CUANTOS NODOS HAY EN EL GRAFO
                             
                if (ventanaVertice.control)
                {
                    if (grafo.BuscarVertice(ventanaVertice.dato) == null)
                    {
                      // 
                        nuevoNodo.Valor = ventanaVertice.dato;
                    }
                    else
                    {
                        MessageBox.Show("El Nodo " + ventanaVertice.dato + "ya existe en el grafo", "Error nuevo Nodo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) ;
                    }
                }
                nuevoNodo = null;
                var_control = 0;
                Pizarra.Refresh();
            }


             if (e.Button == System.Windows.Forms.MouseButtons.Right) // Si se ha presionado el botón
                                                                      // derecho del mouse
             {
                 if (var_control == 0)
                 {
                     if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                     {
                         nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                     }
                     else
                         Pizarra.ContextMenuStrip = this.CMSCrearVertice;
                                  
                }
             }


            if (e.Button == System.Windows.Forms.MouseButtons.Right) //SE SEHA PRESIONADO EL BOTON
                                                                     //DERECHO DEL MOUSE    
            {
                if (var_control == 0)
                {
                    if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                    {
                        nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                    }
                    else
                        Pizarra.ContextMenuStrip = this.CMSCrearVertice;

                }
            }
            
        }

        private void nuevoVerticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoNodo = new CVertice();
            var_control = 2;
            // recordemos que es usado para indicar el estado en la pizarra:
            // 0: Sin acción, 1: Dibujando arco, 2: Nuevo vértice
        }

        //nuevas
        private void recorridoProfundidad(CVertice vertice, Graphics g)
        {
            vertice.Visitado = true;
            vertice.colorear(g);
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            foreach (CArco adya in vertice.ListaAdyacencia)
            {
                if (!adya.nDestino.Visitado) recorridoProfundidad(adya.nDestino, g);
            }
        }

        private void recorridoAnchura(CVertice vertice, Graphics g, string destino)
        {
            vertice.Visitado = true;
            cola.Enqueue(vertice);
            vertice.colorear(g);
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            if (vertice.Valor == destino)
            {
                nodoEncontrado = true;
                return;
            }
            while (cola.Count > 0)
            {
                CVertice aux = (CVertice)cola.Dequeue();
                foreach (CArco adya in aux.ListaAdyacencia)
                {
                    if (!adya.nDestino.Visitado)
                    {
                        if (!nodoEncontrado)
                        {
                            adya.nDestino.Visitado = true;

                            adya.nDestino.colorear(g);
                            Thread.Sleep(1000);
                            adya.nDestino.DibujarVertice(g);
                            if (destino != "")
                            {
                                nodoEncontrado = true;
                                return;
                            }
                        }
                    }
                }
            }
            
        }


        //nuevo
        private int totalNodos; //lista de nodos
        int[] parent; //padre del nodo
        bool[] visitado;//variable para comprobar los nodos ya visitados

        private void calcularMatricesIniciales()
        {
            nodosRuta = new List<CVertice>();
            totalNodos = grafo.nodos.Count;
            parent = new int[totalNodos];
            //se calcula la matriz inicial de distancias
            for (int i = 0; i < totalNodos; i++)
            {
                List<int> filaDistancia = new List<int>();
                for (int j = 0; j < totalNodos; j++)
                {
                    //si origen = destino 
                    if (i == j)
                    {
                        filaDistancia.Add(0);
                    }
                    else
                    {
                        //se busca si tiene relacion i,j; de existit obtenemos la distancia 
                        int distancia = -1;
                        for (int k = 0; k < grafo.nodos[i].ListaAdyacencia.Count; k++)
                        {
                            if (grafo.nodos[i].ListaAdyacencia[k].nDestino == grafo.nodos[j])
                                distancia = grafo.nodos[i].ListaAdyacencia[k].peso;
                        }
                        filaDistancia.Add(distancia);
                    }
                }

            }
        }
              
    }
}
