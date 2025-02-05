using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    Vector3 velocity;
    bool isGrounded;

    float speed = 5.0f;
    float jumpHeight = 1.0f;
    float gravity = -9.81f;

    [SerializeField] GameObject inventory;
    inventoryController inventoryCont;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryCont = inventory.GetComponent<inventoryController>();
    }

    void Update()
    {
        if (inventoryCont != null && inventoryCont.playerMov)
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);

            /*if (moveX != 0 || moveZ != 0)
            {
                animator.SetBool("walk", true);
            }
            else
            {
                animator.SetBool("walk", false);  
            }*/

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                //animator.SetBool("jump", true);
            }
            else
            {
                //animator.SetBool("jump", false);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
    }
}