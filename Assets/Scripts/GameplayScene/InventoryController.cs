using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryBg;
    [SerializeField] GameObject recordsBubble, objectsBubble;
    [SerializeField] GameObject recordsContainer, objectsContainer;
    [SerializeField] GameObject[] recordBubbles;  
    [SerializeField] GameObject[] objectBubbles;
    [SerializeField] TextMeshProUGUI objectsText;
    [SerializeField] TextMeshProUGUI recordsText;

    int selectedBubble = 0;
    bool isBrowsingSection = false;
    int rows, cols;
    GameObject currentContainer;

    int selectedRow = 0; 
    int selectedCol = 0;

    // items
    [SerializeField] GameObject crowbar;
    GameObject[] collectableItems;
    string[] itemsNames;
    string[] recordsNames;

    public bool playerMov = true;

    void Start()
    {
        inventoryBg.SetActive(false);
        recordsContainer.SetActive(false);
        objectsContainer.SetActive(false);
        collectableItems = new GameObject[] { crowbar };
        itemsNames = new string[] { "Palanca", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
        recordsNames = new string[] { "Grabacion 1: Zona de Observacion", "Grabacion 2: Laboratorio cientifico", "Grabacion 3: Sala de Comunicaciones", "Grabacion 4: Zona de Inteligencia Artificial",
                                      "Grabacion 5: Sala de Comunicaciones", "Grabacion 6: Zona de Despresurizacion", "Grabacion 7: Puente de Mando", "Grabacion 8: Bahía de Mantenimiento Tecnológico",
                                      "Grabacion 9: Pasillos Centrales", "Grabacion 10: Zona de Observacion" };
    }

    void Update()
    {
        if (inventoryBg.activeInHierarchy)
        {
            playerMov = false;
        }
        else
        {
            playerMov = true;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryBg.SetActive(!inventoryBg.activeSelf);
            if (!inventoryBg.activeSelf)
            {
                isBrowsingSection = false;
                recordsContainer.SetActive(false);
                objectsContainer.SetActive(false);
            }
        }

        if (!inventoryBg.activeSelf) return;

        if (!isBrowsingSection)
        {
            HandleBubbleSelection();
        }
        else
        {
            HandleItemSelection();
        }
    }

    void HandleBubbleSelection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectedBubble = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectedBubble = 1;

        if (selectedBubble == 0)
        {
            recordsBubble.transform.localScale = Vector3.one * 1.2f;
            objectsBubble.transform.localScale = Vector3.one;
        }
        else
        {
            recordsBubble.transform.localScale = Vector3.one;
            objectsBubble.transform.localScale = Vector3.one * 1.2f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            recordsBubble.SetActive(false);
            objectsBubble.SetActive(false);
            isBrowsingSection = true;

            if (selectedBubble == 0)
            {
                recordsContainer.SetActive(true);
                currentContainer = recordsContainer;
                rows = 2; cols = 5; 
                SetInitialSelection(recordBubbles);
            }
            else
            {
                objectsContainer.SetActive(true);
                currentContainer = objectsContainer;
                rows = 3; cols = 5;  
                SetInitialSelection(objectBubbles);
            }
        }
    }

    void SetInitialSelection(GameObject[] bubbles)
    {
        selectedRow = 0;
        selectedCol = 0;
        UpdateBubbleSelection(bubbles);
    }

    void UpdateBubbleSelection(GameObject[] bubbles)
    {
        foreach (var bubble in bubbles)
        {
            bubble.transform.localScale = Vector3.one;
        }

        int index = selectedRow * cols + selectedCol;
        if (index >= 0 && index < bubbles.Length)
        {
            bubbles[index].transform.localScale = Vector3.one * 1.2f;
            objectsText.text = itemsNames[index];
            recordsText.text = recordsNames[index];
        }
    }

    void HandleItemSelection()
    {
        GameObject[] currentBubbles = (currentContainer == recordsContainer) ? recordBubbles : objectBubbles;

        if (Input.GetKeyDown(KeyCode.UpArrow)) selectedRow = Mathf.Max(0, selectedRow - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) selectedRow = Mathf.Min(rows - 1, selectedRow + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectedCol = Mathf.Max(0, selectedCol - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectedCol = Mathf.Min(cols - 1, selectedCol + 1);

        UpdateBubbleSelection(currentBubbles);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            int index = selectedRow * cols + selectedCol;
            if (index >= 0 && index < currentBubbles.Length)
            {
                ShowItemInfo(currentBubbles[index]);

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBrowsingSection = false;
            recordsContainer.SetActive(false);
            objectsContainer.SetActive(false);
            recordsBubble.SetActive(true);
            objectsBubble.SetActive(true);
        }
    }

    void ShowItemInfo(GameObject selectedBubble)
    {
        Debug.Log("Mostrar información de: " + selectedBubble.name );
    }

    public void addItem(int id)
    {
        collectableItems[id].SetActive(true);
        //objectsText.text = itemsNames[id];
    }
    
    /*void ShowItemInfo(GameObject selectedBubble)
    {
        string infoRecords = "";

        if (selectedBubble.name.Equals("recordsBubble1"))
        {
            infoRecords = "Grabacion 1: Zona de Observacion";
        } 
        else if (selectedBubble.name.Equals("recordsBubble2"))
        {
            infoRecords = "Grabacion 2: Laboratorio cientifico";
        }
        else if (selectedBubble.name.Equals("recordsBubble3"))
        {
            infoRecords = "Grabacion 3: Sala de Comunicaciones";
        }
        else if (selectedBubble.name.Equals("recordsBubble4"))
        {
            infoRecords = "Grabacion 4: Zona de Inteligencia Artificial";
        }
        else if (selectedBubble.name.Equals("recordsBubble5"))
        {
            infoRecords = "Grabacion 5: Sala de Comunicaciones";
        }
        else if (selectedBubble.name.Equals("recordsBubble6"))
        {
            infoRecords = "Grabacion 6: Zona de Despresurizacion";
        }
        else if (selectedBubble.name.Equals("recordsBubble7"))
        {
            infoRecords = "Grabacion 7: Puente de Mando";
        }
        else if (selectedBubble.name.Equals("recordsBubble8"))
        {
            infoRecords = "Grabacion 8: Bahía de Mantenimiento Tecnológico";
        }
        else if (selectedBubble.name.Equals("recordsBubble9"))
        {
            infoRecords = "Grabacion 9: Pasillos Centrales";
        }
        else if (selectedBubble.name.Equals("recordsBubble10"))
        {
            infoRecords = "Grabacion 10: Zona de Observacion";
        }
        textRecords.text = infoRecords;
    }*/
}