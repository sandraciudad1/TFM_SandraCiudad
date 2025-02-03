using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] GameObject initialInfo;
    CanvasGroup canvasGroup;


    void Start()
    {
        canvasGroup = initialInfo.GetComponent<CanvasGroup>();
        StartCoroutine(waitToShow());
    }

    
    void Update()
    {
        
    }

    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(1.5f);
        initialInfo.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCoroutine());
    }

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
