using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vacuumTriggerDetector : MonoBehaviour
{
    [SerializeField] GameObject playerTrigger;
    GameObject[] dirtObjects;
    mission8Controller mission8;
    int activeDirtCount;
    bool finish = false;
    HashSet<GameObject> fadingDirtObjects = new HashSet<GameObject>();

    // Initializes mission8Controller from the playerTrigger component.
    void Start()
    {
        mission8 = playerTrigger.GetComponent<mission8Controller>();
        dirtObjects = GameObject.FindGameObjectsWithTag("dirt");
        activeDirtCount = dirtObjects.Length;
    }

    // Starts fading out dirt objects when staying in the trigger.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("dirt") && mission8.enableControl && !fadingDirtObjects.Contains(other.gameObject))
        {
            fadingDirtObjects.Add(other.gameObject);
            StartCoroutine(fadeOutDisable(other.gameObject));
        }
    }

    // Gradually fades out and disables the dirt object.
    IEnumerator fadeOutDisable(GameObject dirtObject)
    {
        Renderer renderer = dirtObject.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Color initialColor = renderer.material.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0); 
        float duration = 2f, elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            renderer.material.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        dirtObject.SetActive(false);

        fadingDirtObjects.Remove(dirtObject);

        activeDirtCount--;
        if (activeDirtCount == 0 && !finish)
        {
            mission8.finish = true;
            finish = true;
        }
    }
}
