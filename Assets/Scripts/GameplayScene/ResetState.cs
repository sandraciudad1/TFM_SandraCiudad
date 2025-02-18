using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : MonoBehaviour
{
    // objects
    [SerializeField] GameObject crowbar;

    // records
    [SerializeField] GameObject record5;


    // crates
    [SerializeField] GameObject crate1;
    Animator crate1Anim;

    // doors
    [SerializeField] GameObject observationDoor;
    Animator observationDoorAnim;

    int[] objectsUnlocked;
    int[] recordsUnlocked;

    // inventory
    [SerializeField] GameObject inventory;

    [SerializeField] GameObject crowbarImg;
    GameObject[] inventoryObjects;

    [SerializeField] GameObject record5Img;
    GameObject[] inventoryRecords;

    void Start()
    {
        GameManager.GameManagerInstance.LoadProgress();

        objectsUnlocked = GameManager.GameManagerInstance.objectsUnlocked;
        recordsUnlocked = GameManager.GameManagerInstance.recordsUnlocked;

        initializeAnimators();

        for (int i = 0; i < objectsUnlocked.Length; i++)
        {
            if (objectsUnlocked[i] == 1)
            {
                checkObjectsIndex(i);
            }
        }

        for (int i = 0; i < recordsUnlocked.Length; i++)
        {
            if (recordsUnlocked[i] == 1)
            {
                checkRecordsIndex(i);
            }
        }
    }

    void initializeAnimators()
    {
        // crates Animators
        crate1Anim = crate1.GetComponent<Animator>();

        // doors Animators
        observationDoorAnim = observationDoor.GetComponent<Animator>();
    }
    
    void checkObjectsIndex(int index)
    {
        switch (index)
        {
            case 0:
                crowbar.SetActive(false);
                crate1Anim.SetBool("open", true);
                observationDoorAnim.SetBool("open", true);
                break;
            /*case 1:
                
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
            case 12:

                break;
            case 13:

                break;
            case 14:

                break;*/
            default:
                break;
        }
    }


    void checkRecordsIndex(int index)
    {
        switch (index)
        {
            case 0:
                

                break;
            case 1:
                
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:
                record5.SetActive(false);
                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            default:
                break;
        }
    }

}
