using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class record8Controller : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam9;
    [SerializeField] CinemachineVirtualCamera vcam10;

    Quaternion nicoleRotation = Quaternion.Euler(new Vector3(0f, -63.2f, 0f));
    Quaternion nicoleRotation2 = Quaternion.Euler(new Vector3(0f, 92f, 0f));
    [SerializeField] GameObject nicole;
    Animator nicoleAnimator;
    AudioSource nicoleAudio;
    bool rotateNicole = false;
    bool rotateNicole2 = false;
    bool pressBtn = false;

    Vector3 targetPosition = new Vector3(118.848f, 20.66516f, 50.796f);
    [SerializeField] GameObject cooper;
    Animator cooperAnimator;
    AudioSource cooperAudio;
    float moveSpeed = 0.5f;
    float rotationSpeed = 1f;
    float stoppingDistance = 0.4f;
    bool enableAnim = false;
    bool stop = false;

    [SerializeField] GameObject door;
    Animator doorAnimator;
    AudioSource doorAudio;

    [SerializeField] GameObject open;
    [SerializeField] GameObject alarm;

    [SerializeField] GameObject light;
    Animator lightAnimator;
    AudioSource lightAudio;
    bool activeAlarm = false;
    
    // 
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

        setCamPriority(1, 0);
        StartCoroutine(waitToStart());
    }

    // 
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

                // cooper libera un gas y duerme a nicole

                stop = true;
            }
        }

        if (rotateNicole && nicole.transform.rotation != nicoleRotation)
        {
            nicole.transform.rotation = Quaternion.RotateTowards(nicole.transform.rotation, nicoleRotation, rotationSpeed * Time.deltaTime * 100f);
        } else if (nicole.transform.rotation == nicoleRotation)
        {
            rotateNicole = false;
        }

        if (rotateNicole2 && nicole.transform.rotation != nicoleRotation2)
        {
            nicole.transform.rotation = Quaternion.RotateTowards(nicole.transform.rotation, nicoleRotation2, rotationSpeed * Time.deltaTime * 100f);
        } else if (nicole.transform.rotation == nicoleRotation2 && !pressBtn)
        {
            nicoleAnimator.SetBool("push", true);
            rotateNicole2 = false;
            pressBtn = true;
        }

        if (alarm.activeInHierarchy && !activeAlarm)
        {
            lightAnimator.SetBool("alarm", true);
            lightAudio.Play();
            activeAlarm = true;
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

    //
    IEnumerator nicoleActions()
    {
        yield return new WaitForSeconds(2f);
        nicoleAudio.Play();
        nicoleAnimator.SetBool("focus", true);
        yield return new WaitForSeconds(3f);
        rotateNicole2 = true;
        yield return new WaitForSeconds(0.2f);
        nicoleAnimator.SetBool("push", true);

        //nicole activa una alarma
    }

    // Sets camera priority.
    void setCamPriority(int cam1, int cam2)
    {
        vcam9.Priority = cam1;
        vcam10.Priority = cam2;
    }
}
