using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetGame : MonoBehaviour
{
    void Start()
    {
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.objectIndex = 0;
        GameManager.GameManagerInstance.recordIndex = 0;
        for(int i=0; i< 15; i++)
        {
            GameManager.GameManagerInstance.objectsUnlocked[i] = 0;
        }
        for (int i = 0; i < 10; i++)
        {
            GameManager.GameManagerInstance.recordsUnlocked[i] = 0;
        }
        GameManager.GameManagerInstance.samplesCounter = 0;
        for (int i = 0; i < 4; i++)
        {
            GameManager.GameManagerInstance.samplesUnlocked[i] = 0;
        }
        GameManager.GameManagerInstance.correctCodeCounter = 0;

        for (int i = 0; i < 10; i++)
        {
            GameManager.GameManagerInstance.missionsCompleted[i] = 0;
        }
        for (int i = 0; i < 15; i++)
        {
            GameManager.GameManagerInstance.objectsCollected[i] = 0;
        }
        for (int i = 0; i < 10; i++)
        {
            GameManager.GameManagerInstance.recordsCollected[i] = 0;
        }
        GameManager.GameManagerInstance.SaveProgress();
    }
}
