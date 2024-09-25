using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    MouseLook mouseLook;
    public const float SENSITIVITY_RATIO = 10f;

    private void Awake()
    {
        mouseLook = GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>();
    }

    void Start()
    {
        // Preference for fullscreen
        int fullscreen = PlayerPrefs.GetInt("fullscreen", 1);
        SetFullscreen(fullscreen != 0 ? true : false);

        // Preference for volume
        float volume = PlayerPrefs.GetFloat("volume", 0f);
        volumeSlider.value = volume;
        SetVolume(volume);

        // Preference for sensitivity
        float sensitivity = PlayerPrefs.GetFloat("sensitivity", (sensitivitySlider.value * SENSITIVITY_RATIO) / 2f );
        sensitivitySlider.value = sensitivity / SENSITIVITY_RATIO;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivity * SENSITIVITY_RATIO);
    }

    public void SetSensitivityInGame(float sensitivity)
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivity * SENSITIVITY_RATIO);
        mouseLook.sensitivity = sensitivity * SENSITIVITY_RATIO;
    }
}
