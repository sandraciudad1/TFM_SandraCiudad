using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record10Controller : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(waitToStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("getUp", true);

    }
}
