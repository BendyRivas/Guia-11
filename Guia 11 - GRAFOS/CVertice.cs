using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Linq;
using System.Text;


namespace Guia_10___GRAFOS
{
    class CVertice
    {

        public string Valor;
        public List<CArco> ListaAdyacencia;
        //estas banderas representan una coleccion de claves y valores
        //primer valor el tipo de claves en el diccionario, segundo dato el tipo de valores en el mismo
        Dictionary<string, short> _banderas;
        Dictionary<string, short> _banderas_predeterminado;

        //nuevos
        public int distancianodo;//guarda la distancia que hay entre el nodo inicio en el algoritmo Dijkstra
        public Boolean Visitado; //variable que sirve para marcar como visto el nodo en un recorrido
        public CVertice Padre; //nodo que sirve en los recorridos como el antecesor
        public Boolean pesoasignado;



        public Color Color
        {
            get { return color_nodo; }
            set { color_nodo = value; }
        }

        public Color Color2
        {
            get { return color2_n; }
            set { color2_n=value; }
        }
        public Color FontColor
        {
            get { return color_fuente; }
            set { color_fuente = value; }
        }
        public Point Posicion
        {
            get { return _posicion; }
            set { _posicion = value; }
        }
        public Size Dimensiones
        {
            get { return dimensiones; }
            set
            {
                radio = value.Width / 2;
                dimensiones = value;
            }
        }
        
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        static int size = 35; //Tamaño del nodo
        Size dimensiones;
        Color color2_n;
        Color color_nodo;   //color para el nodo
        Color color_fuente; //dolor para la fuente del nombre
        Point _posicion;    //donde se dibuja
        int radio;          //radio del nodo
        string nombre;      //para la propiedad del nombre y para colocarlo
        
        


        public CVertice(string Valor)
        {
            this.Valor = Valor;         //toma el parametro del contructor para la propiedad
            this.ListaAdyacencia = new List<CArco>();   //inicializa una nueva lista de la clase CArco
            this._banderas = new Dictionary<string, short>();   // inicializa las banderas
            this._banderas_predeterminado = new Dictionary<string, short>();
            this.Color = Color.Green;   // Definimos el color del nodo 
            this.Color2 = Color.White;
            this.Dimensiones = new Size(size, size); //dimension del ciruclo

            //nuevo
            this.FontColor = Color.White; //color fuente
            this.Color = Color.FromArgb(51, 204, 255);
            this.FontColor = Color.Black; //color de la fuente
            this.Visitado = false;
        }
        public CVertice() : this(" ") { }
        // Constructor por defecto

        public void DibujarVertice(Graphics g)
        {
            SolidBrush b = new SolidBrush(this.color_nodo); //se define un pinces
            
            // Definimos donde dibujaremos el nodo
            Rectangle areaNodo = new Rectangle(this._posicion.X - radio, this._posicion.Y - radio,
                                                 this.dimensiones.Width, this.dimensiones.Height);

            g.FillEllipse(b, areaNodo); //le decimos que coloree el graphic g y que lo dibuje segun el area asignada con areaNodo

            g.DrawString(this.Valor, new Font("Times New Roman", 14), new SolidBrush(color_fuente), //se crea el nombre asignandole el tipo de letra, tamaño y posicion
                         this._posicion.X, this._posicion.Y,
            new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            }
            );
                    g.DrawEllipse(new Pen(Brushes.Black, (float)1.0), areaNodo); //se dibuja el circulo, con un tipo de color y se define el tamaño
                    b.Dispose(); // Para liberar los recursos utilizados por el objeto
        }


        


        // Método para dibujar los arcos
        public void DibujarArco(Graphics g)
        {
            float distancia;
            int difY, difX; //se instancian variables 

            foreach (CArco arco in ListaAdyacencia)
            {
                difX = this.Posicion.X - arco.nDestino.Posicion.X; //definimos los valores en este caso posiciones para las variables diffX y diffY
                difY = this.Posicion.Y - arco.nDestino.Posicion.Y;

                distancia = (float)Math.Sqrt((difX * difX + difY * difY));//se saca la raiz cuadrada de estas variables para poder determinar la distancia

                AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4, true);//se instancia una linea y se especifica el ancho y alto de este mismo                

                bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle; //se especifica que tipo de forma tiene que ser al final de la linea creada 

                g.DrawLine(new Pen(new SolidBrush(arco.color), arco.grosor_flecha)  //se dibuja la linea especificando la posicion
                { CustomEndCap = bigArrow, Alignment = PenAlignment.Center },
                _posicion,
                new Point(arco.nDestino.Posicion.X + (int)(radio * difX / distancia),
                arco.nDestino.Posicion.Y + (int)(radio * difY / distancia)
                )
                );
                //se escribe el peso que tiene el archo 
                g.DrawString(  
                arco.peso.ToString(),
                new Font("Times New Roman", 12),
                new SolidBrush(Color.Red),
                this._posicion.X - (int)((difX / 3)),
                this._posicion.Y - (int)((difY / 3)),
                new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Far
                }
                );
            }
        }

        // Método para detectar posición en el panel donde se dibujará el nodo
            public bool DetectarPunto(Point p)
            {
            GraphicsPath posicion = new GraphicsPath();
            posicion.AddEllipse(new Rectangle(this._posicion.X - this.dimensiones.Width / 2,
            this._posicion.Y - this.dimensiones.Height / 2,
            this.dimensiones.Width, this.dimensiones.Height));
            bool retval = posicion.IsVisible(p);
            posicion.Dispose();
            return retval;
            }


            public string ToString()
            {
            return this.Valor;
            }

        

        public void colorear(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color.GreenYellow);
            //Definimos donde dibujaremos el nodo

            Rectangle areaNodo = new Rectangle(this._posicion.X - radio, this._posicion.Y - radio, this.dimensiones.Width, this.dimensiones.Height);
            g.FillEllipse(b, areaNodo);
            g.DrawString(this.Valor, new Font("Times New Roman", 14), new SolidBrush(color_fuente), this._posicion.X, this._posicion.Y,
                new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center

                });
            g.DrawEllipse(new Pen(Brushes.Black,(float)1.0),areaNodo);
            b.Dispose();
           
                

        }


    }
}
