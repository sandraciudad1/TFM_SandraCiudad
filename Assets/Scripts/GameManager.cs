using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance { get; private set; }

    public int objectIndex;
    public int recordIndex;
    public int[] objectsUnlocked = new int[15];
    public int[] recordsUnlocked = new int[10];

    public int samplesCounter;
    public int[] samplesUnlocked = new int[4];

    public int correctCodeCounter;

    // Ensures only one instance of GameManager exists (Singleton pattern).
    private void Awake()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Loads saved progress when the game starts.
    private void Start()
    {
        LoadProgress();
    }

    // Saves game progress to PlayerPrefs.
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("objectIndex", objectIndex);
        PlayerPrefs.SetInt("recordIndex", recordIndex);

        for (int i = 0; i < objectsUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("objectsUnlocked" + i, objectsUnlocked[i]);
        }

        for (int i = 0; i < recordsUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("recordsUnlocked" + i, recordsUnlocked[i]);
        }

        PlayerPrefs.SetInt("samplesCounter", samplesCounter);

        for (int i = 0; i < samplesUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("samplesUnlocked" + i, samplesUnlocked[i]);
        }

        PlayerPrefs.SetInt("correctCodeCounter", correctCodeCounter);

        PlayerPrefs.Save();
    }

    // Loads game progress from PlayerPrefs.
    public void LoadProgress()
    {
        objectIndex = PlayerPrefs.GetInt("objectIndex", 0);
        recordIndex = PlayerPrefs.GetInt("recordIndex", 0);

        objectsUnlocked = new int[15];
        recordsUnlocked = new int[10];

        for (int i = 0; i < objectsUnlocked.Length; i++)
        {
            objectsUnlocked[i] = PlayerPrefs.GetInt("objectsUnlocked" + i, 0);
        }

        for (int i = 0; i < recordsUnlocked.Length; i++)
        {
            recordsUnlocked[i] = PlayerPrefs.GetInt("recordsUnlocked" + i, 0);
        }

        samplesCounter = PlayerPrefs.GetInt("samplesCounter", 0);

        for (int i = 0; i < samplesUnlocked.Length; i++)
        {
            samplesUnlocked[i] = PlayerPrefs.GetInt("samplesUnlocked" + i, 0);
        }

        correctCodeCounter = PlayerPrefs.GetInt("correctCodeCounter", 0);
    }

    // Returns the value of the specified array at the given index.
    public int GetArrayUnlocked(string arrayName, int index)
    {
        int[] array = checkArrayType(arrayName);

        if (index >= 0 && index < array.Length)
        {
            return array[index];
        }
        return 0; 
    }

    // Sets the value of the specified array at the given index and saves it.
    public void SetArrayUnlocked(string arrayName, int index, int value)
    {
        int[] array = checkArrayType(arrayName);

        if (array != null && index >= 0 && index < array.Length)
        {
            array[index] = value;
            PlayerPrefs.SetInt((arrayName + "Unlocked" + index), value);
            PlayerPrefs.Save(); 
        }
    }

    // Returns the appropriate array based on the array name.
    int[] checkArrayType(string arrayName)
    {
        int[] array = null;
        if (arrayName.Equals("objects"))
        {
            array = objectsUnlocked;
        }
        else if (arrayName.Equals("records"))
        {
            array = recordsUnlocked;
        } else if (arrayName.Equals("samples"))
        {
            array = samplesUnlocked;
        }
        return array;
    }
}
