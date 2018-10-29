using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class InterfazVender : MonoBehaviour {

    public GameObject mundo;
    public GameObject interfazError;

    SistemaAlmacenamiento granero;

    public GameObject objCantMaiz;
    public GameObject objCantYuca;
    public GameObject objCantPapa;
    public GameObject objCantRemolacha;
    public GameObject objCantCafe;

    public float valorMaiz;
    public float valorPapa;
    public float valorYuca;
    public float valorRemolacha;
    public float valorCafe;

    GameObject item;
    GameObject valorTxt;


    // Use this for initialization
    void Start () {
        granero = mundo.GetComponent<SistemaAlmacenamiento>();

    }

    public void MostrarCantidades()
    {
        objCantMaiz.GetComponent<Text>().text="(x"+granero.ObtenerCantidadDeItem("ItemMaiz")+")";
        objCantYuca.GetComponent<Text>().text = "(x" + granero.ObtenerCantidadDeItem("ItemYuca") + ")";
        objCantPapa.GetComponent<Text>().text = "(x" + granero.ObtenerCantidadDeItem("ItemPapa") + ")";
        objCantRemolacha.GetComponent<Text>().text = "(x" + granero.ObtenerCantidadDeItem("ItemRemolacha") + ")";
        objCantCafe.GetComponent<Text>().text = "(x" + granero.ObtenerCantidadDeItem("ItemCafe") + ")";
    }

    public void setItemActual(GameObject item)
    {
        this.item = item;
    }

    public void setValorActual(GameObject valorTxt)
    {
        this.valorTxt = valorTxt;
    }

    public void VenderUnidad()
    {
        int dinero = 9999;

        if (granero.BorrarItemSegunTipo(item.name))
        {
            dinero = dinero + int.Parse(valorTxt.GetComponent<Text>().text, NumberStyles.Currency);
        }
        else
        {
            interfazError.transform.GetChild(1).GetComponent<Text>().text = "No Posee este Tipo de Cultivo";
            interfazError.SetActive(true);
        }

    }

}
