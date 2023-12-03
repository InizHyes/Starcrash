using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, menuScreen, settingsScreen;
    public GameObject resumeButton, settingsButton, firstSettingsButton;

    private GameObject lastActiveScreen;

    [SerializeField]
    PlayerController playerController;

    private void Update()
    {
        playerController = FindObjectOfType<PlayerController>();
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
        foreach (GameObject screen in new[] { settingsScreen })
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
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else if (lastActiveScreen == settingsScreen)
        {
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }
    }

    public void OpenOptions()
    {
        // Open the options screen and set the selected button
        settingsScreen.SetActive(true);
        menuScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSettingsButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        playerController.playerinput.SwitchCurrentActionMap("PlayerControls");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
}