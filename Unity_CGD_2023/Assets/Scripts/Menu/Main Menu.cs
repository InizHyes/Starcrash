using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject playScreen;
    public GameObject optionScreen;
    public GameObject controlScreen;
    public GameObject creditScreen;
    public GameObject firstMenuButton, playFirstButton, playCloseButton, controlsFirstButton, controlsCloseButton, optionsFirstButton, optionsCloseButton, creditsFirstButton, creditsCloseButton;

    public void OpenPlay()
    {
        playScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(playFirstButton);
    }
    public void QuitPlay()
    {
        playScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(playCloseButton);
    }
    public void OpenControls()
    {
        controlScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void QuitControls()
    {
        controlScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(controlsCloseButton);
    }
    public void OpenOptions()
    {
        optionScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void QuitOptions()
    {
        optionScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }

    public void OpenCredits()
    {
        creditScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }
    public void QuitCredits()
    {
        creditScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(creditsCloseButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
