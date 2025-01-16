using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class record1Controller : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] AudioSource audio;

    // Initializes camera priority and calls coroutines.
    void Start()
    {
        setCamPriority(1, 0);
        StartCoroutine(waitToPlayAudio());
        StartCoroutine(waitUntilChange(4.15f, 0, 1));
        StartCoroutine(waitUntilChange(17f, 1, 0));
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2)
    {
        vcam1.Priority = cam1;
        vcam2.Priority = cam2;
    }

    // Waits until camera priority changes.
    IEnumerator waitUntilChange(float time, int cam1, int cam2)
    {
        yield return new WaitForSeconds(time);
        setCamPriority(cam1, cam2);
    }

    // Waits 0.8 seconds to play Cooper's voice audio.
    IEnumerator waitToPlayAudio()
    {
        yield return new WaitForSeconds(0.8f);
        audio.Play();
    }
}
