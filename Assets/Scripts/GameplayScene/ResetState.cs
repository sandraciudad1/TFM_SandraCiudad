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
    
    // doors
    [SerializeField] GameObject metallicDoor;
    [SerializeField] GameObject observationDoor;
    [SerializeField] GameObject labDoor;
    [SerializeField] GameObject verticalExitDoor;
    [SerializeField] GameObject scifiCrate;
    [SerializeField] GameObject verticalDoor;

    int[] missionsCompleted;
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

    [SerializeField] GameObject puzzle;
    [SerializeField] GameObject navigationScreen9;

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


    GameObject[] crates;
    Animator[] crateAnims;
    // Initializes inventory and updates unlocked objects and records.
    void Start()
    {
        crates = new GameObject[] { crate1, crate2, crate3, crate4, crate5, crate6, crate7, crate8, crate9, crate10, metallicDoor, observationDoor, labDoor, verticalExitDoor, scifiCrate, verticalDoor, kit };

        GameManager.GameManagerInstance.LoadProgress();
        var gm = GameManager.GameManagerInstance;
        mision5 = playerTrigger.GetComponent<mission5Controller>();
        inventoryObjects = new GameObject[] { crowbar, sample1, sample2, sample3, sample4, spannerwrench, securityCard, wireCutters, clipboard, 
                                              emergencyKit, vacuum, compass, uvLight, tape, null };
        missionsCompleted = gm.missionsCompleted;
        recordsUnlocked = gm.recordsUnlocked;

        firstAidKit = new GameObject[] { kit, BandAidRoll, MedicalPackage, HydroCream, MiniAidBox, OxyWater, BurnCream, Scissor, Alcohol };
        firstAidObj = new GameObject[] { BandAidRollObj, MedicalPackageObj, HydroCreamObj, MiniAidBoxObj, OxyWaterObj, BurnCreamObj, ScissorObj, AlcoholObj };
        initializeAnimators();

        StartCoroutine(initializeStates());
    }

    IEnumerator initializeStates()
    {
        for (int i = 0; i < 10; i++)
        {
            if (missionsCompleted[i] == 1)
            {
                resetMissionsStates(i);
                yield return null;
            }
        }

        for (int i = 0; i < recordsUnlocked.Length; i++)
        {
            if (recordsUnlocked[i] == 1)
            {
                checkRecordsIndex(i);
                yield return null;
            }
        }
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

    // Executes actions when an object is collected.
    void resetMissionsStates(int index)
    {
        Debug.Log("index misions " + index);
        switch (index)
        {
            case 0:
                crowbar.SetActive(false);
                code1.gameObject.SetActive(true);
                code1.Play();
                crateAnims[10].SetBool("open", true);
                crateAnims[11].SetBool("open", true);
                Light[] lights = FindObjectsOfType<Light>();
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
                crateAnims[12].SetBool("open", true);
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
                crateAnims[15].SetBool("open", true);
                for (int i = 0; i < firstAidKit.Length; i++)
                {
                    firstAidKit[i].SetActive(true);
                }
                for (int i = 0; i < firstAidObj.Length; i++)
                {
                    firstAidObj[i].SetActive(false);
                }
                crateAnims[16].SetBool("open", true);
                break;
            case 7:
                vacuum.SetActive(false);
                GameObject[] dirtObjects = GameObject.FindGameObjectsWithTag("dirt");
                foreach (GameObject dirt in dirtObjects)
                {
                    dirt.SetActive(false);
                }
                screen8.SetActive(false);
                screenCode8.SetActive(true);
                navigationScreen.SetActive(true);
                break;
            case 8:
                compass.SetActive(false);
                puzzle.SetActive(true);
                navigationScreen9.SetActive(false);
                break;
            case 9:
                uvLight.SetActive(false);
                crateAnims[13].SetBool("open", true);
                crateAnims[14].SetBool("move", true);
                break;
            default:
                break;
        }
    }

    // Manages record visibility based on unlocked items.
    void checkRecordsIndex(int index)
    {
        Debug.Log("index records " + index);
        switch (index)
        {
            case 0:
                crateAnims[3].SetBool("open", true);
                record1.SetActive(false);
                break;
            case 1:
                crateAnims[1].SetBool("open", true);
                record2.SetActive(false);
                break;
            case 2:
                crateAnims[2].SetBool("open", true);
                record3.SetActive(false);
                break;
            case 3:
                crateAnims[4].SetBool("open", true);
                record4.SetActive(false);
                break;
            case 4:
                crateAnims[0].SetBool("open", true);
                record5.SetActive(false);
                break;
            case 5:
                crateAnims[5].SetBool("open", true);
                record6.SetActive(false);
                break;
            case 6:
                crateAnims[6].SetBool("open", true);
                record7.SetActive(false);
                break;
            case 7:
                crateAnims[7].SetBool("open", true);
                record8.SetActive(false);
                break;
            case 8:
                crateAnims[8].SetBool("open", true);
                record9.SetActive(false);
                break;
            case 9:
                crateAnims[9].SetBool("open", true);
                record10.SetActive(false);
                break;
            default:
                break;
        }
    }
}
