using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPanelBotones : MonoBehaviour {

    public GameObject mundo;
    ControladorCuadricula cuadricula;
    public GameObject panelBotonesHUD;
    VerificarPanelBotones verificacionPanel;

    public GameObject tierra;
    public GameObject sembrado;
    public GameObject granero;

    // Use this for initialization
    void Start () {
        cuadricula = mundo.GetComponent<ControladorCuadricula>();
        verificacionPanel = this.GetComponent<VerificarPanelBotones>();
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
        /////////////////////////////////////////
        /*
         * INTERFAZ MOSTRANDO SEMILLAS ACTUALES EN EL GRANERO
         * SI SE ESCOGE UNA, SE CREA UNA CLASE DEL TIPO DE CULTIVO
         * Y LUEGO SE HACE EL CÓDIGO DE ABAJO
        */
        ////////////////////////////////////////


        cuadricula.colocarYReemplazarObjeto(sembrado);
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void Cosechar()
    {
        /////////////////////////////////////////
        /*
         * SE DEBE GUARDAR LA CANTIDAD DEL CULTIVO 
         * COSECHADOEN EL GRANERO AL PRESIONAR ESTE BOTÓN
        */
        ////////////////////////////////////////
    }

    public void Construir()
    {
        /////////////////////////////////////////
        /*
         * INTERFAZ MOSTRANDO EDIFICIOS DISPONIBLES (HASTA AHORA SÓLO TENEMOS EL GRANERO)
        */
        ////////////////////////////////////////
    }

    public void Destruir()
    {
        cuadricula.removerObjetoDeCuadricula();
        verificacionPanel.DesactivarTodosLosBotones();
    }

    public void VerGranero()
    {
        /////////////////////////////////////////
        /*
         * INTERFAZ MOSTRANDO ELEMENTOS DENTRO DEL GRANERO (Inventario)
        */
        ////////////////////////////////////////
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
