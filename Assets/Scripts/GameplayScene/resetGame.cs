using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GameManagerInstance.LoadProgress();
        GameManager.GameManagerInstance.objectIndex = 0;
        GameManager.GameManagerInstance.recordIndex = 0;
        for(int i=0; i< GameManager.GameManagerInstance.objectsUnlocked.Length; i++)
        {
            GameManager.GameManagerInstance.objectsUnlocked[i] = 0;
        }
        for (int i = 0; i < GameManager.GameManagerInstance.recordsUnlocked.Length; i++)
        {
            GameManager.GameManagerInstance.recordsUnlocked[i] = 0;
        }
        GameManager.GameManagerInstance.SaveProgress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
