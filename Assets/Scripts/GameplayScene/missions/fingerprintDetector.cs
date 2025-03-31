using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fingerprintDetector : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam18;
    camera18Controller cam18Controller;
    [SerializeField] GameObject playerTrigger;
    mission10Controller mission10;

    [SerializeField] GameObject letterR1;
    [SerializeField] GameObject letterR2;
    [SerializeField] GameObject letterR3;
    GameObject[] lettersR;

    [SerializeField] Image fingerprint1;
    [SerializeField] Image fingerprint2;
    [SerializeField] Image fingerprint3;
    Image[] fingerprints;

    // Initializes fingerprint and mission-related components.
    void Start()
    {
        cam18Controller = vcam18.GetComponent<camera18Controller>();
        mission10 = playerTrigger.GetComponent<mission10Controller>();

        lettersR = new GameObject[] { letterR1, letterR2, letterR3 };
        fingerprints = new Image[] { fingerprint1, fingerprint2, fingerprint3 };
    }

    // Detects fingerprint triggers and activates corresponding fingerprint.
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("fingerprint")) return;

        int index = -1;

        switch (other.name)
        {
            case "fingerprint1": index = 0; break;
            case "fingerprint2": index = 1; break;
            case "fingerprint3": index = 2; break;
        }

        if (index == -1) return;

        cam18Controller.startMovement = false;
        StartCoroutine(fadeToFullAlpha(fingerprints[index], index));
        if (Input.GetKeyDown(KeyCode.R))
        {
            mission10.analyzeFingerprint(index);
        }
    }

    // Gradually fades in the fingerprint image.
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
