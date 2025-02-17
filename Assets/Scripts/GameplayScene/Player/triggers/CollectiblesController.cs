using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    

    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject record5;

    public bool[] recordsUnlocked;

    // Initializes the recordsUnlocked array with default values
    void Start()
    {
        recordsUnlocked = new bool[] { false, false, false, false, false, false, false, false, false, false };
    }

    // Handles object collection when the player interacts with an object within the trigger zone
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (other.CompareTag("object"))
            {
                // object 1: crowbar
                DoorTriggerController doorController = GetComponent<DoorTriggerController>();
                if (doorController != null)
                {
                    if (other.name.Equals("crowbar") && doorController.doorsOpen[1])
                    {
                        crowbar.SetActive(false);
                        addToInventory(0, 0);
                    }
                    else if (!doorController.doorsOpen[1])
                    {
                        Debug.Log("puerta cerrada");
                    }
                }
            }

            if (other.CompareTag("record"))
            {
                // first record: 5
                if (other.name.Equals("record5") && recordsUnlocked[4])
                { 
                    record5.SetActive(false);
                    addToInventory(1, 0);
                }
            }
        }
    }

    // Adds collected objects or records to the inventory
    public void addToInventory(int type, int id)
    {
        inventoryController inventoryController = inventory.GetComponent<inventoryController>();
        if (inventoryController != null)
        {
            if (type == 0)
            {
                inventoryController.addItem(id);
            } 
            else if (type == 1)
            {
                inventoryController.addRecord(id);
            }
            
        }
    }
}
