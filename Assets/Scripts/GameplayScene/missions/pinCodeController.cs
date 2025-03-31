using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pinCodeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI inputText;
    bool canCheck = false;
    string[] correctCodes;
    string correctCode; 
    string userInput = "";
    int index;

    [SerializeField] GameObject crate1;
    [SerializeField] GameObject crate2;
    [SerializeField] GameObject crate3;
    [SerializeField] GameObject crate4;
    [SerializeField] GameObject crate5;
    [SerializeField] GameObject crate6;
    [SerializeField] GameObject crate7;
    [SerializeField] GameObject crate8;
    [SerializeField] GameObject crate9;
    [SerializeField] GameObject crate10;

    GameObject[] crates;
    Animator[] crateAnims;

    // Initializes the correct codes and gets the crate animator.
    void Start()
    {
        correctCodes = new string[] { "4141", "6375", "4360", "0938", "1235", "0134", "7615", "3962", "8870", "2599" };
        crates = new GameObject[] { crate1, crate2, crate3, crate4, crate5, crate6, crate7, crate8, crate9, crate10 };
        initializeAnimators();
    }

    // Assigns Animator components to crates and doors.
    void initializeAnimators()
    {
        crateAnims = new Animator[crates.Length];
        for (int i = 0; i < crates.Length; i++)
        {
            crateAnims[i] = crates[i].GetComponent<Animator>();
        }
    }

    // Handles user input for the code entry.
    void Update()
    {
        if (canCheck)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) && userInput.Length < 4)  
                {
                    userInput += c;
                }
                else if (c == '\b' && userInput.Length > 0)  
                {
                    userInput = userInput.Substring(0, userInput.Length - 1);
                }
                else if (c == '\n' || c == '\r') 
                {
                    CheckCode();
                }
            }
            inputText.text = userInput;
        }
    }

    // Enables code checking and sets the correct code.
    public void checkCode(int id)
    {
        canCheck = true;
        index = id - 1;
        correctCode = correctCodes[index].ToString();
    }

    // Compares user input with the correct code.
    private void CheckCode()
    {
        if (userInput == correctCode)
        {
            correctCodeAction();
            canCheck = false;
        }
        else
        {
            userInput = "";  
        }
    }

    // Resets the user input field and clears the displayed text.
    public void clearInput()
    {
        userInput = "";
        inputText.text = userInput;
    }

    // Executes the action when the correct code is entered.
    public void correctCodeAction()
    {
        GameManager.GameManagerInstance.LoadProgress();
        var gm = GameManager.GameManagerInstance;
        gm.correctCodeCounter++;
        crateAnims[index].SetBool("open", true);
        gm.SetArrayUnlocked("records", index, 1);
        gm.SaveProgress();
    }
}
