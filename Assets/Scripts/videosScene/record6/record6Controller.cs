using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record6Controller : MonoBehaviour
{
    AudioSource audio;
    Animator animator;

    [SerializeField] GameObject sampleFlag;
    [SerializeField] GameObject staticSample;
    bool activate = false;

    // 
    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
        StartCoroutine(waitToStart());
    }

    // 
    void Update()
    {
        if (sampleFlag.activeInHierarchy && !activate)
        {
            staticSample.SetActive(true);
            activate = true;
        }
    }

    // 
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("place", true);
        yield return new WaitForSeconds(6.13f);
        animator.SetBool("place", false);


        //audio.Play();
    }

}
