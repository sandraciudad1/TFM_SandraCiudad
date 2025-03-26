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
    [SerializeField] Image doorImg6;
    [SerializeField] Image doorImg7;
    [SerializeField] Image doorImg8;
    [SerializeField] Image doorImg9;
    [SerializeField] Image doorImg10;
    [SerializeField] Image doorImg11;
    [SerializeField] Image doorImg12;
    [SerializeField] Image doorImg13;
    [SerializeField] Image doorImg14;
    [SerializeField] Image doorImg15;
    [SerializeField] Image doorImg16;
    [SerializeField] Image doorImg17;
    [SerializeField] Image doorImg18;
    [SerializeField] Image doorImg19;
    [SerializeField] Image doorImg20;
    [SerializeField] Image doorImg21;
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
    [SerializeField] GameObject door6;
    [SerializeField] GameObject door7;
    [SerializeField] GameObject door8;
    [SerializeField] GameObject door9;
    [SerializeField] GameObject door10;
    [SerializeField] GameObject door11;
    [SerializeField] GameObject door12;
    [SerializeField] GameObject door13;
    [SerializeField] GameObject door14;
    [SerializeField] GameObject door15;
    [SerializeField] GameObject door16;
    [SerializeField] GameObject door17;
    [SerializeField] GameObject door18;
    [SerializeField] GameObject door19;
    [SerializeField] GameObject door20;
    [SerializeField] GameObject door21;

    Animator door1Anim;
    Animator door2Anim;
    Animator door3Anim;
    Animator door4Anim;
    Animator door5Anim;
    Animator metallicDoorAnim;
    Animator switchboardDoorAnim;
    Animator door6Anim;
    Animator door7Anim;
    Animator door8Anim;
    Animator door9Anim;
    Animator door10Anim;
    Animator door11Anim;
    Animator door12Anim;
    Animator door13Anim;
    Animator door14Anim;
    Animator door15Anim;
    Animator door16Anim;
    Animator door17Anim;
    Animator door18Anim;
    Animator door19Anim;
    Animator door20Anim;
    Animator door21Anim;
    Animator[] doorsAnim;

    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject switchboard;
    [SerializeField] GameObject inventory;

    [SerializeField] GameObject player;
    Animator playerAnimator;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(151.49f, 25.641f, 52.42f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;

    bool[] setArrayUnlocked = new bool[15];
    public bool swichtInteraction = false;
    int opened;
    bool resetState = false;

    // Initialize arrays and get door animators.
    void Start()
    {
        SwapCameras(1, 0);
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        doorImages = new Image[] { doorImg1, doorImg2, doorImg3, doorImg4, doorImg5, metallicDoorImg, switchboardDoorImg, doorImg6, doorImg7, doorImg8, doorImg9, 
                                   doorImg10, doorImg11, doorImg12, doorImg13, doorImg14, doorImg15, doorImg16, doorImg17, doorImg18, doorImg19, doorImg20, doorImg21 };

        door1Anim = door1.GetComponent<Animator>();
        door2Anim = door2.GetComponent<Animator>();
        door3Anim = door3.GetComponent<Animator>();
        door4Anim = door4.GetComponent<Animator>();
        door5Anim = door5.GetComponent<Animator>();
        metallicDoorAnim = metallicDoor.GetComponent<Animator>();
        switchboardDoorAnim = switchboardDoor.GetComponent<Animator>();
        door6Anim = door6.GetComponent<Animator>();
        door7Anim = door7.GetComponent<Animator>();
        door8Anim = door8.GetComponent<Animator>();
        door9Anim = door9.GetComponent<Animator>();
        door10Anim = door10.GetComponent<Animator>();
        door11Anim = door11.GetComponent<Animator>();
        door12Anim = door12.GetComponent<Animator>();
        door13Anim = door13.GetComponent<Animator>();
        door14Anim = door14.GetComponent<Animator>();
        door15Anim = door15.GetComponent<Animator>();
        door16Anim = door16.GetComponent<Animator>();
        door17Anim = door17.GetComponent<Animator>();
        door18Anim = door18.GetComponent<Animator>();
        door19Anim = door19.GetComponent<Animator>();
        door20Anim = door20.GetComponent<Animator>();
        door21Anim = door21.GetComponent<Animator>();
        doorsAnim = new Animator[] { door1Anim, door2Anim, door3Anim, door4Anim, door5Anim, metallicDoorAnim, switchboardDoorAnim, door6Anim, door7Anim, door8Anim, door9Anim,
                                     door10Anim, door11Anim, door12Anim, door13Anim, door14Anim, door15Anim, door16Anim, door17Anim, door18Anim, door19Anim, door20Anim, door21Anim };
        doorsOpen = new bool[doorsAnim.Length];
        playerAnimator = player.GetComponent<Animator>();

        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[0];

        if(opened==1 && !resetState)
        {
            desactivateImg(6);
            resetState = true;
        }
    }

    // Toggle door animation when pressing 'E'.
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
                    playerMov.canMove = false;
                    cc.enabled = false;
                    player.transform.position = playerPos;
                    player.transform.rotation = playerRot;
                    if(player.transform.position == playerPos && !change)
                    {
                        desactivateImg(5);
                        StartCoroutine(initMission1());
                        playerMov.canMove = true;
                        cc.enabled = true;
                        change = true;
                    }
                }
                else if (doorNum == 6)
                {
                    anim.SetBool("open", true); 
                    desactivateImg(6); 
                    StartCoroutine(swichtboardInteraction());
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

        // mission 1 - crowbar
        allowCollectObject(doorsOpen[1], 0);
        // mission 2 - samples: 1, 2, 3, 4
        allowCollectObject(doorsOpen[0], 1);
        allowCollectObject(doorsOpen[3], 2);
        allowCollectObject(doorsOpen[7], 3);
        allowCollectObject(doorsOpen[10], 4);
        // mission 3 - spannerwrench
        allowCollectObject(doorsOpen[9], 5);
        // mission 4 - securityCard
        allowCollectObject(doorsOpen[8], 6);
        // mission 5 - wireCutters
        allowCollectObject(doorsOpen[14], 7);
        // mission 6 - clipboard
        allowCollectObject(doorsOpen[15], 8);
        // mission 7 - emergencyKit
        allowCollectObject(doorsOpen[13], 9);
        // mission 8 - vacuum
        allowCollectObject(doorsOpen[22], 10);
        // mission 9 - compass
        allowCollectObject(doorsOpen[19], 11);
        // mission 10 - UV light and tape
        allowCollectObject(doorsOpen[16], 12); // UV light
        allowCollectObject(doorsOpen[17], 13); // tape

    }

    // Unlocks and saves an object if conditions are met.
    void allowCollectObject(bool isOpen, int index)
    {
        if(isOpen && !setArrayUnlocked[index] && Input.GetKeyDown(KeyCode.R))
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("objects", GameManager.GameManagerInstance.objectIndex, 1);
            GameManager.GameManagerInstance.SaveProgress();
            setArrayUnlocked[index] = true;
        }
    }

    // Doors and players animations in mission 1.
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
        playerAnimator.SetBool("closeHand", false);
        player.transform.position = new Vector3(151.34f, 25.74532f, 53.5f);
        crowbar.SetActive(false);
        Collider collider = switchboard.GetComponent<Collider>();
        collider.enabled = true;
    }

    // Desactivate switchboard animator and enable switchboard player interactions.
    IEnumerator swichtboardInteraction()
    {
        yield return new WaitForSeconds(3.2f);
        GameManager.GameManagerInstance.missionsCompleted[0] = 1;
        GameManager.GameManagerInstance.SaveProgress();
        switchboardLeverImg.gameObject.SetActive(true);
        switchboardDoorAnim.enabled = false;
        swichtInteraction = true;
    }

    // Change player position and close switchboard door.
    public void closeSwitchboardDoor(int index)
    {
        player.transform.position = new Vector3(151.4594f, 25.74516f, 52.76605f);
        player.transform.rotation = Quaternion.Euler(0, -178.8f, 0);
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = true; 
        }
        switchboardDoorAnim.enabled = true;
        Animator anim = doorsAnim[index];
        anim.SetBool("open", false);
    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam2.Priority = priority2;
    }

    // Detects when the player enters a door trigger.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[0];
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
        else if (other.CompareTag("metallicDoor") && opened == 0)
        {
            activateImg(5);
            doorNum = 5;
        }
        else if (other.CompareTag("swichtboard") && opened == 0)
        {
            activateImg(6);
            doorNum = 6;
        }
        else if (other.CompareTag("door6"))
        {
            activateImg(7);
            doorNum = 7;
        }
        else if (other.CompareTag("door7"))
        {
            activateImg(8);
            doorNum = 8;
        }
        else if (other.CompareTag("door8"))
        {
            activateImg(9);
            doorNum = 9;
        }
        else if (other.CompareTag("door9"))
        {
            activateImg(10);
            doorNum = 10;
        }
        else if (other.CompareTag("door10"))
        {
            activateImg(11);
            doorNum = 11;
        }
        else if (other.CompareTag("door11"))
        {
            activateImg(12);
            doorNum = 12;
        }
        else if (other.CompareTag("door12"))
        {
            activateImg(13);
            doorNum = 13;
        }
        else if (other.CompareTag("door13"))
        {
            activateImg(14);
            doorNum = 14;
        }
        else if (other.CompareTag("door14"))
        {
            activateImg(15);
            doorNum = 15;
        }
        else if (other.CompareTag("door15"))
        {
            activateImg(16);
            doorNum = 16;
        }
        else if (other.CompareTag("door16"))
        {
            activateImg(17);
            doorNum = 17;
        }
        else if (other.CompareTag("door17"))
        {
            activateImg(18);
            doorNum = 18;
        }
        else if (other.CompareTag("door18"))
        {
            activateImg(19);
            doorNum = 19;
        }
        else if (other.CompareTag("door19"))
        {
            activateImg(20);
            doorNum = 20;
        }
        else if (other.CompareTag("door20"))
        {
            activateImg(21);
            doorNum = 21;
        }
        else if (other.CompareTag("door21"))
        {
            activateImg(22);
            doorNum = 22;
        }
    }

    // Activates the corresponding door image.
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

    // Detects when the player exits a door trigger.
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
        else if (other.CompareTag("door6"))
        {
            desactivateImg(7);
        }
        else if (other.CompareTag("door7"))
        {
            desactivateImg(8);
        }
        else if (other.CompareTag("door8"))
        {
            desactivateImg(9);
        }
        else if (other.CompareTag("door9"))
        {
            desactivateImg(10);
        }
        else if (other.CompareTag("door10"))
        {
            desactivateImg(11);
        }
        else if (other.CompareTag("door11"))
        {
            desactivateImg(12);
        }
        else if (other.CompareTag("door12"))
        {
            desactivateImg(13);
        }
        else if (other.CompareTag("door13"))
        {
            desactivateImg(14);
        }
        else if (other.CompareTag("door14"))
        {
            desactivateImg(15);
        }
        else if (other.CompareTag("door15"))
        {
            desactivateImg(16);
        }
        else if (other.CompareTag("door16"))
        {
            desactivateImg(17);
        }
        else if (other.CompareTag("door17"))
        {
            desactivateImg(18);
        }
        else if (other.CompareTag("door18"))
        {
            desactivateImg(19);
        }
        else if (other.CompareTag("door19"))
        {
            desactivateImg(20);
        }
        else if (other.CompareTag("door20"))
        {
            desactivateImg(21);
        }
        else if (other.CompareTag("door21"))
        {
            desactivateImg(22);
        }
        doorNum = -1;
    }

    // Deactivates the corresponding door image.
    void desactivateImg(int index)
    {
        doorImages[index].gameObject.SetActive(false);
    }
}
