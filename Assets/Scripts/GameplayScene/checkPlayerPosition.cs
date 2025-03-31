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

    // Initializes components and sets player position from saved data.
    void Start()
    {
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        
        playerPositions = new Vector3[] { observationPos, laboratoryPos, alarmsPos, storePos, technologyPos };
        playerRotations = new Quaternion[] { observationRot, laboratoryRot, alarmsRot, storeRot, technologyRot };
        triggers = new GameObject[] { labTrigger, alarmsTrigger, storeTrigger, techTrigger };
        
        GameManager.GameManagerInstance.LoadProgress();
        var gm = GameManager.GameManagerInstance;
        int index = gm.triggerPassed;
        setTriggerState(index);
        setPlayerPosRot(index);
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
        if (other.gameObject.CompareTag("position"))
        {
            var gm = GameManager.GameManagerInstance;
            gm.LoadProgress();
            gm.triggerPassed++;
            gm.SaveProgress();
            other.gameObject.SetActive(false);
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
