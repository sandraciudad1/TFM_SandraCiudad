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

    bool correct = false;

    [SerializeField] GameObject crate1;
    Animator crateAnim;

    // Initializes the correct codes and gets the crate animator
    void Start()
    {
        correctCodes = new string[] { "0000", "0000", "0000", "0000", "1235", "0000", "0000", "0000", "0000", "0000" };

        crateAnim = crate1.GetComponent<Animator>();
    }

    // Handles user input for the code entry
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

    // Enables code checking and sets the correct code
    public void checkCode(int id)
    {
        canCheck = true;
        index = id - 1;
        correctCode = correctCodes[index].ToString();
    }

    // Compares user input with the correct code
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

    //
    public void clearInput()
    {
        userInput = "";
        inputText.text = userInput;
    }

    // Executes the action when the correct code is entered
    public void correctCodeAction()
    {
        switch (index)
        {
            case 4:
                crateAnim.SetBool("open", true);
                GameManager.GameManagerInstance.SetArrayUnlocked("records", 4, 1);
                GameManager.GameManagerInstance.SaveProgress();
                break;
        }
        
    }
}
