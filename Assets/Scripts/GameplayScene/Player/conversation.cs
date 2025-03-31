using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class conversation : MonoBehaviour
{
    [SerializeField] GameObject player;
    CharacterController cc;
    PlayerMovement playerMov;
    Vector3 playerPos = new Vector3(119.44f, 20.668f, 50.998f);
    Quaternion playerRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    bool change = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip vibration;
    [SerializeField] AudioClip notification;
    [SerializeField] AudioClip[] tierraInicial;
    [SerializeField] AudioClip[] p1;
    [SerializeField] AudioClip[] p2;
    [SerializeField] AudioClip[] p3;
    [SerializeField] AudioClip[] p4;
    [SerializeField] AudioClip[] t2;
    [SerializeField] AudioClip[] t3;
    [SerializeField] AudioClip[] t5;

    [SerializeField] TextMeshProUGUI option1;
    [SerializeField] TextMeshProUGUI option2;
    [SerializeField] TextMeshProUGUI option3;
    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject lockScreen;

    [SerializeField] VideoPlayer videoplayer;
    [SerializeField] RawImage rawImage;
    [SerializeField] VideoClip notificationVideo;
    [SerializeField] VideoClip callVideo;

    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam22;

    int etapa = 0;
    bool esperandoRespuesta = false;
    int respuestaElegida = -1;
    bool activate = false;
    bool played = false;
    float nextCheckTime = 0f;
    float checkInterval = 5f;

    [SerializeField] Image fadeImage;
    float fadeDuration = 2f;
    bool ending = false;

    // Initialize scene setup and player components.
    void Start()
    {
        SwapCameras(1, 0);
        cc = player.GetComponent<CharacterController>();
        playerMov = player.GetComponent<PlayerMovement>();
        audioSource.volume = 1f;
    }

    // Handle input and check game stage progress.
    void Update()
    {
        if (!played && Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;

            GameManager.GameManagerInstance.LoadProgress();
            played = GameManager.GameManagerInstance.recordsPlayed.All(x => x == 1);
        }

        if (esperandoRespuesta)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                respuestaElegida = 0;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                respuestaElegida = 1;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                respuestaElegida = 2;
            }

            if (respuestaElegida != -1)
            {
                esperandoRespuesta = false;
                ocultarOpciones();
                StartCoroutine(continuarConversacion());
            }
        }

        if (etapa >= 3 && !ending)
        {
            ending = true;
            StartCoroutine(fadeToBlack());
            StartCoroutine(waitUntilFade());
        }
    }

    // Gradually fade screen to black.
    public IEnumerator fadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    // Waits 2 seconds until fade screen  to black.
    IEnumerator waitUntilFade()
    {
        yield return new WaitForSeconds(2f);
        changeScene();
    }

    // Changes scene after 3 seconds.
    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("presentationScene");
    }

    // Unsubscribes from the scene load event.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start initial dialogue sequence with Earth.
    IEnumerator reproducirInicio()
    {
        yield return reproducirClip(tierraInicial[0]);
        videoplayer.gameObject.SetActive(false);
        mostrarOpcionesEtapa(0);
        esperandoRespuesta = true;
    }

    // Continue conversation based on player's response.
    IEnumerator continuarConversacion()
    {
        if (etapa == 0)
        {
            yield return reproducirClip(p1[respuestaElegida]); 
            yield return reproducirClip(t2[respuestaElegida]); 
        }
        else if (etapa == 1)
        {
            yield return reproducirClip(p2[respuestaElegida]); 
            yield return reproducirClip(t3[respuestaElegida]); 
            yield return reproducirClip(p3[respuestaElegida]); 
            yield return reproducirClip(tierraInicial[7]);
        }
        else if (etapa == 2)
        {
            yield return reproducirClip(p4[respuestaElegida]); 
            yield return reproducirClip(t5[respuestaElegida]); 
            yield break;
        }

        etapa++;
        respuestaElegida = -1;
        yield return new WaitForSeconds(1f);

        if (etapa < 3)
        {
            mostrarOpcionesEtapa(etapa);
            esperandoRespuesta = true;
        }
    }

    // Play audio clip and wait for it to finish.
    IEnumerator reproducirClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length + 0.15f);
    }

    // Display options based on current stage.
    void mostrarOpcionesEtapa(int etapa)
    {
        switch (etapa)
        {
            case 0:
                option1.text = "A. Soy el último que queda… creo.";
                option2.text = "B. No estoy seguro… Necesito asimilar todo lo que ha pasado.";
                option3.text = "C. Menos mal… pensé que estaba solo para siempre.";
                break;
            case 1:
                option1.text = "A. Cooper nos traicionó.";
                option2.text = "B. Establecimos contacto con algo no humano.";
                option3.text = "C. No hay rastro de mis compañeros.";
                break;
            case 2:
                option1.text = "A. Volveré a la Tierra.";
                option2.text = "B. Me quedo. Necesito entender por qué respondieron.";
                option3.text = "C. No puedo irme. Debo encontrar a los demás.";
                break;
                
        }

        option1.gameObject.SetActive(true);
        option2.gameObject.SetActive(true);
        option3.gameObject.SetActive(true);
    }

    // Hide all dialogue options.
    void ocultarOpciones()
    {
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        option3.gameObject.SetActive(false);
    }

    // Start conversation when player triggers collider.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("conversation") && played && !activate)
        {
            SwapCameras(0, 1);
            playerMov.canMove = false;
            cc.enabled = false;
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
            if (Vector3.Distance(player.transform.position, playerPos) < 0.01f && !change)
            {
                lockScreen.SetActive(false);
                playVideo(notificationVideo);
                StartCoroutine(waitFinishVideo());
                change = true;
            }
        }
    }

    // Wait for video and start first conversation.
    IEnumerator waitFinishVideo()
    {
        yield return new WaitForSeconds(3f);
        reproducirClip(notification);
        yield return new WaitForSeconds(4f);
        playVideo(callVideo);
        blackScreen.SetActive(true);
        StartCoroutine(reproducirInicio());
        activate = true;
    }

    // Set and prepare video for playback.
    void playVideo(VideoClip newClip)
    {
        rawImage.enabled = false;
        videoplayer.Stop();
        videoplayer.clip = newClip;
        videoplayer.Prepare();
        videoplayer.prepareCompleted += OnPrepared;
    }

    // Start video once it's prepared.
    void OnPrepared(VideoPlayer vp)
    {
        rawImage.enabled = true;
        vp.Play();
        vp.prepareCompleted -= OnPrepared;
    }

    // Swap between virtual cameras.
    void SwapCameras(int priority1, int priority2)
    {
        vcam1.Priority = priority1;
        vcam22.Priority = priority2;
    }
}
