using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    
    // objects 
    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;
    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject securityCard;
    [SerializeField] GameObject wireCutters;

    // records
    [SerializeField] GameObject record5;
    [SerializeField] GameObject record3;
    [SerializeField] GameObject record2;
    [SerializeField] GameObject record1;

    [SerializeField] GameObject pinCodeCanvas;
    [SerializeField] GameObject missionsControllers;

    pinCodeController codeController;


    // Initializes the pin code controller reference.
    void Start()
    {
        codeController = missionsControllers.GetComponent<pinCodeController>();
    }

    // Handles object collection when the player interacts with an object within the trigger zone.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (other.CompareTag("object"))
            {
                canAddToInventory(other, "crowbar", 0, crowbar);
                canAddToInventory(other, "sample1", 1, sample1);
                canAddToInventory(other, "sample2", 2, sample2);
                canAddToInventory(other, "sample3", 3, sample3);
                canAddToInventory(other, "sample4", 4, sample4);
                canAddToInventory(other, "spannerwrench", 5, spannerwrench);
                canAddToInventory(other, "securityCard", 6, securityCard);
                canAddToInventory(other, "wireCutters", 7, wireCutters);
            }
        }

        if (codeController != null && other.CompareTag("record"))
        {
            if (other.name.Equals("record5"))
            {
                recordsManager(5, record5);
            } 
            else if (other.name.Equals("record3"))
            {
                recordsManager(3, record3);
            }
            else if (other.name.Equals("record2"))
            {
                recordsManager(2, record2);
            }
            else if (other.name.Equals("record1"))
            {
                recordsManager(1, record1);
            }
        } 
    }

    // Manages records collection and pin code validation.
    void recordsManager(int id, GameObject record)
    {
        int index = id - 1;
        if ((GameManager.GameManagerInstance.GetArrayUnlocked("records", index) == 1))
        {
            pinCodeCanvas.SetActive(false);
            if (Input.GetKeyDown(KeyCode.R))
            {
                record.SetActive(false);
                addToInventory(1, index);
            }
        }
        else
        {
            pinCodeCanvas.SetActive(true);
            codeController.checkCode(id);
        }
    }

    // Checks if an object can be added to the inventory.
    void canAddToInventory(Collider other, string name, int index, GameObject gameobject)
    {
        GameManager.GameManagerInstance.LoadProgress();
        if (other.name.Equals(name) && (GameManager.GameManagerInstance.GetArrayUnlocked("objects", GameManager.GameManagerInstance.objectIndex) == 1))
        {
            gameobject.SetActive(false);
            addToInventory(0, index);
        }
    }

    // Clears pin code input when the player enters a record trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("record"))
        {
            codeController.clearInput();
        }
    }

    // Hides pin code canvas whan player exits record trigger.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("record"))
        {
            pinCodeCanvas.SetActive(false);
        }
    }

    // Adds collected objects or records to the inventory.
    public void addToInventory(int type, int id)
    {
        GameManager.GameManagerInstance.LoadProgress();
        inventoryController inventoryController = inventory.GetComponent<inventoryController>();
        if (inventoryController != null)
        {
            if (type == 0)
            {
                inventoryController.addItem(GameManager.GameManagerInstance.objectIndex, id);
                GameManager.GameManagerInstance.objectIndex++;
            } 
            else if (type == 1)
            {
                inventoryController.addRecord(GameManager.GameManagerInstance.recordIndex, id);
                GameManager.GameManagerInstance.recordIndex++;
            }
            GameManager.GameManagerInstance.SaveProgress();
        }
    }
}
