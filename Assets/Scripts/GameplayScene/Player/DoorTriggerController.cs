using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTriggerController : MonoBehaviour
{
    [SerializeField] Image doorImg1;
    [SerializeField] Image doorImg2;
    [SerializeField] Image doorImg3;
    [SerializeField] Image doorImg4;
    [SerializeField] Image doorImg5;
    Image[] doorImages;

    int doorNum = -1;
    bool[] doorsOpen;  // Estado de apertura de cada puerta

    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject door3;
    [SerializeField] GameObject door4;
    [SerializeField] GameObject door5;

    Animator door1Anim;
    Animator door2Anim;
    Animator door3Anim;
    Animator door4Anim;
    Animator door5Anim;
    Animator[] doorsAnim;

    void Start()
    {
        doorImages = new Image[] { doorImg1, doorImg2, doorImg3, doorImg4, doorImg5 };

        door1Anim = door1.GetComponent<Animator>();
        door2Anim = door2.GetComponent<Animator>();
        door3Anim = door3.GetComponent<Animator>();
        door4Anim = door4.GetComponent<Animator>();
        door5Anim = door5.GetComponent<Animator>();
        doorsAnim = new Animator[] { door1Anim, door2Anim, door3Anim, door4Anim, door5Anim };

        doorsOpen = new bool[5];  // Inicializar el array de puertas abiertas
    }

    void Update()
    {
        if (doorNum >= 0)  // Asegurarse de que hay una puerta válida
        {
            if (Input.GetKeyDown(KeyCode.E) && !doorsOpen[doorNum])
            {
                // Abrir la puerta
                Animator anim = doorsAnim[doorNum];
                anim.SetBool("open", true);
                doorsOpen[doorNum] = true;  // Marcar como abierta
            }
            else if (Input.GetKeyDown(KeyCode.E) && doorsOpen[doorNum])
            {
                // Cerrar la puerta
                Animator anim = doorsAnim[doorNum];
                anim.SetBool("open", false);
                doorsOpen[doorNum] = false;  // Marcar como cerrada
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("door1"))
        {
            activateImg(0);
            doorNum = 0;
        }
        else if (other.CompareTag("door2"))
        {
            activateImg(1);
            doorNum = 1;
        }
        else if (other.CompareTag("door3"))
        {
            activateImg(2);
            doorNum = 2;
        }
        else if (other.CompareTag("door4"))
        {
            activateImg(3);
            doorNum = 3;
        }
        else if (other.CompareTag("door5"))
        {
            activateImg(4);
            doorNum = 4;
        }
        else
        {
            doorNum = -1;
        }
    }

    void activateImg(int index)
    {
        for (int i = 0; i < doorImages.Length; i++)
        {
            if (i == index)
            {
                doorImages[i].gameObject.SetActive(true);
            }
            else
            {
                doorImages[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("door1"))
        {
            desactivateImg(0);
        }
        else if (other.CompareTag("door2"))
        {
            desactivateImg(1);
        }
        else if (other.CompareTag("door3"))
        {
            desactivateImg(2);
        }
        else if (other.CompareTag("door4"))
        {
            desactivateImg(3);
        }
        else if (other.CompareTag("door5"))
        {
            desactivateImg(4);
        }
        doorNum = -1;
    }

    void desactivateImg(int index)
    {
        doorImages[index].gameObject.SetActive(false);
    }
}
