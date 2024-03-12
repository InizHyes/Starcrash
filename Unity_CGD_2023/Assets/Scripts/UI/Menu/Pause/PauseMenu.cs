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
    PlayerManager playerManager;

    private void Update()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        // Check if the "Cancel" button is pressed to close active screens
        if (Input.GetButtonDown("Cancel"))
        {
            CloseAllScreens();
            menuScreen.SetActive(true);
        }
    }

    public void CloseAllScreens()
    {
        GameObject[] screens = { settingsScreen }; // Add more screens if needed

        foreach (GameObject screen in screens)
        {
            if (screen != null && screen.activeSelf)
            {
                screen.SetActive(false);
                lastActiveScreen = screen;
            }
        }

        if (lastActiveScreen == null)
        {
            lastActiveScreen = menuScreen;
        }

        EventSystem.current.SetSelectedGameObject(null);

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
