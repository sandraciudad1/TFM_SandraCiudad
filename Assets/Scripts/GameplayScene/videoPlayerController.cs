using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoPlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(117.394f, 20.679f, 52.173f);
    Vector3 secondPos = new Vector3(117.394f, 20.679f, 50.833f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;

    [SerializeField] GameObject letterX;
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam20;
    [SerializeField] CinemachineVirtualCamera vcam21;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] AudioClip[] audioClips;
    AudioSource audiosource;

    // Initializes components and sets default camera priority.
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();

        videoPlayer.loopPointReached += OnVideoFinished;
        audiosource = GetComponent<AudioSource>();
        GameManager.GameManagerInstance.LoadProgress();
    }


    // Shows 'X' when enter videoplayer.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("recordsObj"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving videoplayer.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("recordsObj"))
        {
            letterX.SetActive(false);
        }
    }

    bool hasTriggered = false;
    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (!hasTriggered && other.gameObject.CompareTag("recordsObj") && Input.GetKeyDown(KeyCode.X))
        {
            hasTriggered = true;
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = firstPos;
            player.transform.rotation = playerRot;
            if (Vector3.Distance(player.transform.position, firstPos) < 0.01f && !change)
            {
                playerAnim.SetBool("handleRecord", true);
                StartCoroutine(waitUntilFinishAnim(other.gameObject.name));
                change = true;
            }
        }
    }

    // Waits for animation, then changes camera and plays video.
    IEnumerator waitUntilFinishAnim(string name)
    {
        yield return new WaitForSeconds(3f);
        player.transform.position = secondPos;
        SwapCameras(0, 0, 1);
        playerAnim.SetBool("closeHand", false);
        playerAnim.SetBool("handleRecord", false);
        checkVideo(name);
    }

    // Selects video and prepares it based on object name.
    void checkVideo(string name)
    {
        videoPlayer.gameObject.SetActive(true);

        if (name.StartsWith("record") && int.TryParse(name.Substring(6), out int index) && index >= 1 && index <= 10)
        {
            videoPlayer.clip = videoClips[index - 1];
            videoPlayer.Prepare();
            StartCoroutine(PlayVideoWhenReady(index));
        }
    }

    // Waits until video is ready, then plays audio and video.
    IEnumerator PlayVideoWhenReady(int index)
    {
        float timeout = 5f;
        while (!videoPlayer.isPrepared && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        videoPlayer.Play();
        audiosource.clip = audioClips[index - 1];
        audiosource.Play();
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.recordsPlayed[index - 1] = 1;
        GameManager.GameManagerInstance.SaveProgress();
        //GameManager.GameManagerInstance.LoadProgress();
    }

    // Called when video ends, starts coroutine to move player.
    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(waitUntilMovePlayer());
    }

    // Waits and restores player movement after video ends.
    IEnumerator waitUntilMovePlayer()
    {
        videoPlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        SwapCameras(1, 0, 0);
        playerMov.canMove = true;
        cc.enabled = true;
    }
    
    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam20.Priority = priority2;
        vcam21.Priority = priority3;
    }
}
