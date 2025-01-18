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
    [SerializeField] GameObject nicole;
    [SerializeField] GameObject point;
    [SerializeField] GameObject sample;
    [SerializeField] GameObject sampleStatic;

    bool enableAnim = false;
    float moveSpeed = 2f;
    float rotationSpeed = 0.5f;
    float stoppingDistance = 1.4f;

    Vector3 targetPosition = new Vector3(106.5f, 30.665f, 64.66f);
    Quaternion targetRotationCooper = Quaternion.Euler(new Vector3(0f, -114.484f, 0f));
    Quaternion targetRotationNicole = Quaternion.Euler(new Vector3(0f, 116f, 0f));

    bool rotateCooper = false;
    bool rotateNicole = false; 
    bool playAudio = false;

    bool recoverRotCooper = false;
    bool recoverRotNicole = false;
    Quaternion originalRotationNicole = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    Quaternion originalRotationCooper = Quaternion.Euler(new Vector3(0f, -70f, 0f));

    bool stopped = false;
    bool conver = false;
    bool sit = false;
    bool finalRotNicole = false;
    bool walking = false;


    // Initializes animators and audio sources
    void Start()
    {
        cooperAnimator = GetComponent<Animator>();
        nicoleAnimator = nicole.GetComponent<Animator>();
        audioCooper = GetComponent<AudioSource>();
        audioNicole = nicole.GetComponent<AudioSource>();
    }
    
    // Enables character animations and play conversation audios
    void Update()
    {
        // Cooper walks in when the door opens
        if (open.activeInHierarchy && !enableAnim)
        {
            cooperAnimator.SetBool("walk", true);
            enableAnim = true;
        }

        if (enableAnim && !stopped)
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
                
                StartCoroutine(waitRotateCooper());
                StartCoroutine(waitRotateNicole());

                stopped = true;
            }
        }

        // Cooper and Nicole talk
        if (rotateCooper && transform.rotation != targetRotationCooper)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationCooper, rotationSpeed * Time.deltaTime * 100f);
        }

        if (rotateNicole && nicole.transform.rotation != targetRotationNicole)
        {
            nicole.transform.rotation = Quaternion.RotateTowards(nicole.transform.rotation, targetRotationNicole, rotationSpeed * Time.deltaTime * 100f);
        }

        if (transform.rotation == targetRotationCooper && nicole.transform.rotation == targetRotationNicole && !conver)
        {
            rotateCooper = false;
            rotateNicole = false;
            StartCoroutine(startConversation());
            conver = true;
        }

        if (point.activeInHierarchy && !playAudio)
        {
            audioCooper.Play();
            StartCoroutine(nicoleAnswer());

            playAudio = true;
        }

        if (sample.activeInHierarchy)
        {
            sampleStatic.SetActive(false);
        }

        // Cooper leaves and Nicole continues working
        if (recoverRotNicole && nicole.transform.rotation != originalRotationNicole && !finalRotNicole)
        {
            nicole.transform.rotation = Quaternion.RotateTowards(nicole.transform.rotation, originalRotationNicole, rotationSpeed * Time.deltaTime * 100f);
            
            if (!sit)
            {
                nicoleAnimator.SetBool("sitting", false);
                sit = true;
            }
            
            if (nicole.transform.rotation == originalRotationNicole)
            {
                finalRotNicole = true;   
            }
        }
        
        if (recoverRotCooper && transform.rotation != originalRotationCooper && !walking)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotationCooper, rotationSpeed * Time.deltaTime * 100f);

            if (transform.rotation == originalRotationCooper)
            {
                cooperAnimator.SetBool("walk", true);
                walking = true;
            }
        }
    }


    // Waits 0.2 seconds to rotate Cooper
    IEnumerator waitRotateCooper()
    {
        yield return new WaitForSeconds(0.2f);
        rotateCooper = true;
    }

    // Waits 0.5 seconds to rotate Nicole
    IEnumerator waitRotateNicole()
    {
        yield return new WaitForSeconds(0.5f);
        rotateNicole = true;
        yield return new WaitForSeconds(0.5f);
        nicoleAnimator.SetBool("sitting", true);
    }

    // Enables animations to shape the conversation
    IEnumerator startConversation()
    {
        yield return new WaitForSeconds(0.5f);
        cooperAnimator.SetBool("point", true);
        yield return new WaitForSeconds(15f);
        recoverRotNicole = true;
        yield return new WaitForSeconds(2f);
        cooperAnimator.SetBool("pickUp", true);
        yield return new WaitForSeconds(8f);
        recoverRotCooper = true;
    }

    // Waits 4 seconds before playing Nicole's audio
    IEnumerator nicoleAnswer()
    {
        yield return new WaitForSeconds(4f);
        audioNicole.Play();
    }
}
