using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerMovement playerMov;

    public float playerLife;
    public float playerEnergy;
    public float playerOxygen;

    float regenerationSpeed = 3f;

    [SerializeField] Image lifeBg;
    [SerializeField] AudioSource breatheAudio;
    [SerializeField] AudioSource lowOxygenAudio;
    bool[] oxygenAlerts;

    [SerializeField] PostProcessVolume postProcessVolume;
    [SerializeField] Transform cameraTransform;
    DepthOfField depthOfField;
    Vector3 originalCameraPosition;

    // Initializes player stats and settings at game start.
    void Start()
    {
        playerLife = 100;
        playerEnergy = 100;
        playerOxygen = 100;

        playerMov = player.GetComponent<PlayerMovement>();
        oxygenAlerts = new bool[] { false, false, false, false };
        originalCameraPosition = cameraTransform.localPosition;
        if (postProcessVolume.profile.TryGetSettings(out depthOfField) && depthOfField != null)
        {
            depthOfField.active = true;
        }
    }

    // Updates player stats, oxygen, and movement each frame.
    void Update()
    {
        if (playerLife > 0 && playerLife < 100)
        {
            Debug.Log("aumentando la vida " + playerLife);
            playerLife += regenerationSpeed * Time.deltaTime;
            playerLife = Mathf.Clamp(playerLife, 0, 100);
        }
        else if (playerLife == 0)
        {
            // vuelve a la pantalla de inicio
        }
        updateLifeTransparency();

        if (playerEnergy < 100)
        {
            //Debug.Log("aumentando la energia");
            if (!breatheAudio.isPlaying)
            {
                breatheAudio.Play();
            }
            playerEnergy += (regenerationSpeed / 2) * Time.deltaTime;
            playerEnergy = Mathf.Clamp(playerEnergy, 0, 100);
        }
        else if (playerEnergy == 100 && breatheAudio.isPlaying)
        {
            breatheAudio.Stop();
        }
        updatePlayerMovement();

        if (playerOxygen < 100)
        {
            //Debug.Log("aumentando el oxigeno");
            playerOxygen += (regenerationSpeed / 2) * Time.deltaTime;
            playerOxygen = Mathf.Clamp(playerOxygen, 0, 100);
            checkOxygenAlerts();
        }
        else if (playerOxygen == 100)
        {
            ResetOxygenAlerts(); 
        }
        updateOxygenEffects();
    }

    // Adjusts screen transparency based on player life.
    void updateLifeTransparency()
    {
        //Debug.Log("actualizando transparencia");
        float alpha = 1f - (playerLife / 100f);
        Color color = lifeBg.color;
        color.a = alpha;
        lifeBg.color = color;
    }

    // Reduces player life when taking damage.
    public void takeDamage(float amount)
    {
        //Debug.Log("DAÑO " + amount);
        playerLife -= amount;
        playerLife = Mathf.Clamp(playerLife, 0, 100);
    }

    // Updates movement speed and breathing volume based on energy.
    void updatePlayerMovement()
    {
        //Debug.Log("actualizando velocidad movimiento");
        playerMov.speed = Mathf.Lerp(1.0f, 5.0f, playerEnergy / 100f);
        playerMov.jumpHeight = Mathf.Lerp(0.25f, 1.0f, playerEnergy / 100f);
        breatheAudio.volume = Mathf.Lerp(1.0f, 0.0f, playerEnergy / 100f);
    }

    // Reduces player energy when used.
    public void useEnergy(float amount)
    {
       // Debug.Log("ENERGIA");
        playerEnergy -= amount;
        playerEnergy = Mathf.Clamp(playerEnergy, 0, 100);
    }

    // Adjusts blur and camera shake based on oxygen level.
    void updateOxygenEffects()
    {
        //Debug.Log("aztualizando oxigeno");
        if (depthOfField != null)
        {
            float blurIntensity = Mathf.Lerp(0.0f, 2.0f, (playerOxygen / 100f));
            depthOfField.focusDistance.value = blurIntensity;
        }

        float cameraShakeAmount = Mathf.Lerp(0f, 0.1f, 1 - (playerOxygen / 100f));
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalCameraPosition + (Random.insideUnitSphere * cameraShakeAmount), Time.deltaTime * 5f);
    }

    // Triggers oxygen alerts when oxygen drops below thresholds.
    void checkOxygenAlerts()
    {
        if (playerOxygen <= 40 && !oxygenAlerts[0])
        {
            lowOxygenAudio.Play();
            oxygenAlerts[0] = true;
        }
        if (playerOxygen <= 30 && !oxygenAlerts[1])
        {
            lowOxygenAudio.Play();
            oxygenAlerts[1] = true;
        }
        if (playerOxygen <= 20 && !oxygenAlerts[2])
        {
            lowOxygenAudio.Play();
            oxygenAlerts[2] = true;
        }
        if (playerOxygen <= 10 && !oxygenAlerts[3])
        {
            lowOxygenAudio.Play();
            oxygenAlerts[3] = true;
        }
    }

    // Resets oxygen alerts when oxygen is fully restored.
    void ResetOxygenAlerts()
    {
        for (int i = 0; i < oxygenAlerts.Length; i++)
        {
            oxygenAlerts[i] = false;
        }
    }

    // Decreases oxygen level when used.
    public void wasteOxygen(float amount)
    {
        //Debug.Log("OXIGENO");
        playerOxygen -= amount;
        playerOxygen = Mathf.Clamp(playerOxygen, 0, 100);
    }
}