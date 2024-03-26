using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // Prefab of the boss to spawn
    public GameObject bossUI;
    public float delayBeforeSpawn = 5f; // Delay before the boss spawns after player enters the room

    private bool playerEnteredRoom = false; // Flag to track if player entered the room
    private bool bossSpawned = false; // Flag to track if boss has already been spawned

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") // Check if the entering collider is a player and boss hasn't been spawned yet
        {
            print("EnterdRoom");
            playerEnteredRoom = true;
            Invoke("SpawnBoss", delayBeforeSpawn); // Invoke the SpawnBoss method after delayBeforeSpawn seconds
            bossUI.SetActive(true);
            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();
            // Call LockDoors function after spawning enemy
            doorManager.LockDoors();
        }
    }

    private void SpawnBoss()
    {
        if (playerEnteredRoom && !bossSpawned) // Check if the player is still in the room and boss hasn't been spawned yet
        {
            bossPrefab.SetActive(true);
            bossSpawned = true; // Set bossSpawned flag to true
            gameObject.SetActive(false); // Disable the boss spawner to prevent multiple spawns
        }
    }
}
