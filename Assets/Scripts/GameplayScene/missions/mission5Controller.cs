using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Video;

public class mission5Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(108.812f, 30.745f, 52.262f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;

    [SerializeField] GameObject alarm1;
    [SerializeField] GameObject alarm2;
    [SerializeField] GameObject alarm3;
    [SerializeField] GameObject alarm4;
    [SerializeField] GameObject alarm5;
    GameObject[] alarms;

    [SerializeField] GameObject wireCutters;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject progressBar;
    [SerializeField] Image pb;

    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam8;

    [SerializeField] Material cable1;
    [SerializeField] Material cable2;
    [SerializeField] Material cable3;
    [SerializeField] Material cable4;
    Material[] cableMaterials;

    [SerializeField] VideoPlayer alarmOn;
    [SerializeField] Image alarmOff;
    [SerializeField] Image square1;
    [SerializeField] Image square2;
    [SerializeField] Image square3;
    [SerializeField] Image square4;

    static int cableCounter = 0;
    static int spacePressed = 0;
    bool finish = false;
    bool isShowing = false;
    bool solveMission = false;

    [SerializeField] GameObject playerTrigger;
    playerUI ui;
    int opened;
    bool resetState = false;

    // Initializes variables and resets cable colors. 
    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        canvasGroup = info.GetComponent<CanvasGroup>();
        cableMaterials = new Material[] { cable1, cable2, cable3, cable4 };
        for (int i = 0; i < cableMaterials.Length; i++)
        {
            cableMaterials[i].SetColor("_Color", Color.white);
        }

        alarms = new GameObject[] { alarm1, alarm2, alarm3, alarm4, alarm5 };
        ui = playerTrigger.GetComponent<playerUI>();
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[4];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Manages game state and updates player position and UI.
    void Update()
    {
        if (cableCounter >= 0 && cableCounter <= 3)
        {
            setPlayerPosition(cableCounter);
            updateProgressBar();
        }
        else if (cableCounter == 4 && !finish)
        {
            GameManager.GameManagerInstance.LoadProgress();
            GameManager.GameManagerInstance.missionsCompleted[4] = 1;
            GameManager.GameManagerInstance.SaveProgress();
            desactivateAlarms();
            lightInteraction(true);
            playerAnim.SetBool("wireCutters", false);
            progressBar.SetActive(false);
            SwapCameras(1, 0);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 51.082f);
            playerMov.canMove = true;
            cc.enabled = true;
            finish = true;
        }

        if (finish && !isShowing)
        {
            solveMission = true;
            StartCoroutine(showPinCode());
        }
    }

    // Moves the player based on the selected cable.  
    void setPlayerPosition(int cable)
    {
        switch (cable)
        {
            case 1:
                player.transform.position = new Vector3(108.99f, player.transform.position.y, 52.41f);
                break;
            case 2:
                player.transform.position = new Vector3(109.14f, player.transform.position.y, 52.41f);
                break;
            case 3:
                player.transform.position = new Vector3(109.34f, player.transform.position.y, 52.41f);
                break;
            default:
                break;
        }
    }

    // Updates progress bar when pressing space.  
    void updateProgressBar()
    {
        if (progressBar.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed++;
            if (spacePressed < 6)
            {
                pb.fillAmount = (float)spacePressed / 6;
            }
            else
            {
                cableMaterials[cableCounter].SetColor("_Color", Color.black);
                cableCounter++;
                resetvalues();
            }
        }
    }

    // Resets progress bar and space press counter.  
    void resetvalues()
    {
        pb.fillAmount = 0;
        spacePressed = 0;
    }

    // Displays a sequence of squares as a pin code.  
    public IEnumerator showPinCode()
    {
        isShowing = true;
        yield return new WaitForSeconds(0.5f);
        square1.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        square1.gameObject.SetActive(false);
        square2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        square2.gameObject.SetActive(false);
        square3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        square3.gameObject.SetActive(false);
        square4.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        square4.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        isShowing = false;
    }

    // Activates alarms with movement and visual effects.  
    public void initializeAlarms()
    {
        alarmMovement(alarm1, 135f, 0f, 0f);
        alarmMovement(alarm2, 135f, 0f, 0f);
        alarmMovement(alarm3, 135f, 0f, 0f);
        alarmMovement(alarm4, 135f, 90f, 90f);
        alarmMovement(alarm5, 135f, 90f, 90f);
        alarmOn.gameObject.SetActive(true);
    }

    // Deactivates alarms and turns them off.  
    public void desactivateAlarms()
    {
        alarmOn.gameObject.SetActive(false);
        alarmOff.gameObject.SetActive(true);
        alarm1.SetActive(false);
        alarm2.SetActive(false);
        alarm3.SetActive(false);
        alarm4.SetActive(false);
        alarm5.SetActive(false);
    }

    // Animates alarm rotation with a looping effect.  
    void alarmMovement(GameObject alarm, float x, float y, float z)
    {
        float duration = Random.Range(2f, 4f);
        float delay = Random.Range(0f, 1.5f);

        alarm.transform.DORotate(new Vector3(x, y, z), duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay);
    }

    // Detects when the player enters an area.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("alarm"))
        {
            // corte de electricidad 
            lightInteraction(false);
            ui.useEnergy(15f);
        }
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[4];
        if (other.gameObject.CompareTag("securitySystem") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    int lightDecrementation = 0;

    void lightInteraction(bool active)
    {
        Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
        foreach (Light light in lights)
        {
            if (!active)
            {
                light.intensity = Mathf.Max(0.1f, light.intensity - 0.1f);
                lightDecrementation++;
            }
            else
            {
                light.intensity = light.intensity + (0.1f * (lightDecrementation/lights.Length));
            }
        }
        for (int i = 0; i < alarms.Length; i++)
        {
            Light alarmLight = alarms[i].GetComponent<Light>();
            alarmLight.intensity = 5.4f;
        }
    }

    // Hides 'X' when leaving scifi terminal.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("securitySystem"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[4];
        if (other.gameObject.CompareTag("securitySystem") && wireCutters.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            letterX.SetActive(false);
            SwapCameras(0, 1);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                StartCoroutine(waitToShow());
                playerAnim.SetBool("wireCutters", true);
                StartCoroutine(waitUntilMove());
                change = true;
            }
        }
    }

    // Delays player movement for 2 seconds.  
    IEnumerator waitUntilMove()
    {
        yield return new WaitForSeconds(2f);
        player.transform.position = new Vector3(108.809f, player.transform.position.y, 52.41f);
    }

    // Waits before showing the info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        info.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutCoroutine());
        yield return new WaitForSeconds(1.5f);
        progressBar.SetActive(true);
    }

    // Fades out the info over time.
    IEnumerator FadeOutCoroutine()
    {
        float duration = 1.5f;
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
        vcam8.Priority = priority2;
    }
}
