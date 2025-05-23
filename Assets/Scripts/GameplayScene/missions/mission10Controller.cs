using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class mission10Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 firstPos = new Vector3(122.46f, 20.679f, 88.436f);
    Vector3 secondPos = new Vector3(122.46f, 20.679f, 86.87f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;

    [SerializeField] GameObject uvLight;
    [SerializeField] GameObject uvLightInteractable;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject info;
    CanvasGroup canvasGroup;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam17;
    [SerializeField] CinemachineVirtualCamera vcam18;
    [SerializeField] CinemachineVirtualCamera vcam19;
    camera18Controller cam18Controller;

    [SerializeField] GameObject fp1Object, fp2Object, fp3Object;
    GameObject[] fpObjects;
    [SerializeField] Image fingerprint1Img, fingerprint2Img, fingerprint3Img;
    Image[] fingerprints;
    [SerializeField] Sprite fp11, fp12, fp13, fp14;
    [SerializeField] Sprite fp21, fp22, fp23, fp24;
    [SerializeField] Sprite fp31, fp32, fp33, fp34;
    Sprite[][] fingerprintsById;
    Sprite[] part1, part2, part3, part4;
    List<Sprite[]> spriteList;

    [SerializeField] Image part1Img, part2Img, part3Img, part4Img;
    Image[] partsImg;

    int frameCounter = 0;
    [SerializeField] Image frame1, frame2, frame3, frame4;
    Image[] frames;
    [SerializeField] GameObject arrows1, arrows2, arrows3, arrows4;
    GameObject[] arrows;

    [SerializeField] Image rightArrow1, rightArrow2, rightArrow3, rightArrow4;
    Image[] rightArrows;
    [SerializeField] Image leftArrow1, leftArrow2, leftArrow3, leftArrow4;
    Image[] leftArrows;

    [SerializeField] Image code10;
    bool enableFind = false;
    bool finish = false;
    static int indexFp;
    static int completed = 0;
    static int imageIndex = 0;

    [SerializeField] GameObject scifiCrate;
    Animator scifiCrateAnim;
    [SerializeField] GameObject verticalExitDoor;
    Animator doorAnim;
    AudioSource doorAudio;

    [SerializeField] AudioSource effectsAudio;
    [SerializeField] AudioClip error;
    [SerializeField] AudioSource alarmSound;
    [SerializeField] GameObject playerTrigger;
    playerUI ui;
    int opened;
    bool resetState = false;
    bool enableCapture = false;

    // Initializes variables and assigns components.
    void Start()
    {
        SwapCameras(1, 0, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        ui = playerTrigger.GetComponent<playerUI>();

        canvasGroup = info.GetComponent<CanvasGroup>();
        cam18Controller = vcam18.GetComponent<camera18Controller>();
        scifiCrateAnim = scifiCrate.GetComponent<Animator>();
        doorAnim = verticalExitDoor.GetComponent<Animator>();
        doorAudio = verticalExitDoor.GetComponent<AudioSource>();

        fpObjects = new GameObject[] { fp1Object, fp2Object, fp3Object };
        fingerprints = new Image[] { fingerprint1Img, fingerprint2Img, fingerprint3Img };
        frames = new Image[] { frame1, frame2, frame3, frame4 };
        arrows = new GameObject[] { arrows1, arrows2, arrows3, arrows4 };
        partsImg = new Image[] { part1Img, part2Img, part3Img, part4Img };
        rightArrows = new Image[] { rightArrow1, rightArrow2, rightArrow3, rightArrow4 };
        leftArrows = new Image[] { leftArrow1, leftArrow2, leftArrow3, leftArrow4 };
        fingerprintsById = new Sprite[][] {
            new Sprite[] { fp11, fp12, fp13, fp14 },
            new Sprite[] { fp21, fp22, fp23, fp24 },
            new Sprite[] { fp31, fp32, fp33, fp34 }
        };

        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[9];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    static int countReturnKey = 0;

    // Handles user input and updates game state.
    void Update()
    {
        if (enableFind && frameCounter<4)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextImage();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                previousImage();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                checkKeyPressed();
                checkCorrectAnswer();
            }
        }
        manageKeyPressed();
    }

    // Starts UV light interaction when 'X' is pressed.
    void manageKeyPressed()
    {
        if (enableCapture && uvLight.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = firstPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == firstPos && !change)
            {
                StartCoroutine(waitToShow());
                playerAnim.SetBool("uvLight", true);
                StartCoroutine(waitFinishAnimation());
                change = true;
            }
        }
    }

    // Checks puzzle completion and triggers final sequence if all done.
    void checkIfCompleted()
    {
        if (frameCounter == 4 && !finish)
        {
            cam18Controller.startMovement = true;
            SwapCameras(0, 0, 1, 0);
            completed++;
            resetValues();

            if (completed >= 3)
            {
                GameManager.GameManagerInstance.LoadProgress();
                GameManager.GameManagerInstance.missionsCompleted[9] = 1;
                GameManager.GameManagerInstance.SaveProgress();
                StartCoroutine(FadeIn());
                StartCoroutine(waitUntilMoveDoor());
            }
        }
    }

    // Tracks Return key presses and triggers alarm after threshold.
    void checkKeyPressed()
    {
        countReturnKey++;
        if (countReturnKey > 15)
        {
            alarmSound.Play();
            ui.useEnergy(15f);
        }
    }

    // Waits and moves the door after 5 seconds.
    IEnumerator waitUntilMoveDoor()
    {
        SwapCameras(0, 0, 0, 1);
        yield return new WaitForSeconds(5f);
        SwapCameras(1, 0, 0, 0);
        playerMov.canMove = true;
        cc.enabled = true;
        cam18Controller.startMovement = false;
        uvLightInteractable.SetActive(false);
        doorAnim.SetBool("open", true);
        scifiCrateAnim.SetBool("move", true);
        doorAudio.Play();
        finish = true;
        this.enabled = false;
    }

    // Resets fingerprint detection values.
    void resetValues()
    {
        fingerprint1Img.gameObject.SetActive(false);
        fingerprint2Img.gameObject.SetActive(false);
        fingerprint3Img.gameObject.SetActive(false);
        fpObjects[indexFp].SetActive(false);
        enableFind = false;
        imageIndex = 0;
        frameCounter = 0;
    }

    // Advances to the next fingerprint image.
    void nextImage()
    {
        if (imageIndex < 3)
        {
            rightArrows[frameCounter].gameObject.SetActive(true);
            imageIndex++;
            partsImg[frameCounter].sprite = spriteList[frameCounter][imageIndex];
        } else
        {
            rightArrows[frameCounter].gameObject.SetActive(false);
        }
    }

    // Moves back to the previous fingerprint image.
    void previousImage()
    {
        if (imageIndex > 0)
        {
            leftArrows[frameCounter].gameObject.SetActive(true);
            imageIndex--;
            partsImg[frameCounter].sprite = spriteList[frameCounter][imageIndex];
        } else
        {
            leftArrows[frameCounter].gameObject.SetActive(false);
        }
    }

    // Checks if the selected fingerprint part is correct.
    void checkCorrectAnswer()
    {
        var currentSprite = partsImg[frameCounter].sprite;
        if (fingerprintsById.All(set => set.Length > frameCounter) &&
            (currentSprite == fingerprintsById[0][frameCounter] ||
             currentSprite == fingerprintsById[1][frameCounter] ||
             currentSprite == fingerprintsById[2][frameCounter]))
        {
            frameCounter++;
            updateFrameArrows();
            imageIndex = 0;
            checkIfCompleted();
        }
        else
        {
            effectsAudio.clip = error;
            effectsAudio.Play();
            ui.takeDamage(10f);
        }
    }

    // Starts fingerprint analysis based on the given index.
    public void analyzeFingerprint(int index)
    {
        indexFp = index;
        SwapCameras(0, 0, 0, 1);
        fingerprints[index].gameObject.SetActive(true);
        initializeFingerprintParts(index);

        part1Img.sprite = part1[0];
        part2Img.sprite = part2[0];
        part3Img.sprite = part3[0];
        part4Img.sprite = part4[0];
        updateFrameArrows();

        enableFind = true;
    }

    // Updates UI arrows based on the frame counter.
    void updateFrameArrows()
    {
        for(int i = 0; i < 4; i++)
        {
            frames[i].gameObject.SetActive(false);
            arrows[i].SetActive(false);
        }
        if (frameCounter < 4)
        {
            frames[frameCounter].gameObject.SetActive(true);
            arrows[frameCounter].SetActive(true);
        }
    }

    // Randomizes fingerprint image parts for selection.
    void initializeFingerprintParts(int index)
    {
        List<Sprite> imagesToShuffle = fingerprintsById[index].ToList();
        imagesToShuffle = imagesToShuffle.OrderBy(img => Random.value).ToList();

        part1 = new Sprite[] { imagesToShuffle[0], imagesToShuffle[2], imagesToShuffle[1], imagesToShuffle[3] };
        part2 = new Sprite[] { imagesToShuffle[1], imagesToShuffle[3], imagesToShuffle[2], imagesToShuffle[0] };
        part3 = new Sprite[] { imagesToShuffle[2], imagesToShuffle[0], imagesToShuffle[3], imagesToShuffle[1] };
        part4 = new Sprite[] { imagesToShuffle[3], imagesToShuffle[1], imagesToShuffle[0], imagesToShuffle[2] };

        spriteList = new List<Sprite[]> { part1, part2, part3, part4 };
    }


    // Shows 'X' when leaving fingerprint detector.  
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[9];
        if (other.gameObject.CompareTag("fpDetector") && opened == 0)
        {
            letterX.SetActive(true);
            enableCapture = true;
        }
    }

    // Hides 'X' when leaving fingerprint detector.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fpDetector"))
        {
            letterX.SetActive(false);
        }
    }

    // Waits before showing the info.
    IEnumerator waitToShow()
    {
        yield return new WaitForSeconds(0.2f);
        info.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutCoroutine());
    }

    // Gradually increases code10's opacity.
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = code10.color;
        color.a = 0f; 
        code10.color = color;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / 2f); 
            code10.color = color;
            yield return null; 
        }
        color.a = 1f; 
        code10.color = color;
    }

    // Fades out the info over time.
    IEnumerator FadeOutCoroutine()
    {
        float duration = 2f;
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    // Waits for animation to finish before enabling vacuum mode.
    IEnumerator waitFinishAnimation()
    {
        yield return new WaitForSeconds(3f);
        player.transform.position = secondPos;
        playerAnim.SetBool("uvLight", false);
        vcam18.gameObject.SetActive(true);
        uvLightInteractable.SetActive(true);
        uvLight.SetActive(false);
        SwapCameras(0, 0, 1, 0);
        cam18Controller.startMovement = true;

    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2, int priority3, int priority4)
    {
        vcam1.Priority = priority1;
        vcam17.Priority = priority2;
        vcam18.Priority = priority3;
        vcam19.Priority = priority4;
    }
}
