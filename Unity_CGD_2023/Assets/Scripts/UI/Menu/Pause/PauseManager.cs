using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu = transform.GetChild(0).gameObject;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);

    }

    public void Resume() 
    {
       pauseMenu.SetActive(false);
    }
}
