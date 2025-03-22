using Cinemachine;
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

    [SerializeField] CinemachineVirtualCamera vcam18;
    camera18Controller cam18Controller;
    [SerializeField] GameObject playerTrigger;
    mission10Controller mission10;

    // 
    void Start()
    {
        fingerprints = new Image[] { fingerprint1, fingerprint2, fingerprint3 };
        lettersR = new GameObject[] { letterR1, letterR2, letterR3 };

        cam18Controller = vcam18.GetComponent<camera18Controller>();
        mission10 = playerTrigger.GetComponent<mission10Controller>();
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
        cam18Controller.startMovement = false;
        StartCoroutine(fadeToFullAlpha(fingerprints[index], index));
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            mission10.analyzeFingerprint(index);
        }
    }

    IEnumerator fadeToFullAlpha(Image image, int index)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        color.a = 0f; 
        image.color = color;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / 2f);
            image.color = color;
            yield return null; 
        }
        color.a = 1f;
        image.color = color;
        lettersR[index].SetActive(true);
    }
}
