using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    Animator animator; 

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {

    }

    public void centerObject()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x - 0.5f, player.transform.position.y + 1.8f, player.transform.position.z + 1f); 

        transform.position = targetPosition;
        animator.SetBool("collect", true); 
    }
}
