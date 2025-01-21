using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class record6Controller : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam6;
    [SerializeField] CinemachineVirtualCamera vcam7;

    AudioSource audio;
    Animator animator;
    Animator vfxAnimator;

    [SerializeField] GameObject vfx;
    [SerializeField] GameObject sampleFlag;
    [SerializeField] GameObject staticSample;
    bool activate = false;

    // Initializes components and starts coroutine.
    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        vfxAnimator = vfx.GetComponent<Animator>();

        setCamPriority(1, 0);
        StartCoroutine(waitToStart());
    }

    // Activates static sample if the flag is active.
    void Update()
    {
        if (sampleFlag.activeInHierarchy && !activate)
        {
            staticSample.SetActive(true);
            activate = true;
        }
    }

    // Waits, triggers animations, changes cameras, and plays audio.
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("place", true);
        yield return new WaitForSeconds(6.13f);
        animator.SetBool("place", false);
        yield return new WaitForSeconds(1.5f);
        setCamPriority(0, 1);
        yield return new WaitForSeconds(3.5f);
        vfxAnimator.SetBool("scale", true);
        yield return new WaitForSeconds(1f);
        audio.Play();
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2)
    {
        vcam6.Priority = cam1;
        vcam7.Priority = cam2;
    }
}
