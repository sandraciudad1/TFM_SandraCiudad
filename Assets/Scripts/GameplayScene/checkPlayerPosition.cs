using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPlayerPosition : MonoBehaviour
{
    [SerializeField] GameObject player;
    CharacterController cc;
    PlayerMovement playerMov;

    Vector3 observationPos = new Vector3(161.99f, 25.677f, 52.9f);
    Vector3 laboratoryPos = new Vector3(133.775f, 30.688f, 66.52f);
    Vector3 alarmsPos = new Vector3(103.41f, 30.688f, 42.06f);
    Vector3 storePos = new Vector3(124.65f, 25.681f, 42.036f);
    Vector3 technologyPos = new Vector3(131.222f, 20.68f, 59.54f);
    Vector3[] playerPositions;

    Quaternion observationRot = Quaternion.Euler(new Vector3(0f, -100f, 0f));
    Quaternion laboratoryRot = Quaternion.Euler(new Vector3(0f, -90f, 0f));
    Quaternion alarmsRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    Quaternion storeRot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    Quaternion technologyRot = Quaternion.Euler(new Vector3(0f, -75f, 0f));
    Quaternion[] playerRotations;

    [SerializeField] GameObject labTrigger;
    [SerializeField] GameObject alarmsTrigger;
    [SerializeField] GameObject storeTrigger;
    [SerializeField] GameObject techTrigger;
    GameObject[] triggers;
    static int triggerAmount;

    // scripts
    MonoBehaviour mission1, mission2, mission3, mission4, mission5, mission6, mission7, mission8, mission9, mission10;
    MonoBehaviour camera18, fpDetector, puzzleCont, vacuumDetector;
    MonoBehaviour[] scriptControllers;

    // objects
    [SerializeField] GameObject mission1Obj;
    [SerializeField] GameObject vcam18;
    [SerializeField] GameObject spotLight;
    [SerializeField] GameObject puzzle;
    [SerializeField] GameObject vacuum;

    // Initializes components and sets player position from saved data.
    void Start()
    {
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        
        playerPositions = new Vector3[] { observationPos, laboratoryPos, alarmsPos, storePos, technologyPos };
        playerRotations = new Quaternion[] { observationRot, laboratoryRot, alarmsRot, storeRot, technologyRot };
        triggers = new GameObject[] { labTrigger, alarmsTrigger, storeTrigger, techTrigger };
        initializeScripts();

        GameManager.GameManagerInstance.LoadProgress();
        var gm = GameManager.GameManagerInstance;
        int index = gm.triggerPassed;
        triggerAmount = index;
        setTriggerState(index);
        setPlayerPosRot(index);
        manageScripts();
    }

    void initializeScripts()
    {
        mission1 = mission1Obj.GetComponent<mission1Controller>();
        mission2 = GetComponent<mission2Controller>();
        mission3 = GetComponent<mission3Controller>();
        mission4 = GetComponent<mission4Controller>();
        mission5 = GetComponent<mission5Controller>();
        mission6 = GetComponent<mission6Controller>();
        mission7 = GetComponent<mission7Controller>();
        mission8 = GetComponent<mission8Controller>();
        mission9 = GetComponent<mission9Controller>();
        mission10 = GetComponent<mission10Controller>();
        camera18 = vcam18.GetComponent<camera18Controller>();
        fpDetector = spotLight.GetComponent<fingerprintDetector>();
        puzzleCont = puzzle.GetComponent<puzzleController>();
        vacuumDetector = vacuum.GetComponent<vacuumTriggerDetector>();
        scriptControllers = new MonoBehaviour[] { mission1, mission2, mission3, mission4, mission5, mission6, mission7, mission8,
                                                  mission9, mission10, camera18, fpDetector, puzzleCont, vacuumDetector }; 
    }

    // Sets player position and rotation, disables movement during transition.
    void setPlayerPosRot(int index)
    {
        playerMov.canMove = false;
        cc.enabled = false;
        player.transform.position = playerPositions[index];
        player.transform.rotation = playerRotations[index];
        if (Vector3.Distance(player.transform.position, playerPositions[index]) < 0.01f)
        {
            playerMov.canMove = true;
            cc.enabled = true;
        }
    }

    // Handles trigger activation and updates saved progress.
    private void OnTriggerEnter(Collider other)
    {
        if(triggerAmount >= 4) return;

        if (other.gameObject.CompareTag("position"))
        {
            var gm = GameManager.GameManagerInstance;
            gm.LoadProgress();
            gm.triggerPassed++;
            triggerAmount++;
            gm.SaveProgress();
            other.gameObject.SetActive(false);
            manageScripts();
        }
    }

    void manageScripts()
    {
        for(int i = 0; i < scriptControllers.Length; i++)
        {
            scriptControllers[i].enabled = false;
        }

        if (triggerAmount == 0) 
        {
            mission1.enabled = true;
        } 
        else if (triggerAmount == 1)
        {
            mission2.enabled = true;
            mission3.enabled = true;
            mission4.enabled = true;
        } 
        else if (triggerAmount == 2)
        {
            mission5.enabled = true;
        } 
        else if (triggerAmount == 3)
        {
            mission6.enabled = true;
            mission7.enabled = true;
        }
        else if (triggerAmount == 4)
        {
            mission8.enabled = true;
            vacuumDetector.enabled = true;
            mission9.enabled = true;
            puzzleCont.enabled = true;
            mission10.enabled = true;
            camera18.enabled = true;
            fpDetector.enabled = true;
        }
    }

    // Disables triggers the player has already passed.
    void setTriggerState(int index)
    {
        for(int i = 0; i < index; i++)
        {
            triggers[i].SetActive(false);
        }
    }
}
