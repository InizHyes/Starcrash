using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform StartingRoom;
    [SerializeField] private List<Transform> roomList;
    [SerializeField] private int roomsToSpawn;
    [SerializeField] private bool allowRoomRepetition = false;
    [SerializeField] private int minRoomsBeforeHard = 5; // Minimum number of rooms before harder levels start.

    private Vector3 lastRoomExit;
    private int playerProgress = 1; // Represents the player's progression level.

    // A list to keep track of spawned rooms.
    private List<Transform> spawnedRooms = new List<Transform>();
    private bool hardRoomsUnlocked = false;

    private void Start()
    {
        lastRoomExit = StartingRoom.Find("RoomExit").position;

        for (int i = 0; i < roomsToSpawn; i++)
        {
            RoomSpawner();
        }
    }

    private void RoomSpawner()
    {
        if (roomList.Count == 0)
        {
            Debug.LogError("No room prefabs assigned to roomList!");
            return;
        }

        // Adjust the difficulty level based on player progression.
        int difficultyLevel = Mathf.Max(1, playerProgress - minRoomsBeforeHard);

        if (difficultyLevel >= 2)
        {
            hardRoomsUnlocked = true;
        }

        // Select a room from the list based on the current difficulty level.
        List<Transform> roomListForDifficulty = roomList;

        if (hardRoomsUnlocked)
        {
            roomListForDifficulty = roomListHard;
        }

        // Select a room randomly based on room repetition settings.
        Transform roomListSelect = SelectRandomRoom(roomListForDifficulty);

        if (roomListSelect == null)
        {
            // Handle the case when no room can be selected (all rooms have been spawned).
            Debug.Log("All rooms have been spawned.");
            return;
        }

        Transform lastRoomTransform = RoomSpawner(roomListSelect, lastRoomExit);

        if (lastRoomTransform != null)
        {
            lastRoomExit = lastRoomTransform.Find("RoomExit").position;
            spawnedRooms.Add(roomListSelect);

            // Increase the player's progression.
            playerProgress++;
        }
    }

    private Transform SelectRandomRoom(List<Transform> roomListForDifficulty)
    {
        if (!allowRoomRepetition)
        {
            // Create a list of unspawned rooms for the current difficulty level.
            List<Transform> unspawnedRooms = new List<Transform>();

            foreach (Transform room in roomListForDifficulty)
            {
                if (!spawnedRooms.Contains(room))
                {
                    unspawnedRooms.Add(room);
                }
            }

            // Check if there are unspawned rooms left.
            if (unspawnedRooms.Count > 0)
            {
                // Select a random unspawned room from the list.
                int randomIndex = Random.Range(0, unspawnedRooms.Count);
                return unspawnedRooms[randomIndex];
            }
            else
            {
                // No unspawned rooms left for this difficulty level.
                return null;
            }
        }
        else if (roomListForDifficulty.Count > 0)
        {
            // Allow room repetition, so select a random room from the entire list.
            int randomIndex = Random.Range(0, roomListForDifficulty.Count);
            return roomListForDifficulty[randomIndex];
        }

        return null;
    }

    private Transform RoomSpawner(Transform roomPrefab, Vector3 spawnLocation)
    {
        Transform roomTransform = Instantiate(roomPrefab, spawnLocation, Quaternion.identity);
        return roomTransform;
    }

    // define the room lists for different difficulty levels in the Inspector.
    [SerializeField] private List<Transform> roomListHard;
}
