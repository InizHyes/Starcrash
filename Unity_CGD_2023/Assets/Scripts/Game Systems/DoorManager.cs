using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{
    public RoomManager roomManager;
    public GameObject roomCompleteText;

    public void OpenDoors()
    {
        
        GameObject[] lockedDoors = GameObject.FindGameObjectsWithTag("LockedDoor");

        
        foreach (GameObject door in lockedDoors)
        {
            door.GetComponent<TilemapRenderer>().enabled = false;
            door.GetComponent<TilemapCollider2D>().enabled = false;
        }

        
        roomManager.numberOfRoomsCompleted++;

        
        roomCompleteText.SetActive(true);

        
        StartCoroutine(DeactivateRoomCompleteText());
    }

    IEnumerator DeactivateRoomCompleteText()
    {
        
        yield return new WaitForSeconds(2f);

        
        roomCompleteText.SetActive(false);
    }

    public void LockDoors()
    {
        
        GameObject[] lockedDoors = GameObject.FindGameObjectsWithTag("LockedDoor");

        
        foreach (GameObject door in lockedDoors)
        {
            door.GetComponent<TilemapRenderer>().enabled = true;
            door.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
