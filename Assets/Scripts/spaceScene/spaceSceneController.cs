using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spaceSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    float fadeDuration = 3f;

    Animator animator;
    [SerializeField] GameObject spaceship;
    [SerializeField] GameObject earth;

    void Start()
    {
        StartCoroutine(fadeIn(1f, 0f));
        enableAnimation(spaceship, "move");
        enableAnimation(earth, "scale");
        StartCoroutine(waitUntilEnd());
    }

    // Fades the screen.
    public IEnumerator fadeIn(float init, float finish)
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


    void enableAnimation(GameObject gameobject, string name)
    {
        animator = gameobject.GetComponent<Animator>();
        animator.SetBool(name, true);
    }


    IEnumerator waitUntilEnd()
    {
        yield return new WaitForSeconds(18f);
        StartCoroutine(fadeIn(0f, 1f));
        // llamar a gameplayScene
    }
}
