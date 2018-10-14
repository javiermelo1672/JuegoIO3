using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazSembrar : MonoBehaviour {

    public GameObject EspacioSembrar;
    public GameObject Elementos;
    public GameObject interfazSembrar;
    public GameObject mundo;
    SistemaAlmacenamiento sistemaAlmacenamiento;
    ControladorCuadricula cuadricula;

    public GameObject sembrado;


    void Start()
    {
        sistemaAlmacenamiento = mundo.GetComponent<SistemaAlmacenamiento>();
        cuadricula = mundo.GetComponent<ControladorCuadricula>();
    }

    public void SembrarItem(GameObject EspacioNombre)
    {
        GameObject sembradoOBJ = Instantiate(sembrado, new Vector3(0, 0, 0), Quaternion.identity);
        sembradoOBJ.GetComponent<ControlCultivo>().Iniciar(EspacioNombre.GetComponent<Text>().text, cuadricula.getCuadroDetectado(),cuadricula);
        cuadricula.colocarYReemplazarOBJ(sembradoOBJ);
        BorrarSemilla(EspacioNombre.GetComponent<Text>().text);
        CerrarInterfaz();
    }

    public void MostrarInterfaz()
    {
        interfazSembrar.SetActive(true);
        foreach (GameObject elemento in sistemaAlmacenamiento.ObtenerInventario())
        {
            if (elemento.layer == 11) //Layer 11 = Semilla
            {
                GameObject e = Instantiate(EspacioSembrar);
                e.transform.SetParent(Elementos.transform, false);

                //Colocar sprite al item de la interfaz
                e.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = elemento.GetComponent<Item>().icono;

                //Colocar el texto del item de la interfaz
                e.transform.GetChild(1).GetComponent<Text>().text = elemento.GetComponent<Item>().nombre;
            }
        }
    }

    public void CerrarInterfaz()
    {
        interfazSembrar.SetActive(false);
        foreach (Transform espacio in Elementos.transform)
        {
            Destroy(espacio.gameObject);
        }
    }

    public void BorrarSemilla(string nombre)
    {
        foreach (GameObject elemento in sistemaAlmacenamiento.ObtenerInventario())
        {
            if (elemento.GetComponent<Item>().nombre==nombre) 
            {
                sistemaAlmacenamiento.ObtenerInventario().Remove(elemento);
                break;
            }
        }
    }


}
