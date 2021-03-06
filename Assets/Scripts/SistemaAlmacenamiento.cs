﻿using System.Collections;
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
        InvokeRepeating("ActualizarTiempoDeItems", 60f, 60f);
    }

    //Agrega item: Lista de items: SemillaCafe, SemillaMaiz, SemillaPapa, SemillaRemolacha, SemillaYuca, ItemCafe, ItemMaiz, ItemPapa, ItemRemolacha, ItemYuca
    public bool AgregarItem(string nombre)
    {
        if (inventarioGranero.Count < ObtenerLimiteAlmacenamiento())
        {

            GameObject item;
            for (int i = 0; i < todosItems.Count; i++)
            {
                if (todosItems[i].name == nombre)
                {
                    item = Instantiate(todosItems[i]);
                    item.name = item.name.Replace("(Clone)", "");
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

    public int ObtenerCantidadDeItem(string nombre)
    {
        int cantidad = 0;
        for (int i = 0; i < inventarioGranero.Count; i++)
        {
            if (inventarioGranero[i].name == nombre)
            {
                cantidad = cantidad + 1;     
            }
        }
        return cantidad;
    }


    /*
     *LO SIGUIENTE ES USADO PARA MOSTRAR EN LA INTERFAZ DEL GRANERO(INVENTARIO)
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     */
    public string ObtenerTiemposItem(int i)
    {
        string tiempoActual = inventarioGranero[i].GetComponent<Item>().tiempoActual + "";
        string tiempoLimite = inventarioGranero[i].GetComponent<Item>().tiempoLimite + "";
        string tiempos = tiempoActual + " / " + tiempoLimite;
        return tiempos;
    }

    public Sprite ObtenerSprite(int i)
    {
        return inventarioGranero[i].GetComponent<Item>().icono;
    }

    public string ObtenerNombre(int i)
    {
        return inventarioGranero[i].name;
    }

    /*
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     */

    //Aumenta tiempo de todos los items en 1 unidad de tiempo (1 minuto), Si el tiempoActual superó al tiempoLimite, el item se elimina de la lista
    public void ActualizarTiempoDeItems()
    {
        for (int i = 0; i < inventarioGranero.Count; i++)
        {
            inventarioGranero[i].GetComponent<Item>().tiempoActual = inventarioGranero[i].GetComponent<Item>().tiempoActual + 1;
        }
        print("Tiempo de Items Actualizado");
        BorrarItemsExpirados();
    }

    public void BorrarItemsExpirados()
    {
        foreach(GameObject obj in inventarioGranero)
        {
            if(obj.GetComponent<Item>().tiempoActual> obj.GetComponent<Item>().tiempoLimite)
            {
                inventarioGranero.Remove(obj);
                Destroy(obj);
                BorrarItemsExpirados();
                break;
            }
        }
        
    }



    public List<GameObject> ObtenerInventario()
    {
        return inventarioGranero;
    }

    public List<GameObject> ObtenerListaItems()
    {
        return todosItems;
    }

    public bool BorrarItemSegunTipo(string tipo)
    {
        foreach (GameObject obj in inventarioGranero)
        {
            if (obj.name==tipo)
            {
                inventarioGranero.Remove(obj);
                Destroy(obj);
                return true;
            }
        }
        return false;
    }


}
