using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private int activationCounter = 0;
    private List<string> activatedPlates = new List<string>();

    public int requiredPlatesToOpen = 2;  // Adjust this value in the Unity Editor

    public void IncreaseCounter()
    {
       

        // Check if the required number of pressure plates are activated
        if (activationCounter == requiredPlatesToOpen)
        {
            Debug.Log($"Required number of plates ({requiredPlatesToOpen}) activated. Opening the door!");
            OpenDoor();
        }
    }

    public void AddActivatedPlate(string plateColor)
    {
        if (activationCounter < requiredPlatesToOpen)
        {
            if (!activatedPlates.Contains(plateColor))
            {
                activatedPlates.Add(plateColor);
                Debug.Log(plateColor + " pressure plate added to the activated list.");
            }
            else if (activatedPlates.Contains(plateColor))
            {
                activationCounter++;
                Debug.Log(activationCounter);
                activatedPlates.Clear();
            }
        }
    }


    public void RemoveActivatedPlate(string plateColor)
    {
        if (activatedPlates.Contains(plateColor))
        {
            activatedPlates.Remove(plateColor);
            Debug.Log(plateColor + " pressure plate removed from the activated list.");
        }
    }

    private void OpenDoor()
    {
        // Implement your door opening logic here
        Debug.Log("Door is open!");
       
    }
}

//else if (activatedPlates.Contains(plateColor))