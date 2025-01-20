using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record5Controller : MonoBehaviour
{
    AudioSource audioMadison;
    AudioSource audioNicole;
    Animator animatorMadison;
    Animator animatorNicole;

    [SerializeField] GameObject madison;
    [SerializeField] GameObject nicole;

    // Initializes audio sources and animators, starts a coroutine
    void Start()
    {
        audioMadison = madison.GetComponent<AudioSource>();
        audioNicole = nicole.GetComponent<AudioSource>();
        animatorMadison = madison.GetComponent<Animator>();
        animatorNicole = nicole.GetComponent<Animator>();

        StartCoroutine(waitToStart());
    }

    // Waits and manages animations and audio in sequence
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(1.5f);
        animatorMadison.SetBool("talk", true);
        audioMadison.Play();
        yield return new WaitForSeconds(4f);
        animatorMadison.SetBool("talk", false);
        yield return new WaitForSeconds(1f);
        animatorNicole.SetBool("talk", true);
        audioNicole.Play();
        yield return new WaitForSeconds(7f);
        animatorNicole.SetBool("talk", false);
    }
}
