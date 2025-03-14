using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission6Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(124.441f, 25.67f, 51.878f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] GameObject clipboard;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam9;
    [SerializeField] CinemachineVirtualCamera vcam10;

    //
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
    }

    // 
    void Update()
    {
        
    }

    // Shows 'X' when leaving book.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("book"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving book.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("book"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("book") && clipboard.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                // animacion del personaje dejando en la mesa la clipboard
                // habilitar el clipboard2 y deshabilitar el de la mano
                // cambio de camara a la 10 
                // capturar entrada por teclado del usuario
                // dejarlo grabado en la otra pagina del libro
                change = true;
            }
        }
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam9.Priority = priority2;
        vcam10.Priority = priority3;
    }
}
