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
    //public bool[] recordsUnlocked;

    // Initializes the recordsUnlocked array with default values
    void Start()
    {
        
        //recordsUnlocked = new bool[] { false, false, false, false, false, false, false, false, false, false };
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

            if (other.CompareTag("record"))
            {
                // first record: 5 (index 4)
                if (other.name.Equals("record5") && (GameManager.GameManagerInstance.GetArrayUnlocked("records", 4) == 1))
                {
                    record5.SetActive(false);
                    addToInventory(1);
                }
            }
        }

        if (other.CompareTag("record"))
        {
            pinCodeCanvas.SetActive(true);
            pinCodeController codeController = missionsControllers.GetComponent<pinCodeController>();
            if (codeController != null)
            {
                // first record: 5 (index 4)
                if (other.name.Equals("record5"))
                {
                    codeController.checkCode(5);

                    //record5.SetActive(false);
                    //addToInventory(1);
                }
            }
        } 
    }

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
