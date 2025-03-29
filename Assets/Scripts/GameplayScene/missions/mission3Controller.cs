using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mission3Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(88.10962f, 30.74504f, 62.43017f);
    Vector3 secondPos = new Vector3(87.97955f, 30.74504f, 60.40985f);
    Vector3 thirdPos = new Vector3(87.49987f, 30.74504f, 58.16861f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;
    bool updatePos = false;

    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam4;
    [SerializeField] CinemachineVirtualCamera vcam5;
    [SerializeField] CinemachineVirtualCamera vcam6;

    [SerializeField] GameObject gradientBg;
    [SerializeField] RectTransform gradient;
    [SerializeField] RectTransform arrow;
    float minX = -375f, maxX = 375f;
    float flechaSpeed = 200f;
    bool movingRight = true;
    bool stopped = false;
    bool start = false;
    Vector3 initialArrowPos;
    static float counter = 3;
    int solved = 0;

    [SerializeField] GameObject smoke1;
    [SerializeField] GameObject smoke2;
    [SerializeField] GameObject smoke3;
    [SerializeField] GameObject smoke4;

    [SerializeField] GameObject code3;
    [SerializeField] GameObject spaceKeyInfo;
    CanvasGroup canvasGroup;

    [SerializeField] GameObject playerTrigger;
    playerUI ui;
    int opened;
    bool resetState = false;

    // Initializes necessary components and calculates limits.
    void Start()
    {
        initialArrowPos = arrow.transform.position;
        SwapCameras(1, 0, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        calculateLimits();
        canvasGroup = spaceKeyInfo.GetComponent<CanvasGroup>();
        ui = playerTrigger.GetComponent<playerUI>();
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[2];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Recalculates limits when rect transform changes.
    void OnRectTransformDimensionsChange()
    {
        calculateLimits();
    }

    // Calculates the movement limits for the arrow.
    void calculateLimits()
    {
        float halfWidth = gradient.rect.width / 2f;
        minX = -halfWidth + 20f;  
        maxX = halfWidth - 20f;
    }

    // Updates game logic and checks input actions.
    void Update()
    {
        if (start && !stopped)
        {
            moveArrow();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                recalculatePlayerPos();
                checkValue(arrow.transform.position.x);
                stopped = true;
            }
        }

        if (solved == 1 && !updatePos && !stopped)
        {
            player.transform.position = secondPos;
            SwapCameras(0, 0, 1, 0);
            start = true;
            updatePos = true;
        } 
        else if (solved == 2 && !updatePos)
        {
            player.transform.position = thirdPos;
            SwapCameras(0, 0, 0, 1);
            start = true;
            updatePos = true;
        } 
        else if (solved == 3 && !updatePos)
        {
            spannerwrench.SetActive(false);
            GameManager.GameManagerInstance.LoadProgress();
            GameManager.GameManagerInstance.missionsCompleted[2] = 1;
            GameManager.GameManagerInstance.SaveProgress();
            gradientBg.SetActive(false);
            code3.SetActive(true);
            SwapCameras(1, 0, 0, 0);
            playerMov.canMove = true;
            cc.enabled = true;
            updatePos = true;
        }
    }

    // Recalculates player position based on solved state.
    void recalculatePlayerPos()
    {
        if (solved == 0)
        {
            player.transform.position = firstPos;
        } 
        else if (solved == 1)
        {
            player.transform.position = secondPos;
        } 
        else if (solved == 2)
        {
            player.transform.position = thirdPos;
        }
    }

    // Checks the arrow's value and updates game state.
    void checkValue(float value)
    {
        if (value > 296.25f && value < 493.75f)
        {
            updateSmoke(0.1f);
            counter -= 1;  
        }
        else if ((value > 128.38f && value <= 296.25f) || (value >= 493.75f && value < 661.63f))
        {
            ui.takeDamage(15f);
            ui.wasteOxygen(15f);
            updateSmoke(0.05f);
            counter -= 0.5f;
        }
        else if(value <= 128.38f || value >= 661.63f) 
        {
            ui.takeDamage(30f);
            ui.wasteOxygen(30f);
            StartCoroutine(waitToReset());
        }

        if (counter <= 0)
        {   
            gradientBg.SetActive(false);
            StartCoroutine(waitUntilChange());
        }
    }

    // Waits for a set duration before changing state.
    IEnumerator waitUntilChange()
    {
        yield return new WaitForSeconds(10f);
        counter = 3;
        resetValues();
        solved++;
        gradientBg.SetActive(true);
        start = true;
    }

    // Updates smoke effect based on the value passed.
    void updateSmoke(float value)
    {
        playerAnim.SetBool("wrench", true);
        StartCoroutine(waitUntilFinishAnim());
        if (solved == 0)
        {
            smoke1.transform.localScale -= new Vector3(value, value, value);
        } 
        else if (solved == 1)
        {
            smoke2.transform.localScale -= new Vector3(value, value, value);
            smoke3.transform.localScale -= new Vector3(value, value, value);
        } 
        else if (solved == 2)
        {
            smoke4.transform.localScale -= new Vector3(value, value, value);
        }
    }

    // Waits for animation to finish before resetting.
    IEnumerator waitUntilFinishAnim()
    {
        yield return new WaitForSeconds(9.1f);
        StartCoroutine(waitToReset());
    }

    // Waits a short time before resetting values.
    IEnumerator waitToReset()
    {
        yield return new WaitForSeconds(0.5f);
        resetValues();
        start = true;
    }

    // Moves the arrow back and forth across the screen.
    void moveArrow()
    {
        float step = flechaSpeed * Time.deltaTime;

        if (movingRight)
        {
            arrow.anchoredPosition += new Vector2(step, 0);
            if (arrow.anchoredPosition.x >= maxX)
                movingRight = false;
        }
        else
        {
            arrow.anchoredPosition -= new Vector2(step, 0);
            if (arrow.anchoredPosition.x <= minX)
                movingRight = true;
        }
    }

    // Resets values and stops game actions.
    void resetValues()
    {
        playerAnim.SetBool("wrench", false);
        start = false;
        stopped = false;
        arrow.transform.position = initialArrowPos;
        updatePos = false;
    }

    // Shows 'X' when near modular pipes.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[2];
        if (other.gameObject.CompareTag("modularPipes") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving modular pipes.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes"))
        {
            letterX.SetActive(false);
        }
    }

    // Manages actions when staying near modular pipes.
    private void OnTriggerStay(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[2];
        if (other.gameObject.CompareTag("modularPipes") && spannerwrench.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            StartCoroutine(waitToShow());
            letterX.SetActive(false);
            SwapCameras(0, 1, 0, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = firstPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == firstPos && !change)
            {
                gradientBg.SetActive(true);
                start = true;
                change = true;
            }
        }
    }

    // Waits before showing the space key info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.5f);
        spaceKeyInfo.SetActive(true);
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

    // Swap between virtual cameras
    void SwapCameras(int p1, int p2, int p3, int p4)
    {
        vcam1.Priority = p1;
        vcam4.Priority = p2;
        vcam5.Priority = p3;
        vcam6.Priority = p4;
    }
}
