using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;

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

    [SerializeField] GameObject wireCutters;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject progressBar;
    [SerializeField] Image pb;

    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam8;

    static int cableCounter = 0;
    static int spacePressed = 0;

    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        canvasGroup = info.GetComponent<CanvasGroup>();
    }

    
    void Update()
    {
        if(cableCounter >= 0 && cableCounter <= 3)
        {
            setPlayerPosition(cableCounter);
            updateProgressBar();
        } 
        else if (cableCounter == 4)
        {
            playerAnim.SetBool("wireCutters", false);
            progressBar.SetActive(false);
            SwapCameras(1, 0);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 51.082f);
            playerMov.canMove = true;
            cc.enabled = true;
        }
    }

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

    void updateProgressBar()
    {
        if (progressBar.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed++;
            if (spacePressed < 6)
            {
                pb.fillAmount = (float) spacePressed / 6;
            } 
            else
            {
                // animacion de cable cortado
                cableCounter++;
                resetvalues();
            }
            
        }
    }

    void resetvalues()
    {
        pb.fillAmount = 0;
        spacePressed = 0;
    }

    public void initializeAlarms()
    {
        alarmMovement(alarm1, 135f, 0f, 0f);
        alarmMovement(alarm2, 135f, 0f, 0f);
        alarmMovement(alarm3, 135f, 0f, 0f);
        alarmMovement(alarm4, 135f, 90f, 90f);
        alarmMovement(alarm5, 135f, 90f, 90f);
    }

    void alarmMovement(GameObject alarm, float x, float y, float z)
    {
        float duration = Random.Range(2f, 4f);
        float delay = Random.Range(0f, 1.5f);

        alarm.transform.DORotate(new Vector3(x, y, z), duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("alarm"))
        {
            // se le quita vida
        }
        if (other.gameObject.CompareTag("securitySystem"))
        {
            letterX.SetActive(true);
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


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("securitySystem") && wireCutters.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
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
