using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportOnCollision : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats stats;
    // Define the destination positions for each teleporter
    public Vector3 Room1 = new Vector3(0f, 0f, 0f);
    public Vector3 Room2 = new Vector3(32f, 0f, 0f);
    public Vector3 Room3 = new Vector3(64f, 0f, 0f);
    public Vector3 Room4 = new Vector3(0f, 18f, 0f);
    public Vector3 Room5 = new Vector3(32f, 18f, 0f);
    public Vector3 Room6 = new Vector3(64f, 18f, 0f);
    public Vector3 Room7 = new Vector3(0f, 36f, 0f);
    public Vector3 Room8 = new Vector3(32f, 36f, 0f);
    public Vector3 Room9 = new Vector3(64f, 36f, 0f);

    private int reviveHealth = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TeleportAllPlayers();
            playerManager = FindObjectOfType<PlayerManager>();
            playerManager.GetComponent<PlayerInputManager>().enabled = false;
        }
    }

    private void TeleportAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            TeleportPlayer(player);
            ResetHealth(player);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        Vector3 currentPlayerPosition = player.transform.position;
        int roomIndex = GetRoomIndexFromScriptName();
        Vector3 roomDestination = GetRoomPositionByIndex(roomIndex);

        Debug.Log("Script name: " + gameObject.name);
        Debug.Log("Extracted room index: " + roomIndex);

        player.transform.position = roomDestination;
        Debug.Log("Player Teleported from: " + currentPlayerPosition + " to: " + roomDestination);
    }

    private void ResetHealth(GameObject player)
    {
        /*
         * Changed due to issues with reviving a player overriding the health change
         * Player downed scriped runs everyting in the Update() function (-_-) so player will be revived then have health set to 100 instead of 70
         * Doesn't make a difference anyway since if 70 is the max that can be healed then it may as well be the max health
         * reviveHealth int made to prevent hard-coding
         * -OC
         */

        stats = player.GetComponent<PlayerStats>();
        if (stats.health < reviveHealth)
        {
            stats.health = reviveHealth;
        }

        // -Revive player too- OC
        if (player.GetComponent<Down>().downed)
        {
            player.GetComponent<Down>().reviveTimer = 0;
        }

        Debug.Log(stats.health);
    }

    private int GetRoomIndexFromScriptName()
    {
        string scriptName = gameObject.name;
        int underscoreIndex = scriptName.LastIndexOf('_');

        if (underscoreIndex != -1 && underscoreIndex < scriptName.Length - 1)
        {
            string roomIndexString = scriptName.Substring(underscoreIndex + 1);

            if (roomIndexString.StartsWith("Room"))
            {
                roomIndexString = roomIndexString.Substring("Room".Length);
            }

            if (int.TryParse(roomIndexString, out int roomIndex))
            {
                return roomIndex;
            }
            else
            {
                Debug.LogError("Failed to parse room index from script name. RoomIndexString: " + roomIndexString);
            }
        }
        else
        {
            Debug.LogError("Invalid script name format. UnderscoreIndex: " + underscoreIndex + ", ScriptName: " + scriptName);
        }

        Debug.LogWarning("Script name does not follow the expected format. Defaulting to Room1.");
        return 5;
    }

    private Vector3 GetRoomPositionByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return Room1;
            case 2:
                return Room2;
            case 3:
                return Room3;
            case 4:
                return Room4;
            case 5:
                return Room5;
            case 6:
                return Room6;
            case 7:
                return Room7;
            case 8:
                return Room8;
            case 9:
                return Room9;
            default:
                return Room5;
        }
    }
}
