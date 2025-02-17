using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class mission1Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject triggerDetector;
    PlayerMovement playerMov;
    DoorTriggerController doorController;
    CollectiblesController collectiblesController;

    bool[] swichtboardState;
    bool firstPhaseComplete = false;
    bool lockMov = false;

    Quaternion initialRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    Quaternion finalRot = Quaternion.Euler(new Vector3(95f, 0f, 0f));
    float rotationSpeed = 100f;

    [SerializeField] GameObject btn0;
    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;
    [SerializeField] GameObject btn3;
    [SerializeField] GameObject btn4;
    [SerializeField] GameObject btn5;
    GameObject[] buttons;

    [SerializeField] GameObject crate1Top;

    void Start()
    {
        doorController = triggerDetector.GetComponent<DoorTriggerController>();
        collectiblesController = triggerDetector.GetComponent<CollectiblesController>();
        playerMov = player.GetComponent<PlayerMovement>();
        swichtboardState = new bool[] { true, true, true, true, true, true };
        buttons = new GameObject[] { btn0, btn1, btn2, btn3, btn4, btn5 };

        for(int i=0; i<buttons.Length; i++)
        {
            buttons[i].transform.rotation = initialRot;
        }
    }

    
    void Update()
    {
        if (doorController != null && doorController.swichtInteraction)
        {
            if (!lockMov)
            {
                lockPlayerMov();
            }
            checkButtons();
        }
        
    }

    void lockPlayerMov()
    {
        if (playerMov != null)
        {
            playerMov.canMove = false;
            Debug.Log(playerMov.canMove);
        }
        

        player.transform.position = new Vector3(151.3f, 25.74f, 51.5f);
        player.transform.rotation = Quaternion.Euler(0, -175, 0);

        lockMov = true;
    }

    void checkButtons()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            changeState(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            changeState(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            changeState(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            changeState(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            changeState(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            changeState(5);
        }
    }

    float rotationLerpTime = 0f;

    void changeState(int index)
    {
        swichtboardState[index] = !swichtboardState[index];

        rotationLerpTime = 0f;

        if (swichtboardState[index]) 
        {
            StartCoroutine(RotateButton(index, initialRot));
        }
        else
        {
            StartCoroutine(RotateButton(index, finalRot));
        }

        checkWinCondition();
    }

    IEnumerator RotateButton(int index, Quaternion targetRot)
    {
        Quaternion startRot = buttons[index].transform.rotation;


        while (rotationLerpTime < 1f)
        {
            rotationLerpTime += Time.deltaTime * rotationSpeed; 
            buttons[index].transform.rotation = Quaternion.RotateTowards(startRot, targetRot, rotationLerpTime);
            yield return null;
        }
        buttons[index].transform.rotation = targetRot;
    }


    void checkWinCondition()
    {
        if (!firstPhaseComplete && swichtboardState.All(state => state == false))
        {
            firstPhaseComplete = true;
        }
        else if (firstPhaseComplete && swichtboardState.All(state => state == true))
        {
            doorController.closeSwitchboardDoor(6);
            crate1Top.transform.position = new Vector3(0.305f, 0, 0.995f);
            crate1Top.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            collectiblesController.recordsUnlocked[4] = true;
            StartCoroutine(waitToUnlock());

        }
    }

    IEnumerator waitToUnlock()
    {
        yield return new WaitForSeconds(3f);
        playerMov.canMove = true;
    }
}
