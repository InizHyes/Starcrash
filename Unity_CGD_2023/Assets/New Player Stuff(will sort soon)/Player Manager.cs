using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private Dictionary<int, PlayerInput> players = new Dictionary<int, PlayerInput>();
    private int nextPlayerID = 0;
    PauseMenu pauseMenu;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(nextPlayerID, playerInput);
        nextPlayerID++;
    }

    public void Pause()
    {
        foreach (var playerInput in players.Values)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        Debug.Log("Paused");
        pauseMenu.Pause();
    }

    public void Resume()
    {
        foreach (var playerInput in players.Values)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
        Debug.Log("Resumed");
        pauseMenu.ResumeGame();
    }
}
