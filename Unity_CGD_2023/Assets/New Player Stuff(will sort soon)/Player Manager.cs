using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Dictionary<int, PlayerInput> players = new Dictionary<int, PlayerInput>();
    private List<PlayerStats> joinedPlayers = new List<PlayerStats>();
    public int nextPlayerID = 0;
    public List<Sprite> playerSprites;
    PauseMenu pauseMenu;
    HUDManager hudManager;
    GameManager gameManager;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        hudManager = FindObjectOfType<HUDManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(nextPlayerID, playerInput);
        hudManager.ActivatePlayerHUD(nextPlayerID);

        if (nextPlayerID < playerSprites.Count)
        {
            SpriteRenderer playerSpriteRenderer = playerInput.gameObject.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sprite = playerSprites[nextPlayerID];
            }
        }

        nextPlayerID++;

        hudManager.AssignPlayersToHealthBars(); // Call to assign players to health bars
        hudManager.AssignPlayersToWeaponHUD(); // Assigns players to corresponding weapon HUD
        gameManager.CheckPlayers();
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
