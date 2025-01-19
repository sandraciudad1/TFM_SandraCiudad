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

    float moveSpeed = 0.02f;
    float rotationSpeed = 0.8f;
    float stoppingDistance = 0.1f;
    bool move = true;
    bool rotateCooper = false;
    bool rotate2Cooper = false;
    bool rotate3Cooper = false;
    bool rotate4Cooper = false;

    [SerializeField] GameObject cooper;
    Vector3 targetPosition = new Vector3(113.2f, 20.65819f, 64.36f);
    Vector3 secondtargetPosition = new Vector3(113f, 20.65819f, 69f);
    Quaternion targetRotationCooper = Quaternion.Euler(new Vector3(0f, -32f, 0f));
    Quaternion secondRotationCooper = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    Quaternion thirdRotationCooper = Quaternion.Euler(new Vector3(0f, -45f, 0f));

    [SerializeField] CinemachineVirtualCamera vcam3;
    [SerializeField] CinemachineVirtualCamera vcam4;
    [SerializeField] VideoPlayer videoplayer;

    bool rotateLiam = false;
    Quaternion targetRotationLiam = Quaternion.Euler(new Vector3(0f, 106.48f, 0f));
    

    // Initializes variables and sets the starting state
    void Start()
    {
        liamAnimator = GetComponent<Animator>();
        cooperAnimator = cooper.GetComponent<Animator>();
        liamAudio = GetComponent<AudioSource>();
        cooperAudio = cooper.GetComponent<AudioSource>();

        setCamPriority(1, 0);
        cooperAnimator.SetBool("walk", true);
    }

    // Updates positions, rotations, and triggers states
    void Update()
    {
        if (move) moveCooper(targetPosition, 0);

        if (rotateCooper) characterRotation(cooper, targetRotationCooper, 100f, "cooper1");

        if (rotateLiam)characterRotation(gameObject, targetRotationLiam, 200f, "liam1");

        if (rotate2Cooper) characterRotation(cooper, secondRotationCooper, 100f, "cooper2");

        if (rotate3Cooper)characterRotation(cooper, thirdRotationCooper, 100f, "cooper3");

        if (rotate4Cooper) characterRotation(cooper, secondRotationCooper, 100f, "cooper4");
    }

    // Handles character rotation based on target rotation and ID
    void characterRotation(GameObject character, Quaternion targetRotation, float num, string id)
    {
        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * num);
        if (character.transform.rotation == targetRotation)
        {
            if (id.Equals("cooper1"))
            {
                rotateCooper = false;
            } 
            else if (id.Equals("liam1"))
            {
                liamAnimator.SetBool("walk", true);
                rotateLiam = false;
            } 
            else if (id.Equals("cooper2"))
            {
                stoppingDistance = 1.2f;
                moveSpeed = 0.05f;
                moveCooper(secondtargetPosition, 1);
            }
            else if (id.Equals("cooper3"))
            {
                StartCoroutine(waitToManipulate());
                rotate3Cooper = false;
            }
            else if (id.Equals("cooper4"))
            {
                cooperAnimator.SetBool("walk", true);
                rotate4Cooper = false;
            }
        }
    }

    // Handles Cooper's movement and triggers animations based on distance
    void moveCooper(Vector3 targetPos, int id)
    {
        float distance = Vector3.Distance(cooper.transform.position, targetPos);
        if (distance <= stoppingDistance)
        {
            if (id == 0)
            {
                rotateCooper = true;
                cooperAnimator.SetBool("walk", false);
                liamAnimator.SetBool("dimiss", true);
                StartCoroutine(waitToIdle());
                liamAudio.Play();
                StartCoroutine(waitToTalk());
                move = false;
            } 
            else if (id == 1)
            {
                cooperAnimator.SetBool("walk", false);
                rotate3Cooper = true;
                rotate2Cooper = false;
            }   
        }
        else
        {
            Vector3 direction = (targetPos - cooper.transform.position).normalized;
            cooper.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // Sets camera priorities
    void setCamPriority(int cam1, int cam2)
    {
        vcam3.Priority = cam1;
        vcam4.Priority = cam2;
    }

    // Waits and changes Liam's animation to idle
    IEnumerator waitToIdle()
    {
        yield return new WaitForSeconds(2.05f);
        liamAnimator.SetBool("dimiss", false);
    }

    // Waits, triggers Cooper's talk animation, and audio
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

    // Waits and triggers Liam and Cooper's leaving sequence
    IEnumerator waitToLeave()
    {
        yield return new WaitForSeconds(2f);
        rotateLiam = true;
        yield return new WaitForSeconds(2f);
        setCamPriority(0, 1);
        rotate2Cooper = true;
        yield return new WaitForSeconds(2f);
        liamAnimator.SetBool("walk", false);
        cooperAnimator.SetBool("walk", true);
    }

    // Waits and triggers Cooper's manipulation and video play
    IEnumerator waitToManipulate()
    {
        yield return new WaitForSeconds(1.5f);
        cooperAnimator.SetBool("press", true);
        yield return new WaitForSeconds(0.85f);
        videoplayer.Play();
        yield return new WaitForSeconds(2.15f);
        cooperAnimator.SetBool("press", false);
        yield return new WaitForSeconds(10f);
        cooperAnimator.SetBool("look", true);
        yield return new WaitForSeconds(4.10f);
        rotate4Cooper = true;
    }
}
