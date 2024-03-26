using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public enum ShowValueEnum
    {
        RepeatingSound,
        CertainSound,
        BackgroundMusic
        
    }

    public ShowValueEnum SelectMode = ShowValueEnum.RepeatingSound;

    public float volume = 1.0f;

    [DrawIf("SelectMode", ShowValueEnum.RepeatingSound)]
    public bool randomizedOrder;

    public bool randomizedPitch;

    [DrawIf("randomizedPitch", true)]
    public float minPitch;

    [DrawIf("randomizedPitch", true)]
    public float maxPitch;

    public bool playToCompletion = false; //feature to enable/disable overriding sound playing from a specific source
    
    public ArrayOfSongs[] Sounds;


    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }

    public void PlaySound(string actionName)
    {

        if (SelectMode == ShowValueEnum.RepeatingSound)
        {
            audioSource.loop = false;
            if (randomizedOrder)
            {
                audioSource.clip = Sounds[Random.Range(0, Sounds.Length)].audiosource;
            }
            else
            {
                audioSource.clip = Sounds[0].audiosource;
            }

            if (randomizedPitch)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);

            }
        }

        if (SelectMode == ShowValueEnum.CertainSound)
        {
            audioSource.loop = false;
            for (int i = 0; i < Sounds.Length; i++)
            {
                if (actionName == Sounds[i].action)
                {
                    audioSource.clip = Sounds[i].audiosource;
                }
            }

            if (randomizedPitch)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);

            }
        }

        if (SelectMode == ShowValueEnum.BackgroundMusic)
        {
            audioSource.loop = true;
            for (int i = 0; i < Sounds.Length; i++)
            {
                if (actionName == Sounds[i].action)
                {
                    audioSource.clip = Sounds[i].audiosource;
                }
            }
        }

        if(playToCompletion)//checks bool setting
        {
            audioSource.PlayOneShot(audioSource.clip);//plays clip to completion on trigger
        }
        else
        {
            audioSource.Play();//plays clip on trigger
        }
    }

    public void PlayReversed(string actionName)
    {
        audioSource.pitch = -1.0f;
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (actionName == Sounds[i].action)
            {
                audioSource.clip = Sounds[i].audiosource;
                audioSource.time = audioSource.clip.length - 0.01f;
                if (playToCompletion)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    audioSource.Play();
                }
            }
        }
    }

    public void StopSound()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }

    

    [System.Serializable]
    public class ArrayOfSongs
    {
        public string action;
        public AudioClip audiosource;
    }


}
