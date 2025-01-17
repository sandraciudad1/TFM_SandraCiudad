using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cooperController : MonoBehaviour
{
    Animator cooperAnimator;
    Animator nicoleAnimator;
    AudioSource audioCooper;
    AudioSource audioNicole;

    [SerializeField] GameObject open;
    bool enableAnim = false;

    Vector3 targetPosition = new Vector3(106.5f, 30.665f, 64.66f);
    Quaternion targetRotationCooper = Quaternion.Euler(new Vector3(0f, -114.484f, 0f));
    float moveSpeed = 2f;
    float rotationSpeed = 0.5f;
    float stoppingDistance = 1.4f;

    bool rotateCooper = false;
    bool rotateNicole = false;
    Quaternion targetRotationNicole = Quaternion.Euler(new Vector3(0f, 116f, 0f));

    [SerializeField] GameObject nicole;
    [SerializeField] GameObject point;
    [SerializeField] GameObject sample;
    [SerializeField] GameObject sampleStatic;
    bool playAudio = false;

    // 
    void Start()
    {
        cooperAnimator = GetComponent<Animator>();
        nicoleAnimator = nicole.GetComponent<Animator>();
        audioCooper = GetComponent<AudioSource>();
        audioNicole = nicole.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (open.activeInHierarchy && !enableAnim)
        {
            cooperAnimator.SetBool("walk", true);
            enableAnim = true;
        }

        if (enableAnim)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > stoppingDistance)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                cooperAnimator.SetBool("walk", false);
                //stopTyping();

                StartCoroutine(waitRotateCooper());
                StartCoroutine(waitRotateNicole());
                if (rotateCooper)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationCooper, rotationSpeed * Time.deltaTime * 100f);
                }
                if (rotateNicole)
                {
                    nicole.transform.rotation = Quaternion.RotateTowards(nicole.transform.rotation, targetRotationNicole, rotationSpeed * Time.deltaTime * 100f);
                }

                StartCoroutine(startConversation());
                if (point.activeInHierarchy && !playAudio)
                {
                    audioCooper.Play();
                    StartCoroutine(nicoleAnswer());
                    
                    playAudio = true;
                }
            }
        }

        if (sample.activeInHierarchy)
        {
            sampleStatic.SetActive(false);
        }
    }


    // 
    IEnumerator waitRotateCooper()
    {
        yield return new WaitForSeconds(0.2f);
        rotateCooper = true;
    }

    IEnumerator waitRotateNicole()
    {
        yield return new WaitForSeconds(0.5f);
        rotateNicole = true;
        yield return new WaitForSeconds(0.5f);
        nicoleAnimator.SetBool("sitting", true);
    }

    IEnumerator startConversation()
    {
        yield return new WaitForSeconds(0.5f);
        cooperAnimator.SetBool("point", true);
        yield return new WaitForSeconds(10f);
        nicoleAnimator.SetBool("sitting", false);
        yield return new WaitForSeconds(2f);
        cooperAnimator.SetBool("pickUp", true);
        yield return new WaitForSeconds(9f);
        //actualizar pos y rot de cooper
        cooperAnimator.SetBool("walk", true);
    }


    IEnumerator nicoleAnswer()
    {
        yield return new WaitForSeconds(4f);
        audioNicole.Play();
    }
}
