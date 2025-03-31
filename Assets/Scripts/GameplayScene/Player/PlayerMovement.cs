using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    Vector3 velocity;
    bool isGrounded;

    public float speed = 5.0f;
    public float jumpHeight = 1.0f;
    float gravity = -9.81f;

    [SerializeField] GameObject inventory;
    inventoryController inventoryCont;

    public bool canMove = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryCont = inventory.GetComponent<inventoryController>();
    }

    void Update()
    {
        if (!canMove || !inventoryCont.playerMov) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        /*bool isWalking = moveX != 0 || moveZ != 0;
        if (animator.GetBool("walk") != isWalking)
        {
            animator.SetBool("walk", isWalking);
        }*/

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (!animator.GetBool("jump")) animator.SetBool("jump", true);
        }
        else
        {
            if (animator.GetBool("jump")) animator.SetBool("jump", false);
        }

        velocity.y += gravity * Time.deltaTime;
        Vector3 totalMovement = move * speed + Vector3.up * velocity.y;
        controller.Move(totalMovement * Time.deltaTime);
    }
}