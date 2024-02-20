using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public enum ShowValueEnum
    {
        RepeatingSound,
        CertainSound,
        
    }

    public ShowValueEnum SelectMode = ShowValueEnum.RepeatingSound;


    [DrawIf("SelectMode", ShowValueEnum.RepeatingSound)]
    public bool randomizedOrder;

    [DrawIf("SelectMode", ShowValueEnum.RepeatingSound)]
    public bool randomizedPitch;

    [DrawIf("randomizedPitch", true)]
    public float minPitch;

    [DrawIf("randomizedPitch", true)]
    public float maxPitch;

    
    public ArrayOfSongs[] Sounds;


    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string actionName)
    {

        if (SelectMode == ShowValueEnum.RepeatingSound)
        {
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
            for (int i = 0; i < Sounds.Length; i++)
            {
                if (actionName == Sounds[i].action)
                {
                    audioSource.clip = Sounds[i].audiosource;
                }
            }
        }


        audioSource.Play();
    }


    [System.Serializable]
    public class ArrayOfSongs
    {
        public string action;
        public AudioClip audiosource;
    }


}
