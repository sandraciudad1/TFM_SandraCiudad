using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class record10Controller : MonoBehaviour
{
    Animator animator;

    [SerializeField] CinemachineVirtualCamera vcam13;
    [SerializeField] CinemachineVirtualCamera vcam14;

    // Initializes values and starts the coroutine.
    void Start()
    {
        animator = GetComponent<Animator>();
        setCamPriority(1, 0);
        StartCoroutine(waitToStart());
    }

    // Waits 5 seconds to start Austin's animations
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(5f);
        setCamPriority(0, 1);
        animator.SetBool("getUp", true);
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2)
    {
        vcam13.Priority = cam1;
        vcam14.Priority = cam2;
    }
}
