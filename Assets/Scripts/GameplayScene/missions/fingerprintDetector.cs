using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fingerprintDetector : MonoBehaviour
{
    [SerializeField] Image fingerprint1;
    [SerializeField] Image fingerprint2;
    [SerializeField] Image fingerprint3;
    Image[] fingerprints;

    [SerializeField] GameObject letterR1;
    [SerializeField] GameObject letterR2;
    [SerializeField] GameObject letterR3;
    GameObject[] lettersR;



    // 
    void Start()
    {
        fingerprints = new Image[] { fingerprint1, fingerprint2, fingerprint3 };
        lettersR = new GameObject[] { letterR1, letterR2, letterR3 };

    }

    // 
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("fingerprint"))
        {
            if (other.gameObject.name.Equals("fingerprint1"))
            {
                activateFingerprint(0);
            } 
            else if (other.gameObject.name.Equals("fingerprint2"))
            {
                activateFingerprint(1);
            }
            else if (other.gameObject.name.Equals("fingerprint3"))
            {
                activateFingerprint(2);
            }
        }
    }

    void activateFingerprint(int index)
    {
        Color color = fingerprints[index].color;
        color.a = 1f; 
        fingerprints[index].color = color;
        lettersR[index].SetActive(true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("press r");
        }
    }
}
