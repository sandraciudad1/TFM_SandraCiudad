using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Cinemachine;

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
    bool isWaitingForAudio = false;

    [SerializeField] Image fadeImage; 
    float fadeDuration = 3.5f;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;

    // Initializes the video player and sets up the callback for when the video ends.
    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Prepare();
        }
    }

    // Starts playing the video if it's prepared; otherwise, waits for preparation to complete.
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

    // Called when the video is prepared; starts playing the video.
    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        PlayVideo();
    }

    // Hides the initial canvas and starts the video playback.
    void PlayVideo()
    {
        initialCanvas.SetActive(false);
        videoPlayer.Play();
    }

    // Called when the video finishes playing; disables the video and triggers the animator.
    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        videoFinished = true;
        animator = presenter.GetComponent<Animator>();
        animator.SetBool("talk", true);
    }

    // Waits before playing the next audio.
    IEnumerator initialWait()
    {
        isWaitingForAudio = true; 
        yield return new WaitForSeconds(1f); 
        PlayNextAudio();
        isWaitingForAudio = false; 
    }

    // Manages transitions between audios.
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
                if (currentAudioIndex == 2)
                {
                    StartCoroutine(vcamChange(6f, false));
                }
                else if (currentAudioIndex == 3)
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
                    StartCoroutine(vcamChange(24f, true));
                } 
                else if (currentAudioIndex >= 9)
                {
                    StartCoroutine(FadeIn());
                }

                StartCoroutine(initialWait());
            }
        }
    }

    // Switches the camera priority.
    IEnumerator vcamChange(float time, bool cam1Priority)
    {
        yield return new WaitForSeconds(time);
        if (cam1Priority)
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        } 
        else
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
    }

    // Selects the next audio clip and plays it.
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

    // Fades the screen to black.
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f; 
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;  
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);  
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f; 
        fadeImage.color = color;
    }
}
