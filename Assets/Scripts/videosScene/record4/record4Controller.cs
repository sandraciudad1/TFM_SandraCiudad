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

    // Initializes components and sets initial animator state
    void Start()
    {
        audioSystem = GetComponent<AudioSource>();
        audioCooper = cooper.GetComponent<AudioSource>();
        animator = cooper.GetComponent<Animator>();

        animator.SetBool("touch", true);
    }

    // Updates audio and triggers rotation when conditions are met
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

    // Rotates Cooper towards a target and starts walking animation
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
