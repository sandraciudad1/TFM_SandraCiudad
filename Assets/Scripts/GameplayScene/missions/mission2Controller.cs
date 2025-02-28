using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class mission2Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(105.317f, 30.74516f, 63.886f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, -180f, 0f));

    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;

    [SerializeField] GameObject letterX;
    [SerializeField] GameObject waitingVideo;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam3;
    bool swap = false;

    [SerializeField] Image linelBar;
    [SerializeField] TextMeshProUGUI counterAnalyzer;
    [SerializeField] Image radialBar;
    [SerializeField] TextMeshProUGUI percentage;
    float fillTime = 3f;

    static int actualSample = 0;
    //static int samplesAnalyzed = 0;

    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        playerMov = player.GetComponent<PlayerMovement>();

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


    void Update()
    {
        if(GameManager.GameManagerInstance.samplesCounter == 4)
        {
            // ha desbloqueado la siguiente cinta y se termina la mision
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument"))
        {
            letterX.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument"))
        {
            letterX.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("analyticalInstrument") && sampleActive()>0 && Input.GetKeyDown(KeyCode.X))
        {
            if (checkSample(sampleActive()))
            {
                playerMov.canMove = false;
                player.transform.position = playerPos;
                player.transform.rotation = playerRot;
                if (player.transform.position == playerPos && !swap)
                {
                    Debug.Log("player pos x " + player.transform.position.x);
                    Debug.Log("player pos y " + player.transform.position.y);
                    Debug.Log("player pos z " + player.transform.position.z);
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

    IEnumerator waitAnalyzeAnim()
    {
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("analyze", true);
        yield return new WaitForSeconds(0.4f);
        playerAnim.SetBool("closeHand", false);
        playerAnim.SetBool("analyze", false);
        StartCoroutine(fillBar());
        yield return new WaitForSeconds(3f);
        incrementSamplesCounter();
        yield return new WaitForSeconds(0.4f);
        hideSample();
        resetValues();
    }

    void resetValues()
    {
        waitingVideo.SetActive(true);
        SwapCameras(1, 0);
        playerMov.canMove = true;
        radialBar.fillAmount = 0;
        percentage.text = "0%";
        swap = false;
    }

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

    void incrementSamplesCounter()
    {
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.samplesCounter++;
        GameManager.GameManagerInstance.SaveProgress();

        //samplesAnalyzed++;
        GameManager.GameManagerInstance.LoadProgress();
        counterAnalyzer.text = GameManager.GameManagerInstance.samplesCounter.ToString() + "/4";
        linelBar.fillAmount = (float)GameManager.GameManagerInstance.samplesCounter / 4;
    }

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
        else
        {
            return -1;
        }
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam3.Priority = priority2;
    }
}
