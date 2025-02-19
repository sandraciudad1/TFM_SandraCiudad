using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pinCodeController : MonoBehaviour
{
    bool canCheck = false;
    string[] correctCodes;

    [SerializeField] TextMeshProUGUI displayText;
    string correctCode; 
    string userInput = "";

    [SerializeField] GameObject crate1;
    Animator crateAnim;

    void Start()
    {
        correctCodes = new string[] { "0000", "0000", "0000", "0000", "1235", "0000", "0000", "0000", "0000", "0000" };
        UpdateDisplay();

        crateAnim = crate1.GetComponent<Animator>();
    }

    void Update()
    {
        if (canCheck)
        {
            DetectKeyInput();
        }
    }

    private void DetectKeyInput()
    {
        // Detecta números del 0 al 9
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                AddNumber(i.ToString());
            }
        }

        // Detecta la tecla de borrar 
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteNumber();
        }

        // Detecta la tecla Enter para comprobar
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (userInput == correctCode)
            {
                Debug.Log("Clave Correcta!");
                displayText.color = Color.green;

                correctCodeAction();
            }
            else
            {
                Debug.Log("Clave Incorrecta!");
                displayText.color = Color.red;
            }
            canCheck = false;
        }
    }

    public void AddNumber(string number)
    {
        if (userInput.Length > 0)
        {
            userInput = userInput.Substring(0, userInput.Length - 1);
            UpdateDisplay();
        }
    }

    public void DeleteNumber()
    {
        if (userInput.Length > 0)
        {
            userInput = userInput.Substring(0, userInput.Length - 1);
            UpdateDisplay();
        }
    }


    private void UpdateDisplay()
    {
        displayText.text = userInput;
    }
    int index;
    public void checkCode(int id)
    {
        canCheck = true;
        userInput = ""; 
        displayText.color = Color.white;
        index = id - 1;
        correctCode = correctCodes[index].ToString();
        UpdateDisplay();
    }

    public void correctCodeAction()
    {
        switch (index)
        {
            case 4:
                /*crateAnim.SetBool("open", true);
                GameManager.GameManagerInstance.SetArrayUnlocked("records", 4, 1);
                GameManager.GameManagerInstance.SaveProgress();*/
                Debug.Log("aqui");
                break;
        }
        
    }
}
