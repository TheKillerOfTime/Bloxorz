using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer audioMixer; // Arrastra aquí tu MainMixer

    // Métodos públicos para asignar desde el Slider en la UI

    public void SetMasterVolume(float sliderValue)
    {
        // Convertimos valor lineal (0 a 1) a Decibelios (-80 a 0)
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
}