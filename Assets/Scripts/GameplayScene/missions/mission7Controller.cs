using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mission7Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(117.927f, 25.67f, 49.789f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] GameObject verticalDoor;
    Animator doorAnimator;
    AudioSource doorAudio;

    [SerializeField] GameObject bg;
    [SerializeField] TextMeshProUGUI inputText;
    string userInput = "";
    bool readCode = false;

    [SerializeField] GameObject emergencyKit;
    [SerializeField] GameObject emergencyKitStatic;
    Animator emergencyKitStaticAnim;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam11;

    [SerializeField] GameObject code;
    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;
    bool canCollect = false;
    bool finish = false;
    static int objectsCollected = 0;

    [SerializeField] GameObject alcohol, bandAidRoll, medicalPackage, burnCream, oxyWater, scissor, miniAidBox, hydroCream;
    [SerializeField] GameObject alcoholKit, bandAidRollKit, medicalPackageKit, burnCreamKit, oxyWaterKit, scissorkit, miniAidBoxKit, hydroCreamKit;
    GameObject[] objectsArray;
    GameObject[] objectsKitArray;

    [SerializeField] GameObject playerTrigger;
    playerUI ui;

    float startTime = 150f;
    float currentTime;
    [SerializeField] TextMeshProUGUI timerText;
    bool startTimer = false, isRunning = false;
    int opened;
    bool resetState = false;

    // Initializes references and sets up objects at the start of the game.
    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        doorAnimator = verticalDoor.GetComponent<Animator>();
        emergencyKitStaticAnim = emergencyKitStatic.GetComponent<Animator>();
        doorAudio = verticalDoor.GetComponent<AudioSource>();

        canvasGroup = info.GetComponent<CanvasGroup>();
        objectsArray = new GameObject[] { alcohol, bandAidRoll, medicalPackage, burnCream, oxyWater, scissor, miniAidBox, hydroCream };
        objectsKitArray = new GameObject[] { alcoholKit, bandAidRollKit, medicalPackageKit, burnCreamKit, oxyWaterKit, scissorkit, miniAidBoxKit, hydroCreamKit };

        ui = playerTrigger.GetComponent<playerUI>();
        currentTime = startTime;
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[6];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Checks for input and manages object collection and door code input.
    void Update()
    {
        HandleTimer();
        HandleDoorCodeInput();
        HandleFinishCondition();
    }

    // Updates timer countdown and triggers effects on zero.
    void HandleTimer()
    {
        if (startTimer && !isRunning)
            isRunning = true;

        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            timerEnded();
            isRunning = false;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.color = currentTime <= 60f ? Color.red : Color.white;
    }

    // Reads user input and opens door if code is correct.
    void HandleDoorCodeInput()
    {
        if (!readCode) return;

        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c) && userInput.Length < 4)
            {
                userInput += c;
            }
            else if (c == '\b' && userInput.Length > 0)
            {
                userInput = userInput[..^1];
            }
            else if (c == '\n' || c == '\r')
            {
                if (userInput == "5726")
                {
                    doorAnimator.SetBool("open", true);
                    doorAudio.Play();
                    StartCoroutine(hideDoor());
                    bg.SetActive(false);
                    readCode = false;
                }
                else
                {
                    userInput = "";
                }
            }
        }

        inputText.text = userInput;
    }

    // Checks if mission is complete and finalizes state.
    void HandleFinishCondition()
    {
        if (objectsCollected < 8 || finish) return;

        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.missionsCompleted[6] = 1;
        GameManager.GameManagerInstance.SaveProgress();

        startTimer = false;
        isRunning = false;
        timerText.gameObject.SetActive(false);
        movePlayer();
        code.SetActive(true);
        playerMov.canMove = true;
        cc.enabled = true;
        finish = true;
        this.enabled = false;
    }

    // Restarts mission and applies heavy energy loss.
    void timerEnded()
    {
        ui.takeDamage(30f);
        ui.useEnergy(40f);
        currentTime = startTime / 2;
    }

    // Hides the door after a 3-second delay.
    IEnumerator hideDoor()
    {
        yield return new WaitForSeconds(3f);
        verticalDoor.SetActive(false);
    }

    // Moves the player to a specific position and rotation.
    void movePlayer()
    {
        playerMov.canMove = false;
        cc.enabled = false;
        player.transform.position = playerPos;
        player.transform.rotation = playerRot;
    }

    // Activates letter X when player enters the "kit" trigger area.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[6];
        if (other.gameObject.CompareTag("kit") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    // Deactivates background and letter X on trigger exit.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(false);
        }
        if (other.gameObject.CompareTag("kit"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area and handles interactions.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(true);
            readCode = true;            
        }

        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[6];
        if (other.gameObject.CompareTag("kit") && emergencyKit.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            startTimer = true;
            timerText.gameObject.SetActive(true);
            timerText.text = "05:00";
            letterX.SetActive(false);
            movePlayer();
            if (player.transform.position == playerPos && !change)
            {
                SwapCameras(0, 1);
                playerAnim.SetBool("kit", true);
                StartCoroutine(waitFinishAnim());
                change = true;
            }
        }

        if (other.CompareTag("emergencyKit") && canCollect && Input.GetKeyDown(KeyCode.R))
        {
            string name = other.name;
            int index = name switch
            {
                "Alcohol" => 0,
                "BandAidRoll" => 1,
                "MedicalPackage" => 2,
                "BurnCream" => 3,
                "OxyWater" => 4,
                "Scissor1" => 5,
                "MiniAidBox" => 6,
                "HydroCream" => 7,
                _ => -1
            };

            if (index >= 0) collectObject(index);
        }
    }

    // Collects objects and activates their corresponding kit.
    void collectObject(int index)
    {
        objectsArray[index].SetActive(false);
        objectsKitArray[index].SetActive(true);
        objectsCollected++;
    }

    // Handles animation transitions after the kit interaction.
    IEnumerator waitFinishAnim()
    {
        yield return new WaitForSeconds(2.5f);
        emergencyKit.SetActive(false);
        emergencyKitStatic.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        playerAnim.SetBool("kit", false);
        emergencyKitStaticAnim.SetBool("open", true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(waitToShow());
        SwapCameras(1, 0);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 51.77582f);
        playerMov.canMove = true;
        cc.enabled = true;
        canCollect = true;
    }

    // Waits before showing the info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.2f);
        info.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCoroutine());
    }

    // Fades out the info over time.
    IEnumerator FadeOutCoroutine()
    {
        float duration = 2f;
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam11.Priority = priority2;
    }
}
