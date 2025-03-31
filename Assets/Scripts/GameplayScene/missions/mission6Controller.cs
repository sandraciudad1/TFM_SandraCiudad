using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mission6Controller : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator playerAnim;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(123.945f, 25.67f, 51.546f);
    Vector3 endPosition = new Vector3(124.749f, 25.67f, 53.155f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    bool change = false;

    [SerializeField] GameObject clipboard;
    [SerializeField] GameObject clipboardStatic;
    [SerializeField] GameObject letterX;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam9;
    [SerializeField] CinemachineVirtualCamera vcam10;

    [SerializeField] TextMeshProUGUI inputText1, inputText2, inputText3;
    TextMeshProUGUI[] inputTexts;
    [SerializeField] GameObject textBox1, textBox2, textBox3;
    GameObject[] textBoxs;

    string userInput = "";
    string[] correctAnswers = { "C0D1GO", "PU3RT4", "5726" };
    static int wordCounter = 0;
    bool canCheck = true;

    [SerializeField] GameObject inventory;
    inventoryController inventoryCont;
    int opened;
    bool resetState = false;

    // Initializes references and sets up objects at the start of the game.
    void Start()
    {
        SwapCameras(1, 0, 0);
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        inputTexts = new TextMeshProUGUI[] { inputText1, inputText2, inputText3 };
        textBoxs = new GameObject[] { textBox1, textBox2, textBox3 };
        inventoryCont = inventory.GetComponent<inventoryController>();

        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[5];
        if (opened == 1 && !resetState)
        {
            letterX.gameObject.SetActive(false);
            resetState = true;
        }
    }

    // Checks input and manages word validation and movement restrictions.
    void Update()
    {
        HandleInput();
        HandleMissionCompletion();
    }

    // Handles keyboard input and updates answer field.
    void HandleInput()
    {
        if (!change || !canCheck) return;

        foreach (char c in Input.inputString)
        {
            if (c != '\b') userInput += c;
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && userInput.Length > 0)
        {
            userInput = userInput[..^1];
        }

        inputTexts[wordCounter].text = userInput;
        checkAnswer(userInput);
    }

    // Completes mission when all answers are correct.
    void HandleMissionCompletion()
    {
        if (wordCounter < 3) return;

        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.missionsCompleted[5] = 1;
        GameManager.GameManagerInstance.SaveProgress();

        canCheck = false;
        SwapCameras(1, 0, 0);
        player.transform.position = endPosition;
        inventoryCont.blockInventory = false;
        playerMov.canMove = true;
        cc.enabled = true;

        foreach (var box in textBoxs)
        {
            box.SetActive(false);
        }
        //habilitar 7
        this.enabled = false;
    }

    // Enables the text box for the next input.
    void enableTextBox()
    {
        StartCoroutine(clearInputBuffer());
        textBoxs[wordCounter].SetActive(true);
    }

    // Checks if the user's input matches the correct answer.
    void checkAnswer(string userInput)
    {
        if (userInput.ToUpper() == correctAnswers[wordCounter].ToUpper())
        {
            wordCounter++;
            if (wordCounter < 3)
            {
                enableTextBox();
            }
        }
    }

    // Shows 'X' when leaving book.  
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[5];
        if (other.gameObject.CompareTag("book") && opened == 0)
        {
            letterX.SetActive(true);
        }
    }

    // Hides 'X' when leaving book.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("book"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        GameManager.GameManagerInstance.LoadProgress();
        opened = GameManager.GameManagerInstance.missionsCompleted[5];
        if (other.gameObject.CompareTag("book") && clipboard.activeInHierarchy && Input.GetKeyDown(KeyCode.X) && opened == 0)
        {
            letterX.SetActive(false);
            SwapCameras(0, 1, 0);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (Vector3.Distance(player.transform.position, playerPos) < 0.01f && !change)
            {
                inventoryCont.blockInventory = true;
                playerAnim.SetBool("clipboard", true);
                StartCoroutine(waitEndAnimation());
                change = true;
                StartCoroutine(clearInputBuffer());
            }
        }
    }

    // Clears the input buffer after a frame delay.
    private IEnumerator clearInputBuffer()
    {
        yield return null; 
        userInput = ""; 
    }

    // Handles the animation transition after interacting with the clipboard.
    IEnumerator waitEndAnimation()
    {
        yield return new WaitForSeconds(2f);
        SwapCameras(0, 0, 1);
        clipboard.SetActive(false);
        clipboardStatic.SetActive(true);
        playerAnim.SetBool("clipboard", false);
        enableTextBox();
    }

    // Swap between virtual cameras
    void SwapCameras(int priority1, int priority2, int priority3)
    {
        vcam1.Priority = priority1;
        vcam9.Priority = priority2;
        vcam10.Priority = priority3;
    }
}
