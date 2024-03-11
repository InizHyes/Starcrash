using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomManager : MonoBehaviour
{
    public int numberOfRoomsCompleted = 0;
    public int numberOfRoomsTotal = 9;
    public GameObject bossToSpawn;

    public TMP_Text roomsCompletedText; // Reference to the Text UI element

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomsCompletedText();

        if (numberOfRoomsCompleted == 8)
        {
            bossToSpawn.SetActive(true);
        }
    }

    public void SpawnBoss()
    {
        bossToSpawn.SetActive(true);
    }

    // Method to update the text value to reflect the current value of numberOfRoomsCompleted
    void UpdateRoomsCompletedText()
    {
        roomsCompletedText.text = "Rooms Completed: " + numberOfRoomsCompleted.ToString() + " / " + numberOfRoomsTotal.ToString();
    }
}
