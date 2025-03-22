using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission10Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(122.46f, 20.679f, 88.436f);
    Vector3 secondPos = new Vector3(122.46f, 20.679f, 86.87f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;

    [SerializeField] GameObject uvLight;
    [SerializeField] GameObject uvLightInteractable;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam17;
    [SerializeField] CinemachineVirtualCamera vcam18;


    // 
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        canvasGroup = info.GetComponent<CanvasGroup>();
    }

    // 
    void Update()
    {
        /// pasos:
        /// 1. buscar huellas en la puerta con la lintera
        /// 2. capturar la huella
        /// 3. en el detector replicarla
        /// 
    }

    // Shows 'X' when leaving grids.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fpDetector"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving grids.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fpDetector"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("fpDetector") && uvLight.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = firstPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == firstPos && !change)
            {
                StartCoroutine(waitToShow());
                playerAnim.SetBool("uvLight", true);
                StartCoroutine(waitFinishAnimation());
                change = true;
            }
        }
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

    // Waits for animation to finish before enabling vacuum mode.
    IEnumerator waitFinishAnimation()
    {
        yield return new WaitForSeconds(3f);
        player.transform.position = secondPos;
        playerAnim.SetBool("uvLight", false);
        uvLightInteractable.SetActive(true);
        uvLight.SetActive(false);
        SwapCameras(0, 0, 1);

    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam17.Priority = priority2;
        vcam18.Priority = priority3;
    }
}
