using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
       if(PlayerPrefs.HasKey("masterVol"))
        {
            mixer.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
        }
        if (PlayerPrefs.HasKey("sfxVol"))
        {
            mixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));
        }
        if (PlayerPrefs.HasKey("musicVol"))
        {
            mixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        }
    }
}
