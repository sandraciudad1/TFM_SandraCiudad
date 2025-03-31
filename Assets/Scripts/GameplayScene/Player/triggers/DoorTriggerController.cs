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

    GameObject[] doors;
    Animator[] doorAnims;
    readonly int[] doorIndices = { 1, 0, 3, 7, 10, 9, 8, 14, 15, 13, 22, 19, 16, 17 };

    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject switchboard;
    [SerializeField] GameObject inventory;

    [SerializeField] GameObject player;
    Animator playerAnimator;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(151.49f, 25.641f, 52.42f);
    Vector3 playerPos2 = new Vector3(151.34f, 25.74532f, 53.5f);
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
        playerAnimator = player.GetComponent<Animator>();

        doors = new GameObject[] { door1, door2, door3, door4, door5, metallicDoor, switchboardDoor, door6, door7, door8, door9,
                                     door10, door11, door12, door13, door14, door15, door16, door17, door18, door19, door20, door21 };
        doorImages = new Image[] { doorImg1, doorImg2, doorImg3, doorImg4, doorImg5, metallicDoorImg, switchboardDoorImg, doorImg6, doorImg7, doorImg8, doorImg9, 
                                   doorImg10, doorImg11, doorImg12, doorImg13, doorImg14, doorImg15, doorImg16, doorImg17, doorImg18, doorImg19, doorImg20, doorImg21 };
        initializeAnimators();
        doorsOpen = new bool[doorAnims.Length];
        
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[0];

        if(opened==1 && !resetState)
        {
            desactivateImg(6);
            resetState = true;
        }
    }

    // Assigns Animator components to crates and doors.
    void initializeAnimators()
    {
        doorAnims = new Animator[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            doorAnims[i] = doors[i].GetComponent<Animator>();
        }
    }

    // Toggle door animation when pressing 'E'.
    void Update()
    {
        if (doorNum >= 0)  
        {
            if (Input.GetKeyDown(KeyCode.E) && !doorsOpen[doorNum])
            {
                Animator anim = doorAnims[doorNum];
                if (doorNum == 5 && crowbar.activeInHierarchy)
                {
                    SwapCameras(0, 1);
                    playerMov.canMove = false;
                    cc.enabled = false;
                    player.transform.position = playerPos;
                    player.transform.rotation = playerRot;
                    if (Vector3.Distance(player.transform.position, playerPos) < 0.01f && !change)
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
                Animator anim = doorAnims[doorNum];
                anim.SetBool("open", false);
                doorsOpen[doorNum] = false;  
            }
        }

        for (int i = 0; i < doorIndices.Length; i++)
        {
            int doorIndex = doorIndices[i];
            allowCollectObject(doorsOpen[doorIndex], i);
        }
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
        Animator anim = doorAnims[5];
        anim.SetBool("open", true);
        yield return new WaitForSeconds(1.5f);
        SwapCameras(1, 0);
        playerAnimator.SetBool("open", false);
        playerAnimator.SetBool("closeHand", false);
        playerMov.canMove = false;
        cc.enabled = false;
        player.transform.position = playerPos2;
        if(player.transform.position == playerPos2)
        {
            crowbar.SetActive(false);
            Collider collider = switchboard.GetComponent<Collider>();
            collider.enabled = true;
            playerMov.canMove = true;
            cc.enabled = true;
        }
    }

    // Desactivate switchboard animator and enable switchboard player interactions.
    IEnumerator swichtboardInteraction()
    {
        yield return new WaitForSeconds(3.2f);
        GameManager.GameManagerInstance.missionsCompleted[0] = 1;
        GameManager.GameManagerInstance.SaveProgress();
        switchboardLeverImg.gameObject.SetActive(true);
        doorAnims[6].enabled = false;
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
        doorAnims[6].enabled = true;
        Animator anim = doorAnims[index];
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
