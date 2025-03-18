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
    [SerializeField] GameObject crate2;
    [SerializeField] GameObject crate3;
    [SerializeField] GameObject crate4;
    [SerializeField] GameObject crate5;
    [SerializeField] GameObject crate6;
    [SerializeField] GameObject crate7;
    [SerializeField] GameObject crate8;
    [SerializeField] GameObject crate9;
    [SerializeField] GameObject crate10;
    Animator crateAnim1;
    Animator crateAnim2;
    Animator crateAnim3;
    Animator crateAnim4;
    Animator crateAnim5;
    Animator crateAnim6;
    Animator crateAnim7;
    Animator crateAnim8;
    Animator crateAnim9;
    Animator crateAnim10;
    Animator[] crateAnimators;

    // Initializes the correct codes and gets the crate animator.
    void Start()
    {
        correctCodes = new string[] { "4141", "6375", "4360", "0938", "1235", "0134", "7615", "3962", "0000", "0000" };

        crateAnim1 = crate1.GetComponent<Animator>();
        crateAnim2 = crate2.GetComponent<Animator>();
        crateAnim3 = crate3.GetComponent<Animator>();
        crateAnim4 = crate4.GetComponent<Animator>();
        crateAnim5 = crate5.GetComponent<Animator>();
        crateAnim6 = crate6.GetComponent<Animator>();
        crateAnim7 = crate7.GetComponent<Animator>();
        crateAnim8 = crate8.GetComponent<Animator>();
        crateAnim9 = crate9.GetComponent<Animator>();
        crateAnim10 = crate10.GetComponent<Animator>();
        crateAnimators = new Animator[] { crateAnim4, crateAnim3, crateAnim2, crateAnim5, crateAnim1, crateAnim6, crateAnim7, crateAnim8, crateAnim9, crateAnim10 };
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
        GameManager.GameManagerInstance.correctCodeCounter++;
        GameManager.GameManagerInstance.SaveProgress();
        crateAnimators[index].SetBool("open", true);
        GameManager.GameManagerInstance.SetArrayUnlocked("records", index, 1);
        GameManager.GameManagerInstance.SaveProgress();
    }
}
