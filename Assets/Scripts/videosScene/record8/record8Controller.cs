using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class record8Controller : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam9;
    [SerializeField] CinemachineVirtualCamera vcam10;

    [SerializeField] GameObject nicole;
    [SerializeField] GameObject cooper;
    Animator nicoleAnimator;
    Animator cooperAnimator;
    AudioSource nicoleAudio;
    AudioSource cooperAudio;

    [SerializeField] GameObject door;
    [SerializeField] GameObject open;
    Animator doorAnimator;
    AudioSource doorAudio;


    // 
    void Start()
    {
        nicoleAnimator = nicole.GetComponent<Animator>();
        cooperAnimator = cooper.GetComponent<Animator>();
        doorAnimator = door.GetComponent<Animator>();
        nicoleAudio = nicole.GetComponent<AudioSource>();
        cooperAudio = cooper.GetComponent<AudioSource>();
        doorAudio = door.GetComponent<AudioSource>();

        setCamPriority(1, 0);
        StartCoroutine(waitToStart());
    }

    // 
    void Update()
    {
        if (open.activeInHierarchy)
        {
            //cooper anda hasta la pos destino
            //audio nicole y animacion de sorpresa
        }
    }

    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2f);
        nicoleAnimator.SetBool("look", true);
        yield return new WaitForSeconds(4.2f);
        nicoleAnimator.SetBool("look", false);
        setCamPriority(0, 1);
        yield return new WaitForSeconds(3f);
        doorAnimator.SetBool("up", true);
        doorAudio.Play();
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2)
    {
        vcam9.Priority = cam1;
        vcam10.Priority = cam2;
    }
}
