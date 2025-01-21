using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record7Controller : MonoBehaviour
{
    Animator animatorLiam;
    AudioSource audioMadison;

    [SerializeField] GameObject madison;

    // Initializes components and starts coroutine.
    void Start()
    {
        animatorLiam = GetComponent<Animator>();
        audioMadison = madison.GetComponent<AudioSource>();

        StartCoroutine(waitToStart());
    }

    // Waits, plays audio and triggers the animation.
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2f);
        audioMadison.Play();
        yield return new WaitForSeconds(2f);
        animatorLiam.SetBool("defeat", true);
    }
}
