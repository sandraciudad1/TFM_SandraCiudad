using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record2Controller : MonoBehaviour
{
    [SerializeField] GameObject mouse;
    [SerializeField] GameObject mouseStatic;

    // 
    void Start()
    {
        
    }

    // 
    void Update()
    {
        if (mouse.activeInHierarchy)
        {
            mouseStatic.SetActive(false);
        } else
        {
            mouseStatic.SetActive(true);
        }
    }
}
