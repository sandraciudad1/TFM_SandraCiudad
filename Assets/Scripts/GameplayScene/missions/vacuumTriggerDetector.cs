using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vacuumTriggerDetector : MonoBehaviour
{
    [SerializeField] GameObject playerTrigger;
    mission8Controller mission8;
    bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        mission8 = playerTrigger.GetComponent<mission8Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allDirtDisabled() && !finish)
        {
            mission8.finish = true;
            finish = true;
        }
        
    }

    bool allDirtDisabled()
    {
        GameObject[] dirtObjects = GameObject.FindGameObjectsWithTag("dirt");
        foreach (GameObject dirt in dirtObjects)
        {
            if (dirt.activeSelf)
            {
                return false; 
            }
        }
        return true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("dirt") && mission8.enableControl)
        {
            StartCoroutine(fadeOutDisable(other.gameObject));
        }
    }

    IEnumerator fadeOutDisable(GameObject dirtObject)
    {
        Renderer renderer = dirtObject.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Color initialColor = renderer.material.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0); 

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            renderer.material.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        dirtObject.SetActive(false);
    }
}
