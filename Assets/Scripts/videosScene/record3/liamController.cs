using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class liamController : MonoBehaviour
{
    Animator liamAnimator;
    Animator cooperAnimator;
    AudioSource liamAudio;
    AudioSource cooperAudio;

    float moveSpeed = 0.07f;
    float rotationSpeed = 0.8f;
    float stoppingDistance = 0.1f;
    bool move = true;
    bool move2 = false;
    bool rotateCooper = false;
    bool rotate2Cooper = false;
    bool rotate3Cooper = false;

    [SerializeField] GameObject cooper;
    Vector3 targetPosition = new Vector3(113f, 20.65819f, 64.36f);
    Vector3 secondtargetPosition = new Vector3(113f, 20.65819f, 69f);
    Quaternion targetRotationCooper = Quaternion.Euler(new Vector3(0f, -32f, 0f));
    Quaternion secondRotationCooper = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    Quaternion thirdRotationCooper = Quaternion.Euler(new Vector3(0f, -45f, 0f));

    [SerializeField] CinemachineVirtualCamera vcam3;
    [SerializeField] CinemachineVirtualCamera vcam4;

    bool rotateLiam = false;
    Quaternion targetRotationLiam = Quaternion.Euler(new Vector3(0f, 106.48f, 0f));

    [SerializeField] VideoPlayer videoplayer;

    void Start()
    {
        liamAnimator = GetComponent<Animator>();
        cooperAnimator = cooper.GetComponent<Animator>();
        liamAudio = GetComponent<AudioSource>();
        cooperAudio = cooper.GetComponent<AudioSource>();

        setCamPriority(1, 0);
        cooperAnimator.SetBool("walk", true);
        
    }

    
    void Update()
    {
        if (move)
        {
            float distance = Vector3.Distance(cooper.transform.position, targetPosition);
            if (distance > stoppingDistance)
            {
                Vector3 direction = (targetPosition - cooper.transform.position).normalized;
                cooper.transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                rotateCooper = true;
                cooperAnimator.SetBool("walk", false);
                liamAnimator.SetBool("dimiss", true);
                StartCoroutine(waitToIdle());
                liamAudio.Play();
                StartCoroutine(waitToTalk());
                move = false;
            }
        }

        if (rotateCooper)
        {
            cooper.transform.rotation = Quaternion.RotateTowards(cooper.transform.rotation, targetRotationCooper, rotationSpeed * Time.deltaTime * 100f);
            if(cooper.transform.rotation == targetRotationCooper)
            {
                rotateCooper = false;
            }
        }

        if (rotateLiam)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationLiam, rotationSpeed * Time.deltaTime * 100f);
            if (transform.rotation == targetRotationLiam)
            {
                liamAnimator.SetBool("walk", true);
                rotateLiam = false;
            }
        }

        if (rotate2Cooper)
        {
            cooper.transform.rotation = Quaternion.RotateTowards(cooper.transform.rotation, secondRotationCooper, rotationSpeed * Time.deltaTime * 100f);
            if (cooper.transform.rotation == secondRotationCooper)
            {
                stoppingDistance = 1.2f;
                moveSpeed = 0.05f;
                float distance = Vector3.Distance(cooper.transform.position, secondtargetPosition);
                if (distance <= stoppingDistance)
                {
                    cooperAnimator.SetBool("walk", false);
                    
                    rotate3Cooper = true;
                    rotate2Cooper = false;
                }
                else
                {
                    Vector3 direction = (secondtargetPosition - cooper.transform.position).normalized;
                    cooper.transform.position += direction * moveSpeed * Time.deltaTime;
                }
            } 
        }

        if (rotate3Cooper)
        {
            cooper.transform.rotation = Quaternion.RotateTowards(cooper.transform.rotation, thirdRotationCooper, rotationSpeed * Time.deltaTime * 100f);
            if (cooper.transform.rotation == thirdRotationCooper)
            {
                StartCoroutine(waitToManipulate());
                rotate3Cooper = false;
            }
        }
    }


    void setCamPriority(int cam1, int cam2)
    {
        vcam3.Priority = cam1;
        vcam4.Priority = cam2;
    }

    
    IEnumerator waitToIdle()
    {
        yield return new WaitForSeconds(2.05f);
        liamAnimator.SetBool("dimiss", false);
    }


    IEnumerator waitToTalk()
    {
        yield return new WaitForSeconds(5.25f);
        cooperAnimator.SetBool("walk", false);
        cooperAnimator.SetBool("talk", true);
        cooperAudio.Play();
        yield return new WaitForSeconds(3.25f);
        cooperAnimator.SetBool("talk", false);
        StartCoroutine(waitToLeave());
    }


    IEnumerator waitToLeave()
    {
        yield return new WaitForSeconds(2f);
        rotateLiam = true;
        yield return new WaitForSeconds(2f);
        setCamPriority(0, 1);
        //cooper rot
        rotate2Cooper = true;
        yield return new WaitForSeconds(2f);
        liamAnimator.SetBool("walk", false);
        cooperAnimator.SetBool("walk", true);
    }


    IEnumerator waitToManipulate()
    {
        yield return new WaitForSeconds(1.5f);
        cooperAnimator.SetBool("press", true);
        yield return new WaitForSeconds(0.85f);
        videoplayer.Play();


    }
}
