using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission9Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(117.063f, 20.679f, 87.984f);
    Vector3 playerFinalPos = new Vector3(118.02f, 20.679f, 87.24f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;
    public bool finishGame = false;
    bool finish = false;

    [SerializeField] GameObject compass;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject navigationScreen;
    [SerializeField] GameObject syncronizationScreen;
    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] GameObject puzzle;
    puzzleController puzzlecontroller;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam15;
    [SerializeField] CinemachineVirtualCamera vcam16;

    int opened;
    bool resetState = false;
    bool enableCapture = false;

    // Initializes components and sets up initial camera view.
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        canvasGroup = info.GetComponent<CanvasGroup>();
        puzzlecontroller = puzzle.GetComponent<puzzleController>();

        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[8];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
        navigationScreen.SetActive(true);
    }

    // Updates game state when finished and handles final movement.
    void Update()
    {
        manageKeyPressed();
        if (!(finishGame && !finish)) return;
        CompleteMission();
        
    }

    // Starts compass interaction when 'X' is pressed.
    void manageKeyPressed()
    {
        if (enableCapture && compass.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (Vector3.Distance(player.transform.position, playerPos) < 0.01f && !change)
            {
                playerAnim.SetBool("compass", true);
                StartCoroutine(waitUntilFinish());
                change = true;
            }
        }
    }

    // Finalizes mission, unlocks player and sets completion state.
    void CompleteMission()
    {
        GameManager.GameManagerInstance.missionsCompleted[8] = 1;
        GameManager.GameManagerInstance.SaveProgress();

        SwapCameras(1, 0, 0);
        player.transform.position = playerFinalPos;
        playerMov.canMove = true;
        cc.enabled = true;
        finish = true;
        this.enabled = false;
    }

    // Shows 'X' when leaving grids.  
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[8];
        if (other.gameObject.CompareTag("console") && opened == 0)
        {
            letterX.SetActive(true);
            enableCapture = true;
        }
    }

    // Hides 'X' when leaving grids.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("console"))
        {
            letterX.SetActive(false);
        }
    }

    // Waits until finish animation and syncronization video.
    IEnumerator waitUntilFinish()
    {
        yield return new WaitForSeconds(3f);
        syncronizationScreen.SetActive(true);
        puzzle.SetActive(true);
        yield return new WaitForSeconds(5f);
        StartCoroutine(waitToShow());
        navigationScreen.SetActive(false);
        syncronizationScreen.SetActive(false);
        playerAnim.SetBool("compass", false);
        compass.SetActive(false);
        SwapCameras(0, 0, 1);
        puzzlecontroller.canMove = true;
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
        vcam15.Priority = priority2;
        vcam16.Priority = priority3;
    }
}
