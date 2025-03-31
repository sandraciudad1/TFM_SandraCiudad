using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerMovement playerMov;

    public float playerLife;
    public float playerEnergy;
    public float playerOxygen;

    float regenerationSpeed = 3f;
    bool change;

    [SerializeField] Image lifeBg;
    [SerializeField] AudioSource breatheAudio;
    [SerializeField] AudioSource lowOxygenAudio;
    bool[] oxygenAlerts;

    [SerializeField] PostProcessVolume postProcessVolume;
    [SerializeField] Transform cameraTransform;
    DepthOfField depthOfField;

    [SerializeField] Image energyIcon;
    [SerializeField] Sprite[] energy;
    [SerializeField] Image oxygenIcon;
    [SerializeField] Sprite[] oxygen;

    float lastLife = -1f;
    float lastBlurIntensity = -1f;
    int lastEnergyIconIndex = -1;
    int lastOxygenIconIndex = -1;

    // Initializes player stats and settings at game start.
    void Start()
    {
        playerLife = 100;
        playerEnergy = 100;
        playerOxygen = 100;
        change = false;

        playerMov = player.GetComponent<PlayerMovement>();
        oxygenAlerts = new bool[] { false, false, false, false };
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
            playerLife += regenerationSpeed * Time.deltaTime;
            playerLife = Mathf.Clamp(playerLife, 0, 100);
        }
        else if (playerLife == 0 && !change)
        {
            changeScene();
            change = true;
        }
        updateLifeTransparency();

        if (playerEnergy < 100)
        {
            if (!breatheAudio.isPlaying)
            {
                breatheAudio.Play();
            }
            playerEnergy += (regenerationSpeed / 2) * Time.deltaTime;
            playerEnergy = Mathf.Clamp(playerEnergy, 0, 100);
            updateIcons(playerEnergy, energyIcon, energy, ref lastEnergyIconIndex);
        }
        else if (playerEnergy == 100 && breatheAudio.isPlaying)
        {
            breatheAudio.Stop();
        }
        updatePlayerMovement();

        if (playerOxygen < 100)
        {
            playerOxygen += (regenerationSpeed / 2) * Time.deltaTime;
            playerOxygen = Mathf.Clamp(playerOxygen, 0, 100);
            checkOxygenAlerts();
            updateIcons(playerOxygen, oxygenIcon, oxygen, ref lastOxygenIconIndex);
        }
        else if (playerOxygen == 100)
        {
            ResetOxygenAlerts(); 
        }
        updateOxygenEffects();
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

    // Updates the icon based on the value range.
    void updateIcons(float value, Image image, Sprite[] spriteArray, ref int lastIndex)
    {
        int index = Mathf.FloorToInt(value / 25);
        index = Mathf.Clamp(index, 0, spriteArray.Length - 1);

        if (index != lastIndex)
        {
            image.sprite = spriteArray[index];
            lastIndex = index;
        }
    }

    // Adjusts screen transparency based on player life.
    void updateLifeTransparency()
    {
        if (Mathf.Approximately(playerLife, lastLife)) return;

        lastLife = playerLife;
        float alpha = 1f - (playerLife / 100f);
        Color color = lifeBg.color;
        color.a = alpha;
        lifeBg.color = color;
    }

    // Reduces player life when taking damage.
    public void takeDamage(float amount)
    {
        playerLife -= amount;
        playerLife = Mathf.Clamp(playerLife, 0, 100);
    }

    // Updates movement speed and breathing volume based on energy.
    void updatePlayerMovement()
    {
        playerMov.speed = Mathf.Lerp(1.0f, 5.0f, playerEnergy / 100f);
        playerMov.jumpHeight = Mathf.Lerp(0.25f, 1.0f, playerEnergy / 100f);
        breatheAudio.volume = Mathf.Lerp(1.0f, 0.0f, playerEnergy / 100f);
    }

    // Reduces player energy when used.
    public void useEnergy(float amount)
    {
        playerEnergy -= amount;
        playerEnergy = Mathf.Clamp(playerEnergy, 0, 100);
    }

    // Adjusts blur and camera shake based on oxygen level.
    void updateOxygenEffects()
    {
        if (depthOfField != null)
        {
            float blurIntensity = Mathf.Lerp(0.0f, 1.0f, (playerOxygen / 100f));

            // Solo actualiza si el valor ha cambiado de verdad
            if (!Mathf.Approximately(blurIntensity, lastBlurIntensity))
            {
                depthOfField.focusDistance.value = blurIntensity;
                lastBlurIntensity = blurIntensity;
            }
        }
    }

    // Triggers oxygen alerts when oxygen drops below thresholds.
    void checkOxygenAlerts()
    {
        if (playerOxygen <= 40)
        {
            int level = Mathf.FloorToInt((40 - playerOxygen) / 10);
            if (!oxygenAlerts[level])
            {
                if (!lowOxygenAudio.isPlaying)
                    lowOxygenAudio.Play();

                oxygenAlerts[level] = true;
            }
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
        playerOxygen -= amount;
        playerOxygen = Mathf.Clamp(playerOxygen, 0, 100);
    }
}