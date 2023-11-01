using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public GameObject playScreen, optionScreen, controlScreen, creditScreen;
    public GameObject playButton, optionsButton, controlsButton, creditsButton, firstPlayButton, firstOptionsButton;

    private GameObject lastActiveScreen;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            CloseAllScreens();
        }
    }

    public void CloseAllScreens()
    {
        foreach (GameObject screen in new[] { playScreen, controlScreen, optionScreen, creditScreen })
        {
            if (screen.activeSelf)
            {
                screen.SetActive(false);
                lastActiveScreen = screen;
            }
        }

        EventSystem.current.SetSelectedGameObject(null);

        if (lastActiveScreen == playScreen)
        {
            EventSystem.current.SetSelectedGameObject(playButton);
        }
        else if (lastActiveScreen == controlScreen)
        {
            EventSystem.current.SetSelectedGameObject(controlsButton);
        }
        else if (lastActiveScreen == optionScreen)
        {
            EventSystem.current.SetSelectedGameObject(optionsButton);
        }
        else if (lastActiveScreen == creditScreen)
        {
            EventSystem.current.SetSelectedGameObject(creditsButton);
        }
    }

    public void OpenPlay()
    {
        playScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstPlayButton);
    }

    public void OpenControls()
    {
        controlScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OpenOptions()
    {
        optionScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsButton);
    }

    public void OpenCredits()
    {
        creditScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}