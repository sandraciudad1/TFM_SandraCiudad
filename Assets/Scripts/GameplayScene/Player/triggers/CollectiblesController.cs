using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (other.CompareTag("object"))
            {
                DoorTriggerController doorController = GetComponent<DoorTriggerController>();
                if (doorController != null)
                {
                    if (other.name.Equals("crowbar") && doorController.doorsOpen[1])
                    {
                        crowbar.SetActive(false);
                        addToInventory(0);
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

    public void addToInventory(int id)
    {
        inventoryController inventoryController = inventory.GetComponent<inventoryController>();
        if (inventoryController != null)
        {
            inventoryController.addItem(id);
        }
    }
}
