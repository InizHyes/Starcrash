using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform StartingRoom;
    [SerializeField] private List<Transform> roomList;

    private Vector3 lastRoomExit;
    [SerializeField] private int roomsToSpawn;

    // Create a list to keep track of spawned rooms.
    private List<Transform> spawnedRooms = new List<Transform>();

    [SerializeField] private bool allowRoomRepetition = false; // Toggle for room repetition.

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

        Transform roomListSelect = roomList[Random.Range(0, roomList.Count)];

        // Check if the selected room has not already been spawned, or room repetition is allowed.
        if (!allowRoomRepetition && spawnedRooms.Contains(roomListSelect))
        {
            Debug.Log("Selected room already spawned. Skipping.");
        }
        else
        {
            Transform lastRoomTransform = RoomSpawner(roomListSelect, lastRoomExit);
            lastRoomExit = lastRoomTransform.Find("RoomExit").position;

            // Add the spawned room to the list of spawned rooms.
            spawnedRooms.Add(roomListSelect);
        }
    }

    private Transform RoomSpawner(Transform roomPrefab, Vector3 spawnLocation)
    {
        Transform roomTransform = Instantiate(roomPrefab, spawnLocation, Quaternion.identity);
        return roomTransform;
    }
}
