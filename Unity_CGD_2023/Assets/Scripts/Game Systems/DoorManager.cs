using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{
    public RoomManager roomManager; // Reference to the RoomManager script

    public void OpenDoors()
    {
        // Find all game objects with the tag "LockedDoor"
        GameObject[] lockedDoors = GameObject.FindGameObjectsWithTag("LockedDoor");

        // Iterate through each locked door and set it inactive
        foreach (GameObject door in lockedDoors)
        {
            door.GetComponent<TilemapRenderer>().enabled = false;
            door.GetComponent<TilemapCollider2D>().enabled = false;
        }

        // Increment the numberOfRoomsCompleted variable in RoomManager
        roomManager.numberOfRoomsCompleted++;
    }

    public void LockDoors()
    {
        // Find all game objects with the tag "LockedDoor"
        GameObject[] lockedDoors = GameObject.FindGameObjectsWithTag("LockedDoor");

        // Iterate through each locked door and set it inactive
        foreach (GameObject door in lockedDoors)
        {
            door.GetComponent<TilemapRenderer>().enabled = true;
            door.GetComponent<TilemapCollider2D>().enabled = true;
        }

    }
}
