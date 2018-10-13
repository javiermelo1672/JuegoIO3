using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaAlmacenamiento : MonoBehaviour {

    public GameObject cuadricula;
    public int capacidadGranero = 20;
    private List<GameObject> inventarioGranero;
    public List<GameObject> todosItems;

    void Start()
    {
        inventarioGranero = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AgregarItem("A");
        }
    }

    public bool AgregarItem(string nombre)
    {
        if (inventarioGranero.Count < ObtenerLimiteAlmacenamiento())
        {

            GameObject item = new GameObject();
            for (int i = 0; i < todosItems.Count; i++)
            {
                if (todosItems[i].name == nombre)
                {
                    item = todosItems[i];
                    inventarioGranero.Add(item);
                    return true;
                }
            }
        }
        return false;
    }

    //Verifica la cantidad de graneros que tenga y suma la capacidad de todos estos
    public int ObtenerLimiteAlmacenamiento()
    {
        int capacidad = 0;
        foreach (Transform cuadro in cuadricula.transform)
        {
            if (cuadro.childCount!=0 && cuadro.GetChild(0).name == "Granero(Clone)")
            {
                capacidad = capacidad + capacidadGranero;
            }
        }

        return capacidad;
    }


    public List<GameObject> ObtenerInventario()
    {
        return inventarioGranero;
    }




}
