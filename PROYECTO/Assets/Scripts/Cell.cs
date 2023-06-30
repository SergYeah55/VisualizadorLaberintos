using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Variables que indican las coordenadas dentro del tablero del laberinto.
    private int coordX;
    private int coordY;

    // Variables que  van a recalcar el estado de cada celda.
    private Cell padre;
    private bool visitado;
    private int[] direcciones;
    private int dirActual;

    // Las direcciones de cada una de las celdas están determinadas por booleanos.
    private bool norte;
    private bool sur;
    private bool este;
    private bool oeste;

    // Todas las variables que determinan los elementos relacionados con los hijos.
    private Cell[] hijos;
    private int numhijos;

    // Variables que tienen relación con A estrella.
    private int costeA;
    private int costeH;
    private int costeG;
    private List<Cell> hijosPorCoste;

    // Constructor básico de esta clase.
    public Cell()
    {
        coordX = 0;
        coordY = 0;

        padre = null;
        visitado = false;
        direcciones = new int[] { -1, -1, -1, -1 };
        dirActual = 0;

        norte = false;
        sur = false;
        este = false;
        oeste = false;

        numhijos = 0;

        costeH = 0;
        costeA = 0;
        hijosPorCoste = new List<Cell>();

    }

    // Métodos para conseguir el valor e inicializarlo con respecto a las coordenadas.
    public int getX()
    {
        return this.coordX;
    }
    public int getY()
    {
        return this.coordY;
    }

    public void setX(int x)
    {
        this.coordX = x;
    }

    public void setY(int y)
    {
        this.coordY = y;
    }

    // Todos los métodos relacionados con el estado de la celda, tanto visita como padre.
    public void setPadre(Cell nuevaCell)
    {
        this.padre = nuevaCell;
    }

    public void resetPadre()
    {
        this.padre = null;
    }

    public Cell getPadre()
    {
        return this.padre;
    }

    public bool tienePadre()
    {
        bool tiene;
        if(padre == null)
        {
            tiene = false;
        }
        else
        {
            tiene = true;
        }

        return tiene;
    }

    public void hacerVisitado()
    {
        this.visitado = true;
    }
    public void hacerNoVisitado()
    {
        this.visitado = false;
    }
    public bool estaVisitado()
    {
        return this.visitado;
    }

    // Métodos relacionados con la elaboración del laberinto y el aspecto de la celda en sus diferentes direcciones.
    public bool estaDireccion(int dir)
    {
        bool esta = false;

        for (int i = 0; i< this.direcciones.Length; i++)
        {
            if (direcciones[i] == dir)
            {
                esta = true;
            }
        }

        return esta;
    }

    public void sumarDir()
    {
        this.dirActual++;
    }

    public int getDirActual()
    {
        return this.dirActual;
    }

    public void rellenarDirecciones()
    {
        int rellenoCompleto = 0;

        while (rellenoCompleto < 4)
        {
            int dirPrueba = Random.Range(0, 4);

            if (!estaDireccion(dirPrueba))
            {
                direcciones[rellenoCompleto] = dirPrueba;
                rellenoCompleto++;
            }
        }
    }

    public int getAvance()
    {
        return this.direcciones[dirActual];
    }

    public void cambNorte(bool nuevoEstado)
    {
        this.norte = nuevoEstado;
    }

    public bool getNorte()
    {
        return this.norte;
    }

    public void cambSur(bool nuevoEstado)
    {
        this.sur = nuevoEstado;
    }

    public bool getSur()
    {
        return this.sur;
    }
    public void cambEste(bool nuevoEstado)
    {
        this.este = nuevoEstado;
    }

    public bool getEste()
    {
        return this.este;
    }
    public void cambOeste(bool nuevoEstado)
    {
        this.oeste = nuevoEstado;
    }

    public bool getOeste()
    {
        return this.oeste;
    }

    // Forma de rellenar los hijos de cada una de las celdas o sus supuestas direcciones.
    public void rellenarHijos(GameObject [,] lista)
    {
        Cell [] aux = new Cell[4];

        if(this.getX() == 0 && this.getY() == 0)
        {
            this.setPadre(null);
        }

        for (int i = 0; i < 4; i++)
        {
            aux[i] = null;
        }

        if (this.getOeste())
        {
            aux[numhijos] = lista[this.getX(), (this.getY() - 1)].GetComponent<Cell>();
            numhijos++;
        }

        if (this.getNorte())
        {
            aux[numhijos] = lista[(this.getX()-1), this.getY()].GetComponent<Cell>();
            numhijos++;
        }

        if (this.getEste())
        {
            aux[numhijos] = lista[this.getX(), (this.getY() + 1)].GetComponent<Cell>();
            numhijos++;
        }

        if (this.getSur())
        {
            aux[numhijos] = lista[(this.getX() + 1), this.getY()].GetComponent<Cell>();
            numhijos++;
        }

        hijos = new Cell[numhijos];

        for (int j = 0; j < numhijos; j++)
        {
            hijos[j] = aux[j];
        }
    }

    public int getNumHijos()
    {
        return this.numhijos;
    }

    public Cell[] getHijos()
    {
        return this.hijos;
    }

    // Métodos relacionados con A estrella.
    public void calculoCosteH(int metaX, int metaY)
    {
        this.costeH = (metaX - this.coordX) + (metaY - this.coordY);
        this.costeA = this.costeA + this.costeH;
    }

    public void calculoCosteG()
    {
        int costeAux;
        int contadorAux = 0;
        Cell aux = this;

        while (aux.getX() != 0 || aux.getY() != 0)
        {
            if (this.getPadre() != null)
            {
                contadorAux++;
                aux = aux.getPadre();
            }
        }

        costeAux = contadorAux;

        if (this.getPadre() != null)
        {
            this.costeA = costeAux + this.costeH;
        }
    }

    public int getCosteH()
    {
        return this.costeH;
    }

    public int getCoste()
    {
        return this.costeA;
    }

    public void ordenarHijosPorCoste()
    {
        List<Cell> auxList = new List<Cell>();

        for(int i = 0; i < this.getHijos().Length; i++)
        {
            if(this.getHijos()[i] != this.getPadre())
            {
                auxList.Add(this.getHijos()[i]);
            }
        }

        hijosPorCoste = auxList.OrderBy(auxHijo => auxHijo.getCoste()).ToList(); ;
    }

    public List<Cell> getHijosOrdenados()
    {
        return this.hijosPorCoste;
    } 

}

