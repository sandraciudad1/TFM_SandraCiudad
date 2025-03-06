using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : MonoBehaviour
{
    // objects
    [SerializeField] GameObject crowbar;
    [SerializeField] GameObject sample1;
    [SerializeField] GameObject sample2;
    [SerializeField] GameObject sample3;
    [SerializeField] GameObject sample4;
    [SerializeField] GameObject spannerwrench;
    GameObject[] inventoryObjects = new GameObject[15];

    // records
    [SerializeField] GameObject record5;
    [SerializeField] GameObject record3;
    [SerializeField] GameObject record2;

    // crates
    [SerializeField] GameObject crate1;
    [SerializeField] GameObject crate2;
    [SerializeField] GameObject crate3;
    Animator crate1Anim;
    Animator crate2Anim;
    Animator crate3Anim;

    // doors
    [SerializeField] GameObject observationDoor;
    Animator observationDoorAnim;

    int[] objectsUnlocked;
    int[] recordsUnlocked;

    // inventory
    [SerializeField] GameObject inventory;

    // other objects
    [SerializeField] GameObject smoke1;
    [SerializeField] GameObject smoke2;
    [SerializeField] GameObject smoke3;
    [SerializeField] GameObject smoke4;

    GameObject[] inventoryRecords = new GameObject[10];

    void Start()
    {

        inventoryObjects = new GameObject[] { crowbar, sample1, sample2, sample3, sample4, spannerwrench, null, null, null, null, null, null, null, null, null };
        GameManager.GameManagerInstance.LoadProgress();
        objectsUnlocked = GameManager.GameManagerInstance.objectsUnlocked;
        recordsUnlocked = GameManager.GameManagerInstance.recordsUnlocked;
        
        initializeAnimators();

        for (int i = 0; i < objectsUnlocked.Length; i++)
        {
            if (objectsUnlocked[i] == 1)
            {
                inventoryObjects[i].SetActive(false);
                additionalActions(i);
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
        crate2Anim = crate2.GetComponent<Animator>();
        crate3Anim = crate3.GetComponent<Animator>();

        // doors Animators
        observationDoorAnim = observationDoor.GetComponent<Animator>();
    }
    
    void additionalActions(int index)
    {
        switch (index)
        {
            case 0:
                crate1Anim.SetBool("open", true);
                observationDoorAnim.SetBool("open", true);
                Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
                foreach (Light light in lights)
                {
                    light.gameObject.SetActive(true);
                }
                break;
            case 1:
                crate2Anim.SetBool("open", true);
                break;
            case 2:
                crate3Anim.SetBool("open", true);
                smoke1.SetActive(false);
                smoke2.SetActive(false);
                smoke3.SetActive(false);
                smoke4.SetActive(false);
                break;
            /*case 3:

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
                record2.SetActive(false);
                break;
            case 2:
                record3.SetActive(false);
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
