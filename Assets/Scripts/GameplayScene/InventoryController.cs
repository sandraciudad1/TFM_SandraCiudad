using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryBg;
    [SerializeField] GameObject recordsBubble, objectsBubble;
    [SerializeField] GameObject recordsContainer, objectsContainer;
    [SerializeField] GameObject[] recordBubbles;  // Arreglo para las burbujas de grabaciones
    [SerializeField] GameObject[] objectBubbles;  // Arreglo para las burbujas de objetos

    int selectedBubble = 0;
    bool isBrowsingSection = false;
    int rows, cols;
    GameObject currentContainer;

    int selectedRow = 0; // Fila seleccionada en la matriz
    int selectedCol = 0; // Columna seleccionada en la matriz

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
        // Selección entre burbujas principales
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectedBubble = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectedBubble = 1;

        // Ampliar la burbuja seleccionada
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

        // Acceder a la sección seleccionada
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

    // Configurar la selección inicial de la matriz (por ejemplo, seleccionando el primer elemento)
    void SetInitialSelection(GameObject[] bubbles)
    {
        selectedRow = 0;
        selectedCol = 0;
        UpdateBubbleSelection(bubbles);
    }

    // Actualizar el tamaño de la burbuja seleccionada
    void UpdateBubbleSelection(GameObject[] bubbles)
    {
        // Restablecer el tamaño de todas las burbujas
        foreach (var bubble in bubbles)
        {
            bubble.transform.localScale = Vector3.one;
        }

        // Aumentar el tamaño de la burbuja seleccionada
        int index = selectedRow * cols + selectedCol;
        if (index >= 0 && index < bubbles.Length)
        {
            bubbles[index].transform.localScale = Vector3.one * 1.2f;
        }
    }

    void HandleItemSelection()
    {
        GameObject[] currentBubbles = (currentContainer == recordsContainer) ? recordBubbles : objectBubbles;

        // Moverse por las filas y columnas
        if (Input.GetKeyDown(KeyCode.UpArrow)) selectedRow = Mathf.Max(0, selectedRow - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) selectedRow = Mathf.Min(rows - 1, selectedRow + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectedCol = Mathf.Max(0, selectedCol - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectedCol = Mathf.Min(cols - 1, selectedCol + 1);

        // Actualizar selección visual
        UpdateBubbleSelection(currentBubbles);

        // Seleccionar un elemento
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int index = selectedRow * cols + selectedCol;
            if (index >= 0 && index < currentBubbles.Length)
            {
                ShowItemInfo(currentBubbles[index]);
            }
        }

        // Volver a la sección principal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBrowsingSection = false;
            recordsContainer.SetActive(false);
            objectsContainer.SetActive(false);
            recordsBubble.SetActive(true);
            objectsBubble.SetActive(true);
        }
    }

    // Mostrar la información del objeto seleccionado
    void ShowItemInfo(GameObject selectedBubble)
    {
        // Aquí puedes mostrar la información del objeto de acuerdo a tu lógica,
        // ya sea mostrando un panel con detalles, un texto, etc.
        Debug.Log("Mostrar información de: " + selectedBubble.name);
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