using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    
    Animator anim;
    Rigidbody playerRigidbody;

    //Variables para el movimiento en el entorno isométrico
    float speed = 2f;
    public Camera cam;
    Transform camTransform;
    Vector3 forward, right;

    //Variables para que el personaje gire
    int floormask;
    float camRayLength = 100f;

    //Variables para la animación del personaje respecto a donde está mirando
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    float forwardAmount;
    float turnAmount;

    public float anguloError1;
    public float anguloError2;

    void Awake()
    {
        floormask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        camTransform = cam.transform;
        //Se asigna el sistema de coordenadas que usa la cámara, la cual posee la perspectiva isométrica
        forward = camTransform.forward;
        right = camTransform.right;
        forward.y = 0;
        right = Vector3.Normalize(right);
        forward = Vector3.Normalize(forward);

    }


    void FixedUpdate()
    {
        //Se obtiene el cambio horizontal y vertical a partir de dos nuevos Input, el HorizontalKey(A, D) y VerticalKey (W, S)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
		Move (h, v);
		Turning ();
        Animate (h, v);

    }

    //Método para realizar el movimiento del personaje en el entorno isométrico
    void Move(float h, float v)
    {
        right.Normalize();
        forward.Normalize();
        //Se mueve respecto a right y forward, que son los ejes que se definieron anteriormente, que eran respecto a la camara
        Vector3 rightMovement = right * speed * Time.deltaTime * h;
        Vector3 upMovement = forward * speed * Time.deltaTime * v;

        //Se realiza el movimiento mediante la transform
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    //Método para que el personaje rote hacia la dirección del mouse
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floormask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }

    }

    void Animate(float h, float v)
    {
        //Para la animación respecto a donde está mirando
        if (cam != null)
        {
            //Se obtienen los valores a partir de donde apunta a la camara, en coordenadas de la camara similar a como se hizo para el movimiento
            camForward = Vector3.Scale(camTransform.up, new Vector3(1, 0, 1)).normalized;
            move = v * camForward + h * camTransform.right;
        }
        else
        {
            move = v * Vector3.forward + h * Vector3.right;
        }
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        this.moveInput = move;
        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        //Se obtienen los valores de la rotación y hacia adelante dependiendo de la dirección del jugador
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        //Se le envían los valores al blend tree, para que ejecute la correspondiente animación según sea el caso
        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

    }
}