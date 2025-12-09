using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Necesario para manipular imágenes de UI

public class AudioManager : MonoBehaviour
{
    [Header("Referencias Mixer y UI")]
    public AudioMixer mainMixer;
    public GameObject settingsPanel;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Icono del Parlante")]
    // LA IMAGEN que está en el botón (el componente Image)
    public Image speakerIconImage;
    // El Sprite cuando hay sonido
    public Sprite soundOnSprite;
    // El Sprite cuando ESTÁ MUTEADO
    public Sprite soundOffSprite;


    // --- El Start y ToggleSettings siguen igual ---
    void Start()
    {
        // Aseguramos que empiece con el icono correcto al iniciar
        UpdateSpeakerIcon(masterSlider.value);
    }

    public void ToggleSettings()
    {
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);
    }
    // ----------------------------------------------


    public void SetMasterVolume(float sliderValue)
    {
        float db = Mathf.Log10(sliderValue) * 20;
        mainMixer.SetFloat("MasterVolume", db);

        // AQUÍ ESTÁ EL CAMBIO: Llamamos a la función que revisa el icono
        UpdateSpeakerIcon(sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float db = Mathf.Log10(sliderValue) * 20;
        mainMixer.SetFloat("MusicVolume", db);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float db = Mathf.Log10(sliderValue) * 20;
        mainMixer.SetFloat("SFXVolume", db);
    }

    // --- NUEVA FUNCIÓN PARA CAMBIAR EL ICONO ---
    private void UpdateSpeakerIcon(float value)
    {
        // Si el valor del slider está muy cerca del mínimo (0.0001)
        // Usamos un pequeño margen (0.0002) por seguridad con los decimales
        if (value <= 0.0002f)
        {
            // Si no está ya puesta la imagen de muteado, la ponemos
            if (speakerIconImage.sprite != soundOffSprite)
            {
                speakerIconImage.sprite = soundOffSprite;
            }
        }
        else
        {
            // Si el volumen subió y no está la imagen de sonido, la ponemos
            if (speakerIconImage.sprite != soundOnSprite)
            {
                speakerIconImage.sprite = soundOnSprite;
            }
        }
    }
}