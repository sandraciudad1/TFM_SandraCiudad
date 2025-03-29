using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;
using TMPro;

public class mission4Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(85.9f, 30.745f, 43.46f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, -81.7f, 0f));
    bool change = false;

    [SerializeField] GameObject securityCard;
    [SerializeField] GameObject scifi_terminal;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam7;

    [SerializeField] VideoPlayer cardReader;
    [SerializeField] VideoPlayer analyzingScreen;
    [SerializeField] VideoPlayer loadingScreen;
    [SerializeField] VideoPlayer sequence3;
    [SerializeField] VideoPlayer sequence4;
    [SerializeField] VideoPlayer sequence5;
    [SerializeField] VideoPlayer code4;
    [SerializeField] VideoPlayer electricalProblem;
    bool readSequence = false;

    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] GameObject inputTextBg;
    [SerializeField] TextMeshProUGUI inputText;
    static int charCounter = 3;
    string userInput = "";
    string[] codes;
    bool hasFinish = false;

    [SerializeField] GameObject labDoor;
    Animator labDoorAnim;
    AudioSource audioDoor;

    mission5Controller mision5;
    [SerializeField] GameObject playerTrigger;
    playerUI ui;
    int opened;
    bool resetState = false;

    // Initializes components and sets up the game state.
    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        canvasGroup = info.GetComponent<CanvasGroup>();

        codes = new string[] { "DBK", "ASLZ", "OYLWP" };
        labDoorAnim = labDoor.GetComponent<Animator>();
        audioDoor = labDoor.GetComponent<AudioSource>();
        mision5 = playerTrigger.GetComponent<mission5Controller>();

        ui = playerTrigger.GetComponent<playerUI>();
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[3];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Checks user input and triggers mission completion events.
    void Update()
    {
        if (readSequence)
        {
            readUserInput();
        }

        if (charCounter > 5 && !hasFinish)
        {
            GameManager.GameManagerInstance.LoadProgress();
            GameManager.GameManagerInstance.missionsCompleted[3] = 1;
            GameManager.GameManagerInstance.SaveProgress();
            inputTextBg.SetActive(false);
            SwapCameras(1, 0);
            playerMov.canMove = true;
            cc.enabled = true;
            code4.gameObject.SetActive(true);
            labDoorAnim.SetBool("open", true);
            audioDoor.Play();
            mision5.initializeAlarms();
            hasFinish = true;
        }
    }

    // Reads and validates the player's input code.
    void readUserInput()
    {
        foreach (char c in Input.inputString)
        {
            if (userInput.Length < charCounter)
            {
                userInput += c;
            }
            else if (c == '\b' && userInput.Length > 0)
            {
                userInput = userInput.Substring(0, userInput.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                string correctCode = codes[charCounter - 3];
                if (userInput.ToUpper() == correctCode.ToUpper())
                {
                    userInput = "";
                    charCounter++;
                    readSequence = false;
                    checkSequence();   
                }
                else
                {
                    float time = 0;
                    if (charCounter > 3)
                    {
                        time = 3f;
                    }
                    StartCoroutine(waitShowSequence(time));
                    ui.takeDamage(40f);
                    ui.wasteOxygen(25f);
                    userInput = "";
                }
            }
        }
        inputText.text = userInput;
    }

    // Plays and hides particle effect before checking sequence
    IEnumerator waitShowSequence(float time)
    {
        electricalProblem.gameObject.SetActive(true);
        electricalProblem.Play();
        yield return new WaitForSeconds(time);
        checkSequence();
    }

    // Shows 'X' when near scifi terminal.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[3];
        if (other.gameObject.CompareTag("scifi_terminal") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving scifi terminal.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("scifi_terminal"))
        {
            letterX.SetActive(false);
        }
    }

    // Manages actions when staying near scifi terminal.
    private void OnTriggerStay(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[3];
        if (other.gameObject.CompareTag("scifi_terminal") && securityCard.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            letterX.SetActive(false);
            SwapCameras(0, 1);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                playerAnim.SetBool("securityCard", true);
                StartCoroutine(showVideos());
                change = true;
            }
        }
    }

    // Displays security card analysis and loading screens.
    IEnumerator showVideos()
    {
        cardReader.gameObject.SetActive(false);
        analyzingScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(8f);
        playerAnim.SetBool("securityCard", false);
        analyzingScreen.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        loadingScreen.gameObject.SetActive(false);
        StartCoroutine(waitToShow());
        checkSequence();
    }

    // Checks and updates the input sequence.
    void checkSequence()
    {
        if (charCounter == 3)
        {
            StartCoroutine(showSequences(4, 6, sequence3));
        } 
        else if (charCounter == 4)
        {
            StartCoroutine(showSequences(1, 8, sequence4));
        }
        else if (charCounter == 5)
        {
            StartCoroutine(showSequences(1, 10, sequence5));
        }
    }

    // Plays and manages video sequences.
    IEnumerator showSequences(float wait1, float wait2, VideoPlayer sequence)
    {
        yield return new WaitForSeconds(wait1);
        inputTextBg.SetActive(false);
        electricalProblem.gameObject.SetActive(false);
        sequence.gameObject.SetActive(false);
        sequence.frame = 0;  
        sequence.targetTexture.Release(); 

        yield return null; 
        sequence.gameObject.SetActive(true);
        sequence.Play();

        yield return new WaitForSeconds(wait2);
        sequence.gameObject.SetActive(false);
        inputTextBg.SetActive(true);
        readSequence = true;
    }

    // Waits before showing the info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.5f);
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
        vcam7.Priority = priority2;
    }
}
