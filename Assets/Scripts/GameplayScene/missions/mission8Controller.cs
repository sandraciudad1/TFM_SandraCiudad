using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission8Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(117.927f, 25.67f, 49.789f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] GameObject vacuum;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam13;


    // 
    void Start()
    {
        SwapCameras(1, 0);
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
        if (other.gameObject.CompareTag("grids"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving book.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("grids"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("grids") && vacuum.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                // animacion del personaje sbiendo la aspiradora
                //se acerca la camara
                //controlar aspiradora con flechas y mensaje informativo
                // detectar si el trigger de la aspiradora permanece por dos segundos en los de la suciedad
                // desactivar suciedad
                // contador de suciedad regresivo que cuando llegue a 0 se vuelve a alejar la camara


                /*inventoryCont.blockInventory = true;
                playerAnim.SetBool("clipboard", true);
                StartCoroutine(waitEndAnimation());*/
                change = true;
            }
        }
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam13.Priority = priority2;
    }

}
