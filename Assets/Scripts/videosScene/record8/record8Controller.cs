using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class record8Controller : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam9;
    [SerializeField] CinemachineVirtualCamera vcam10;
    [SerializeField] CinemachineVirtualCamera vcam11;

    Quaternion nicoleRotation = Quaternion.Euler(new Vector3(0f, -63.2f, 0f));
    Quaternion nicoleRotation2 = Quaternion.Euler(new Vector3(0f, 92f, 0f));
    [SerializeField] GameObject nicole;
    Animator nicoleAnimator;
    AudioSource nicoleAudio;
    bool rotateNicole = false;
    bool rotateNicole2 = false;
    bool pressBtn = false;

    Vector3 targetPosition = new Vector3(118.848f, 20.66516f, 50.796f);
    Quaternion cooperRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
    [SerializeField] GameObject cooper;
    Animator cooperAnimator;
    AudioSource cooperAudio;
    float moveSpeed = 0.5f;
    float rotationSpeed = 1f;
    float stoppingDistance = 0.4f;
    bool enableAnim = false;
    bool stop = false;
    bool rotateCooper = false;

    [SerializeField] GameObject door;
    Animator doorAnimator;
    AudioSource doorAudio;

    [SerializeField] GameObject open;
    [SerializeField] GameObject alarm;

    [SerializeField] GameObject light;
    Animator lightAnimator;
    AudioSource lightAudio;
    bool activeAlarm = false;

    // Initialize components and set initial camera priority.
    void Start()
    {
        nicoleAnimator = nicole.GetComponent<Animator>();
        cooperAnimator = cooper.GetComponent<Animator>();
        doorAnimator = door.GetComponent<Animator>();
        lightAnimator = light.GetComponent<Animator>();
        nicoleAudio = nicole.GetComponent<AudioSource>();
        cooperAudio = cooper.GetComponent<AudioSource>();
        doorAudio = door.GetComponent<AudioSource>();
        lightAudio = light.GetComponent<AudioSource>();

        setCamPriority(1, 0, 0);
        StartCoroutine(waitToStart());
    }

    // Update logic for animations and character movement.
    void Update()
    {
        if (open.activeInHierarchy && !enableAnim)
        {
            cooperAnimator.SetBool("walk", true);
            rotateNicole = true;
            StartCoroutine(nicoleActions());
            enableAnim = true;
        }

        if(enableAnim && !stop)
        {
            float distance = Vector3.Distance(cooper.transform.position, targetPosition);
            if (distance > stoppingDistance)
            {
                Vector3 direction = (targetPosition - cooper.transform.position).normalized;
                cooper.transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                cooperAnimator.SetBool("walk", false);
                stop = true;
            }
        }

        characterRotation(ref rotateNicole, nicole, nicoleRotation, 0);
        characterRotation(ref rotateNicole2, nicole, nicoleRotation2, 1);
        characterRotation(ref rotateCooper, cooper, cooperRotation, 2);

        if (alarm.activeInHierarchy && !activeAlarm)
        {
            lightAnimator.SetBool("alarm", true);
            lightAudio.Play();
            StartCoroutine(cooperActions());
            activeAlarm = true;
        }
    }

    // Handle character rotation logic.
    void characterRotation(ref bool rotate, GameObject character, Quaternion targetRotation, int id)
    {
        if (rotate && character.transform.rotation != targetRotation)
        {
            character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100f);
        }
        else if (character.transform.rotation == targetRotation)
        {
            if (id == 1)
            {
                if (!pressBtn)
                {
                    nicoleAnimator.SetBool("push", true);
                    pressBtn = true;
                }
            } 
            else if(id == 2)
            {
                cooperAnimator.SetBool("walk", true);
            }

            rotate = false;
        }
    }

    // Initial animations and camera transitions.
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2f);
        nicoleAnimator.SetBool("look", true);
        yield return new WaitForSeconds(4.2f);
        nicoleAnimator.SetBool("look", false);
        setCamPriority(0, 1, 0);
        yield return new WaitForSeconds(3f);
        doorAnimator.SetBool("up", true);
        doorAudio.Play();
    }

    // Nicole's sequence of actions.
    IEnumerator nicoleActions()
    {
        yield return new WaitForSeconds(2f);
        nicoleAudio.Play();
        nicoleAnimator.SetBool("focus", true);
        yield return new WaitForSeconds(3f);
        rotateNicole2 = true;
        yield return new WaitForSeconds(0.2f);
        nicoleAnimator.SetBool("focus", false);
        nicoleAnimator.SetBool("push", true);
        yield return new WaitForSeconds(3f);
        nicoleAnimator.SetBool("push", false);
    }

    // Cooper's sequence of actions.
    IEnumerator cooperActions()
    {
        yield return new WaitForSeconds(0.5f);
        cooperAnimator.SetBool("reach", true);
        cooperAudio.Play();
        setCamPriority(0, 0, 1);
        yield return new WaitForSeconds(3f);
        nicoleAnimator.SetBool("spat", true);
        yield return new WaitForSeconds(2.15f);
        nicoleAnimator.SetBool("pain", true);
        cooperAnimator.SetBool("reach", false); 
        yield return new WaitForSeconds(1f);
        rotateCooper = true;
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2, int cam3)
    {
        vcam9.Priority = cam1;
        vcam10.Priority = cam2;
        vcam11.Priority = cam3;
    }
}
