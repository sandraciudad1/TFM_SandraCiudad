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
    Vector3 firstPos = new Vector3(88.028f, 30.74504f, 62.476f);
    Vector3 secondPos = new Vector3(88.028f, 30.74504f, 60.484f);
    Vector3 thirdPos = new Vector3(88.028f, 30.74504f, 59.8f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;
    bool updatePos = false;

    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject letterX;
    bool finish = false;

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

    void Start()
    {
        initialArrowPos = arrow.transform.position;
        SwapCameras(1, 0, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        calculateLimits();
        
    }

    void OnRectTransformDimensionsChange()
    {
        calculateLimits();
    }

    void calculateLimits()
    {
        float halfWidth = gradient.rect.width / 2f;
        minX = -halfWidth + 20f;  
        maxX = halfWidth - 20f;
    }

    void Update()
    {
        Debug.Log("Counter " + counter);
        Debug.Log("Start " + start + " stopped " + stopped);

        if (start && !stopped)
        {
            Debug.Log("Moviendose");
            moveArrow();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Espacio");
                counter -= checkValue(arrow.transform.position.x);

                if (counter <= 0)
                {
                    Debug.Log("Contador <= 0");
                    solved++;
                    gradientBg.SetActive(false);
                    counter = 3;
                    resetValues();
                    
                }
                Debug.Log("stopped se pone a true");
                stopped = true;
            }
        }

        if (solved == 1 && !updatePos && !stopped)
        {
            Debug.Log("Solved==1");
            player.transform.position = secondPos;
            SwapCameras(0, 0, 1, 0);
            start = true;
            updatePos = true;
        } 
        else if (solved == 2 && !updatePos)
        {
            Debug.Log("Solved==2");
            player.transform.position = thirdPos;
            SwapCameras(0, 0, 0, 1);
            start = true;
            updatePos = true;
        } 
        else if (solved == 3 && !updatePos)
        {
            Debug.Log("Solved==3");
            SwapCameras(1, 0, 0, 0);
            playerMov.canMove = true;
            cc.enabled = true;
            updatePos = true;
        }
    }
    
    float checkValue(float value)
    {
        if (value > 296.25f && value < 493.75f) // verde
        {
            Debug.Log("Verde");
            // hacer animacion de apretar y se corta el humo
            updateSmoke(0.1f);
            return 1;  
        }
        else if ((value > 128.38f && value <= 296.25f) || (value >= 493.75f && value < 661.63f)) // amarillo 
        {
            Debug.Log("Amarillo");
            // quitar vida y decrementar en 0.5
            updateSmoke(0.05f);
            return 0.5f;
        }
        else if(value <= 128.38f || value >= 661.63f) // rojo
        {
            Debug.Log("Rojo");
            StartCoroutine(waitToReset());
            return 0;
        } 
        else
        {
            return 0;
        }
    }

    void updateSmoke(float value)
    {
        playerAnim.SetBool("wrench", true);
        StartCoroutine(waitUntilFinishAnim());
        if (solved == 0)
        {
            smoke1.transform.localScale -= new Vector3(value, value, value);
            Debug.Log("Reduciendo smoke 1 " + smoke1.transform.localScale);
        } 
        else if (solved == 1)
        {
            smoke2.transform.localScale -= new Vector3(value, value, value);
            smoke3.transform.localScale -= new Vector3(value, value, value);
            Debug.Log("Reduciendo smoke 2 y 3 " + smoke2.transform.localScale + " " + smoke3.transform.localScale);
        } else if (solved == 2)
        {
            smoke4.transform.localScale -= new Vector3(value, value, value);
            Debug.Log("Reduciendo smoke 4 " + smoke4.transform.localScale);
        }
        
    }

    IEnumerator waitUntilFinishAnim()
    {
        Debug.Log("Esperando 9 segundos a la animacion");
        yield return new WaitForSeconds(9.1f);
        StartCoroutine(waitToReset());
    }

    IEnumerator waitToReset()
    {
        Debug.Log("Esperando a resetear");
        yield return new WaitForSeconds(0.5f);
        resetValues();
        start = true;
    }

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

    void resetValues()
    {
        Debug.Log("Reset");
        playerAnim.SetBool("wrench", false);
        start = false;
        Debug.Log("stopped se pone a FALSE");
        stopped = false;
        arrow.transform.position = initialArrowPos;
        updatePos = false;
    }

    // Shows 'X' when near modular pipes.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes") && !finish)
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

    // 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("modularPipes") && spannerwrench.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
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


    // Swap between virtual cameras
    void SwapCameras(int p1, int p2, int p3, int p4)
    {
        vcam1.Priority = p1;
        vcam4.Priority = p2;
        vcam5.Priority = p3;
        vcam6.Priority = p4;
    }
}
