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
    bool finish = false;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam3;
    bool swap = false;

    [SerializeField] Image linelBar;
    [SerializeField] TextMeshProUGUI counterAnalyzer;
    [SerializeField] Image radialBar;
    [SerializeField] TextMeshProUGUI percentage;
    float fillTime = 3f;

    static int actualSample = 0;

    // Initializes variables and resets sample progress.
    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        playerMov = player.GetComponent<PlayerMovement>();
        cc = player.GetComponent<CharacterController>();

        GameManager.GameManagerInstance.LoadProgress();
        for(int i=0; i<GameManager.GameManagerInstance.samplesUnlocked.Length; i++)
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("samples", i, 0);
        }
        GameManager.GameManagerInstance.samplesCounter = 0;
        GameManager.GameManagerInstance.SaveProgress();
        
        counterAnalyzer.text = GameManager.GameManagerInstance.samplesCounter.ToString() + "/4";
        linelBar.fillAmount = (float)GameManager.GameManagerInstance.samplesCounter / 4;
        
    }

    // Handles player repositioning and checks mission completion.
    void Update()
    {
        if (changePos)
        {
            cc.enabled = false;  
            player.transform.position = initialPos;  
            cc.enabled = true;   
            changePos = false;
        }

        if (GameManager.GameManagerInstance.samplesCounter == 4)
        {
            code2.gameObject.SetActive(true);
            code2.Play();
            unknownSamples.gameObject.SetActive(true);
            unknownSamples.Play();
            waitingVideo.SetActive(false);
            finish = true;
        }
    }

    // Shows 'X' when near an analytical instrument.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument") && !finish)
        {
            letterX.SetActive(true);
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

    // Analyzes a sample when pressing 'X' near an instrument.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument") && sampleActive()>0 && Input.GetKeyDown(KeyCode.X))
        {
            if (checkSample(sampleActive()))
            {
                playerMov.canMove = false;
                cc.enabled = false;
                player.transform.position = playerPos;
                player.transform.rotation = playerRot;
                if (player.transform.position == playerPos && player.transform.rotation == playerRot && !swap)
                {
                    cc.enabled = true;
                    SwapCameras(0, 1);
                    StartCoroutine(waitAnalyzeAnim());
                    swap = true;
                }
            } else
            {
                Debug.Log("ya ha sido analizada");
            }
        }
    }

    // Checks if a sample was already analyzed.
    bool checkSample(int sampleId)
    {
        int value = GameManager.GameManagerInstance.GetArrayUnlocked("samples", (sampleId - 1));
        if (value == 1)
        {
            return false;
        } else
        {
            return true;
        }
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
        GameManager.GameManagerInstance.LoadProgress();
        if (actualSample == 1)
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("samples", 0, 1);
            GameManager.GameManagerInstance.SaveProgress();
            sample1.SetActive(false);
        } 
        else if (actualSample == 2)
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("samples", 1, 1);
            GameManager.GameManagerInstance.SaveProgress();
            sample2.SetActive(false);
        } 
        else if (actualSample == 3) 
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("samples", 2, 1);
            GameManager.GameManagerInstance.SaveProgress();
            sample3.SetActive(false);
        }
        else if (actualSample == 4)
        {
            GameManager.GameManagerInstance.SetArrayUnlocked("samples", 3, 1);
            GameManager.GameManagerInstance.SaveProgress();
            sample4.SetActive(false);
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
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.samplesCounter++;
        GameManager.GameManagerInstance.SaveProgress();

        GameManager.GameManagerInstance.LoadProgress();
        counterAnalyzer.text = GameManager.GameManagerInstance.samplesCounter.ToString() + "/4";
        linelBar.fillAmount = (float)GameManager.GameManagerInstance.samplesCounter / 4;
    }

    // Returns the currently active sample.
    int sampleActive()
    {
        if(sample1.activeInHierarchy)
        {   
            actualSample = 1;
            return 1;
        } 
        else if(sample2.activeInHierarchy)
        {
            actualSample = 2;
            return 2;
        }
        else if (sample3.activeInHierarchy)
        {
            actualSample = 3;
            return 3;
        }
        else if (sample4.activeInHierarchy)
        {
            actualSample = 4;
            return 4;
        } 
        else { return -1; }
    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam3.Priority = priority2;
    }
}
