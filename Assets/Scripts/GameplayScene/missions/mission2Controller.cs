using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class mission2Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(105.317f, 30.74516f, 63.886f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, -180f, 0f));

    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;

    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam3;
    bool swap = false;

    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        playerMov = player.GetComponent<PlayerMovement>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument"))
        {
            letterX.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument"))
        {
            letterX.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument") && sampleActive()>0 && Input.GetKeyDown(KeyCode.X))
        {
            // fijar posicion del jugador e impedir que pueda moverse
            playerMov.canMove = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !swap)
            {
                Debug.Log("dentro");
                SwapCameras(0, 1);
                // poner muestra a analizar (animacion analyze true)
                StartCoroutine(waitAnalyzeAnim(2f, true));
                swap = true;
            } 
            
            
            
            
            // aumentar el contador y el pb 
            //cambiar la imagen pequeña por un circulo que se rellena
            
            //desactivar el gameobject sample
        }
    }

    IEnumerator waitAnalyzeAnim(float time, bool value)
    {
        yield return new WaitForSeconds(time);
        playerAnim.SetBool("analyze", value);
    }

    int sampleActive()
    {
        if(sample1.activeInHierarchy)
        {
            return 1;
        } 
        else if(sample2.activeInHierarchy)
        {
            return 2;
        }
        else if (sample3.activeInHierarchy)
        {
            return 3;
        }
        else if (sample4.activeInHierarchy)
        {
            return 4;
        } 
        else
        {
            return -1;
        }
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam3.Priority = priority2;
    }
}
