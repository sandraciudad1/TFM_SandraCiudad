using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class conversation : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Audios de Tierra")]
    public AudioClip[] tierraInicial; // T1, T2-1, T2-2, T2-3, T3-1, T3-2, T3-3, T4, T5-1, T5-2, T5-3

    [Header("Respuestas del jugador")]
    public AudioClip[] p1; // P1-1, P1-2, P1-3
    public AudioClip[] p2; // P2-1, P2-2, P2-3
    public AudioClip[] p3; // P3-1, P3-2, P3-3
    public AudioClip[] p4; // P4-1, P4-2, P4-3

    [Header("Respuestas de Tierra según la elección")]
    public AudioClip[] t2; // T2-1, T2-2, T2-3
    public AudioClip[] t3; // T3-1, T3-2, T3-3
    public AudioClip[] t5; // T5-1, T5-2, T5-3

    [Header("UI")]
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;
    public GameObject blackScreen;

    private int etapa = 0;
    private bool esperandoRespuesta = false;
    private int respuestaElegida = -1;

    void Start()
    {
        StartCoroutine(ReproducirInicio());
    }

    void Update()
    {
        if (esperandoRespuesta)
        {
            if (Input.GetKeyDown(KeyCode.A))
                respuestaElegida = 0;
            else if (Input.GetKeyDown(KeyCode.B))
                respuestaElegida = 1;
            else if (Input.GetKeyDown(KeyCode.C))
                respuestaElegida = 2;

            if (respuestaElegida != -1)
            {
                esperandoRespuesta = false;
                OcultarOpciones();
                StartCoroutine(ContinuarConversacion());
            }
        }
    }

    IEnumerator ReproducirInicio()
    {
        yield return ReproducirClip(tierraInicial[0]); // T1
        MostrarOpcionesEtapa(0);
        esperandoRespuesta = true;
    }

    IEnumerator ContinuarConversacion()
    {
        if (etapa == 0)
        {
            yield return ReproducirClip(p1[respuestaElegida]); // P1-*
            yield return ReproducirClip(t2[respuestaElegida]); // T2-*
        }
        else if (etapa == 1)
        {
            yield return ReproducirClip(p2[respuestaElegida]); // P2-*
            yield return ReproducirClip(t3[respuestaElegida]); // T3-*
        }
        else if (etapa == 2)
        {
            yield return ReproducirClip(p3[respuestaElegida]); // P3-*
            yield return ReproducirClip(tierraInicial[7]);     // T4
        }
        else if (etapa == 3)
        {
            yield return ReproducirClip(p4[respuestaElegida]); // P4-*
            yield return ReproducirClip(t5[respuestaElegida]); // T5-*
            Debug.Log("Conversación finalizada.");
            yield break;
        }

        etapa++;
        respuestaElegida = -1;

        yield return new WaitForSeconds(1f);

        if (etapa < 4)
        {
            MostrarOpcionesEtapa(etapa);
            esperandoRespuesta = true;
        }
    }

    IEnumerator ReproducirClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
    }

    void MostrarOpcionesEtapa(int etapa)
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
                option1.text = "A. Cooper robó una muestra y saboteó la IA.";
                option2.text = "B. Solo los he visto en grabaciones. Responden a estímulos.";
                option3.text = "C. Todo quedó en silencio tras el temblor.";
                break;
            case 3:
                option1.text = "A. Volveré a la Tierra.";
                option2.text = "B. Me quedo. Necesito entender por qué respondieron.";
                option3.text = "C. No puedo irme. Debo encontrar a los demás.";
                break;
        }

        option1.gameObject.SetActive(true);
        option2.gameObject.SetActive(true);
        option3.gameObject.SetActive(true);
    }

    void OcultarOpciones()
    {
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        option3.gameObject.SetActive(false);
    }



    /*[SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip vibration;
    [SerializeField] AudioClip notification;
    [SerializeField] AudioClip[] dialogue1;
    [SerializeField] AudioClip[] dialogue2;
    [SerializeField] AudioClip[] dialogue3;

    bool notify = false;
    bool activate = false;
    [SerializeField] GameObject lockScreen;
    [SerializeField] VideoPlayer videoplayer;
    [SerializeField] RawImage rawImage;
    [SerializeField] VideoClip notificationVideo;
    [SerializeField] VideoClip callVideo;

    static int dialogue = 0;
    static int index = 0;
    bool enableDetection = false;
    bool updateValues = false;
    [SerializeField] GameObject blackScreen;
    [SerializeField] TextMeshProUGUI option1;
    [SerializeField] TextMeshProUGUI option2;
    [SerializeField] TextMeshProUGUI option3;

    // 
    void Start()
    {
        
    }
    static int option = 0;
    // 
    void Update()
    {
        if (enableDetection)
        {
            if (!updateValues)
            {
                updateOptions();
                enableOptions(true);
                updateValues = true;
            }
            
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                playAudio(dialogue1);
                option = 1;
            } 
            else if (Input.GetKeyDown(KeyCode.B))
            {
                playAudio(dialogue2);
                option = 2;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                playAudio(dialogue3);
                option = 3;
            }

            if (!audioSource.isPlaying)
            {
                if (option == 1)
                {
                    playAudio(dialogue1);
                } 
                else if (option == 2)
                {
                    playAudio(dialogue2);
                }
                else if (option == 3)
                {
                    playAudio(dialogue3);
                }
                enableDetection = false;
            }
            
        }

        GameManager.GameManagerInstance.LoadProgress();
        for(int i=0; i< 10; i++)
        {
            if (GameManager.GameManagerInstance.missionsCompleted[i] == 1 && !notify)
            {
                audioSource.clip = vibration;
                audioSource.Play();
                notify = true;
            }
        }
    }

    void updateOptions()
    {
        if (index == 0)
        {
            option1.text = "A. Soy el último que queda… creo.";
            option2.text = "B. No estoy seguro… Necesito asimilar todo lo que ha pasado.";
            option3.text = "C. Menos mal… pensé que estaba solo para siempre.";
        }
    }

    void enableOptions(bool value)
    {
        option1.gameObject.SetActive(value);
        option2.gameObject.SetActive(value);
        option3.gameObject.SetActive(value);
    }

    void playAudio(AudioClip[] dialogue)
    {
        index++;
        audioSource.clip = dialogue[index];
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("conversation") && notify && !activate)
        {
            lockScreen.SetActive(false);
            playVideo(notificationVideo);
            StartCoroutine(waitFinishVideo());
        }
    }

    IEnumerator waitFinishVideo()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = notification;
        audioSource.Play();
        yield return new WaitForSeconds(4f);
        playVideo(callVideo);
        StartCoroutine(initConversation());
        activate = true;
    }

    void playVideo(VideoClip newClip)
    {
        rawImage.enabled = false; 
        videoplayer.Stop(); 
        videoplayer.clip = newClip;
        videoplayer.Prepare(); 
        videoplayer.prepareCompleted += OnPrepared;
    }

    void OnPrepared(VideoPlayer vp)
    {
        rawImage.enabled = true; 
        vp.Play(); 
        vp.prepareCompleted -= OnPrepared; 
    }


    IEnumerator initConversation()
    {
        yield return new WaitForSeconds(1f);
        audioSource.clip = dialogue1[index];
        audioSource.Play();
        yield return new WaitForSeconds(4f);
        blackScreen.SetActive(true);
        enableDetection = true;
    }*/
}
