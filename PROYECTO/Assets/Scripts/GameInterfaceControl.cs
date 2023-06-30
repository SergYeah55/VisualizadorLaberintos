using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Se trata de la clase que va a gestionar toda la interfaz
public class GameInterfaceControl : MonoBehaviour
{
    // Se declaran los objetos que harán de interfaces
    public GameObject startInterface;    
    public GameObject selectionInterface;
    public GameObject gameInterface;

    // Se declaran los elementos de interfaz que se usarán más adelante

    // Variables del menú de selección
    public GameObject selectorAlg;
    public GameObject selectorDim;
    private int algoritmo;
    private int dim;

    // Variables del menú de juego
    public GameObject titleAlgo;
    public GameObject numDim;

    // Variables de texto de la navegación
    public GameObject textMeta;
    public GameObject textPasos;
    public GameObject textPrev;
    public GameObject textActual;
    public GameObject textNext;

    // Variables de botones de navegación
    public GameObject btnSol;
    public GameObject btnVis;
    public GameObject btnPrev;
    public GameObject btnNext;

    // Variables de imagen de algoritmo
    public GameObject imagenAlgo;
    public GameObject btnInfo;
    public GameObject btnSalirInfo;

    // Variable de ventana resumen
    public GameObject ventanaFinal;

    // Nada más comenzar tan solo se verá el menú principal
    void Start()
    {   
        // Se desactivan las interfaces que no se van a ver
        selectionInterface.SetActive(false);
        gameInterface.SetActive(false);

    }

    // Se trata del método que nos lleva al menú de selección
    public void goToSelectionMode()
    {
        // Se desactivan las interfaces que no se van a ver y se activan los que sí
        startInterface.SetActive(false);
        gameInterface.SetActive(false);
        selectionInterface.SetActive(true);

    }

    // Se trata del método que nos lleva al menú de juego
    public void goToGameMode()
    {
        // Se desactivan los botones e imagen que no deberían verse con el método adecuado.
        inactivarInterfazJuego();

        // Hay que recoger el algoritmo que se ha escogido de la interfaz
        algoritmo = selectorAlg.GetComponent<TMPro.TMP_Dropdown>().value;

        // Hay que recoger el valor del ancho y el alto del laberinto según lo escogido por el usuario
        seleccionDimension();

        // Se declara la dimensión dentro del script que tiene que manejar la aplicación.
        gameInterface.GetComponent<GameFunctions>().setDimension(dim);
        numDim.GetComponent<TMPro.TMP_Text>().SetText("" + gameInterface.GetComponent<GameFunctions>().getDimension()+" x "+gameInterface.GetComponent<GameFunctions>().getDimension());

        // Realizamos lo mismo con el tipo de algoritmo escogido por el usuario
        gameInterface.GetComponent<GameFunctions>().setNombre(seleccionAlgo());
        titleAlgo.GetComponent<TMPro.TMP_Text>().SetText( gameInterface.GetComponent<GameFunctions>().getNombre());

        // Se desactivan las interfaces que no se van a ver y se activan las que sí
        selectionInterface.SetActive(false);
        gameInterface.SetActive(true);
    }

    // Método que permite desactivar la vista de elementos por pantalla en el menú de juego.
    public void inactivarInterfazJuego()
    {
        // Se desactiva la vista de las imágenes del algoritmo.
        imagenAlgo.SetActive(false);
        btnInfo.SetActive(false);
        btnSalirInfo.SetActive(false);

        // Se desactiva la vista de los botones de navegación.
        btnSol.SetActive(false);
        btnVis.SetActive(false);
        btnPrev.SetActive(false);
        btnNext.SetActive(false);

        // Se desactivan los textos de navegación.
        textMeta.SetActive(false);
        textPasos.SetActive(false);
        textPrev.SetActive(false);
        textActual.SetActive(false);
        textNext.SetActive(false);

        // Se desactiva la ventana de resumen final.
        ventanaFinal.SetActive(false);
    }

    // Método que permite identificar la dimensión que se va a tener en mente para el laberinto.
    public void seleccionDimension()
    {
        if (selectorDim.GetComponent<TMPro.TMP_Dropdown>().value == 0)
        {
            dim = 3;
        }
        else if (selectorDim.GetComponent<TMPro.TMP_Dropdown>().value == 1)
        {
            dim = 4;

        }
        else
        {
            dim = 6;
        }
    }

    // Método que permite tener en mente el algoritmo que se va a analizar en primer lugar.
    public string seleccionAlgo()
    {
        string nombre = null;
        switch (algoritmo)
        {
            case 0:
                nombre = "En amplitud";
                break;
            case 1:
                nombre = "En profundidad";
                break;
            case 2:
                nombre = "A Estrella";
                break;
        }
        return nombre;
    }

    public void salirApp()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
     
    }
}
