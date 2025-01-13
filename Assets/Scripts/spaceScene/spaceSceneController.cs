using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class spaceSceneController : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    float fadeDuration = 3f;

    Animator animator;
    [SerializeField] GameObject spaceship;
    [SerializeField] GameObject earth;

    [SerializeField] GameObject lightsOff;
    [SerializeField] GameObject emergencyLights;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip alertMsg;
    [SerializeField] AudioClip alarm;

    // Fades screen and activates animations.
    void Start()
    {
        StartCoroutine(fadeIn(1f, 0f));
        enableAnimation(spaceship, "move");
        enableAnimation(earth, "scale");
        StartCoroutine(waitUntilEnd());
    }

    bool alertPlay = false;
    bool alarmPlay = false;

    // Activates the corresponding audios and configures the lights.
    void Update()
    {
        if (lightsOff.activeInHierarchy && !alertPlay)
        {
            GameObject[] lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject lightObject in lights)
            {
                lightObject.SetActive(false);
            }
            audioSource.clip = alertMsg;
            audioSource.Play();
            alertPlay = true;
        }
        if (emergencyLights.activeInHierarchy && !alarmPlay)
        {
            audioSource.clip = alarm;
            audioSource.Play();
            alarmPlay = true;
        }
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

    // Enables animations.
    void enableAnimation(GameObject gameobject, string name)
    {
        animator = gameobject.GetComponent<Animator>();
        animator.SetBool(name, true);
    }

    // Waits for the animations to finish.
    IEnumerator waitUntilEnd()
    {
        yield return new WaitForSeconds(25f);
        StartCoroutine(fadeIn(0f, 1f));
        StartCoroutine(changeScene());
    }

    // Changes scene after 3 seconds.
    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("transitionScene");
    }

    // Unsubscribes from the scene load event.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
