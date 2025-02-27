using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission2Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(105.231f, 30.676f, 64.234f);

    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;

    [SerializeField] GameObject letterX;

    void Start()
    {
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
            // poner muestra a analizar (animacion analyze true)
            playerAnim.SetBool("analyze", true);
            
            
            // aumentar el contador y el pb 
            //cambiar la imagen pequeña por un circulo que se rellena
            
            //desactivar el gameobject sample
        }
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
}
