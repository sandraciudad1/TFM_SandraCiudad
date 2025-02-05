using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem particles;

    float fadeDuration = 2f; 
    Renderer objectRenderer;
    Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public void collect()
    {
        
        /*particles.gameObject.SetActive(true);
        particles.Play();
        StartCoroutine(FadeOut());*/
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.2f);
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration; // Valor entre 0 y 1

            // Cambiar la transparencia
            float alpha = Mathf.Lerp(1f, 0f, progress);
            Color newColor = originalColor;
            newColor.a = alpha;
            objectRenderer.material.color = newColor;

            // Hacer más pequeño el objeto
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

            yield return null;
        }

        gameObject.SetActive(false);
    }

}
