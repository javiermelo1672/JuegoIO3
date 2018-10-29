using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazComprar : MonoBehaviour
{


    public GameObject mundo;
    public GameObject interfazError;

     SistemaAlmacenamiento granero;

    void Start()
    {
        granero = mundo.GetComponent<SistemaAlmacenamiento>();
    }

    public void ComprarSemilla(GameObject item)
    {
        int dinero = 10000;
        if (item.GetComponent<Item>().valor <= dinero)
        {
            if (!granero.AgregarItem(item.name))
            {
                interfazError.transform.GetChild(1).GetComponent<Text>().text = "El granero está lleno";
                interfazError.SetActive(true);
            }
        }
        else
        {
            interfazError.transform.GetChild(1).GetComponent<Text>().text = "No tiene dinero suficiente para comprar esto";
            interfazError.SetActive(true);
        }
    }

    
}
