using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace Guia_10___GRAFOS
{
    class CGrafo
    {
        public List<CVertice> nodos;
        public CGrafo()
        {
            nodos = new List<CVertice>(); //se inicializa un nuevo objecto de la clase CVertice
        }





        //====================OPERACIONES BASICAS==================//
        //construye un nodo a partir de su valor y lo agrega a las lista de nodos 

        public CVertice AgregarVertice(string valor) //se crea un metodo pasando como parametro el valor
        {
            CVertice nodo = new CVertice(valor);//se inicializa con su valor
            nodos.Add(nodo); //se agrega el vertice (nodo)
            return nodo; //retorna este mismo
        }

        public void AgregarVertice(CVertice nuevonodo)
        {
            nodos.Add(nuevonodo); //se agrega el valor a la lista grafo
        }

        


        //Busca un nodo en la lista de nodos del grafo
        public CVertice BuscarVertice(string valor)
        {
            return nodos.Find(v => v.Valor == valor);
        }
        //Crea una arista a partir de los valores de los nodos de origen y de destino
        public bool AgregarArco(string origen, string nDestino, int peso = 1)
        {
            CVertice vOrigen, vnDestino;
            //Si alguno de los nodos no existe, se activa una excepción
            if ((vOrigen = nodos.Find(v => v.Valor == origen)) == null) //si se quiere agregar una linea (arco) entre nodos (vertices) y el nodo de destino no exite laza la excepcion
                throw new Exception("El nodo " + origen + " no existe dentro del grafo");
            if ((vnDestino = nodos.Find(v => v.Valor == nDestino)) == null)
                throw new Exception("El nodo " + nDestino + "no existe dentro del grafo");
            return AgregarArco(vOrigen, vnDestino);
        }
        // Crea la arista a partir de los nodos de origen y de destino
        public bool AgregarArco(CVertice origen, CVertice nDestino, int peso = 1)
        {
            if (origen.ListaAdyacencia.Find(v => v.nDestino == nDestino) == null) //si se encuentra el origen y destino de este se agrega el arco
            {
                origen.ListaAdyacencia.Add(new CArco(nDestino, peso));
                return true;
            }
            return false;
        }

        // Método para dibujar el grafo
        public void DibujarGrafo(Graphics g)
        {
            // Dibujando los arcos
            foreach (CVertice nodo in nodos)
                nodo.DibujarArco(g);
            // Dibujando los vértices
            foreach (CVertice nodo in nodos)
                nodo.DibujarVertice(g);
        }
        // Método para detectar si se ha posicionado sobre algún nodo y lo devuelve
        public CVertice DetectarPunto(Point posicionMouse)
        {
            foreach (CVertice nodoActual in nodos)
                if (nodoActual.DetectarPunto(posicionMouse)) return nodoActual;
            return null;
        }

        // Método para regresar al estado original
        public void ReestablecerGrafo(Graphics g)
        {
            foreach (CVertice nodo in nodos)
            {
                nodo.Color = Color.White;
                nodo.FontColor = Color.Black;
                foreach (CArco arco in nodo.ListaAdyacencia)
                {
                    arco.grosor_flecha = 1;
                    arco.color = Color.Black;
                }
            }
            DibujarGrafo(g);
        }


        ///NUEVOS
        public void ColoArista(string o, string d)
        {
            foreach  (CVertice nodo in nodos)
            {
                foreach (CArco a in nodo.ListaAdyacencia)
                {
                    if (nodo.ListaAdyacencia != null && nodo.Valor == o && a.nDestino.Valor == d)
                    {
                        a.color = Color.Red;
                        a.grosor_flecha = 4;
                    }

                }
            }
        }

        public void Colorear (CVertice nodo)
        {
            nodo.Color = Color.AliceBlue;
            nodo.FontColor = Color.Black;
        }

        public CVertice nododistanciaminima()
        {
            int min = int.MaxValue;
            CVertice temp = null;
            foreach (CVertice origen in nodos)
            {
                if (origen.Visitado)
                {
                    foreach (CVertice destino in nodos)
                    {
                        if (!destino.Visitado)
                        {
                            foreach (CArco a in origen.ListaAdyacencia)
                            {
                                if(a.nDestino==destino && min > a.peso)
                                {
                                    min = a.peso;
                                    temp = destino;
                                }

                            }
                        }
                    }
                }
            }
            return temp;
        }

        public int posicionNodo(string Nodo)
        {
            for (int i = 0; i < nodos.Count; i++)
            {
                if (string.Compare(nodos[i].Valor, Nodo) == 0)
                    return i;
            }
            return -1;
        }


        //Funcion para re-dibujar los arcos que llegan a un nodo 
        public void DibujarEntrantes(CVertice nDestino)
        {

            foreach (CVertice nodos in nodos)
            {
                foreach (CArco a in nodos.ListaAdyacencia)
                {
                    if(nodos.ListaAdyacencia != null && nodos != nDestino)
                    {
                        if (a.nDestino == nDestino)
                        {
                            a.color = Color.Black;
                            a.grosor_flecha = 2;
                            break;
                        }
                    }
                }
            }

        }

        public void Desmarcar()
        {
            foreach (CVertice n in nodos)
            {
                n.Visitado = false;
                n.Padre = null;
                n.distancianodo = int.MaxValue;
                n.pesoasignado = false;
            }
        }

    }
}
