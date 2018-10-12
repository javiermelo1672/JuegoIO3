using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificarPanelBotones : MonoBehaviour {

    public GameObject mundo;
    ControladorCuadricula cuadricula;
    public GameObject panelBotonesHUD;

    void Start()
    {
        cuadricula = mundo.GetComponent<ControladorCuadricula>();   
    }

    public void actualizarPanel()
    {
        BotonArarActivo();
        BotonSembrarActivo();
        BotonCosecharActivo();
        BotonDestruirActivo();
        BotonConstruirActivo();
    }

    public void BotonArarActivo()
    {
        if (cuadricula.getCuadroDetectado()!=null && cuadricula.getCuadroDetectado().transform.childCount == 0)
        {
            panelBotonesHUD.transform.Find("BotonArar").gameObject.SetActive(true);
        }
        else
        {
            panelBotonesHUD.transform.Find("BotonArar").gameObject.SetActive(false);
        }
    }

    public void BotonSembrarActivo()
    {
        if (cuadricula.getCuadroDetectado() != null && cuadricula.getCuadroDetectado().transform.childCount == 1 && cuadricula.getCuadroDetectado().transform.GetChild(0).name== "Tierra(Clone)")
        {
            panelBotonesHUD.transform.Find("BotonSembrar").gameObject.SetActive(true);
        }
        else
        {
            panelBotonesHUD.transform.Find("BotonSembrar").gameObject.SetActive(false);
        }
    }

    public void BotonCosecharActivo()
    {
        /////////////////////////////////////////
        /*
         * SE DEBE VERIFICAR QUE SEA UN TIPO DE CULTIVO, YA CRECIDO
        */
        ////////////////////////////////////////
    }

    public void BotonDestruirActivo()
    {
        if (cuadricula.getCuadroDetectado() != null && cuadricula.getCuadroDetectado().transform.childCount == 1)
        {
            panelBotonesHUD.transform.Find("BotonDestruir").gameObject.SetActive(true);
        }
        else
        {
            panelBotonesHUD.transform.Find("BotonDestruir").gameObject.SetActive(false);
        }
    }

    public void BotonConstruirActivo()
    {
        if (cuadricula.getCuadroDetectado() != null && cuadricula.getCuadroDetectado().transform.childCount == 0)
        {
            panelBotonesHUD.transform.Find("BotonConstruir").gameObject.SetActive(true);
        }
        else
        {
            panelBotonesHUD.transform.Find("BotonConstruir").gameObject.SetActive(false);
        }
    }

    public void DesactivarTodosLosBotones()
    {
        panelBotonesHUD.transform.Find("BotonArar").gameObject.SetActive(false);
        panelBotonesHUD.transform.Find("BotonSembrar").gameObject.SetActive(false);
        panelBotonesHUD.transform.Find("BotonDestruir").gameObject.SetActive(false);
        panelBotonesHUD.transform.Find("BotonConstruir").gameObject.SetActive(false);
    }


}
