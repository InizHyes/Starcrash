using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TitleScreen : MonoBehaviour
{
    public GameObject mainMenu, titleScreen;
    public GameObject firstButton;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);

        mainMenu.SetActive(true);

        titleScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
