using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetGame : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("initGame", 1);

        int reset = PlayerPrefs.GetInt("reset", 0);
        if (reset == 1)
        {
            GameManager.GameManagerInstance.LoadProgress();
            var gm = GameManager.GameManagerInstance;
            gm.objectIndex = 0;
            gm.recordIndex = 0;
            for (int i = 0; i < 15; i++)
            {
                gm.objectsUnlocked[i] = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                gm.recordsUnlocked[i] = 0;
            }

            gm.samplesCounter = 0;
            for (int i = 0; i < 4; i++)
            {
                gm.samplesUnlocked[i] = 0;
            }

            gm.correctCodeCounter = 0;
            for (int i = 0; i < 10; i++)
            {
                gm.missionsCompleted[i] = 0;
            }
            for (int i = 0; i < 15; i++)
            {
                gm.objectsCollected[i] = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                gm.recordsCollected[i] = 0;
            }

            gm.triggerPassed = 0;
            for (int i = 0; i < 10; i++)
            {
                gm.recordsPlayed[i] = 0;
            }
            gm.SaveProgress();
            PlayerPrefs.SetInt("reset", 0);
        }
    }
}
