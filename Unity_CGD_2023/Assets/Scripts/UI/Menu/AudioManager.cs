using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    void Start()
    {
       if(PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        }
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            mixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }
    }
}
