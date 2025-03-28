using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mission8Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(124.41f, 20.679f, 69.42f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;

    [SerializeField] GameObject vacuum;
    [SerializeField] GameObject vacuumMobile;
    [SerializeField] GameObject limits;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam13;
    [SerializeField] CinemachineVirtualCamera vcam14;

    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;
    [SerializeField] GameObject screen;
    [SerializeField] GameObject screenCode;
    [SerializeField] GameObject navigationScreen;

    public bool enableControl = false;
    public bool finish = false;
    bool exit = false;
    bool solveMission = false;
    float speed = 1f;

    [SerializeField] GameObject playerTrigger;
    playerUI ui;

    static int actualSample = 0;
    float startTime = 300f;
    float currentTime;
    [SerializeField] TextMeshProUGUI timerText;
    bool startTimer = false, isRunning = false;
    int opened;
    bool resetState = false;

    // Initializes components and sets the initial camera configuration.
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        canvasGroup = info.GetComponent<CanvasGroup>();
        ui = playerTrigger.GetComponent<playerUI>();
        currentTime = startTime;
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[1];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Handles movement and controls the game's finishing sequence.
    void Update()
    {
        if (enableControl)
        {
            float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime; 
            float moveZ = -Input.GetAxis("Horizontal") * speed * Time.deltaTime; 
            vacuumMobile.transform.position += new Vector3(0, moveY, moveZ);
        }

        if (startTimer && !isRunning)
        {
            isRunning = true;
        }

        if (isRunning)
        {
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

            if (currentTime <= 60f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }

        if (finish && !exit)
        {
            GameManager.GameManagerInstance.LoadProgress();
            GameManager.GameManagerInstance.missionsCompleted[7] = 1;
            GameManager.GameManagerInstance.SaveProgress();
            startTimer = false;
            isRunning = false;
            timerText.gameObject.SetActive(false);
            solveMission = true;
            limits.SetActive(false);
            screen.SetActive(false);
            screenCode.SetActive(true);
            vacuumMobile.SetActive(false);
            playerMov.canMove = true;
            cc.enabled = true;
            SwapCameras(1, 0, 0);
            navigationScreen.SetActive(true);
            exit = true;
        }
    }

    // Releases particles, consuming energy and wasting oxygen.
    void timerEnded()
    {
        // se desprenden particulas al aire
        ui.useEnergy(40f);
        ui.wasteOxygen(40f);
        currentTime = startTime / 2;
    }

    // Shows 'X' when leaving grids.  
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[7];
        if (other.gameObject.CompareTag("grids") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving grids.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("grids"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[7];
        if (other.gameObject.CompareTag("grids") && vacuum.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            startTimer = true;
            timerText.gameObject.SetActive(true);
            timerText.text = "05:00";
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                playerAnim.SetBool("vacuum", true);
                StartCoroutine(waitFinishAnimation());
                change = true;
            }
        }
    }

    // Waits for animation to finish before enabling vacuum mode.
    IEnumerator waitFinishAnimation()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(waitToShow());
        vacuum.SetActive(false);
        vacuumMobile.SetActive(true);
        playerAnim.SetBool("vacuum", false);
        SwapCameras(0, 0, 1);
        limits.SetActive(true);
        enableControl = true;
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

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam13.Priority = priority2;
        vcam14.Priority = priority3;
    }
}
