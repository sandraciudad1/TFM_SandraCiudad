using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    

    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject record5;

    [SerializeField] GameObject pinCodeCanvas;
    [SerializeField] GameObject missionsControllers;

    pinCodeController codeController;


    void Start()
    {
        codeController = missionsControllers.GetComponent<pinCodeController>();
    }

    // Handles object collection when the player interacts with an object within the trigger zone
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.GameManagerInstance.LoadProgress();
            if (other.CompareTag("object"))
            {
                // object 1: crowbar (index 0)
                if (other.name.Equals("crowbar") && (GameManager.GameManagerInstance.GetArrayUnlocked("objects", 0) == 1))
                {
                    crowbar.SetActive(false);
                    addToInventory(0);
                }
            }
        }

        if (codeController != null && other.CompareTag("record"))
        {
            // first record: 5 (index 4)
            if (other.name.Equals("record5"))
            {
                
                if ((GameManager.GameManagerInstance.GetArrayUnlocked("records", 4) == 1))
                {
                    pinCodeCanvas.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        record5.SetActive(false);
                        addToInventory(1);
                    }

                }
                else
                {
                    pinCodeCanvas.SetActive(true);
                    codeController.checkCode(5);
                }
            }
        } 
    }

    // Hides pin code canvas whan player exits record trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("record"))
        {
            pinCodeCanvas.SetActive(false);
        }
    }

    // Adds collected objects or records to the inventory
    public void addToInventory(int type)
    {
        GameManager.GameManagerInstance.LoadProgress();
        inventoryController inventoryController = inventory.GetComponent<inventoryController>();
        if (inventoryController != null)
        {
            if (type == 0)
            {
                inventoryController.addItem(GameManager.GameManagerInstance.objectIndex);
                GameManager.GameManagerInstance.objectIndex++;
            } 
            else if (type == 1)
            {
                inventoryController.addRecord(GameManager.GameManagerInstance.recordIndex);
                GameManager.GameManagerInstance.recordIndex++;
            }
            GameManager.GameManagerInstance.SaveProgress();
        }
    }
}
