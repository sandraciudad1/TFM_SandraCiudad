using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class DoorTriggerController : MonoBehaviour
{
    [SerializeField] Image doorImg1;
    [SerializeField] Image doorImg2;
    [SerializeField] Image doorImg3;
    [SerializeField] Image doorImg4;
    [SerializeField] Image doorImg5;
    [SerializeField] Image metallicDoorImg;
    [SerializeField] Image switchboardDoorImg;
    [SerializeField] Image switchboardLeverImg;
    Image[] doorImages;

    int doorNum = -1;
    public bool[] doorsOpen;  

    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject door3;
    [SerializeField] GameObject door4;
    [SerializeField] GameObject door5;
    [SerializeField] GameObject metallicDoor;
    [SerializeField] GameObject switchboardDoor;

    Animator door1Anim;
    Animator door2Anim;
    Animator door3Anim;
    Animator door4Anim;
    Animator door5Anim;
    Animator metallicDoorAnim;
    Animator switchboardDoorAnim;
    Animator[] doorsAnim;

    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject switchboard;
    [SerializeField] GameObject inventory;

    [SerializeField] GameObject player;
    Animator playerAnimator;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;

    public bool swichtInteraction = false;

    // Initialize arrays and get door animators
    void Start()
    {
        SwapCameras(1, 0);
        doorImages = new Image[] { doorImg1, doorImg2, doorImg3, doorImg4, doorImg5, metallicDoorImg, switchboardDoorImg, switchboardLeverImg };

        door1Anim = door1.GetComponent<Animator>();
        door2Anim = door2.GetComponent<Animator>();
        door3Anim = door3.GetComponent<Animator>();
        door4Anim = door4.GetComponent<Animator>();
        door5Anim = door5.GetComponent<Animator>();
        metallicDoorAnim = metallicDoor.GetComponent<Animator>();
        switchboardDoorAnim = switchboardDoor.GetComponent<Animator>();
        doorsAnim = new Animator[] { door1Anim, door2Anim, door3Anim, door4Anim, door5Anim, metallicDoorAnim, switchboardDoorAnim };

        doorsOpen = new bool[doorsAnim.Length];
        playerAnimator = player.GetComponent<Animator>();

    }

    // Toggle door animation when pressing 'E'
    void Update()
    {
        if (doorNum >= 0)  
        {
            if (Input.GetKeyDown(KeyCode.E) && !doorsOpen[doorNum])
            {
                Animator anim = doorsAnim[doorNum];
                if (doorNum == 5 && crowbar.activeInHierarchy)
                {
                    SwapCameras(0, 1);
                    player.transform.position = new Vector3(151.49f, 25.641f, 51.3f);
                    player.transform.rotation = Quaternion.Euler(new Vector3 (0f, 180f, 0f));
                    desactivateImg(5);
                    StartCoroutine(initMission1());
                }
                else if (doorNum == 6)
                {
                    anim.SetBool("open", true); //se abre la puerta
                    desactivateImg(6); //se quita la e de la puerta
                    StartCoroutine(swichtboardInteraction());
                    //a partir de aqui el usuario puede interactuar con los botones y la palanca pulsando teclas 
                    swichtInteraction = true;
                    
                }
                else
                {
                    anim.SetBool("open", true);
                    doorsOpen[doorNum] = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && doorsOpen[doorNum])
            {
                Animator anim = doorsAnim[doorNum];
                anim.SetBool("open", false);
                doorsOpen[doorNum] = false;  
            }
        }

    }

    IEnumerator initMission1()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("open", true);
        yield return new WaitForSeconds(1.1f);
        Animator anim = doorsAnim[5];
        anim.SetBool("open", true);
        yield return new WaitForSeconds(1.5f);
        SwapCameras(1, 0);
        playerAnimator.SetBool("open", false);
        crowbar.SetActive(false);
        Collider collider = switchboard.GetComponent<Collider>();
        collider.enabled = true;
    }

    IEnumerator swichtboardInteraction()
    {
        yield return new WaitForSeconds(3.2f);
        activateImg(7); //se activa la e de la palanca
        switchboardDoorAnim.enabled = false;
    }

    public void closeSwitchboardDoor(int index)
    {
        Animator anim = doorsAnim[index];
        anim.SetBool("open", false);
    }

    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam2.Priority = priority2;
    }

    // Detects when the player enters a door trigger
    private void OnTriggerEnter(Collider other)
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
        else if (other.CompareTag("metallicDoor"))
        {
            activateImg(5);
            doorNum = 5;
        }
        else if (other.CompareTag("swichtboard"))
        {
            activateImg(6);
            doorNum = 6;
        }
    }

    // Activates the corresponding door image
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

    // Detects when the player exits a door trigger
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
        else if (other.CompareTag("metallicDoor"))
        {
            desactivateImg(5);
        }
        else if (other.CompareTag("swichtboard"))
        {
            desactivateImg(6);
        }
        doorNum = -1;
    }

    // Deactivates the corresponding door image
    void desactivateImg(int index)
    {
        doorImages[index].gameObject.SetActive(false);
    }
}
