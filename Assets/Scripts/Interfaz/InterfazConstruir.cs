using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazConstruir : MonoBehaviour {

    public GameObject interfazConstruir;
    public GameObject mundo;
    ControladorCuadricula cuadricula;

    public GameObject granero;

    void Start()
    {
        cuadricula = mundo.GetComponent<ControladorCuadricula>();
    }

    public void ConstruirEdificio(GameObject EspacioNombre)
    {
        if (EspacioNombre.GetComponent<Text>().text == "Granero")
        {
            int precioGranero = 50000;
            
            
            //$$$$$$$$$$$$$$$$$$$---FALTA VERIFICAR QUE TENGA EL DINERO PARA COMPRARLO



            cuadricula.colocarObjeto(granero);
            CerrarInterfaz();
        }
    }

    public void MostrarInterfaz()
    {
        interfazConstruir.SetActive(true);

    }

    public void CerrarInterfaz()
    {
        interfazConstruir.SetActive(false);
    }

}
