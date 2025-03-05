using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission3Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(88.015f, 30.745f, 62.533f);
    Vector3 secondPos = new Vector3();
    Vector3 thirdPos = new Vector3();
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));

    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject letterX;
    bool finish = false;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam4;

    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
    }

    bool change = false;
    void Update()
    {     
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
                playerAnim.SetBool("wrench", true);
                change = true;
            }
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
