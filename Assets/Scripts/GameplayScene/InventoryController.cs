using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class inventoryController : MonoBehaviour
{
    Animator playerAnim;
    [SerializeField] GameObject player;

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

    // objects
    [SerializeField] Image object1;
    [SerializeField] Image object2;
    [SerializeField] Image object3;
    [SerializeField] Image object4;
    [SerializeField] Image object5;
    [SerializeField] Image object6;
    [SerializeField] Image object7;
    [SerializeField] Image object8;
    [SerializeField] Image object9;
    [SerializeField] Image object10;
    [SerializeField] Image object11;
    [SerializeField] Image object12;
    [SerializeField] Image object13;
    [SerializeField] Image object14;
    [SerializeField] Image object15;
    Image[] collectableItemsImgs;
    string[] itemsNames = new string[15];

    // objects sprites
    [SerializeField] Sprite crowbarSprite;
    [SerializeField] Sprite sample1Sprite;
    [SerializeField] Sprite sample2Sprite;
    [SerializeField] Sprite sample3Sprite;
    [SerializeField] Sprite sample4Sprite;
    [SerializeField] Sprite spannerwrenchSprite;
    [SerializeField] Sprite securityCardSprite;
    [SerializeField] Sprite wireCuttersSprite;
    [SerializeField] Sprite clipboardSprite;
    Sprite[] objectsSprites;

    // records
    [SerializeField] Image record1Img;
    [SerializeField] Image record2Img;
    [SerializeField] Image record3Img;
    [SerializeField] Image record4Img;
    [SerializeField] Image record5Img;
    [SerializeField] Image record6Img;
    [SerializeField] Image record7Img;
    [SerializeField] Image record8Img;
    [SerializeField] Image record9Img;
    [SerializeField] Image record10Img;
    Image[] collectableRecordsImgs;
    string[] recordsNames;

    // records sprites
    [SerializeField] Sprite record1Sprite;
    [SerializeField] Sprite record2Sprite;
    [SerializeField] Sprite record3Sprite;
    [SerializeField] Sprite record4Sprite;
    [SerializeField] Sprite record5Sprite;
    [SerializeField] Sprite record6Sprite;
    [SerializeField] Sprite record7Sprite;
    [SerializeField] Sprite record8Sprite;
    [SerializeField] Sprite record9Sprite;
    [SerializeField] Sprite record10Sprite;
    Sprite[] recordsSprites;

    public bool playerMov = true;

    // unlock arays
    public bool[] unlockedObjects;
    public bool[] unlockedRecords;

    // 3d objects
    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;
    [SerializeField] GameObject spannerwrench;
    [SerializeField] GameObject securityCard;
    [SerializeField] GameObject wireCutters;
    [SerializeField] GameObject clipboard;
    GameObject[] collectable3dObjects = new GameObject[15];

    public bool blockInventory = false;

    // Initializes inventory variables.
    void Start()
    {
        loadProgress();

        playerAnim = player.GetComponent<Animator>();
        inventoryBg.SetActive(false);
        recordsContainer.SetActive(false);
        objectsContainer.SetActive(false);

        collectableItemsImgs = new Image[] { object1, object2, object3, object4, object5, object6, object7, object8, object9, object10, object11, object12, object13, object14, object15 };
        objectsSprites = new Sprite[] { crowbarSprite, sample1Sprite, sample2Sprite, sample3Sprite, sample4Sprite, spannerwrenchSprite, securityCardSprite, wireCuttersSprite, clipboardSprite };
        collectableRecordsImgs = new Image[] { record1Img, record2Img, record3Img, record4Img, record5Img, record6Img, record7Img, record8Img, record9Img, record10Img };
        recordsSprites = new Sprite[] { record1Sprite, record2Sprite, record3Sprite, record4Sprite, record5Sprite, record6Sprite, record7Sprite, record8Sprite, record9Sprite, record10Sprite };
        recordsNames = new string[] { "Grabacion 5: Sala de Comunicaciones", "Grabacion 2: Laboratorio cientifico", "Grabacion 3: Sala de Comunicaciones", "Grabacion 1: Zona de Observacion",
                                      "Grabacion 4: Zona de Inteligencia Artificial", "Grabacion 6: Zona de Despresurizacion", "Grabacion 7: Puente de Mando", "Grabacion 8: Bahía de Mantenimiento " +
                                      "Tecnológico", "Grabacion 9: Pasillos Centrales", "Grabacion 10: Zona de Observacion" };

        unlockedObjects = new bool[objectBubbles.Length];
        unlockedRecords = new bool[recordBubbles.Length];

        resetState();
    }

    // Handles inventory visibility and section navigation.
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

        if (Input.GetKeyDown(KeyCode.I) && !blockInventory)
        {
            inventoryBg.SetActive(true);
            playerMov = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryBg.SetActive(false);
            isBrowsingSection = false;
            recordsContainer.SetActive(false);
            objectsContainer.SetActive(false);
            recordsBubble.SetActive(true);
            objectsBubble.SetActive(true);
            playerMov = true;
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

    // Handles selection between records and objects sections.
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

    // Sets the initial selection for a section.
    void SetInitialSelection(GameObject[] bubbles)
    {
        selectedRow = 0;
        selectedCol = 0;
        UpdateBubbleSelection(bubbles);
    }

    // Updates the selection visual effect.
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

    // Handles navigation inside a section.
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
            for (int i = 0; i < GameManager.GameManagerInstance.objectIndex; i++)
            {
                collectable3dObjects[i].SetActive(false);
            }

            int index = selectedRow * cols + selectedCol;
            if (index >= 0 && index < currentBubbles.Length && unlockedItems[index])
            {
                collectable3dObjects[index].SetActive(true);
                playerAnim.SetBool("closeHand", true);
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

    // Moves selection inside the grid.
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

    // Checks if an item is unlocked.
    bool IsUnlocked(int index)
    {
        return (currentContainer == recordsContainer) ? unlockedRecords[index] : unlockedObjects[index];
    }
    

    // Adds an item to the inventory.
    public void addItem(int index, int id)
    {
        itemsNames[index] = addItemNames(id);
        collectable3dObjects[index] = add3DItems(id);
        collectableItemsImgs[index].sprite = objectsSprites[id];
        collectableItemsImgs[index].gameObject.SetActive(true);
        unlockedObjects[index] = true; 
    }

    // Adds a record to the inventory.
    public void addRecord(int index, int id)
    {
        collectableRecordsImgs[index].sprite = recordsSprites[id];
        collectableRecordsImgs[index].gameObject.SetActive(true);
        unlockedRecords[index] = true;
    }

    // Loads progress and updates collected objects.
    void loadProgress()
    {
        GameManager.GameManagerInstance.LoadProgress();

        int objectsIndex = GameManager.GameManagerInstance.objectIndex;
        for(int i = 0; i < objectsIndex; i++)
        {
            itemsNames[i] = addItemNames(i);
            collectable3dObjects[i] = add3DItems(i);
        }
    }

    // Returns the name of an item by its ID.
    string addItemNames(int id)
    {
        switch (id)
        {
            case 0: return "Palanca";
            case 1: return "Muestra 1";
            case 2: return "Muestra 2";
            case 3: return "Muestra 3";
            case 4: return "Muestra 4";
            case 5: return "Llave inglesa";
            case 6: return "Tarjeta de Seguridad";
            case 7: return "Cortador de Cables";
            case 8: return "Tabla de traduccion";
            default: return "";
        }
    }

    // Returns the 3D object corresponding to an item ID.
    GameObject add3DItems(int id)
    {
        switch (id)
        {
            case 0: return crowbar;
            case 1: return sample1;
            case 2: return sample2;
            case 3: return sample3;
            case 4: return sample4;
            case 5: return spannerwrench;
            case 6: return securityCard;
            case 7: return wireCutters;
            case 8: return clipboard;
            default: return null;
        }
    }

    // Resets inventory previous state.
    void resetState()
    {
        GameManager.GameManagerInstance.LoadProgress();

        int objectsIndex = GameManager.GameManagerInstance.objectIndex;
        for (int i = 0; i < objectsIndex; i++)
        {
            collectableItemsImgs[i].gameObject.SetActive(true);
            unlockedObjects[i] = true;
        }

        int recordsIndex = GameManager.GameManagerInstance.recordIndex;
        for (int i = 0; i < recordsIndex; i++)
        {
            collectableRecordsImgs[i].gameObject.SetActive(true);
            unlockedRecords[i] = true;
        }
    }
}