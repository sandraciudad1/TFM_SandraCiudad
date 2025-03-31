using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] GameObject initialInfo;
    CanvasGroup canvasGroup;

    // Initialize canvasGroup.
    void Start()
    {
        canvasGroup = initialInfo.GetComponent<CanvasGroup>();
        StartCoroutine(waitToShow());
    }

    // Waits before showing the initial info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(1.5f);
        initialInfo.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCoroutine());
    }

    // Fades out the initial info over time.
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
}
