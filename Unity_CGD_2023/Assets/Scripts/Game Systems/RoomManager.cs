using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int numberOfRoomsCompleted = 0;
    public int numberOfRoomsTotal = 9;
    public GameObject bossToSpawn;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfRoomsCompleted == 8)
        {
            bossToSpawn.SetActive(true);
        }
    }

    public void SpawnBoss()
    {
        bossToSpawn.SetActive(true);
    }
}
