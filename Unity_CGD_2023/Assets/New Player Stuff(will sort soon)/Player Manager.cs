using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Dictionary<int, PlayerInput> players = new Dictionary<int, PlayerInput>();
    public int nextPlayerID = 0;
    PauseMenu pauseMenu;
    HUDManager hudManager;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        hudManager = FindObjectOfType<HUDManager>();
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(nextPlayerID, playerInput);
        hudManager.ActivatePlayerHUD(nextPlayerID);
        nextPlayerID++;

        hudManager.AssignPlayersToHealthBars(); // Call to assign players to health bars
        hudManager.AssignPlayersToWeaponHUD(); // Assigns players to corresponding weapon HUD
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
