using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    DoorTriggerController doorController;
    CrowbarController crowbarCont;

    [SerializeField] GameObject crowbar;

    // Start is called before the first frame update
    void Start()
    {
        doorController = gameObject.GetComponent<DoorTriggerController>();
        crowbarCont = crowbar.GetComponent<CrowbarController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (other.CompareTag("object"))
            {
                if (doorController != null)
                {
                    if (other.name.Equals("crowbar") && doorController.doorsOpen[1])
                    {
                        
                        if (crowbarCont != null)
                        {
                            crowbarCont.centerObject();
                        }
                    }
                    else if (!doorController.doorsOpen[1])
                    {
                        Debug.Log("puerta cerrada");
                    }
                }
            }

            if (other.CompareTag("record"))
            {

            }


            
            
        }
    }
}
