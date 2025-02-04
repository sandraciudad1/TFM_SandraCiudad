using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryBg;
    [SerializeField] GameObject recordsBubble, objectsBubble;
    [SerializeField] GameObject recordsContainer, objectsContainer;
    [SerializeField] GameObject[] recordBubbles;  
    [SerializeField] GameObject[] objectBubbles;

    int selectedBubble = 0; 
    bool isBrowsingSection = false;
    int rows, cols;
    GameObject currentContainer;

    int selectedRow = 0; 
    int selectedCol = 0;

    void Start()
    {
        inventoryBg.SetActive(false);
        recordsContainer.SetActive(false);
        objectsContainer.SetActive(false);
    }

    void Update()
    {
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
                rows = 2; cols = 5;  // Estructura de la matriz de grabaciones
                // Asumimos que tienes las burbujas dentro de un arreglo 'recordBubbles'
                SetInitialSelection(recordBubbles);
            }
            else
            {
                objectsContainer.SetActive(true);
                currentContainer = objectsContainer;
                rows = 3; cols = 5;  // Estructura de la matriz de objetos
                // Asumimos que tienes las burbujas dentro de un arreglo 'objectBubbles'
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
        Debug.Log("Mostrar información de: " + selectedBubble.name);
    }

}