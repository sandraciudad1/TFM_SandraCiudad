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
    [SerializeField] GameObject securityCard;
    [SerializeField] GameObject wireCutters;
    GameObject[] inventoryObjects = new GameObject[15];

    // records
    [SerializeField] GameObject record5;
    [SerializeField] GameObject record3;
    [SerializeField] GameObject record2;
    [SerializeField] GameObject record1;

    // crates
    [SerializeField] GameObject crate1;
    [SerializeField] GameObject crate2;
    [SerializeField] GameObject crate3;
    [SerializeField] GameObject crate4;
    Animator crate1Anim;
    Animator crate2Anim;
    Animator crate3Anim;
    Animator crate4Anim;

    // doors
    [SerializeField] GameObject observationDoor;
    Animator observationDoorAnim;
    [SerializeField] GameObject labDoor;
    Animator labDoorAnim;

    int[] objectsUnlocked;
    int[] recordsUnlocked;

    // inventory
    [SerializeField] GameObject inventory;

    // other objects
    [SerializeField] GameObject code3;
    [SerializeField] GameObject smoke1;
    [SerializeField] GameObject smoke2;
    [SerializeField] GameObject smoke3;
    [SerializeField] GameObject smoke4;
    [SerializeField] GameObject code4;

    GameObject[] inventoryRecords = new GameObject[10];

    // Initializes inventory and updates unlocked objects and records.
    void Start()
    {

        inventoryObjects = new GameObject[] { crowbar, sample1, sample2, sample3, sample4, spannerwrench, securityCard, wireCutters, null, null, null, null, null, null, null };
        GameManager.GameManagerInstance.LoadProgress();
        objectsUnlocked = GameManager.GameManagerInstance.objectsUnlocked;
        

        recordsUnlocked = GameManager.GameManagerInstance.recordsUnlocked;
        initializeAnimators();

        for (int i = 0; i < GameManager.GameManagerInstance.objectIndex; i++)
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

    // Assigns Animator components to crates and doors.
    void initializeAnimators()
    {
        // crates Animators
        crate1Anim = crate1.GetComponent<Animator>();
        crate2Anim = crate2.GetComponent<Animator>();
        crate3Anim = crate3.GetComponent<Animator>();
        crate4Anim = crate4.GetComponent<Animator>();

        // doors Animators
        observationDoorAnim = observationDoor.GetComponent<Animator>();
        labDoorAnim = labDoor.GetComponent<Animator>();
    }

    // Executes actions when an object is collected.
    void additionalActions(int index)
    {
        switch (index)
        {
            case 0: // crowbar
                crate1Anim.SetBool("open", true);
                //activar el video del codigo
                observationDoorAnim.SetBool("open", true);
                Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
                foreach (Light light in lights)
                {
                    light.gameObject.SetActive(true);
                }
                break;
            case 1: //muestra1
                crate2Anim.SetBool("open", true);
                //activar el video del codigo
                break;
            case 2: //muestra 2
                
                break;
            case 3: // muestra 3

                break;
            case 4: // muestra 4

                break;
            case 5: // llave inglesa
                crate3Anim.SetBool("open", true);
                code3.SetActive(true);
                smoke1.SetActive(false);
                smoke2.SetActive(false);
                smoke3.SetActive(false);
                smoke4.SetActive(false);
                break;
            case 6: // tarjeta de seguridad
                crate4Anim.SetBool("open", true);
                code4.SetActive(true);
                labDoorAnim.SetBool("open", true);
                break;
            /*case 7:

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

    // Manages record visibility based on unlocked items.
    void checkRecordsIndex(int index)
    {
        switch (index)
        {
            case 0:
                record1.SetActive(false);
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
