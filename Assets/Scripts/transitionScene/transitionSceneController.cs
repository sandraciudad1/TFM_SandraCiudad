using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class transitionSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    float fadeDuration = 2f;

    // Fades screen and changes scene.
    void Start()
    {
        StartCoroutine(fadeIn(1f, 0f));
        StartCoroutine(waitUntilEnd());
        
    }

    // Fades screen after player animations.
    IEnumerator waitUntilEnd()
    {
        yield return new WaitForSeconds(8f);
        StartCoroutine(fadeIn(0f, 1f));
        StartCoroutine(waitFade());
    }

    // Changes scene after 2 seconds.
    IEnumerator waitFade()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("gameplayScene");
    }

    // Unsubscribes from the scene load event.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Fades the screen.
    IEnumerator fadeIn(float init, float finish)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(init, finish, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = finish;
        fadeImage.color = color;
    }
}
