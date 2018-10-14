using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCultivo : MonoBehaviour {

    string nombreCultivo;
    GameObject cuadro;
    ControladorCuadricula cuadricula;
    float tiempoCrecimiento=0;
    float tiempoInicial;
    float tiempoSegundo=0;
    RectTransform TimeBar;

    //Prefabs Tipos Cultivo:
    public GameObject cultivoMaiz;
    public GameObject cultivoCafe;
    public GameObject cultivoPapa;
    public GameObject cultivoRemolacha;
    public GameObject cultivoYuca;


    public void Iniciar(string nombreCultivo, GameObject cuadro, ControladorCuadricula cuadricula)
    {
        this.nombreCultivo = nombreCultivo;
        this.cuadricula = cuadricula;
        this.cuadro = cuadro;
        ObtenerTiempoCrecimiento();
    }

    public void ObtenerTiempoCrecimiento()
    {
        switch (nombreCultivo)
        {
            case "Semilla Maíz":
                tiempoCrecimiento = 0.2f;
                break;
            case "Semilla Café":
                tiempoCrecimiento = 1.5f;
                break;
            case "Semilla Papa":
                tiempoCrecimiento = 0.5f;
                break;
            case "Semilla Yuca":
                tiempoCrecimiento = 0.5f;
                break;
            case "Semilla Remolacha":
                tiempoCrecimiento = 1f;
                break;
        }
    }

    private void InstanciarTipoCultivo()
    {
        switch (nombreCultivo)
        {
            case "Semilla Maíz":
                cuadricula.colocarYReemplazarObjetoEnCuadro(cultivoMaiz, cuadro);
                break;
            case "Semilla Café":
                cuadricula.colocarYReemplazarObjetoEnCuadro(cultivoCafe, cuadro);
                break;
            case "Semilla Papa":
                cuadricula.colocarYReemplazarObjetoEnCuadro(cultivoPapa, cuadro);
                break;
            case "Semilla Yuca":
                cuadricula.colocarYReemplazarObjetoEnCuadro(cultivoYuca, cuadro);
                break;
            case "Semilla Remolacha":
                cuadricula.colocarYReemplazarObjetoEnCuadro(cultivoRemolacha, cuadro);
                break;
        }
    }

    private void Start()
    {
        tiempoInicial = Time.time;
        TimeBar = this.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>();
    }

    private void Update()
    {

        if (Time.time - tiempoInicial > tiempoSegundo)
        {
            TimeBar.sizeDelta = new Vector2((tiempoSegundo * 0.5f) / (tiempoCrecimiento * 60),TimeBar.sizeDelta.y);
            tiempoSegundo= tiempoSegundo+0.1f;
        }

        if (Time.time-tiempoInicial > tiempoCrecimiento*60)
        {
            InstanciarTipoCultivo();
        }
    }

}
