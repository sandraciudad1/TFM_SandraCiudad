using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactersController : MonoBehaviour
{
    public int counter = 0;
    Animator animator;

    float speed = 2f;
    float rotationSpeed = 5f;

    [SerializeField] GameObject presenter;
    Vector3 presenterPosition = new Vector3(-1f, 0f, 0f);
    Vector3 presenterRotation = new Vector3(0f, 140f, 0f);

    [SerializeField] GameObject madison;
    Vector3 madisonPosition = new Vector3(-1f, 0f, 1.8f);
    Vector3 madisonRotation = new Vector3(0f, -195f, 0f);

    [SerializeField] GameObject liam;
    Vector3 liamPosition = new Vector3(-0.15f, 0f, 1.8f);
    Vector3 liamRotation = new Vector3(0f, -180f, 0f);

    [SerializeField] GameObject nicole;
    Vector3 nicolePosition = new Vector3(0.7f, 0f, 1.8f);
    Vector3 nicoleRotation = new Vector3(0f, -170f, 0f);

    [SerializeField] GameObject austin;
    Vector3 austinPosition = new Vector3(1.55f, 0f, 1.8f);
    Vector3 austinRotation = new Vector3(0f, -155f, 0f);

    [SerializeField] GameObject cooper;
    Vector3 cooperPosition = new Vector3(2.4f, 0f, 1.8f);
    Vector3 cooperRotation = new Vector3(0f, -140f, 0f);


    void Update()
    {
        if (counter == 1)
        {
            moveToTarget(madison, madisonPosition, madisonRotation);
        } else if (counter == 2)
        {
            moveToTarget(liam, liamPosition, liamRotation);
        }
        else if (counter == 3)
        {
            moveToTarget(nicole, nicolePosition, nicoleRotation);
        }
        else if (counter == 4)
        {
            moveToTarget(austin, austinPosition, austinRotation);
        }
        else if (counter == 5)
        {
            moveToTarget(cooper, cooperPosition, cooperRotation);
        }


    }

    void moveToTarget(GameObject character, Vector3 targetPosition, Vector3 targetRotation)
    {
        animator = character.GetComponent<Animator>();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            animator.SetBool("walk", false);

            Quaternion targetQuatRotation = Quaternion.Euler(targetRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuatRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("walk", true);
        }
    }
}
