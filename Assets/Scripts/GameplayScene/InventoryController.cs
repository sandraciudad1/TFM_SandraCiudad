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
    [SerializeField] GameObject crowbarImg;
    GameObject[] collectableItemsImgs;
    string[] itemsNames;
    string[] recordsNames;

    public bool playerMov = true;

    // unlock arays
    bool[] unlockedObjects;
    bool[] unlockedRecords;

    void Start()
    {
        inventoryBg.SetActive(false);
        recordsContainer.SetActive(false);
        objectsContainer.SetActive(false);

        collectableItemsImgs = new GameObject[] { crowbarImg };
        itemsNames = new string[] { "Palanca", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
        recordsNames = new string[] { "Grabacion 1: Zona de Observacion", "Grabacion 2: Laboratorio cientifico", "Grabacion 3: Sala de Comunicaciones", "Grabacion 4: Zona de Inteligencia Artificial",
                                      "Grabacion 5: Sala de Comunicaciones", "Grabacion 6: Zona de Despresurizacion", "Grabacion 7: Puente de Mando", "Grabacion 8: Bahía de Mantenimiento Tecnológico",
                                      "Grabacion 9: Pasillos Centrales", "Grabacion 10: Zona de Observacion" };

        unlockedObjects = new bool[objectBubbles.Length];
        unlockedRecords = new bool[recordBubbles.Length];
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
        if (index >= 0 && index < bubbles.Length && IsUnlocked(index))
        {
            bubbles[index].transform.localScale = Vector3.one * 1.2f;
            objectsText.text = itemsNames[index];
            recordsText.text = recordsNames[index];
        }
    }

    void HandleItemSelection()
    {
        GameObject[] currentBubbles;
        bool[] unlockedItems;

        if (currentContainer == recordsContainer)
        {
            currentBubbles = recordBubbles;
            unlockedItems = unlockedRecords;
        }
        else
        {
            currentBubbles = objectBubbles;
            unlockedItems = unlockedObjects;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelection(-1, 0, unlockedItems);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelection(1, 0, unlockedItems);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelection(0, -1, unlockedItems);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelection(0, 1, unlockedItems);

        UpdateBubbleSelection(currentBubbles);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            int index = selectedRow * cols + selectedCol;
            if (index >= 0 && index < currentBubbles.Length && unlockedItems[index])
            {
                Debug.Log("Mostrar información de: " + currentBubbles[index].name);
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

    void MoveSelection(int rowChange, int colChange, bool[] unlockedItems)
    {
        int newRow = Mathf.Clamp(selectedRow + rowChange, 0, rows - 1);
        int newCol = Mathf.Clamp(selectedCol + colChange, 0, cols - 1);
        int newIndex = newRow * cols + newCol;

        if (unlockedItems[newIndex])
        {
            selectedRow = newRow;
            selectedCol = newCol;
        }
    }

    bool IsUnlocked(int index)
    {
        return (currentContainer == recordsContainer) ? unlockedRecords[index] : unlockedObjects[index];
    }

    public void addItem(int id)
    {
        collectableItemsImgs[id].SetActive(true);
        unlockedObjects[id] = true; 
    }
}