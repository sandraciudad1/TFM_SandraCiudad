using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission9Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(117.063f, 20.679f, 88.062f);
    Vector3 playerFinalPos = new Vector3(118.02f, 20.679f, 87.24f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    bool change = false;
    public bool finishGame = false;
    bool finish = false;

    [SerializeField] GameObject compass;
    [SerializeField] GameObject letterX;
    [SerializeField] GameObject syncronizationScreen;
    [SerializeField] GameObject puzzle;
    puzzleController puzzlecontroller;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam15;
    [SerializeField] CinemachineVirtualCamera vcam16;

    // Initializes components and sets up initial camera view.
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        puzzlecontroller = puzzle.GetComponent<puzzleController>();
    }

    // Updates game state when finished and handles final movement.
    void Update()
    {
        if (finishGame && !finish)
        {
            SwapCameras(1, 0, 0);
            player.transform.position = playerFinalPos;
            playerMov.canMove = true;
            cc.enabled = true;
            finish = true;
        }
    }


    // Shows 'X' when leaving grids.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("console"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving grids.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("console"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("console") && compass.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                playerAnim.SetBool("compass", true);
                StartCoroutine(waitUntilFinish());
                change = true;
            }
        }
    }

    // Waits until finish animation and syncronization video.
    IEnumerator waitUntilFinish()
    {
        yield return new WaitForSeconds(3f);
        syncronizationScreen.SetActive(true);
        puzzle.SetActive(true);
        yield return new WaitForSeconds(5f);
        playerAnim.SetBool("compass", true);
        compass.SetActive(false);
        SwapCameras(0, 0, 1);
        puzzlecontroller.canMove = true;
    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam15.Priority = priority2;
        vcam16.Priority = priority3;
    }
}
