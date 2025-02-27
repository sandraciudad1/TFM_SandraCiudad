using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission2Controller : MonoBehaviour
{
    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;

    [SerializeField] GameObject letterX;

    void Start()
    {
        
    }


    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument") && sampleActive()>0)
        {
            letterX.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                // fijar posicion del jugador e impedir que pueda moverse
                // poner muestra a analizar (animacion analyze true)
                // aumentar el contador y el pb 
                //cambiar la imagen pequeña por un circulo que se rellena
            }
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
