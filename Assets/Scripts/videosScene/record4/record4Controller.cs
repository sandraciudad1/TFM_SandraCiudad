using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record4Controller : MonoBehaviour
{
    AudioSource audioSystem;
    AudioSource audioCooper;
    Animator animator;

    [SerializeField] GameObject cooper;
    [SerializeField] GameObject audioSystemObj;
    [SerializeField] GameObject audioCooperObj;
    [SerializeField] GameObject finish;

    bool systemPlayed = false;
    bool cooperPlayed = false;
    bool rotate = false;

    float rotationSpeed = 0.8f;
    Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

    // 
    void Start()
    {
        audioSystem = GetComponent<AudioSource>();
        audioCooper = cooper.GetComponent<AudioSource>();
        animator = cooper.GetComponent<Animator>();

        animator.SetBool("touch", true);
    }

    // 
    void Update()
    {
        if (audioSystemObj.activeInHierarchy && !systemPlayed)
        {
            audioSystem.Play();
            systemPlayed = true;
        }

        if (audioCooperObj.activeInHierarchy && !cooperPlayed)
        {
            audioCooper.Play();
            cooperPlayed = true;
        }

        if (finish.activeInHierarchy && !rotate)
        {
            cooperRotation();
        }
    }

    //
    void cooperRotation()
    {
        cooper.transform.rotation = Quaternion.RotateTowards(cooper.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        if (cooper.transform.rotation == targetRotation)
        {
            animator.SetBool("walk", true);
            rotate = true;
        }
    }

}
