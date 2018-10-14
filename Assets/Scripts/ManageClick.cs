using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageClick : MonoBehaviour {

    public void ClickBtnEspacioSembrado(GameObject EspacioNombre)
    {
        GameObject.Find("CanvasHUD").GetComponent<InterfazSembrar>().SembrarItem(EspacioNombre);
    }

}
