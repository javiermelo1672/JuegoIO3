using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazGranero : MonoBehaviour {

    public GameObject Espacio;
    public GameObject Elementos;
    public GameObject interfazGranero;
    public GameObject mundo;
    SistemaAlmacenamiento sistemaAlmacenamiento; 

	// Use this for initialization
	void Start () {
        sistemaAlmacenamiento = mundo.GetComponent<SistemaAlmacenamiento>();
	}

    public void MostrarInterfaz()
    {
        interfazGranero.SetActive(true);
        foreach(GameObject elemento in sistemaAlmacenamiento.ObtenerInventario())
        {
            GameObject e = Instantiate(Espacio);
            e.transform.SetParent(Elementos.transform,false);

            //Colocar sprite al item de la interfaz
            e.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite= elemento.GetComponent<Item>().icono;

            //Colocar el texto del item de la interfaz
            e.transform.GetChild(1).GetComponent<Text>().text = elemento.GetComponent<Item>().nombre;

            //Colocar el texto de tiempo restante de la interfaz
            int tiempo = elemento.GetComponent<Item>().tiempoLimite - elemento.GetComponent<Item>().tiempoActual;
            string tiempoRestante = "Tiempo Restante: " + tiempo + " min";
            e.transform.GetChild(2).GetComponent<Text>().text = tiempoRestante;
        }
        EstablecerValorCapacidad();
    }

    public void CerrarInterfaz()
    {
        interfazGranero.SetActive(false);
        foreach(Transform espacio in Elementos.transform)
        {
            Destroy(espacio.gameObject);
        }
    }

    public void EstablecerValorCapacidad()
    {
        string texto = sistemaAlmacenamiento.ObtenerInventario().Count +" / "+ sistemaAlmacenamiento.ObtenerLimiteAlmacenamiento();
        interfazGranero.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Capacidad: "+texto;
    }

    public SistemaAlmacenamiento getSistema()
    {
        return sistemaAlmacenamiento;
    }



}
