using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Video;

public class mission1Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject triggerDetector;
    PlayerMovement playerMov;
    DoorTriggerController doorController;
    CollectiblesController collectiblesController;
    playerUI ui;

    bool[] swichtboardState;
    bool firstPhaseComplete = false;
    bool lockMov = false;

    Quaternion initialRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    Quaternion finalRot = Quaternion.Euler(new Vector3(95f, 0f, 0f));
    float rotationSpeed = 100f;
    float rotationLerpTime = 0f;

    [SerializeField] GameObject btn0;
    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;
    [SerializeField] GameObject btn3;
    [SerializeField] GameObject btn4;
    [SerializeField] GameObject btn5;
    GameObject[] buttons;

    [SerializeField] GameObject observationDoor;
    Animator observationDoorAnim;
    AudioSource audioDoor;
    [SerializeField] VideoPlayer code1;

    [SerializeField] AudioSource alarmSound;
    static int keyPressCount = 0;

    // Initializes variables and sets up the button states.
    void Start()
    {
        doorController = triggerDetector.GetComponent<DoorTriggerController>();
        collectiblesController = triggerDetector.GetComponent<CollectiblesController>();
        playerMov = player.GetComponent<PlayerMovement>();
        ui = triggerDetector.GetComponent<playerUI>();

        swichtboardState = new bool[] { true, true, true, true, true, true };
        buttons = new GameObject[] { btn0, btn1, btn2, btn3, btn4, btn5 };

        for(int i=0; i<buttons.Length; i++)
        {
            buttons[i].transform.rotation = initialRot;
        }
        observationDoorAnim = observationDoor.GetComponent<Animator>();
        audioDoor = observationDoor.GetComponent<AudioSource>();
    }

    // Checks for player interaction with the switchboard.
    void Update()
    {
        if (doorController != null && doorController.swichtInteraction)
        {
            if (!lockMov)
            {
                lockPlayerMov();
            }
            checkButtons();
            checkFailCondition();
        }
    }

    // Checks key presses and applies damage if threshold passed.
    void checkFailCondition()
    {
        if (Input.anyKeyDown)
        {
            keyPressCount++;

            if (keyPressCount >= 15 && keyPressCount < 20)
            {
                ui.takeDamage(2f);
            }
            else if (keyPressCount >= 20 && keyPressCount < 30)
            {
                ui.takeDamage(5f);
            }
            else if (keyPressCount >= 30)
            {
                ui.takeDamage(8f);
            }

            checkAlarm();
        }
    }

    // Plays alarm sound if key press limit is exceeded.
    void checkAlarm()
    {
        if (keyPressCount < 30)
        {
            alarmSound.Stop();
        } 
        else
        {
            alarmSound.Play();
        }
    }

    // Locks player movement and sets a fixed position and rotation.
    void lockPlayerMov()
    {
        if (playerMov != null)
        {
            playerMov.canMove = false;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false; 
        }
        player.transform.position = new Vector3(151.441f, 25.74532f, 52.121f);
        player.transform.rotation = Quaternion.Euler(0, -180, 0);

        lockMov = true;
    }

    // Checks for key presses to change button states.
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

    // Changes the state of a button and rotates it accordingly.
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

    // Rotates a button smoothly to a target rotation.
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

    // Checks if all buttons are pressed and triggers the next phase.
    void checkWinCondition()
    {
        if (!firstPhaseComplete && swichtboardState.All(state => state == false))
        {
            firstPhaseComplete = true;
        }
        else if (firstPhaseComplete && swichtboardState.All(state => state == true))
        {
            doorController.closeSwitchboardDoor(6);
            observationDoorAnim.SetBool("open", true);
            audioDoor.Play();
            code1.gameObject.SetActive(true);
            code1.Play();
            ActivateAllLights();
            StartCoroutine(waitToUnlock());
        }
    }

    // Activates all light objects in the scene.
    void ActivateAllLights()
    {
        Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(true); 
        }
    }

    // Waits before unlocking player movement again.
    IEnumerator waitToUnlock()
    {
        yield return new WaitForSeconds(3f);
        playerMov.canMove = true;
    }
}
