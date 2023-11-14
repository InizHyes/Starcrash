using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public SettingsManager settingsManager;

    private void Start()
    {
        // Load settings when the game starts
        settingsManager.LoadSettings();
    }

    public void ApplyGraphics()
    {
        // Apply graphics settings
        settingsManager.ApplySettings();
    }

    public void ResetToDefaults()
    {
        // Reset settings to defaults
        settingsManager.ResetToDefaults();
    }

    public void SetMasterVolume()
    {
        // Set the master volume
        settingsManager.SetMasterVolume();
    }

    public void SetSfxVolume()
    {
        // Set the sound effects volume
        settingsManager.SetSfxVolume();
    }

    public void SetMusicVolume()
    {
        // Set the music volume
        settingsManager.SetMusicVolume();
    }

    public void FrameRateLeft()
    {
        // Navigate to the previous frame rate option
        settingsManager.FrameRateLeft();
    }

    public void FrameRateRight()
    {
        // Navigate to the next frame rate option
        settingsManager.FrameRateRight();
    }

    public void ResLeft()
    {
        // Navigate to the previous resolution option
        settingsManager.ResLeft();
    }

    public void ResRight()
    {
        // Navigate to the next resolution option
        settingsManager.ResRight();
    }
}
