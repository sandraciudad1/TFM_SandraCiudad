using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject crowbar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("collectable") && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("collectible");
            if (other.name.Equals("crowbar"))
            {
                Debug.Log("crowboar");
                DoorTriggerController doorController = gameObject.GetComponent<DoorTriggerController>();
                if (doorController != null && doorController.doorsOpen[1])
                {
                    Debug.Log("doorcontroller");
                    CrowbarController crowbarCont = crowbar.GetComponent<CrowbarController>();
                    if (crowbarCont != null)
                    {
                        Debug.Log("crowbarcontroller");
                        crowbarCont.centerObject();
                    }
                } 
                else if (!doorController.doorsOpen[1])
                {
                    Debug.Log("puerta cerrada");
                }
            }
            
        }
    }
}
