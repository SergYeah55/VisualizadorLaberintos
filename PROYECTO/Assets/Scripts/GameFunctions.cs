using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Clase encargada de poder gestionar todo lo relacionado con la ejecución de los algoritmos y lo que se verá por pantalla.
public class GameFunctions : MonoBehaviour
{
    // Se trata de las variables que han quedado definidas por el usuario y servirán para la ejecución del laberinto
    private string nombreAlgo;
    private int dimension;

    // Variables centradas en la manipulación del laberinto
    public GameObject celdaPrefab;
    private GameObject[,] listaCeldas;

    // Variables para controlar visualización de botones
    public GameObject btnCreate;
    public GameObject btnSol;
    public GameObject btnVis;
    public GameObject btnPrev;
    public GameObject btnNext;

    // Variables que controlan la visualización de textos de navegación
    public GameObject textMeta;
    public GameObject textPasos;
    public GameObject textPrev;
    public GameObject textActual;
    public GameObject textNext;

    // Variables que controla la imagen del algoritmo
    public GameObject imagenAlgo;
    public GameObject btnInfo;
    public GameObject btnSalirInfo;

    // Variable que controla la visualización de la ventana resumen
    public GameObject btnAlgo1;
    public GameObject btnAlgo2Izq;
    public GameObject btnAlgo2Dch;
    public GameObject btnAlgo3;
    public GameObject ventanaFinal;

    // Variables para la visualización y resolución de algoritmos de forma interna
    private Queue<Cell> colaDeBusqueda;
    private Stack<Cell> filaDeNavegacionAvance;
    private Stack<Cell> filaDeNavegacionRetroceso;
    private Cell celdaNav;
    private int conteoVis;
    private bool metaAlcanzadaVis;

    // Constructor de GameFunctions
    public GameFunctions()
    {
        this.nombreAlgo = "No se sabe";
        this.dimension = 0;
    }

    // Métodos básicos para GameFunctions

    // Método para darle valor a la dimensión del laberinto.
    public void setDimension(int num)
    {
        this.dimension = num;
    }

    // Método para poder determinar el algoritmo que se quiere observar.
    public void setNombre(string nomb)
    {
        this.nombreAlgo = nomb;
    }

    // Método para saber el algoritmo que se está analizando.
    public string getNombre()
    {
        return this.nombreAlgo;
    }

    // Método para saber la dimensión del laberinto que se está valorando.
    public int getDimension()
    {
        return this.dimension;
    }

    // Considerado el método para la creación aleatoria del laberinto.
    public void createMaze()
    {
        // Se permite visualizar los botones que faltaban
        btnSol.SetActive(true);

        // Se deja visualizar y se inicializan los textos de meta y actual
        inicializacionTextos();

        // Se declara una matriz de GameObjects que van a determinar la visualización del laberinto.
        listaCeldas = new GameObject[dimension, dimension];

        // Según la dimensión habrá un tamaño u otro para las casillas
        switch (dimension)
        {
            case 3:
                inicializacionLaberintoDim1();
                break;
            case 4:
                inicializacionLaberintoDim2();
                break;
            case 6:
                inicializacionLaberintoDim3();
                break;
        }
        
        // Una vez colocado el objeto que hará de laberinto se va a rellenar de forma aleaotria con el método.
        listaCeldas = rellenarMaze(listaCeldas, dimension);

        // Se realiza el dibujado del laberinto por pantalla según el tipo de celda
        dibujarLaberinto();

        // Se borra el botón para que no se puedan duplicar
        btnCreate.SetActive(false);

    }

    // Método que permite inicializar y mostrar los textos básicos de navegación.
    public void inicializacionTextos()
    {
        // Se prepara el texto que indica la meta.
        textMeta.SetActive(true);
        GameObject.Find("textoMetaFinal").GetComponent<TMPro.TMP_Text>().SetText("Meta = [" + (dimension - 1) + " , " + (dimension - 1) + "]"); ;

        // Se prepara el texto que indica el paso actual
        textPasos.SetActive(true);
        GameObject.Find("textoPasosFinal").GetComponent<TMPro.TMP_Text>().SetText("Paso 0");

        // Se prepara el texto que indica la casilla actual
        textActual.SetActive(true);
        GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [ , ]");
    }

    // Método que instancia los objetos de forma física en la pantalla para crear el laberinto 3x3
    public void inicializacionLaberintoDim1()
    {
        // Se va a recorrer la matriz para ubicar en el espacio cada casilla
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Se establece una celda y se coloca en la pantalla del usuario.
                GameObject nuevaCelda = Instantiate(celdaPrefab);
                nuevaCelda.transform.parent = GameObject.Find("mazeZone").transform;
                nuevaCelda.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 290);
                nuevaCelda.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().gameObject.SetActive(false);


                // Se añade a la lista de celdas del laberinto estableciendo sus coordenadas dentro del mismo.
                listaCeldas[i, j] = nuevaCelda;
                nuevaCelda.GetComponent<Cell>().setX(i);
                nuevaCelda.GetComponent<Cell>().setY(j);

                // Se van estableciendo las posiciones de cada una de las celdas, teniendo en mente las separaciones entre ellas.
                if (i == 0)
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
                        nuevaCelda.transform.position = GameObject.Find("mazeZone").transform.position;
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(50, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
                        nuevaCelda.transform.position = listaCeldas[(i - 1), j].transform.position + new Vector3(0, -50);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(50, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);

                        if(i == (dimension-1) && j == (dimension - 1))
                        {
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(true);
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/metaIcon");
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                        }
                    }
                }

                // Lo último que se realiza es darles un color a cada una para que puedan llegar a verse por pantalla.
                nuevaCelda.GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }

    // Método que instancia los objetos de forma física en la pantalla para crear el laberinto 4x4
    public void inicializacionLaberintoDim2()
    {
        // Se va a recorrer la matriz para ubicar en el espacio cada casilla
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Se establece una celda y se coloca en la pantalla del usuario.
                GameObject nuevaCelda = Instantiate(celdaPrefab);
                nuevaCelda.transform.parent = GameObject.Find("mazeZone").transform;
                nuevaCelda.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 290);
                nuevaCelda.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().gameObject.SetActive(false);

                // Se añade a la lista de celdas del laberinto estableciendo sus coordenadas dentro del mismo.
                listaCeldas[i, j] = nuevaCelda;
                nuevaCelda.GetComponent<Cell>().setX(i);
                nuevaCelda.GetComponent<Cell>().setY(j);

                // Se van estableciendo las posiciones de cada una de las celdas, teniendo en mente las separaciones entre ellas.
                if (i == 0)
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 450);
                        nuevaCelda.transform.position = GameObject.Find("mazeZone").transform.position - new Vector3 (25,10,0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;

                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(450,450);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(45, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(450,450);
                        nuevaCelda.transform.position = listaCeldas[(i - 1), j].transform.position + new Vector3(0, -45);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(450,450);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(45, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);

                        if (i == (dimension - 1) && j == (dimension - 1))
                        {
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(true);
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/metaIcon");
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                        }
                    }
                }

                // Lo último que se realiza es darles un color a cada una para que puedan llegar a verse por pantalla.
                nuevaCelda.GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }

    // Método que instancia los objetos de forma física en la pantalla para crear el laberinto 6x6
    public void inicializacionLaberintoDim3()
    {
        // Se va a recorrer la matriz para ubicar en el espacio cada casilla
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Se establece una celda y se coloca en la pantalla del usuario.
                GameObject nuevaCelda = Instantiate(celdaPrefab);
                nuevaCelda.transform.parent = GameObject.Find("mazeZone").transform;
                nuevaCelda.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = new Vector3(3, 3, 1);
                nuevaCelda.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().gameObject.SetActive(false);

                // Se añade a la lista de celdas del laberinto estableciendo sus coordenadas dentro del mismo.
                listaCeldas[i, j] = nuevaCelda;
                nuevaCelda.GetComponent<Cell>().setX(i);
                nuevaCelda.GetComponent<Cell>().setY(j);

                // Se van estableciendo las posiciones de cada una de las celdas, teniendo en mente las separaciones entre ellas.
                if (i == 0)
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                        nuevaCelda.transform.position = GameObject.Find("mazeZone").transform.position - new Vector3(30, -10, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;

                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(30, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                        nuevaCelda.transform.position = listaCeldas[(i - 1), j].transform.position + new Vector3(0, -30);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);
                    }
                    else
                    {
                        nuevaCelda.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                        nuevaCelda.transform.position = listaCeldas[i, (j - 1)].transform.position + new Vector3(30, 0);
                        nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(false);

                        if (i == (dimension - 1) && j == (dimension - 1))
                        {
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().gameObject.SetActive(true);
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/metaIcon");
                            nuevaCelda.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                        }
                    }
                }

                // Lo último que se realiza es darles un color a cada una para que puedan llegar a verse por pantalla.
                nuevaCelda.GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }

    // Forma de rellenar de forma aleatoria el laberinto
    public GameObject[,] rellenarMaze(GameObject[,] nuevaLista, int dim)
    {
        // Variable que va a poder establecer el número de celdas que se han visitado para crear el laberinto.
        int visitadas = 0;

        // Con este bucle se establece el orden aleaotorio de cómo se van a explorar cada una de las direcciones.
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                nuevaLista[i, j].GetComponent<Cell>().rellenarDirecciones();
            }
        }

        // Se escoge una celda del laberinto aleaotria para poder empezar a crear el laberinto.
        int primerX = Random.Range(0, dim);
        int primerY = Random.Range(0, dim);
        nuevaLista[primerX, primerY].GetComponent<Cell>().hacerVisitado();
        visitadas++;

        // Inicialización del bucle para crear el laberinto.
        while (visitadas < (dim * dim))
        {
            // Variable que indicará si se ha realizado un avance o no.
            bool avanceRealizado = false;

            // Comprobación de todas las posibles direciones y avances que se pueden llegar a realizar.
            while (avanceRealizado == false && nuevaLista[primerX, primerY].GetComponent<Cell>().getDirActual() < 4)
            {
                // Hacer forma de almacenar aleatoriamente las 4 direcciones en memoria
                int direccion = nuevaLista[primerX, primerY].GetComponent<Cell>().getAvance();

                // Se comprueba qué dirección es, siempre que no se haya visitado.
                switch (direccion)
                {
                    // Para cada caso se va a observar si la dirección es posibles, ya que existen límites en el laberinto y si se viene de esa casilla o no.
                    // En cada uno de ellos se hace visitada la nueva casilla y se inicializa los ndoos padres y se cambian las varibales de las direcciones.

                    case 0:
                        if ((primerY + 1) >= 0 && (primerY + 1) < dim && (nuevaLista[primerX, (primerY + 1)].GetComponent<Cell>().estaVisitado() == false))
                        {
                            nuevaLista[primerX, (primerY + 1)].GetComponent<Cell>().hacerVisitado();
                            nuevaLista[primerX, (primerY + 1)].GetComponent<Cell>().setPadre(nuevaLista[primerX, primerY].GetComponent<Cell>());

                            nuevaLista[primerX, primerY].GetComponent<Cell>().cambEste(true);
                            nuevaLista[primerX, (primerY + 1)].GetComponent<Cell>().cambOeste(true);

                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();

                            visitadas++;
                            primerY = primerY + 1;
                            avanceRealizado = true;
                        }
                        else
                        {
                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();
                        }
                        break;


                    case 1:
                        if ((primerY - 1) >= 0 && (primerY - 1) < dim && (nuevaLista[primerX, (primerY - 1)].GetComponent<Cell>().estaVisitado() == false))
                        {
                            nuevaLista[primerX, (primerY - 1)].GetComponent<Cell>().hacerVisitado();
                            nuevaLista[primerX, (primerY - 1)].GetComponent<Cell>().setPadre(nuevaLista[primerX, primerY].GetComponent<Cell>());

                            nuevaLista[primerX, primerY].GetComponent<Cell>().cambOeste(true);
                            nuevaLista[primerX, (primerY - 1)].GetComponent<Cell>().cambEste(true);

                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();

                            visitadas++;
                            primerY = primerY - 1;
                            avanceRealizado = true;
                        }
                        else
                        {
                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();
                        }
                        break;


                    case 2:
                        if ((primerX - 1) >= 0 && (primerX - 1) < dim && (nuevaLista[(primerX - 1), primerY].GetComponent<Cell>().estaVisitado() == false))
                        {
                            nuevaLista[(primerX - 1), primerY].GetComponent<Cell>().hacerVisitado();
                            nuevaLista[(primerX - 1), primerY].GetComponent<Cell>().setPadre(nuevaLista[primerX, primerY].GetComponent<Cell>());

                            nuevaLista[primerX, primerY].GetComponent<Cell>().cambNorte(true);
                            nuevaLista[(primerX - 1), primerY].GetComponent<Cell>().cambSur(true);

                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();

                            visitadas++;
                            primerX = primerX - 1;
                            avanceRealizado = true;
                        }
                        else
                        {
                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();
                        }
                        break;


                    case 3:
                        if ((primerX + 1) >= 0 && (primerX + 1) < dim && (nuevaLista[(primerX + 1), primerY].GetComponent<Cell>().estaVisitado() == false))
                        {
                            nuevaLista[(primerX + 1), primerY].GetComponent<Cell>().hacerVisitado();
                            nuevaLista[(primerX + 1), primerY].GetComponent<Cell>().setPadre(nuevaLista[primerX, primerY].GetComponent<Cell>());

                            nuevaLista[primerX, primerY].GetComponent<Cell>().cambSur(true);
                            nuevaLista[(primerX + 1), primerY].GetComponent<Cell>().cambNorte(true);

                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();

                            visitadas++;
                            primerX = primerX + 1;
                            avanceRealizado = true;
                        }
                        else
                        {
                            nuevaLista[primerX, primerY].GetComponent<Cell>().sumarDir();
                        }
                        break;
                }
            }

            // En el caso de que el avance no se realiza y es un nodo con padres se vuelve hacia el padre.
            if (avanceRealizado == false && nuevaLista[primerX, primerY].GetComponent<Cell>().tienePadre() == true)
            {

                int cellPadreX = nuevaLista[primerX, primerY].GetComponent<Cell>().getPadre().getX();
                int cellPadreY = nuevaLista[primerX, primerY].GetComponent<Cell>().getPadre().getY();
                primerX = cellPadreX;
                primerY = cellPadreY;

            }
        }

        // Una vez se ha creado el alberinto se vuelve la estructura de datos.
        return nuevaLista;
    }

    // Con este método se va a realizar el dibujo de cada celda que conforma el laberinto
    public void dibujarLaberinto()
    {
        // Con el laberinto ya creado de forma aleatoria se le va a otorgar una imagen según la codificación de caminos que tenga.
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Se recoge el estado de cada una de las posibles salidas de cada celda.
                bool muroN = listaCeldas[i, j].GetComponent<Cell>().getNorte();
                bool muroS = listaCeldas[i, j].GetComponent<Cell>().getSur();
                bool muroO = listaCeldas[i, j].GetComponent<Cell>().getOeste();
                bool muroE = listaCeldas[i, j].GetComponent<Cell>().getEste();

                // Se reinician algunos estados de las celdas y se recogen los hijos, para posteriormente realizar los algoritmos.
                listaCeldas[i, j].GetComponent<Cell>().hacerNoVisitado();
                listaCeldas[i, j].GetComponent<Cell>().resetPadre();
                listaCeldas[i, j].GetComponent<Cell>().rellenarHijos(listaCeldas);

                // Se van analizando los casos para asignar una imagen según cada uno.
                // Caso 0
                if (muroN == false && muroS == false && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case0");
                }

                // Caso 1
                if (muroN == true && muroS == false && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case1");
                }

                // Caso 2
                if (muroN == false && muroS == false && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case2");
                }

                // Caso 3
                if (muroN == false && muroS == true && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case3");
                }

                // Caso 4
                if (muroN == false && muroS == false && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case4");
                }

                // Caso 5
                if (muroN == true && muroS == false && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case5");
                }

                // Caso 6
                if (muroN == false && muroS == true && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case6");
                }

                // Caso 7
                if (muroN == false && muroS == true && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case7");
                }

                // Caso 8
                if (muroN == true && muroS == false && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case8");
                }

                // Caso 9
                if (muroN == false && muroS == false && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case9");
                }

                // Caso 10
                if (muroN == true && muroS == true && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case10");
                }

                // Caso 11
                if (muroN == true && muroS == false && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case11");
                }

                // Caso 12
                if (muroN == true && muroS == true && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case12");
                }

                // Caso 13
                if (muroN == false && muroS == true && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case13");
                }

                // Caso 14
                if (muroN == true && muroS == true && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case14");
                }

                // Caso 15
                if (muroN == true && muroS == true && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case15");
                }
            }
        }
    }

    // Método que permitirá borrar el laberinto que se ha creado con anterioridad
    public void destroyMaze()
    {
        // Se vuelve a visualizar el botón por si se viene de un momento en el que estuviera desactivado.
        btnCreate.SetActive(true);
        
        // Se observa si existe laberinto como tal y se va destruyendo lo que hay por pantalla
        if (listaCeldas != null)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    GameObject.Destroy(listaCeldas[i, j]);
                }
            }
        }
    }

    // Método para buscar una solución al laberinto según el método escogido
    public void getSolution()
    {
        // Se visualiza el botón que debería verse tras obtener una solución
        btnVis.SetActive(true);

        // Variable que ira acumulando las celdas que se irán visitando según el algoritmo escogido.
        colaDeBusqueda = new Queue<Cell>();

        // Con este método se establece, a partir de la casilla de salida, el grafo posible del laberinto.
        crearGrafo(colaDeBusqueda, listaCeldas);
        colaDeBusqueda.Clear();

        // Se establece la meta y el inicio del laberinto, por si se estuviera buscando poder escoger la celda que se quiera.
        int xInicio = 0;
        int yInicio = 0;
        int xFinal = (dimension - 1);
        int yFinal = (dimension - 1);

        // Según el algoritmo escogido se realizarán unas operaciones u otras.

        if (this.nombreAlgo.Equals("En amplitud"))
        {
            // Establecemos la búsqueda en amplitud, indicando la celda de inicio y empezando a buscar.
            colaDeBusqueda = busquedaEnAmplitud(colaDeBusqueda, listaCeldas, xInicio, yInicio, dimension);

            // Se va preparando la visualización
            imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1");

        }

        if (this.nombreAlgo.Equals("En profundidad"))
        {
            // Establecemos la búsqueda en profundidad, indicando la celda de inicio y empezando a buscar.
            colaDeBusqueda = busquedaEnProfundidad(colaDeBusqueda, listaCeldas, xInicio, yInicio);

            // Se va preparando la visualización
            imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2");

        }

        if (this.nombreAlgo.Equals("A Estrella"))
        {
            // Para poder aplicar A estrella, lo primero es calcular la H que será la distancia Manhattan de cada celda.
            listaCeldas = calculoHCeldas(listaCeldas, dimension, xFinal, yFinal);

            // Como otra variable de la heurística, es decir, G(x) se ha establecido el coste acumulado por cada celda.
            listaCeldas = calculoGCeldas(listaCeldas);

            // Un paso adicional para poder realizar los cálculos mejor es la ordenación de los hijos, según los valores de A estrella.
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    listaCeldas[i, j].GetComponent<Cell>().ordenarHijosPorCoste();
                }
            }

            // Con todas las variables para la A estrella calculados se estbalece la cola de búsqueda.
            colaDeBusqueda = busquedaEnAEstrella(colaDeBusqueda, listaCeldas, xInicio, yInicio, (dimension - 1), (dimension - 1));

            // Tras realizar toda la búsqueda, se decide ordenar las celdas por su coste.
            ordenarColaAEstrella();

            // Se va preparando la visualización
            imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3");
        }

    }

    // Para poder establecer las búsquedas en grafo, se ha decidido transcribir el laberinto en forma de grafo teniendo en mente la casilla inicial.
    public void crearGrafo(Queue<Cell> cola, GameObject[,] lista)
    {
        // Se va a utilizar la estructura de datos de colas, para poder ir almacenando los hijos e ir explorándolos.
        cola.Enqueue(lista[0, 0].GetComponent<Cell>());

        // Hasta que la cola se quede enteramente vacía, se va a ir analizando los hijos y se van inicializando los padres y las diferentes direcciones.
        while (cola.Count > 0)
        {
            Cell aux = cola.Peek();

            for (int i = 0; i < aux.getHijos().Length; i++)
            {
                if (aux.getHijos()[i] != aux.getPadre())
                {
                    aux.getHijos()[i].setPadre(aux);
                    cola.Enqueue(aux.getHijos()[i]);
                }
            }

            // Cada vez que se ha explorado una celda, se elimina de la cola.
            cola.Dequeue();
        }
    }

    // Con el fin de poder explorar bien el orden de las celdas según el algoritmo, se devolverá una cola.
    public Queue<Cell> busquedaEnAmplitud(Queue<Cell> cola, GameObject[,] lista, int primeraX, int primeraY, int dim)
    {
        // Se establece el inicio de la cola con la casilla de salida.
        Queue<Cell> auxCola = new Queue<Cell>();
        cola.Enqueue(lista[primeraX, primeraY].GetComponent<Cell>());
        int conteo = 0;

        // Mientras el conteo de casillas sea menor que el número total, se seguirán añadiendo celdas a la cola.
        while (conteo < (dim * dim))
        {
            Cell aux = cola.Peek();

            // Deberá explorar los hijos en su amplitud y mantener eso para todos los niveles.
            for (int i = 0; i < aux.getHijos().Length; i++)
            {
                if (aux.getHijos()[i] != aux.getPadre())
                {
                    cola.Enqueue(aux.getHijos()[i]);
                }
            }
            auxCola.Enqueue(cola.Peek());
            conteo++;

            // Una vez que se ha completado la exploración de una celda, se quitará de la cola.
            cola.Dequeue();
        }

        // Con la cola ya completa, se devolverá al programa principal.
        return auxCola;
    }

    // Repitiendo el esquema de la búsqueda en amplitud, también se devolverá una cola con el orden de recorrido de las celdas.
    public Queue<Cell> busquedaEnProfundidad(Queue<Cell> cola, GameObject[,] lista, int actualX, int actualY)
    {
        // Se recurrirá a un método recursivo, ya que va a estar buscando hasta encontrar un nodo hoja, luego volviendo a su hermano y repitiendo la hazaña.
        if (lista[actualX, actualY].GetComponent<Cell>().getHijos().Length == 1 && lista[actualX, actualY].GetComponent<Cell>().getPadre() != null)
        {
            cola.Enqueue(lista[actualX, actualY].GetComponent<Cell>());
            return cola;
        }

        // Para cuando no sea un caso de nodo hoja simplemente lo añade y sigue hacia su primer hijo, metiéndolo en la cola
        else
        {
            Cell aux = lista[actualX, actualY].GetComponent<Cell>();
            cola.Enqueue(aux);
            for (int i = 0; i < aux.getHijos().Length; i++)
            {
                if (aux.getHijos()[i] != aux.getPadre())
                {
                    cola = busquedaEnProfundidad(cola, lista, lista[actualX, actualY].GetComponent<Cell>().getHijos()[i].getX(),
                    lista[actualX, actualY].GetComponent<Cell>().getHijos()[i].getY());
                }
            }
            return cola;
        }
        // Tanto en un caso como en otro al final se devolverá una cola para poder ver el orden en que se ha recorrido.
    }

    // Va a ser importante saber la forma en la que se calculan las variables de A estrella, ya que el orden influirá.
    // Como el coste H no depende de otra variable sino de la disposición de las celdas en el laberinto, lo que se hará será calcualrlo en orden.
    public GameObject[,] calculoHCeldas(GameObject[,] lista, int dim, int xFin, int yFin)
    {
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                // Básicamente se aplica el cálculo de la distancia manhattan de cada celda a la meta.
                lista[i, j].GetComponent<Cell>().calculoCosteH(xFin, yFin);
            }
        }

        return lista;
    }

    // Se necesita otro coste más para poder entender al completo este algorito y se realiza con el coste acumulado de H.
    public GameObject[,] calculoGCeldas(GameObject[,] lista)
    {
        Queue<Cell> cola = new Queue<Cell>();

        cola.Enqueue(lista[0, 0].GetComponent<Cell>());

        // Analizamos cada una de las celdas y le vamos calculando el coste final G
        while (cola.Count > 0)
        {
            Cell aux = cola.Peek();

            for (int i = 0; i < aux.getHijos().Length; i++)
            {
                if (aux.getHijos()[i] != aux.getPadre())
                {
                    aux.getHijos()[i].calculoCosteG();
                    cola.Enqueue(aux.getHijos()[i]);
                }
            }
            cola.Dequeue();
        }

        return lista;
    }

    // Método que realiza la búsqueda del nodo celda teniendo en mente todo lo que tiene que ver con los costes.
    public Queue<Cell> busquedaEnAEstrella(Queue<Cell> cola, GameObject[,] lista, int actualX, int actualY, int metaX, int metaY)
    {
        // Se comprueba si es hijo único y de ser así se añade y nada más.
        if (lista[actualX, actualY].GetComponent<Cell>().getHijos().Length == 1 && lista[actualX, actualY].GetComponent<Cell>().getPadre() != null)
        {
            cola.Enqueue(lista[actualX, actualY].GetComponent<Cell>());
            return cola;
        }

        // En caso de que no sea el único hijo lo que se hace es ir añadiendo los hijos de forma ordenada
        else
        {
            Cell aux = lista[actualX, actualY].GetComponent<Cell>();
            cola.Enqueue(aux);

            for (int i = 0; i < aux.getHijosOrdenados().Count; i++)
            {
                if (aux.getHijosOrdenados()[i] != aux.getPadre())
                {
                    cola = busquedaEnAEstrella(cola, lista, lista[actualX, actualY].GetComponent<Cell>().getHijosOrdenados()[i].getX(),
                    lista[actualX, actualY].GetComponent<Cell>().getHijosOrdenados()[i].getY(), metaX, metaY);
                }
            }
            return cola;
        }
    }

    // Método que permite ordenar las celdas de la búsqueda de A* según su coste, una vez realizado todo
    public void ordenarColaAEstrella()
    {
        // Con una lista de auxiliar se realizará la comparativa según el coste, usando un delegate y un compareTo
        List<Cell> listaAux = new List<Cell>();
        foreach (Cell c in colaDeBusqueda)
        {
            listaAux.Add(c);
        }
        listaAux.Sort(delegate (Cell x, Cell y)
        {
            return x.getCoste().CompareTo(y.getCoste());
        });

        // Se reincia la cola de búsqueda y se vuelve a rellenar con toda la lista ordenada.
        colaDeBusqueda.Clear();
        foreach (Cell c in listaAux)
        {
            colaDeBusqueda.Enqueue(c);
        }
    }

    // Método para visualizar la solución
    public void visualizarSol()
    {
        // Se realizan las inicializaciones de la parte de navegación visual, tanto estéticamente como técnicamente
        inicializacionVisualElementos();
        inicializacionTecnicaElementos();

    }

    // Método para poder comenzar con la visualizacion de elementos de navegación en forma directa
    public void inicializacionVisualElementos()
    {
        // Se deja ver la imagen que representa el algoritmo
        imagenAlgo.SetActive(true);
        btnInfo.SetActive(true);

        // Se deja ver el botón que da el siguiente paso
        btnNext.SetActive(true);

        // Se deja ver el texto que sirve para la navegación
        textNext.SetActive(true);
        GameObject.Find("textoCeldaProxima").GetComponent<TMPro.TMP_Text>().SetText("Vamos a [" + colaDeBusqueda.Peek().getX() + "," + colaDeBusqueda.Peek().getY() + "]");
    }

    // Método para inicializar todo lo que tiene que ver con la parte técnica de la navegación
    public void inicializacionTecnicaElementos()
    {
        // Se va comienza con el conteo de la visualización
        conteoVis = 0;

        // Se crean de cero las filas que van a ayudar en la navegación
        filaDeNavegacionAvance = new Stack<Cell>();
        filaDeNavegacionRetroceso = new Stack<Cell>();

        // Se deja a false el caso de la meta alcanzada
        metaAlcanzadaVis = false;
    }

    // Método que se ejecuta al pulsar el bótón de siguiente en la navegación
    public void nextInstruccion()
    {
        // Mientras la meta no sea alcanzada se hará una serie de situaciones, de lo contrario no
        if (!metaAlcanzadaVis)
        {
            // Para cuando se pueda dar para atrás, el botón de paso atrás aparecerá
            if (conteoVis == 0)
            {
                btnPrev.SetActive(true);
            }

            // Por cada vez que se presione el botón de siguiente se aumentará 1 al conteo de visualización
            conteoVis++;

            // Si el conteo de visualización es mayor a 0 y está comenzando a comprobar una casilla se realizará lo siguiente
            if (conteoVis % 3 == 1 && conteoVis > 0)
            {
                GameObject.Find("textoPasosFinal").GetComponent<TMPro.TMP_Text>().SetText("Paso " + ((conteoVis / 3) + 1));

                // Hay que comprobar si está viniendo de la cola inicial o de la navegación de retroceso

                // Lo primero es comprobar el texto de la casilla actual en la que se encuentra la navegación
                if (filaDeNavegacionRetroceso.Count >= 1)
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [" + filaDeNavegacionRetroceso.Peek().getX() + "," + filaDeNavegacionRetroceso.Peek().getY() + "]");
                }
                else
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [ , ]");
                }

                // Se comprueba si debe de existir un texto de casilla previa o no
                if (filaDeNavegacionRetroceso.Count >= 2)
                {
                    textPrev.SetActive(true);
                    GameObject.Find("textoCeldaPrevia").GetComponent<TMPro.TMP_Text>().SetText("Venimos de [" + filaDeNavegacionRetroceso.ToArray()[1].getX() + "," + filaDeNavegacionRetroceso.ToArray()[1].getY() + "]");
                }
                else
                {
                    textPrev.SetActive(false);
                }

                // Lo último a realizar es conocer el texto de la casilla a la que se va a dirigir tras esto
                if (filaDeNavegacionAvance.Count > 0)
                {
                    celdaNav = filaDeNavegacionAvance.Peek();
                    GameObject.Find("textoCeldaProxima").GetComponent<TMPro.TMP_Text>().SetText("Vamos a [" + celdaNav.getX() + "," + celdaNav.getY() + "]");
                }
                else
                {
                    celdaNav = colaDeBusqueda.Peek();
                    GameObject.Find("textoCeldaProxima").GetComponent<TMPro.TMP_Text>().SetText("Vamos a [" + celdaNav.getX() + "," + celdaNav.getY() + "]");
                }
            }

            // Para el caso de que se vaya a realizar un cambio de casilla a comprobar en la navegación hay que tener en mente el punto al que se partirá
            if (conteoVis % 3 == 0)
            {
                // Será conveniente diferenciar si se viene de la fila de avance o de la cola de búsqueda inicial
                if (filaDeNavegacionAvance.Count > 0)
                {
                    filaDeNavegacionAvance.Pop();

                }
                else
                {
                    colaDeBusqueda.Dequeue();
                }

                // En cualquiera de los casos, la fila de retroceso aumentará uno
                filaDeNavegacionRetroceso.Push(celdaNav);
            }

            // Se modifica la visualización total de los elementos
            modificacionVisualizacion();

            // Se comprueba si el próximo elemento a analizar es la meta
            if (conteoVis % 3 == 1 && (celdaNav.getX() == (dimension - 1) && celdaNav.getY() == (dimension - 1)))
            {
                metaAlcanzadaVis = true;
            }
        }
        else
        {
            // Para el caso de que sea la meta, se va a realizar un último paso y visualización de elementos.
            if (conteoVis % 3 != 1)
            {
                conteoVis++;
            }
            modificacionVisualizacion();

            if (celdaNav.getX() == (dimension - 1) && celdaNav.getY() == (dimension - 1))
            {
                // Se desactivan diferentes elementos visuales y se deja ver la ventana resumen
                toResumeWindow();
            }
            else
            {
                metaAlcanzadaVis = false;
            }
        }
    }

    // Método que trata de colocar la ventana de resumen al final de analizar un laberinto
    public void toResumeWindow()
    {
        // Hace invisible el espacio de visualización del algoritmo
        imagenAlgo.SetActive(false);
        btnInfo.SetActive(false);

        // Hace invisibles los botones de navegación
        btnNext.SetActive(false);
        btnPrev.SetActive(false);

        // Hace invisibles los textos de navegación
        textActual.SetActive(false);
        textMeta.SetActive(false);
        textNext.SetActive(false);
        textPrev.SetActive(false);
        textPasos.SetActive(false);

        // Hace invisibles los botones de interfaz previa del laberinto
        btnCreate.SetActive(false);
        btnSol.SetActive(false);
        btnVis.SetActive(false);

        // Se hace visible la ventana de resumen de todo el proceso
        ventanaFinal.SetActive(true);

        rellenarMazeFinal();

        // Modificación del texto para poder indicar el resumen de todo
        GameObject.Find("textFinal").GetComponent<TMPro.TMP_Text>().SetText("El algoritmo:\n " + this.nombreAlgo + " ha tardado " + ((conteoVis / 3)+1) + " pasos.");

        // Según el tipo de algoritmo que se esté analizando se mostrará unos resultados u otros de botones
        if (this.nombreAlgo.Equals("En amplitud"))
        {
            btnAlgo1.SetActive(false);
            btnAlgo2Izq.SetActive(true);
            btnAlgo2Dch.SetActive(false);
            btnAlgo3.SetActive(true);
        }

        if (this.nombreAlgo.Equals("En profundidad"))
        {
            btnAlgo1.SetActive(true);
            btnAlgo2Izq.SetActive(false);
            btnAlgo2Dch.SetActive(false);
            btnAlgo3.SetActive(true);
        }

        if (this.nombreAlgo.Equals("A Estrella"))
        {
            btnAlgo1.SetActive(true);
            btnAlgo2Izq.SetActive(false);
            btnAlgo2Dch.SetActive(true);
            btnAlgo3.SetActive(false);
        }

    }

    // Se trata de colocar la parte final del laberinto con el resumen de los pasos tomados.
    public void rellenarMazeFinal()
    {
        // Se realizan los cambios pertinentes dentro del laberinto
        foreach (Cell c in filaDeNavegacionRetroceso)
        {
            listaCeldas[c.getX(), c.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon3");
        }

        Cell aux = celdaNav;

        listaCeldas[aux.getX(), aux.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon2");
        while (aux.getPadre() != null)
        {
            aux = aux.getPadre();
            listaCeldas[aux.getX(), aux.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon2");
        }
    }

    // Método para realizar el comportamiento del botón de paso hacia atrás
    public void prevInstruccion()
    {
        // Control de que desaparezca este botón en caso de que no haga falta
        if (conteoVis - 1 == 0)
        {
            btnPrev.SetActive(false);
            GameObject.Find("textoPasosFinal").GetComponent<TMPro.TMP_Text>().SetText("Paso 0");

            if (filaDeNavegacionRetroceso.Count > 0)
            {
                filaDeNavegacionAvance.Push(filaDeNavegacionRetroceso.Pop());
            }
        }

        // Restar uno al conteo de visualización para tener en cuenta los pasos a realizar
        conteoVis--;

        // Analizar la situación para poder mostrar un texto por pantalla u otro
        if (conteoVis % 3 == 0 && conteoVis != 0)
        {
            GameObject.Find("textoPasosFinal").GetComponent<TMPro.TMP_Text>().SetText("Paso " + (conteoVis / 3));

            // Hay que tener en mente el lugar del que se va a realizar la navegación hacia atrás
            if (celdaNav.getX() == filaDeNavegacionRetroceso.Peek().getX() && celdaNav.getY() == filaDeNavegacionRetroceso.Peek().getY())
            {
                filaDeNavegacionAvance.Push(filaDeNavegacionRetroceso.Pop());
                celdaNav = filaDeNavegacionRetroceso.Peek();

                // Se valoran los textos de la celda actual
                if (filaDeNavegacionRetroceso.Count >= 1)
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [" + filaDeNavegacionRetroceso.Peek().getX() + "," + filaDeNavegacionRetroceso.Peek().getY() + "]");
                }
                else
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [ , ]");
                }

                // Se valoran los textos de la celda anterior
                if (filaDeNavegacionRetroceso.Count >= 2)
                {
                    textPrev.SetActive(true);
                    GameObject.Find("textoCeldaPrevia").GetComponent<TMPro.TMP_Text>().SetText("Venimos de [" + filaDeNavegacionRetroceso.ToArray()[1].getX() + "," + filaDeNavegacionRetroceso.ToArray()[1].getY() + "]");
                }
                else
                {
                    textPrev.SetActive(false);
                }

                // Se analiza el próximo destino
                GameObject.Find("textoCeldaProxima").GetComponent<TMPro.TMP_Text>().SetText("Vamos a [" + celdaNav.getX() + "," + celdaNav.getY() + "]");
                
            }
            else
            {
                celdaNav = filaDeNavegacionRetroceso.Pop();
                filaDeNavegacionAvance.Push(celdaNav);

                // Se valoran los textos de la celda actual
                if (filaDeNavegacionRetroceso.Count >= 1)
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [" + filaDeNavegacionRetroceso.Peek().getX() + "," + filaDeNavegacionRetroceso.Peek().getY() + "]");
                }
                else
                {
                    GameObject.Find("textoCeldaActual").GetComponent<TMPro.TMP_Text>().SetText("Estamos en [ , ]");
                }

                // Se valoran los textos de la celda anterior
                if (filaDeNavegacionRetroceso.Count >= 2)
                {
                    textPrev.SetActive(true);
                    GameObject.Find("textoCeldaPrevia").GetComponent<TMPro.TMP_Text>().SetText("Venimos de [" + filaDeNavegacionRetroceso.ToArray()[1].getX() + "," + filaDeNavegacionRetroceso.ToArray()[1].getY() + "]");
                }
                else
                {
                    textPrev.SetActive(false);
                }

                // Se analiza el próximo destino
                GameObject.Find("textoCeldaProxima").GetComponent<TMPro.TMP_Text>().SetText("Vamos a [" + celdaNav.getX() + "," + celdaNav.getY() + "]");
                
            }

        }

        modificacionVisualizacion();

    }

    // Método que permite cambiar la visualización en el laberinto
    public void modificacionVisualizacion()
    {
        // Para el caso del algoritmo 1 se realizarán los siguientes cambios
        if (this.nombreAlgo.Equals("En amplitud"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1");
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
            }
            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_1");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon");

            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_2");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon1");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_3");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon2");
            }


        }
        if (this.nombreAlgo.Equals("En profundidad"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2");
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_1");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon");
            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_2");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon1");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_3");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon2");
            }
        }
        if (this.nombreAlgo.Equals("A Estrella"))
        {
            if (conteoVis == 0)
            {
                
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3");
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");

            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_1");
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(1).gameObject.SetActive(false);

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon");

            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_2");

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon1");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_3");

                foreach (Cell c in celdaNav.getHijosOrdenados())
                {
                    listaCeldas[c.getX(), c.getY()].transform.GetChild(1).gameObject.SetActive(true);
                    listaCeldas[c.getX(), c.getY()].transform.GetChild(1).GetComponent<TMPro.TMP_Text>().SetText(""+c.getCoste());
                }

                listaCeldas[celdaNav.getX(), celdaNav.getY()].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/stepsIcon2");
            }
        }
    }

    public void reDibujarContenidoLaberinto()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (i == 0 && j == 0)
                {
                    listaCeldas[i, j].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/startIcon");
                }
                else if (i == (dimension-1) && j == (dimension - 1))
                {
                    listaCeldas[i, j].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/metaIcon");
                }
                else
                {
                    listaCeldas[i,j].transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
                }
            }
        }
    }

    // Método que se activa al presionar el botónd de información
    public void infoZone()
    {
        interfazInfoActivation();

        activacionImagenInfo();

    }
    
    // Méetodo que activa todo lo que tiene que ver con la zona de información.
    public void interfazInfoActivation()
    {
        // Se desactivan los botones de navegación en primer lugar.
        btnPrev.SetActive(false);
        btnNext.SetActive(false);

        // Se desactivan los botones de visualización
        btnCreate.SetActive(false);
        btnSol.SetActive(false);
        btnVis.SetActive(false);

        // Se oculta el boton info y se muestra el de salir.
        btnInfo.SetActive(false);
        btnSalirInfo.SetActive(true);
    }

    // Activará lo que tenga que verse por pantalla según el estado.
    public void activacionImagenInfo()
    {
        // Se modifica la imagen del algo
        if (this.nombreAlgo.Equals("En amplitud"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1info");
            }
            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1info_1");
            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1info_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1info_3");
            }
        }

        if (this.nombreAlgo.Equals("En profundidad"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2info");
            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2info_1");
            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2info_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2info_3");
            }
        }
        if (this.nombreAlgo.Equals("A Estrella"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3info");
            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3info_1");

            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3info_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3info_3");
            }
        }
    }

    // Método para poder salir de la información y traer el curso de nuevo.
    public void exitInfoZone()
    {
        interfazInfoDesactivation();

        desactivacionImagenInfo();
    }

    // Método para desactivar la zona de información
    public void interfazInfoDesactivation()
    {
        // Se desactivan los botones de navegación en primer lugar.
        if (conteoVis > 0)
        {
            btnPrev.SetActive(true);
        }
        btnNext.SetActive(true);

        // Se activan los botones de visualización
        btnCreate.SetActive(true);
        btnSol.SetActive(true);
        btnVis.SetActive(true);

        // Se oculta el boton de salir y se muestra el de info.
        btnSalirInfo.SetActive(false);
        btnInfo.SetActive(true);

    }
    
    public void desactivacionImagenInfo()
    {
        // Se modifica la imagen del algo
        if (this.nombreAlgo.Equals("En amplitud"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1");
            }
            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_1");
            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo1_3");
            }
        }

        if (this.nombreAlgo.Equals("En profundidad"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2");
            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_1");
            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo2_3");
            }
        }
        if (this.nombreAlgo.Equals("A Estrella"))
        {
            if (conteoVis == 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3");
            }

            if (conteoVis % 3 == 1)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_1");

            }

            if (conteoVis % 3 == 2)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_2");
            }

            if (conteoVis % 3 == 0 && conteoVis != 0)
            {
                imagenAlgo.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/algo3_3");
            }
        }
    }

    // Los diferentes métodos para volver a hacer los algoritmos.
    public void reHacerAlgo1()
    {
        this.nombreAlgo = "En amplitud";
        GameObject.Find("textAlgo").GetComponent<TMPro.TMP_Text>().SetText(this.nombreAlgo);

        ventanaFinal.SetActive(false);

        reDibujarLaberinto();
        reDibujarContenidoLaberinto();

        btnSol.SetActive(true);

        // Se deja visualizar y se inicializan los textos de meta y actual.
        inicializacionTextos();
    }

    public void reHacerAlgo2()
    {
        this.nombreAlgo = "En profundidad";
        GameObject.Find("textAlgo").GetComponent<TMPro.TMP_Text>().SetText(this.nombreAlgo);

        ventanaFinal.SetActive(false);

        reDibujarLaberinto();
        reDibujarContenidoLaberinto();

        btnSol.SetActive(true);

        // Se deja visualizar y se inicializan los textos de meta y actual.
        inicializacionTextos();
    }

    public void reHacerAlgo3()
    {
        this.nombreAlgo = "A Estrella";
        GameObject.Find("textAlgo").GetComponent<TMPro.TMP_Text>().SetText(this.nombreAlgo);

        ventanaFinal.SetActive(false);

        reDibujarLaberinto();
        reDibujarContenidoLaberinto();

        btnSol.SetActive(true);

        // Se deja visualizar y se inicializan los textos de meta y actual.
        inicializacionTextos();
    }

    // Redibujado del laberinto cuando se está partiendo de un recorrido ya hecho.
    public void reDibujarLaberinto()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Se recoge el estado de cada una de las posibles salidas de cada celda.
                bool muroN = listaCeldas[i, j].GetComponent<Cell>().getNorte();
                bool muroS = listaCeldas[i, j].GetComponent<Cell>().getSur();
                bool muroO = listaCeldas[i, j].GetComponent<Cell>().getOeste();
                bool muroE = listaCeldas[i, j].GetComponent<Cell>().getEste();

                listaCeldas[i, j].GetComponent<Image>().color = Color.white;

                // Se van analizando los casos para asignar una imagen según cada uno.
                // Caso 0
                if (muroN == false && muroS == false && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case0");
                }

                // Caso 1
                if (muroN == true && muroS == false && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case1");
                }

                // Caso 2
                if (muroN == false && muroS == false && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case2");
                }

                // Caso 3
                if (muroN == false && muroS == true && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case3");
                }

                // Caso 4
                if (muroN == false && muroS == false && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case4");
                }

                // Caso 5
                if (muroN == true && muroS == false && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case5");
                }

                // Caso 6
                if (muroN == false && muroS == true && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case6");
                }

                // Caso 7
                if (muroN == false && muroS == true && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case7");
                }

                // Caso 8
                if (muroN == true && muroS == false && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case8");
                }

                // Caso 9
                if (muroN == false && muroS == false && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case9");
                }

                // Caso 10
                if (muroN == true && muroS == true && muroO == false && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case10");
                }

                // Caso 11
                if (muroN == true && muroS == false && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case11");
                }

                // Caso 12
                if (muroN == true && muroS == true && muroO == false && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case12");
                }

                // Caso 13
                if (muroN == false && muroS == true && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case13");
                }

                // Caso 14
                if (muroN == true && muroS == true && muroO == true && muroE == false)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case14");
                }

                // Caso 15
                if (muroN == true && muroS == true && muroO == true && muroE == true)
                {
                    listaCeldas[i, j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/case15");
                }
            }
        }
    }
}