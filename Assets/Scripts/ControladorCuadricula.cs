using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCuadricula : MonoBehaviour {

    public GameObject cuadricula;
    public Material materialSombreado;
    public Material materialNormal;
    Material materialAnterior;

    GameObject cuadroDetectado;

    float camRayLength = 100f;
    int cuadroMask;


    //TEMP----------------------------------
    public GameObject cultivoMaiz;
    public GameObject granero;



    // Use this for initialization
    void Start () {
        cuadroMask = LayerMask.GetMask("Cuadro");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            seleccionarCuadro();
        if (cuadroDetectado != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                colocarObjeto(cultivoMaiz);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                colocarObjeto(granero);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                removerObjetoDeCuadricula();
        }

    }

    //Coloca el objeto ingresado, y se reemplaza por un objeto anterior, en el caso de que exista
    private void colocarYReemplazarObjeto(GameObject objetoPrefab)
    {
        GameObject objeto = Instantiate(objetoPrefab, new Vector3(0, 0, 0),Quaternion.identity);
        if (cuadroDetectado.transform.childCount == 0)
        {
            objeto.transform.parent = cuadroDetectado.transform;
            objeto.transform.localPosition = new Vector3(0, 0.1f, 0);
            cuadroDetectado = null;

        }
        else if (cuadroDetectado.transform.childCount == 1)
        {
            Destroy(cuadroDetectado.transform.GetChild(0).gameObject);
            objeto.transform.parent = cuadroDetectado.transform;
            objeto.transform.localPosition = new Vector3(0, 0.1f, 0);
            cuadroDetectado = null;
        }
    }

    //Coloca el objeto ingresado, pero si ya existe un objeto en el lugar, no se coloca y retorna falso
    private bool colocarObjeto(GameObject objetoPrefab)
    {
        GameObject objeto = Instantiate(objetoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        if (cuadroDetectado.transform.childCount == 0)
        {
            objeto.transform.parent = cuadroDetectado.transform;
            objeto.transform.localPosition = new Vector3(0, 0.1f, 0);
            cuadroDetectado = null;
            return true;
        }
        else if (cuadroDetectado.transform.childCount == 1)
        {
            Destroy(objeto);
            ResetearCuadroDetectadoAAnterior();
            return false;
        }
        return false;
    }

    //Remover objeto del cuadro detectado, pero si no existe un objeto en el cuadro, retorna falso
    private bool removerObjetoDeCuadricula()
    {
        if (cuadroDetectado.transform.childCount == 0)
        {
            ResetearCuadroDetectado();
            return false;
        }
        else if (cuadroDetectado.transform.childCount == 1)
        {
            Destroy(cuadroDetectado.transform.GetChild(0).gameObject);
            ResetearCuadroDetectado();
            return true;
        }
        return false;
    }

    public void ResetearCuadroDetectado()
    {
        Material[] m = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        m[0] = materialNormal;
        cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = m;
    }

    public void ResetearCuadroDetectadoAAnterior()
    {
        Material[] m = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        m[0] = materialAnterior;
        cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = m;
    }


    public void seleccionarCuadro()
    {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, cuadroMask))
        {
            
            if (cuadroDetectado != null && floorHit.collider.gameObject != cuadroDetectado)
            {
                //LE HIZO CLICK A UN NUEVO CUADRO, HAY CUADRO ANTERIOR

                Material[] materials;
                
                //Cambia Cuadro Anterior a Normal
                materials = cuadroDetectado.GetComponent<MeshRenderer>().materials;
                materials[0] = materialNormal;
                cuadroDetectado.GetComponent<MeshRenderer>().materials = materials;

                //Cambia Objeto en Cuadro Anterior a Normal
                if (cuadroDetectado.transform.childCount != 0)
                {
                    materials = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
                    materials[0] = materialAnterior;
                    cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;

                }

                //Asigna Nuevo Cuadro
                cuadroDetectado = floorHit.collider.gameObject;

                //Cambia Cuadro Actual a Sombreado
                materials = cuadroDetectado.GetComponent<MeshRenderer>().materials;
                materials[0] = materialSombreado;
                cuadroDetectado.GetComponent<MeshRenderer>().materials = materials;


                //Cambia Objeto en Cuadro Actual a Sombreado
                if (cuadroDetectado.transform.childCount != 0)
                {
                    materials = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
                    materialAnterior = materials[0];
                    materials[0] = materialSombreado;
                    cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
                }

            }
            else if(cuadroDetectado == null)
            {
                //LE HIZO CLICK A UN NUEVO CUADRO, NO HAY CUADRO ANTERIOR

                //Asigna Nuevo Cuadro
                cuadroDetectado = floorHit.collider.gameObject;

                //Cambia Cuadro Actual a Sombreado
                Material[] materials;
                materials = cuadroDetectado.GetComponent<MeshRenderer>().materials;
                materials[0] = materialSombreado;
                cuadroDetectado.GetComponent<MeshRenderer>().materials = materials;


                //Cambia Objeto en Cuadro Actual a Sombreado
                if (cuadroDetectado.transform.childCount != 0)
                {
                    materials = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
                    materialAnterior = materials[0];
                    materials[0] = materialSombreado;
                    cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
                }

            }
            else if (floorHit.collider.gameObject == cuadroDetectado)
            {
                //LE HIZO CLICK AL MISMO CUADRO

                Material[] materials;
                //Cambia Cuadro Actual a Normal
                materials = cuadroDetectado.GetComponent<MeshRenderer>().materials;
                materials[0] = materialNormal;
                cuadroDetectado.GetComponent<MeshRenderer>().materials = materials;

                //Cambia Objeto en Cuadro Actual a Normal
                if (cuadroDetectado.transform.childCount != 0)
                {
                    materials = cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
                    materials[0] = materialAnterior;
                    cuadroDetectado.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;

                }
                cuadroDetectado = null;

            }
        }  
    }

}
