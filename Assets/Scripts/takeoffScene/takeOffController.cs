using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class takeOffController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam0;
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;

    [SerializeField] GameObject spaceship;
    [SerializeField] GameObject smoke;
    [SerializeField] ParticleSystem smoke1;
    [SerializeField] ParticleSystem smoke2;
    [SerializeField] ParticleSystem smoke3;
    [SerializeField] ParticleSystem smoke4;
    [SerializeField] ParticleSystem smoke5;
    [SerializeField] GameObject flames;
    [SerializeField] ParticleSystem flames1;
    [SerializeField] ParticleSystem flames2;
    [SerializeField] ParticleSystem flames3;
    [SerializeField] ParticleSystem flames4;
    [SerializeField] ParticleSystem flames5;
    [SerializeField] GameObject finish;

    Animator animator;

    [SerializeField] Image fadeImage;
    float fadeDuration = 3f;

    // Fades the screen and set camera priority.
    void Start()
    {
        StartCoroutine(fadeIn(1f, 0f));
        camPriority(0);
        camera0Animation();
    }

    // Plays particle systems
    void Update()
    {
        if (smoke.activeInHierarchy)
        {
            smoke1.Play();
            smoke2.Play();
            smoke3.Play();
            smoke4.Play();
            smoke5.Play();
        }
        if (flames.activeInHierarchy)
        {
            flames1.Play();
            flames2.Play();
            flames3.Play();
            flames4.Play();
            flames5.Play();
        }
        if (finish.activeInHierarchy)
        {
            StartCoroutine(fadeIn(0f, 1f));
        }
    }

    // Sets camera priority.
    void camPriority(int cam)
    {
        CinemachineVirtualCamera[] cameras = { vcam0, vcam1, vcam2 };
        
        for(int i = 0; i < cameras.Length; i++)
        {
            if (i == cam)
            {
                cameras[i].Priority = 2;
            } else
            {
                cameras[i].Priority = 0;
            }
        }
    }

    // Manages camera 0 animation. 
    void camera0Animation()
    {
        animator = vcam0.GetComponent<Animator>();
        animator.SetBool("rotate", true);
        StartCoroutine(waitCam0Anim());
    }

    // Manages camera priority.
    IEnumerator waitCam0Anim()
    {
        yield return new WaitForSeconds(13f);
        animator.SetBool("rotate", false);
        camPriority(1);
        spaceshipAnimation();
    }

    // Manages spaceship animation.
    void spaceshipAnimation()
    {
        animator = spaceship.GetComponent<Animator>();
        animator.SetBool("takeoff", true);
        StartCoroutine(changeCam());
        StartCoroutine(waitAnim(3f, "rotateAngle"));
        StartCoroutine(waitAnim(8f, "takeoff"));
    }

    // Waits until the animation ends.
    IEnumerator waitAnim(float time, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
    }

    // Changes camera priority and starts camera rotation.
    IEnumerator changeCam()
    {
        yield return new WaitForSeconds(4f);
        camPriority(2);
        animator = vcam2.GetComponent<Animator>();
        animator.SetBool("rotateAngle", true);
    }

    // Fades the screen.
    public IEnumerator fadeIn(float init, float finish)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(init, finish, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = finish;
        fadeImage.color = color;
    }
}
