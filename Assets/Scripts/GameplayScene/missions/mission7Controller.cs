using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mission7Controller : MonoBehaviour
{
    [SerializeField] GameObject verticalDoor;
    Animator doorAnimator;
    AudioSource doorAudio;

    [SerializeField] GameObject bg;
    [SerializeField] TextMeshProUGUI inputText;
    string userInput = "";
    bool readCode = false;

    [SerializeField] GameObject letterX;

    void Start()
    {
        doorAnimator = verticalDoor.GetComponent<Animator>();
        doorAudio = verticalDoor.GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (readCode)
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
                    if(userInput == "5726")
                    {
                        doorAnimator.SetBool("open", true);
                        doorAudio.Play();
                        bg.SetActive(false);
                        readCode = false;
                    } 
                    else
                    {
                        userInput = "";
                    }
                }
            }
            inputText.text = userInput;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("kit"))
        {
            letterX.SetActive(true);
        }
    }

    //
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(false);
        }
        if (other.gameObject.CompareTag("kit"))
        {
            letterX.SetActive(false);
        }
    }

    // Detects continuous presence in a trigger area.  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("verticalDoor"))
        {
            bg.SetActive(true);
            readCode = true;            
        }
        if(other.gameObject.CompareTag("kit") && Input.GetKeyDown(KeyCode.X))
        {

        }
        
    }


}
