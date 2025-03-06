using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class mission4Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3();
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;

    [SerializeField] GameObject scifi_terminal;
    [SerializeField] GameObject letterX;
    bool finish = false;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Shows 'X' when near scifi terminal.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("scifi_terminal") && !finish)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving scifi terminal.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("scifi_terminal"))
        {
            letterX.SetActive(false);
        }
    }

    // Manages actions when staying near modular pipes.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("scifi_terminal") && scifi_terminal.activeInHierarchy && Input.GetKeyDown(KeyCode.X))
        {
            letterX.SetActive(false);
            SwapCameras(0, 1);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (player.transform.position == playerPos && !change)
            {
                // actions 
                change = true;
            }
        }
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam7.Priority = priority2;
    }
}
