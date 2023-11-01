using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private Transform StartingRoom;
    [SerializeField] private List <Transform> roomList;

    private Vector3 lastRoomExit;
    [SerializeField] private int roomsToSpawn;

    private void Awake()
    { 
        lastRoomExit = StartingRoom.Find("RoomExit").position;
        
        for (int i = 0; i < roomsToSpawn; i++)
        {
        RoomSpawner();
        }


    }

    private void RoomSpawner()
    {
        Transform roomListSelect = roomList[Random.Range(0, roomList.Count)];
        Transform lastRoomTransform = RoomSpawner(roomListSelect, lastRoomExit);
        lastRoomExit = lastRoomTransform.Find("RoomExit").position;
    }

    private Transform RoomSpawner(Transform roomList, Vector3 spawnLocation)
    {
        Transform RoomTransform = Instantiate(roomList, spawnLocation, Quaternion.identity);

        return RoomTransform;
    }
}
