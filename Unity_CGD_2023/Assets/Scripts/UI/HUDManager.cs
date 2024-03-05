using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    // Array to hold references to all player HUDs
    private GameObject[] playerHUDs;

    private void Start()
    {
        // Find all child objects with the name "P1 HUD", "P2 HUD", etc.
        playerHUDs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            playerHUDs[i] = transform.GetChild(i).gameObject;
        }

        // Disable all player HUDs initially
        DeactivateAllPlayerHUDs();
    }

    // Activate the HUD for the given player index
    public void ActivatePlayerHUD(int playerIndex)
    {
        // Ensure the index is within bounds
        if (playerIndex >= 0 && playerIndex < playerHUDs.Length)
        {
            playerHUDs[playerIndex].SetActive(true);

        }
        else
        {
            Debug.LogError("Player index out of bounds: " + playerIndex);
        }
    }

    // Deactivate all player HUDs
    private void DeactivateAllPlayerHUDs()
    {
        foreach (GameObject hud in playerHUDs)
        {
            hud.SetActive(false);
        }
    }

    // Assign player GameObjects to corresponding health bars
    public void AssignPlayersToHealthBars()
    {
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            HealthBar healthBar = playerHUDs[i].GetComponentInChildren<HealthBar>();
            if (healthBar != null)
            {
                healthBar.playerStats = playerManager.players[i].GetComponent<PlayerStats>();
            }
        }
    }

    //Archie - Assigning players to corresponding Weapon HUD
    public void AssignPlayersToWeaponHUD()
    {
        
        for (int i = 0; i < playerManager.players.Count; i++)
        {
            WeaponAmmoHUD ammoHUD = playerHUDs[i].GetComponentInChildren<WeaponAmmoHUD>();
            WeaponHUD imgHUD = playerHUDs[i].GetComponentInChildren<WeaponHUD>();
            if(imgHUD != null & ammoHUD != null)
            {
                imgHUD.character = playerManager.players[i].gameObject;
                ammoHUD.character = playerManager.players[i].gameObject;
            }
        }
    }
}

