using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mission3Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(88.028f, 30.74504f, 62.476f);
    Vector3 secondPos = new Vector3();
    Vector3 thirdPos = new Vector3();
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;

    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject letterX;
    bool finish = false;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam4;

    [SerializeField] GameObject gradientBg;
    [SerializeField] RectTransform gradient;
    [SerializeField] GameObject arrow;
    float minX = -375f, maxX = 375f;
    float flechaSpeed = 200f;
    bool movingRight = true;
    bool stopped = false;
    bool start = false;


    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        calculateLimits();
        
    }

    
    void Update()
    {
        Debug.Log(arrow.transform.position);
        if (start && !stopped)
        {
            moveArrow();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkValue(arrow.transform.position.x);
                stopped = true;
            }
        }
    }


    void calculateLimits()
    {
        float halfWidth = gradient.rect.width / 2f;
        minX = gradient.position.x - halfWidth + 20f;
        maxX = gradient.position.x + halfWidth - 20f;
    }

    void checkValue(float value)
    {
        if(value <= 128.38f) //rojo izq
        {
            // quitar vida y repetir
        } 
        else if (value > 128.38f && value <= 296.25f) // amarillo izq
        {
            // quitar vida y dejar un poco humo
        } 
        else if (value > 296.25f && value < 493.75f) // verde
        {
            // hacer animacion de apretar y se corta el humo
            playerAnim.SetBool("wrench", true);
        } 
        else if (value >= 493.75f && value < 661.63f) // amarillo der
        {
            // quitar vida y dejar un poco humo
        }
        else if (value >= 661.63f) // rojo der
        {
            // quitar vida y repetir
        }
    }

    void moveArrow()
    {
        float step = flechaSpeed * Time.deltaTime;

        if (movingRight)
        {
            arrow.transform.position += new Vector3(step, 0, 0);
            if (arrow.transform.position.x >= maxX)
                movingRight = false;
        }
        else
        {
            arrow.transform.position -= new Vector3(step, 0, 0);
            if (arrow.transform.position.x <= minX)
                movingRight = true;
        }
    }

    // Shows 'X' when near modular pipes.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes") && !finish)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving modular pipes.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes"))
        {
            letterX.SetActive(false);
        }
    }

    // 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes") && spannerwrench.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            // cambio de camaras?
            SwapCameras(0, 1);
            // se coloca al personaje en la primera llave 
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = firstPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == firstPos && !change)
            {
                // barra que oscila de uin lado a otro antes de activar animacion
                gradientBg.SetActive(true);
                start = true;
                //playerAnim.SetBool("wrench", true);
                change = true;
            }
            
            //cc.enabled = true;
            // se muestra 3 veces la barra
            //desactivar el character controller par acolocarlo bien
            // por cada vez que el usuario ajuste bien la presio se muestra la animacion de apretar
            // a la tercera vez de apretar bien se corta el humo y se va a la sigueinte barra
        }
    }


    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam4.Priority = priority2;
    }
}
