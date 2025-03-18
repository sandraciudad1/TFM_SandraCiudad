using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission8Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(124.41f, 20.679f, 69.42f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;

    [SerializeField] GameObject vacuum;
    [SerializeField] GameObject vacuumMobile;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam13;
    [SerializeField] CinemachineVirtualCamera vcam14;

    public bool enableControl = false;
    public bool finish = false;
    bool exit = false;
    float speed = 1f;

    // 
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
    }

    // 
    void Update()
    {
        if (enableControl)
        {
            float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime; 
            float moveZ = -Input.GetAxis("Horizontal") * speed * Time.deltaTime; 
            vacuumMobile.transform.position += new Vector3(0, moveY, moveZ);
        }

        if (finish && !exit)
        {
            vacuumMobile.SetActive(false);
            playerMov.canMove = true;
            cc.enabled = true;
            SwapCameras(1, 0, 0);
            exit = true;
        }
    }

    // Shows 'X' when leaving book.  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("grids"))
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving book.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("grids"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("grids") && vacuum.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                playerAnim.SetBool("vacuum", true);
                StartCoroutine(waitFinishAnimation());
                change = true;
            }
        }
    }

    IEnumerator waitFinishAnimation()
    {
        yield return new WaitForSeconds(3f);
        vacuum.SetActive(false);
        vacuumMobile.SetActive(true);
        playerAnim.SetBool("vacuum", false);
        SwapCameras(0, 0, 1);
        enableControl = true;
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam13.Priority = priority2;
        vcam14.Priority = priority3;
    }

}
