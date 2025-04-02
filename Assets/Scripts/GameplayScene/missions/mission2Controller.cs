using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class mission2Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    CharacterController cc;
    Animator playerAnim;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(105.317f, 30.74516f, 63.886f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, -180f, 0f));
    Vector3 initialPos = new Vector3(104f, 30.74516f, 65.328f);
    bool changePos = false;

    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;

    [SerializeField] GameObject letterX;
    [SerializeField] GameObject waitingVideo;
    [SerializeField] VideoPlayer code2;
    [SerializeField] VideoPlayer unknownSamples;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam3;
    bool swap = false;

    [SerializeField] Image linelBar;
    [SerializeField] TextMeshProUGUI counterAnalyzer;
    [SerializeField] Image radialBar;
    [SerializeField] TextMeshProUGUI percentage;
    float fillTime = 3f;

    [SerializeField] GameObject playerTrigger;
    playerUI ui;

    static int actualSample = 0;
    float startTime = 240f;
    float currentTime;
    [SerializeField] TextMeshProUGUI timerText;
    bool startTimer = false, isRunning = false;

    [SerializeField] ParticleSystem lightingParticles;
    [SerializeField] GameObject sampleAnalyzedInfo;
    CanvasGroup canvasGroup;
    int opened;
    bool resetState = false;
    bool enableCapture = false;

    // Initializes variables and resets sample progress.
    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        playerMov = player.GetComponent<PlayerMovement>();
        cc = player.GetComponent<CharacterController>();

        ui = playerTrigger.GetComponent<playerUI>();
        counterAnalyzer.text = GameManager.GameManagerInstance.samplesCounter.ToString() + "/4";
        linelBar.fillAmount = (float)GameManager.GameManagerInstance.samplesCounter / 4;
        
        currentTime = startTime;
        canvasGroup = sampleAnalyzedInfo.GetComponent<CanvasGroup>();
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[1];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Calls all game management methods every frame.
    void Update()
    {
        manageTimer();
        manageKeyPressed();
        managePosition();
        manageFinal();
    }

    // Handles countdown timer and updates UI color.
    void manageTimer()
    {
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
    }

    // Checks key input and starts analysis sequence.
    void manageKeyPressed()
    {
        if(enableCapture && sampleActive() > 0 && Input.GetKeyDown(KeyCode.X))
        {
            if (checkSample(sampleActive()))
            {
                startTimer = true;
                timerText.gameObject.SetActive(true);
                timerText.text = "05:00";
                playerMov.canMove = false;
                cc.enabled = false;
                player.transform.position = playerPos;
                player.transform.rotation = playerRot;
                if (player.transform.position == playerPos && !swap)
                {
                    SwapCameras(0, 1);
                    StartCoroutine(waitAnalyzeAnim());
                    swap = true;
                }
            }
        }
    }

    // Resets player position when changePos is true.
    void managePosition()
    {
        if (changePos)
        {
            cc.enabled = false;
            player.transform.position = initialPos;
            cc.enabled = true;
            changePos = false;
        }
    }

    // Ends mission when all samples are completed.
    void manageFinal()
    {
        if (GameManager.GameManagerInstance.samplesCounter == 4)
        {
            GameManager.GameManagerInstance.LoadProgress();
            GameManager.GameManagerInstance.missionsCompleted[1] = 1;
            GameManager.GameManagerInstance.SaveProgress();
            startTimer = false;
            isRunning = false;
            timerText.gameObject.SetActive(false);
            code2.gameObject.SetActive(true);
            code2.Play();
            unknownSamples.gameObject.SetActive(true);
            unknownSamples.Play();
            waitingVideo.SetActive(false);
            this.enabled = false;
        }
    }

    // Triggers lab explosion and applies major damage to player
    void timerEnded()
    {
        StartCoroutine(playParticleSystem(lightingParticles));
        ui.takeDamage(30f);
        ui.useEnergy(40f);
        currentTime = startTime/3;
    }

    // Plays particle system for 5 seconds then stops it
    IEnumerator playParticleSystem(ParticleSystem particles)
    {
        particles.gameObject.SetActive(true);
        particles.Play();
        yield return new WaitForSeconds(3f);
        particles.Stop();
    }

    // Shows 'X' when near an analytical instrument.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[1];
        if (other.gameObject.CompareTag("analyticalInstrument") && opened == 0)
        {
            letterX.SetActive(true);
            enableCapture = true;
        }
    }

    // Hides 'X' when leaving an analytical instrument.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument"))
        {
            letterX.SetActive(false);
        }
    }

    // Checks if a sample was already analyzed.
    bool checkSample(int sampleId)
    {
        int value = GameManager.GameManagerInstance.GetArrayUnlocked("samples", (sampleId - 1));
        if (value == 1)
        {
            StartCoroutine(waitToShow());
            ui.useEnergy(5f);
            return false;
        } else
        {
            return true;
        }
    }

    // Waits before showing analyzed sample info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        sampleAnalyzedInfo.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCoroutine());
    }

    // Fades out the space key info over time.
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

    // Plays animation and updates sample progress. 
    IEnumerator waitAnalyzeAnim()
    {
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("analyze", true);
        yield return new WaitForSeconds(1.5f);
        hideSample();
        playerAnim.SetBool("analyze", false);
        playerAnim.SetBool("closeHand", false);
        StartCoroutine(fillBar());
        yield return new WaitForSeconds(3f);
        incrementSamplesCounter();
        yield return new WaitForSeconds(0.4f);
        resetValues();
    }

    // Resets position, UI, and movement after analysis.
    void resetValues()
    {
        waitingVideo.SetActive(true);
        SwapCameras(1, 0);
        changePos = true;
        playerMov.canMove = true;
        radialBar.fillAmount = 0;
        percentage.text = "0%";
        swap = false;
    }

    // Marks a sample as analyzed and hides it.
    void hideSample()
    {
        int index = actualSample - 1;
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.SetArrayUnlocked("samples", index, 1);
        GameManager.GameManagerInstance.SaveProgress();

        GameObject[] samples = { sample1, sample2, sample3, sample4 };
        if (index >= 0 && index < samples.Length)
        {
            samples[index].SetActive(false);
        }
    }

    // Fills the radial progress bar over time.
    IEnumerator fillBar()
    {
        waitingVideo.SetActive(false);
        float elapsedTime = 0f;
        while (elapsedTime < fillTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / fillTime);
            radialBar.fillAmount = fillAmount;
            percentage.text = Mathf.RoundToInt(fillAmount * 100) + "%"; 
            yield return null;
        }
        radialBar.fillAmount = 1f;
        percentage.text = "100%";
    }

    // Increases the analyzed sample counter and updates UI.
    void incrementSamplesCounter()
    {
        var gm = GameManager.GameManagerInstance;
        gm.samplesCounter++;
        gm.SaveProgress();
        gm.LoadProgress();
        counterAnalyzer.text = gm.samplesCounter.ToString() + "/4";
        linelBar.fillAmount = (float)gm.samplesCounter / 4;
    }

    // Returns the currently active sample.
    int sampleActive()
    {
        GameObject[] samples = { sample1, sample2, sample3, sample4 };
        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i].activeInHierarchy)
            {
                actualSample = i + 1;
                return actualSample;
            }
        }
        return -1;
    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam3.Priority = priority2;
    }
}
