using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;
    public TMP_Text resolutionLabel;

    public TMP_Text masterNum, sfxNum, musicNum;
    public Slider masterSlider, sfxSlider, musicSlider;
    public AudioMixer mixer;


    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;

        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;

        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedRes = i;

                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);

            selectedRes = resolutions.Count - 1;

            UpdateResLabel();
        }

        float vol = 0f;
        mixer.GetFloat("masterVol", out vol);
        masterSlider.value = vol;
        mixer.GetFloat("sfxVol", out vol);
        sfxSlider.value = vol;
        mixer.GetFloat("musicVol", out vol);
        musicSlider.value = vol;

        masterNum.text = (masterSlider.value + 80).ToString();
        sfxNum.text = (sfxSlider.value + 80).ToString();
        musicNum.text = (musicSlider.value + 80).ToString();
    }

    void Update()
    {

    }

    public void ResLeft()
    {
        selectedRes--;
        if (selectedRes < 0)
        {
            selectedRes = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedRes++;
        if (selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedRes].horizontal.ToString() + " X " + resolutions[selectedRes].vertical.ToString();
    }

    public void ApplyGrphics()
    {
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVol()
    {
        masterNum.text = (masterSlider.value + 80).ToString();

        mixer.SetFloat("masterVol", masterSlider.value);
        PlayerPrefs.SetFloat("masterVol", masterSlider.value);
    }
    public void SetSFXVol()
    {
        sfxNum.text = (sfxSlider.value + 80).ToString();

        mixer.SetFloat("sfxVol", sfxSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
    }
    public void SetMusicVol()
    {
        musicNum.text = (musicSlider.value + 80).ToString();

        mixer.SetFloat("musicVol", musicSlider.value);
        PlayerPrefs.SetFloat("musicVol", musicSlider.value);
    }
}
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
