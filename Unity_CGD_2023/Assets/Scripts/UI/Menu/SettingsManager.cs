using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public TMP_Text resolutionLabel;
    public TMP_Text frameRateLabel;
    public TMP_Text masterVolumeLabel;
    public TMP_Text sfxVolumeLabel;
    public TMP_Text musicVolumeLabel;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Resolution Options")]
    public List<ResolutionOption> resolutionOptions = new List<ResolutionOption>();
    private int selectedResolutionIndex;

    [Header("Frame Rate Options")]
    public List<FrameRateOption> frameRateOptions = new List<FrameRateOption>();
    private int selectedFrameRateIndex;

    private float cachedMasterVolume;
    private float cachedSfxVolume;
    private float cachedMusicVolume;

    private void Start()
    {
        // Load settings when the game starts
        LoadSettings();
    }

    public void ApplySettings()
    {
        ApplyResolution();
        ApplyScreenMode();
        ApplyVolumeSettings();
        SaveSettings();

        if (vsyncToggle.isOn)
        {
            Application.targetFrameRate = 60;
            frameRateLabel.text = "60";
            selectedFrameRateIndex = 1;
        }
        else
        {
            ApplyFrameRate();
        }
    }

    public void ResetToDefaults()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();

        // Reset cached volume settings
        cachedMasterVolume = 0f;
        cachedSfxVolume = 0f;
        cachedMusicVolume = 0f;

        // Update volume sliders and labels
        masterVolumeSlider.value = cachedMasterVolume;
        sfxVolumeSlider.value = cachedSfxVolume;
        musicVolumeSlider.value = cachedMusicVolume;
        UpdateVolumeLabels();
    }

    public void FrameRateLeft()
    {
        selectedFrameRateIndex--;
        if (selectedFrameRateIndex < 0)
        {
            selectedFrameRateIndex = 0;
        }
        UpdateFrameRateLabel();
    }

    public void FrameRateRight()
    {
        selectedFrameRateIndex++;
        if (selectedFrameRateIndex >= frameRateOptions.Count)
        {
            selectedFrameRateIndex = frameRateOptions.Count - 1;
        }
        UpdateFrameRateLabel();
    }

    public void ResLeft()
    {
        selectedResolutionIndex--;
        if (selectedResolutionIndex < 0)
        {
            selectedResolutionIndex = 0;
        }
        UpdateResolutionLabel();
    }

    public void ResRight()
    {
        selectedResolutionIndex++;
        if (selectedResolutionIndex >= resolutionOptions.Count)
        {
            selectedResolutionIndex = resolutionOptions.Count - 1;
        }
        UpdateResolutionLabel();
    }

    private void UpdateResolutionLabel()
    {
        resolutionLabel.text = resolutionOptions[selectedResolutionIndex].ToString();
    }

    private void UpdateFrameRateLabel()
    {
        frameRateLabel.text = frameRateOptions[selectedFrameRateIndex].ToString();
    }

    private void UpdateVolumeLabels()
    {

        masterVolumeSlider.value = cachedMasterVolume;
        sfxVolumeSlider.value = cachedSfxVolume;
        musicVolumeSlider.value = cachedMusicVolume;

        masterVolumeLabel.text = (masterVolumeSlider.value + 80).ToString();
        sfxVolumeLabel.text = (sfxVolumeSlider.value + 80).ToString();
        musicVolumeLabel.text = (musicVolumeSlider.value + 80).ToString();

    }

    public void LoadSettings()
    {
        // Load settings from PlayerPrefs
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        selectedResolutionIndex = PlayerPrefs.GetInt("SelectedResolution", 1);
        selectedFrameRateIndex = PlayerPrefs.GetInt("SelectedFrameRate", 1);
        cachedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0f);
        cachedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0f);
        cachedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);

        // Update UI elements with loaded settings
        UpdateResolutionLabel();
        UpdateFrameRateLabel();
        UpdateVolumeLabels();

        // Apply the loaded settings
        ApplyResolution();
        ApplyFrameRate();
        ApplyScreenMode();
        ApplyVolumeSettings();
    }

    public void SaveSettings()
    {
        // Save current settings to PlayerPrefs
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("SelectedResolution", selectedResolutionIndex);
        PlayerPrefs.SetInt("SelectedFrameRate", selectedFrameRateIndex);
        PlayerPrefs.SetFloat("MasterVolume", cachedMasterVolume);
        PlayerPrefs.SetFloat("SfxVolume", cachedSfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", cachedMusicVolume);
    }

    private void ApplyResolution()
    {
        ResolutionOption selectedResolution = resolutionOptions[selectedResolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
    }

    private void ApplyFrameRate()
    {
        FrameRateOption selectedFrameRate = frameRateOptions[selectedFrameRateIndex];
        Application.targetFrameRate = selectedFrameRate.frameRate;
    }

    private void ApplyScreenMode()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
    }

    private void ApplyVolumeSettings()
    {
        audioMixer.SetFloat("MasterVolume", cachedMasterVolume);
        audioMixer.SetFloat("SfxVolume", cachedSfxVolume);
        audioMixer.SetFloat("MusicVolume", cachedMusicVolume);
    }

    public void SetMasterVolume()
    {
        cachedMasterVolume = masterVolumeSlider.value;
        UpdateVolumeLabels();
        ApplyVolumeSettings();
    }

    public void SetSfxVolume()
    {
        cachedSfxVolume = sfxVolumeSlider.value;
        UpdateVolumeLabels();
        ApplyVolumeSettings();
    }

    public void SetMusicVolume()
    {
        cachedMusicVolume = musicVolumeSlider.value;
        UpdateVolumeLabels();
        ApplyVolumeSettings();
    }

    [System.Serializable]
    public class ResolutionOption
    {
        public int width;
        public int height;

        public override string ToString()
        {
            return $"{width} X {height}";
        }
    }

    [System.Serializable]
    public class FrameRateOption
    {
        public int frameRate;

        public override string ToString()
        {
            return frameRate.ToString();
        }
    }
}
