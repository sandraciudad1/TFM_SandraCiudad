using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record9Controller : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject open;
    [SerializeField] List<GameObject> aliens;

    Animator doorAnimator;
    AudioSource doorAudio;
    List<Animator> alienAnimators = new List<Animator>();

    Vector3 targetPosition = new Vector3(124.87f, 20.66516f, 58f);
    float moveSpeed = 0.5f;
    float stoppingDistance = 0.4f;
    bool initAnim = false;
    bool initWalk = false;


    // Initializes animators, door audio, and starts the coroutine.
    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
        doorAudio = door.GetComponent<AudioSource>();
        foreach (GameObject alien in aliens)
        {
            Animator animator = alien.GetComponent<Animator>();
            if (animator != null)
            {
                alienAnimators.Add(animator);
            }
        }

        StartCoroutine(waitToStart());
    }

    // Updates aliens' movement and animations.
    void Update()
    {
        if (open.activeInHierarchy && !initAnim)
        {
            foreach (Animator animator in alienAnimators)
            {
                animator.SetBool("walk", true);
            }
            initAnim = true;
        }

        if (initAnim && !initWalk)
        {
            foreach (GameObject alien in aliens)
            {
                float distance = Vector3.Distance(alien.transform.position, targetPosition);
                if (distance > stoppingDistance)
                {
                    Vector3 direction = (targetPosition - alien.transform.position).normalized;
                    alien.transform.position += direction * moveSpeed * Time.deltaTime;
                }
                else
                {
                    alien.GetComponent<Animator>().SetBool("walk", false);
                    initWalk = true;
                }
            }
        }
    }

    // Handles door animation and audio.
    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(1f);
        doorAnimator.SetBool("open", true);
        doorAudio.Play();
    }
}
