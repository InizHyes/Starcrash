using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform StartingRoom;
    [SerializeField] private List<Transform> roomList; // List of regular rooms
    [SerializeField] private List<Transform> finalRoomList; // List of final rooms
    [SerializeField] private int roomsToSpawn;
    [SerializeField] private bool allowRoomRepetition = false; // Allow the same room to be spawned multiple times
    [SerializeField] private int minRoomsBeforeHard = 5; // Minimum number of rooms before harder levels start
    [SerializeField] private int bossRoomFrequency = 5; // Spawn a boss room every 'bossRoomFrequency' regular rooms
    [SerializeField] private Transform bossRoomPrefab; // Assign the boss room prefab in the Inspector
    [SerializeField] private int minRoomsBeforeBoss = 3; // Minimum number of regular rooms required before a boss room can spawn

    private Vector3 lastRoomExit;
    private int playerProgress = 1;
    private List<Transform> spawnedRooms = new List<Transform>();
    private bool hardRoomsUnlocked = false;
    private int bossRoomCounter = 0;

    private void Start()
    {
        lastRoomExit = StartingRoom.Find("RoomExit").position;
        int regularRoomsBeforeBoss = minRoomsBeforeBoss > 0 ? minRoomsBeforeBoss : 1;

        for (int i = 0; i < roomsToSpawn; i++)
        {
            if (i >= roomsToSpawn - 1)
            {
                // Spawn the final room at the end
                SpawnFinalRoom();
            }
            else if (bossRoomCounter >= bossRoomFrequency && i >= regularRoomsBeforeBoss)
            {
                // Spawn a boss room based on the frequency
                SpawnBossRoom();
                bossRoomCounter = 0;
            }
            else
            {
                // Spawn a regular room
                RoomSpawner();
                bossRoomCounter++;
            }
        }
    }

    private void RoomSpawner()
    {
        if (roomList.Count == 0)
        {
            Debug.LogError("No room prefabs assigned to roomList!");
            return;
        }

        int difficultyLevel = Mathf.Max(1, playerProgress - minRoomsBeforeHard);

        if (difficultyLevel >= 2)
        {
            hardRoomsUnlocked = true;
        }

        Transform roomListSelect = SelectRandomRoom(hardRoomsUnlocked ? roomListHard : roomList);

        if (roomListSelect == null)
        {
            // All rooms have been spawned
            Debug.Log("All rooms have been spawned.");
            return;
        }

        Transform lastRoomTransform = RoomSpawner(roomListSelect, lastRoomExit);

        if (lastRoomTransform != null)
        {
            lastRoomExit = lastRoomTransform.Find("RoomExit").position;
            spawnedRooms.Add(roomListSelect);
            playerProgress++;
        }
    }

    private Transform SelectRandomRoom(List<Transform> roomListForDifficulty)
    {
        if (!allowRoomRepetition)
        {
            List<Transform> unspawnedRooms = new List<Transform>();

            foreach (Transform room in roomListForDifficulty)
            {
                if (!spawnedRooms.Contains(room))
                {
                    unspawnedRooms.Add(room);
                }
            }

            if (unspawnedRooms.Count > 0)
            {
                // Select a random unspawned room from the list
                int randomIndex = Random.Range(0, unspawnedRooms.Count);
                return unspawnedRooms[randomIndex];
            }
            else
            {
                // No unspawned rooms left for this difficulty level
                Debug.Log("No unspawned rooms left for this difficulty level.");
                return null;
            }
        }
        else if (roomListForDifficulty.Count > 0)
        {
            // Allow room repetition, so select a random room from the entire list
            int randomIndex = Random.Range(0, roomListForDifficulty.Count);
            return roomListForDifficulty[randomIndex];
        }

        return null;
    }

    private Transform RoomSpawner(Transform roomPrefab, Vector3 spawnLocation)
    {
        // Instantiate and spawn a room based on the provided roomPrefab and spawnLocation
        Transform roomTransform = Instantiate(roomPrefab, spawnLocation, Quaternion.identity);
        return roomTransform;
    }

    private void SpawnBossRoom()
    {
        if (bossRoomPrefab != null)
        {
            // Spawn a boss room using the provided bossRoomPrefab
            Transform bossRoomTransform = Instantiate(bossRoomPrefab, lastRoomExit, Quaternion.identity);
            lastRoomExit = bossRoomTransform.Find("RoomExit").position;
        }
        else
        {
            Debug.LogError("Boss room prefab is not assigned.");
        }
    }

    private void SpawnFinalRoom()
    {
        if (finalRoomList.Count > 0)
        {
            // Select a final room prefab from the finalRoomList
            int randomIndex = Random.Range(0, finalRoomList.Count);
            Transform finalRoomPrefab = finalRoomList[randomIndex];

            if (finalRoomPrefab != null)
            {
                // Spawn the selected final room using the finalRoomPrefab
                Transform finalRoomTransform = Instantiate(finalRoomPrefab, lastRoomExit, Quaternion.identity);
                lastRoomExit = finalRoomTransform.Find("RoomExit").position;
            }
            else
            {
                Debug.LogError("Final room prefab is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("No final room prefabs assigned to finalRoomList.");
        }
    }

    [SerializeField] private List<Transform> roomListHard; // List of hard rooms
}
