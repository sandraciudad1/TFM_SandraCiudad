using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

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
    [SerializeField] GameObject clipboard;
    [SerializeField] GameObject emergencyKit;
    [SerializeField] GameObject vacuum;
    [SerializeField] GameObject compass;
    [SerializeField] GameObject uvLight;
    [SerializeField] GameObject tape;
    GameObject[] inventoryObjects = new GameObject[15];

    // records
    [SerializeField] GameObject record5;
    [SerializeField] GameObject record3;
    [SerializeField] GameObject record2;
    [SerializeField] GameObject record1;
    [SerializeField] GameObject record4;
    [SerializeField] GameObject record6;
    [SerializeField] GameObject record7;
    [SerializeField] GameObject record8;
    [SerializeField] GameObject record9;
    [SerializeField] GameObject record10;

    // crates
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
    Animator crate1Anim;
    Animator crate2Anim;
    Animator crate3Anim;
    Animator crate4Anim;
    Animator crate5Anim;
    Animator crate6Anim;
    Animator crate7Anim;
    Animator crate8Anim;
    Animator crate9Anim;
    Animator crate10Anim;

    // doors
    [SerializeField] GameObject metallicDoor;
    Animator metallicDoorAnim;
    [SerializeField] GameObject observationDoor;
    Animator observationDoorAnim;
    [SerializeField] GameObject labDoor;
    Animator labDoorAnim;
    [SerializeField] GameObject verticalExitDoor;
    Animator verticalExitDoorAnim;
    [SerializeField] GameObject scifiCrate;
    Animator scifiCrateAnim;
    [SerializeField] GameObject verticalDoor;
    Animator verticalDoorAnim;

    int[] missionsCompleted;
    int[] objectsUnlocked;
    int[] recordsUnlocked;

    // inventory
    [SerializeField] GameObject inventory;

    // other objects
    [SerializeField] GameObject smoke1;
    [SerializeField] GameObject smoke2;
    [SerializeField] GameObject smoke3;
    [SerializeField] GameObject smoke4;
    [SerializeField] GameObject screen8;
    [SerializeField] GameObject navigationScreen;
    [SerializeField] GameObject waitingVideo;
    [SerializeField] VideoPlayer unknownSamples;

    [SerializeField] GameObject kit;
    Animator kitAnim;
    [SerializeField] GameObject BandAidRoll;
    [SerializeField] GameObject MedicalPackage;
    [SerializeField] GameObject HydroCream;
    [SerializeField] GameObject MiniAidBox;
    [SerializeField] GameObject OxyWater;
    [SerializeField] GameObject BurnCream;
    [SerializeField] GameObject Scissor;
    [SerializeField] GameObject Alcohol;
    GameObject[] firstAidKit;
    [SerializeField] GameObject BandAidRollObj;
    [SerializeField] GameObject MedicalPackageObj;
    [SerializeField] GameObject HydroCreamObj;
    [SerializeField] GameObject MiniAidBoxObj;
    [SerializeField] GameObject OxyWaterObj;
    [SerializeField] GameObject BurnCreamObj;
    [SerializeField] GameObject ScissorObj;
    [SerializeField] GameObject AlcoholObj;
    GameObject[] firstAidObj;

    // codes
    [SerializeField] VideoPlayer code1;
    [SerializeField] VideoPlayer code2;
    [SerializeField] GameObject code3;
    [SerializeField] GameObject code4;
    [SerializeField] GameObject screenCode8;
    

    GameObject[] inventoryRecords = new GameObject[10];

    [SerializeField] GameObject playerTrigger;
    mission5Controller mision5;

    [SerializeField] TextMeshProUGUI inputText1;
    [SerializeField] TextMeshProUGUI inputText2;
    [SerializeField] TextMeshProUGUI inputText3;
    
    // Initializes inventory and updates unlocked objects and records.
    void Start()
    {
        mision5 = playerTrigger.GetComponent<mission5Controller>();
        inventoryObjects = new GameObject[] { crowbar, sample1, sample2, sample3, sample4, spannerwrench, securityCard, wireCutters, clipboard, 
                                              emergencyKit, vacuum, compass, uvLight, tape, null };
        GameManager.GameManagerInstance.LoadProgress();
        missionsCompleted = GameManager.GameManagerInstance.missionsCompleted;
        objectsUnlocked = GameManager.GameManagerInstance.objectsUnlocked;
        recordsUnlocked = GameManager.GameManagerInstance.recordsUnlocked;

        firstAidKit = new GameObject[] { kit, BandAidRoll, MedicalPackage, HydroCream, MiniAidBox, OxyWater, BurnCream, Scissor, Alcohol };
        firstAidObj = new GameObject[] { BandAidRollObj, MedicalPackageObj, HydroCreamObj, MiniAidBoxObj, OxyWaterObj, BurnCreamObj, ScissorObj, AlcoholObj };
        initializeAnimators();

        for (int i = 0; i < GameManager.GameManagerInstance.objectIndex; i++)
        {
            if (missionsCompleted[i] == 1)
            {
                resetMissionsStates(i);
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
        crate5Anim = crate5.GetComponent<Animator>();
        crate6Anim = crate6.GetComponent<Animator>();
        crate7Anim = crate7.GetComponent<Animator>();
        crate8Anim = crate8.GetComponent<Animator>();
        crate9Anim = crate9.GetComponent<Animator>();
        crate10Anim = crate10.GetComponent<Animator>();

        // doors Animators
        metallicDoorAnim = metallicDoor.GetComponent<Animator>();
        observationDoorAnim = observationDoor.GetComponent<Animator>();
        labDoorAnim = labDoor.GetComponent<Animator>();
        verticalExitDoorAnim = verticalExitDoor.GetComponent<Animator>();
        scifiCrateAnim = scifiCrate.GetComponent<Animator>();
        verticalDoorAnim = verticalDoor.GetComponent<Animator>();
        kitAnim = kit.GetComponent<Animator>();
    }

    
    void additionalActions(int index)
    {
        switch (index)
        {
            case 8: // tabla de traduccion 
                
                break;
            case 9: // kit de emergencia
                break;
            case 10: // aspiradora
                GameObject[] dirtObjects = GameObject.FindGameObjectsWithTag("dirt");
                foreach (GameObject dirt in dirtObjects)
                {
                    dirt.SetActive(false);
                }
                screen8.SetActive(false);
                screenCode8.SetActive(true);
                navigationScreen.SetActive(true);
                break;
            case 11: // brujula
                break;
            case 12: // luz ultravioleta
                verticalExitDoorAnim.SetBool("open", true);
                scifiCrateAnim.SetBool("move", true);
                break;
            default:
                break;
        }
    }

    // Executes actions when an object is collected.
    void resetMissionsStates(int index)
    {
        switch (index)
        {
            case 0:
                crowbar.SetActive(false);
                code1.gameObject.SetActive(true);
                code1.Play();
                metallicDoorAnim.SetBool("open", true);
                observationDoorAnim.SetBool("open", true);
                Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
                foreach (Light light in lights)
                {
                    light.gameObject.SetActive(true);
                }
                break;
            case 1:
                sample1.SetActive(false);
                sample2.SetActive(false);
                sample3.SetActive(false);
                sample4.SetActive(false);
                navigationScreen.gameObject.SetActive(false);
                code2.gameObject.SetActive(true);
                code2.Play();
                unknownSamples.gameObject.SetActive(true);
                unknownSamples.Play();
                break;
            case 2:
                spannerwrench.SetActive(false);
                code3.SetActive(true);
                smoke1.SetActive(false);
                smoke2.SetActive(false);
                smoke3.SetActive(false);
                smoke4.SetActive(false);
                break;
            case 3:
                securityCard.SetActive(false);
                code4.SetActive(true);
                labDoorAnim.SetBool("open", true);
                mision5.initializeAlarms();
                break;
            case 4:
                wireCutters.SetActive(false);
                mision5.desactivateAlarms();
                mision5.showPinCode();
                break;
            case 5:
                clipboard.SetActive(false);
                inputText1.text = "C0D1GO";
                inputText2.text = "PU3RT4";
                inputText3.text = "5726";
                break;
            case 6:
                emergencyKit.SetActive(false);
                verticalDoorAnim.SetBool("open", true);
                for (int i = 0; i < firstAidKit.Length; i++)
                {
                    firstAidKit[i].SetActive(true);
                }
                for (int i = 0; i < firstAidObj.Length; i++)
                {
                    firstAidObj[i].SetActive(false);
                }
                kitAnim.SetBool("open", true);
                break;
            case 7:
                vacuum.SetActive(false);
                break;
            case 8:
                compass.SetActive(false);
                break;
            case 9:
                uvLight.SetActive(false);
                break;
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
                crate4Anim.SetBool("open", true);
                record1.SetActive(false);
                break;
            case 1:
                crate2Anim.SetBool("open", true);
                record2.SetActive(false);
                break;
            case 2:
                crate3Anim.SetBool("open", true);
                record3.SetActive(false);
                break;
            case 3:
                crate5Anim.SetBool("open", true);
                record4.SetActive(false);
                break;
            case 4:
                crate1Anim.SetBool("open", true);
                record5.SetActive(false);
                break;
            case 5:
                crate6Anim.SetBool("open", true);
                record6.SetActive(false);
                break;
            case 6:
                crate7Anim.SetBool("open", true);
                record7.SetActive(false);
                break;
            case 7:
                record8.SetActive(false);
                break;
            case 8:
                record9.SetActive(false);
                break;
            case 9:
                record10.SetActive(false);
                break;
            default:
                break;
        }
    }
}
