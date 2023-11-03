using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject playScreen, optionScreen, controlScreen, creditScreen;
    public GameObject playButton, optionsButton, controlsButton, creditsButton, firstPlayButton, firstOptionsButton;

    private GameObject lastActiveScreen;

    private void Update()
    {
        // Check if the "Cancel" button is pressed to close active screens
        if (Input.GetButtonDown("Cancel"))
        {
            CloseAllScreens();
        }
    }

    public void CloseAllScreens()
    {
        // Close all screens and set the selected button based on the last active screen
        foreach (GameObject screen in new[] { playScreen, controlScreen, optionScreen, creditScreen })
        {
            if (screen.activeSelf)
            {
                screen.SetActive(false);
                lastActiveScreen = screen;
            }
        }

        EventSystem.current.SetSelectedGameObject(null);

        // Set the selected button based on the last active screen
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
        // Open the play screen and set the selected button
        playScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstPlayButton);
    }

    public void OpenControls()
    {
        // Open the control screen and set the selected button
        controlScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OpenOptions()
    {
        // Open the options screen and set the selected button
        optionScreen.SetActive (true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsButton);
    }

    public void OpenCredits()
    {
        // Open the credits screen and set the selected button
        creditScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
