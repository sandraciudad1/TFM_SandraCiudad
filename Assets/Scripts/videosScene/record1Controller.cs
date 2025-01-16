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

    [SerializeField] VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
        setCamPriority(1, 0);
        StartCoroutine(waitToReproduceAudio());
        StartCoroutine(waitUntilChange());
    }

    void setCamPriority(int cam1, int cam2)
    {
        vcam1.Priority = cam1;
        vcam2.Priority = cam2;
    }

    IEnumerator waitUntilChange()
    {
        yield return new WaitForSeconds(4.15f);
        setCamPriority(0, 1);
    }

    IEnumerator waitToReproduceAudio()
    {
        yield return new WaitForSeconds(0.8f);
        audio.Play();
    }
}
