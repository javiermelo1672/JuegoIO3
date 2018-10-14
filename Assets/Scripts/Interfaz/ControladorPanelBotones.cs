using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPanelBotones : MonoBehaviour {

    public GameObject mundo;
    ControladorCuadricula cuadricula;
    public GameObject panelBotonesHUD;
    VerificarPanelBotones verificacionPanel;
    InterfazGranero intGranero;
    InterfazSembrar intSembrar;
    InterfazConstruir intConstruir;

    public GameObject tierra;
    public GameObject sembrado;

    // Use this for initialization
    void Start () {
        cuadricula = mundo.GetComponent<ControladorCuadricula>();
        verificacionPanel = this.GetComponent<VerificarPanelBotones>();
        intGranero = this.GetComponent<InterfazGranero>();
        intSembrar = this.GetComponent<InterfazSembrar>();
        intConstruir = this.GetComponent<InterfazConstruir>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ArarTierra()
    {
        cuadricula.colocarObjeto(tierra);
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void SembrarTierra()
    {
        intSembrar.MostrarInterfaz();
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void Cosechar()
    {
        switch (cuadricula.getCuadroDetectado().transform.GetChild(0).name)
        {
            case ("Tierra_Maiz(Clone)"):
                intGranero.getSistema().AgregarItem("ItemMaiz");
                break;
            case ("Tierra_Papa(Clone)"):
                intGranero.getSistema().AgregarItem("ItemPapa");
                break;
            case ("Tierra_Yuca(Clone)"):
                intGranero.getSistema().AgregarItem("ItemYuca");
                break;
            case ("Tierra_Cafe(Clone)"):
                intGranero.getSistema().AgregarItem("ItemCafe");
                break;
            case ("Tierra_Remolacha(Clone)"):
                intGranero.getSistema().AgregarItem("ItemRemolacha");
                break;
        }
        cuadricula.colocarYReemplazarObjeto(tierra);
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void Construir()
    {
        intConstruir.MostrarInterfaz();
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void Destruir()
    {
        cuadricula.removerObjetoDeCuadricula();
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void VerGranero()
    {
        intGranero.MostrarInterfaz();
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void AbrirTienda()
    {
        /////////////////////////////////////////
        /*
         * INTERFAZ DE LA TIENDA PARA COMPRAR SEMILLAS/VENDER CULTIVOS
        */
        ////////////////////////////////////////
    }

}
