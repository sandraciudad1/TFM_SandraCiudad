using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class presentationController : MonoBehaviour
{
    [SerializeField] GameObject presenter;
    Animator animator;

    [SerializeField] GameObject initialCanvas;
    [SerializeField] VideoPlayer videoPlayer;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip intro1;     
    [SerializeField] AudioClip intro2;
    [SerializeField] AudioClip applause;
    [SerializeField] AudioClip madison;
    [SerializeField] AudioClip liam;
    [SerializeField] AudioClip nicole;
    [SerializeField] AudioClip austin;
    [SerializeField] AudioClip cooper;
    [SerializeField] AudioClip fin;

    bool isPlaying = false;   
    int currentAudioIndex = 0;
    bool videoFinished = false;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Prepare();
        }
    }

    public void startBtnPressed()
    {
        if (videoPlayer.isPrepared)
        {
            PlayVideo();
        }
        else
        {
            videoPlayer.prepareCompleted += OnVideoPrepared;
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        PlayVideo();
    }

    void PlayVideo()
    {
        initialCanvas.SetActive(false);
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        videoFinished = true;
        animator = presenter.GetComponent<Animator>();
        animator.SetBool("talk", true);
    }

    bool isWaitingForAudio = false; 

    IEnumerator initialWait()
    {
        isWaitingForAudio = true; 
        yield return new WaitForSeconds(1f); 
        PlayNextAudio();
        isWaitingForAudio = false; 
    }

   
    void Update()
    {
        if (videoFinished && !isWaitingForAudio)
        {
            StartCoroutine(initialWait());
            videoFinished = false;
        }

        if (!audioSource.isPlaying && isPlaying && !isWaitingForAudio)
        {
            charactersController controller = GameObject.Find("charactersController").GetComponent<charactersController>();
            if (controller != null)
            {
                if (currentAudioIndex == 3)
                {
                    controller.counter = 1;
                }
                else if (currentAudioIndex == 4)
                {
                    controller.counter = 2;
                }
                else if (currentAudioIndex == 5)
                {
                    controller.counter = 3;
                }
                else if (currentAudioIndex == 6)
                {
                    controller.counter = 4;
                }
                else if (currentAudioIndex == 7)
                {
                    controller.counter = 5;
                }

                StartCoroutine(initialWait());
            }
        }
    }

    void PlayNextAudio()
    {
        switch (currentAudioIndex)
        {
            case 0:
                audioSource.clip = intro1;
                break;
            case 1:
                audioSource.clip = intro2;
                break;
            case 2:
                audioSource.clip = applause;
                break;
            case 3:
                audioSource.clip = madison;
                break;
            case 4:
                audioSource.clip = liam;
                break;
            case 5:
                audioSource.clip = nicole;
                break;
            case 6:
                audioSource.clip = austin;
                break;
            case 7:
                audioSource.clip = cooper;
                break;
            case 8:
                audioSource.clip = fin;
                break;
            case 9:
                audioSource.clip = applause;
                break;
        }

        audioSource.Play();
        isPlaying = true;
        currentAudioIndex++;
    }

}
