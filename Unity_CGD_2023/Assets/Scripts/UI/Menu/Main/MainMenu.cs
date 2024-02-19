using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject menuScreen, settingsScreen, controlScreen, howtoplayScreen, creditScreen;
    public GameObject playButton, settingsButton, controlsButton, creditsButton, howtoplayButton, firstSettingsButton;

    private GameObject lastActiveScreen;

    private void Update()
    {
        // Check if the "Cancel" button is pressed to close active screens
        if (Input.GetButtonDown("Cancel"))
        {
            CloseAllScreens();
            menuScreen.SetActive(true);
        }
    }

    public void CloseAllScreens()
    {
        // Close all screens and set the selected button based on the last active screen
        foreach (GameObject screen in new[] { controlScreen, settingsScreen, creditScreen })
        {
            if (screen.activeSelf)
            {
                screen.SetActive(false);
                lastActiveScreen = screen;
            }
            else 
            {
                lastActiveScreen = menuScreen;
            }
        }

        EventSystem.current.SetSelectedGameObject(null);

        // Set the selected button based on the last active screen
        if (lastActiveScreen == menuScreen)
        {
            EventSystem.current.SetSelectedGameObject(playButton);
        }
        else if (lastActiveScreen == controlScreen)
        {
            EventSystem.current.SetSelectedGameObject(controlsButton);
        }
        else if (lastActiveScreen == settingsScreen)
        {
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }
        else if (lastActiveScreen == creditScreen)
        {
            EventSystem.current.SetSelectedGameObject(creditsButton);
        }
    }
    public void OpenControls()
    {
        // Open the control screen and set the selected button
        controlScreen.SetActive(true);
        menuScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OpenOptions()
    {
        // Open the options screen and set the selected button
        settingsScreen.SetActive (true);
        menuScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSettingsButton);
    }

    public void OpenCredits()
    {
        // Open the credits screen and set the selected button
        creditScreen.SetActive(true);
        menuScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
