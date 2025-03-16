using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mission7Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(117.927f, 25.67f, 49.789f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] GameObject verticalDoor;
    Animator doorAnimator;
    AudioSource doorAudio;

    [SerializeField] GameObject bg;
    [SerializeField] TextMeshProUGUI inputText;
    string userInput = "";
    bool readCode = false;

    [SerializeField] GameObject emergencyKit;
    [SerializeField] GameObject emergencyKitStatic;
    Animator emergencyKitStaticAnim;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam11;

    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;
    bool canCollect = false;

    void Start()
    {
        SwapCameras(1, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        doorAnimator = verticalDoor.GetComponent<Animator>();
        emergencyKitStaticAnim = emergencyKitStatic.GetComponent<Animator>();
        doorAudio = verticalDoor.GetComponent<AudioSource>();

        canvasGroup = info.GetComponent<CanvasGroup>();
    }

    
    void Update()
    {
        if (readCode)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) && userInput.Length < 4)
                {
                    userInput += c;
                }
                else if (c == '\b' && userInput.Length > 0)
                {
                    userInput = userInput.Substring(0, userInput.Length - 1);
                }
                else if (c == '\n' || c == '\r')
                {
                    if(userInput == "5726")
                    {
                        doorAnimator.SetBool("open", true);
                        doorAudio.Play();
                        bg.SetActive(false);
                        readCode = false;
                    } 
                    else
                    {
                        userInput = "";
                    }
                }
            }
            inputText.text = userInput;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("kit"))
        {
            letterX.SetActive(true);
        }
    }

    //
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(false);
        }
        if (other.gameObject.CompareTag("kit"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(true);
            readCode = true;            
        }

        if (other.gameObject.CompareTag("kit") && emergencyKit.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                SwapCameras(0, 1);
                playerAnim.SetBool("kit", true);
                StartCoroutine(waitFinishAnim());
                change = true;
            }
        }

        if (other.gameObject.CompareTag("emergencyKit") && canCollect && Input.GetKeyDown(KeyCode.R))
        {
            // desactivarlo de su sitio y actvarlo en el maletin

            if (other.gameObject.name.Equals("Alcohol"))
            {

            }
            else if (other.gameObject.name.Equals("BandAidRoll"))
            {

            }
            else if (other.gameObject.name.Equals("MedicalPackage"))
            {

            }
            else if (other.gameObject.name.Equals("BurnCream"))
            {

            }
            else if (other.gameObject.name.Equals("OxyWater"))
            {

            }
            else if (other.gameObject.name.Equals("Scissor1"))
            {

            }
            else if (other.gameObject.name.Equals("MiniAidBox"))
            {

            }
            else if (other.gameObject.name.Equals("HydroCream"))
            {

            }

            // incvremenar el contador de objetos recogidos
            // cuando el contador sea 6 lleavr al personaje enfrente del maletin para cerrarlo
            // mostrarle el pin de la caja
        }
    }

    IEnumerator waitFinishAnim()
    {
        yield return new WaitForSeconds(2.5f);
        emergencyKit.SetActive(false);
        emergencyKitStatic.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        playerAnim.SetBool("kit", false);
        emergencyKitStaticAnim.SetBool("open", true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(waitToShow());
        SwapCameras(1, 0);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 51.77582f);
        playerMov.canMove = true;
        cc.enabled = true;
        canCollect = true;
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

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam11.Priority = priority2;
    }
}
