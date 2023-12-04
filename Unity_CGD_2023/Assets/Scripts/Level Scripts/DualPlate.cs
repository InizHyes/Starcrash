using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPlate : MonoBehaviour
{
    public string plateColor;  // Assign the color (e.g., "orange", "green", "blue", "purple") in the Unity Editor
    public DoorScript doorScript;  // Reference to the DoorScript attached to the corresponding door

    private bool isActivated = false;
    private float activationTime = 0f;

    private bool isOccupied = false;  // New variable to track plate occupancy




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOccupied)
        {
            Debug.Log("Player entered " + plateColor + " pressure plate area.");
            ActivatePlate();
            isOccupied = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited " + plateColor + " pressure plate area.");
            DeactivatePlate();
            isOccupied = false;
        }
    }

    public void Update()
    {
        if (isActivated)
        {
            activationTime += Time.deltaTime;

            if (activationTime >= 7.5f)
            {
                Debug.Log(plateColor + " pressure plates activated for 7.5 seconds.");
                doorScript.AddActivatedPlate(plateColor);
                isActivated = false;
            }
        }
    }

    private void ActivatePlate()
    {
        if (!isActivated)
        {
            Debug.Log(plateColor + " pressure plate activated.");
            isActivated = true;
            activationTime = 0f;
           
            
        }
    }

   

    private void DeactivatePlate()
    {
        if (isActivated)
        {
            Debug.Log(plateColor + " pressure plate deactivated.");
            isActivated = false;
            activationTime = 0f;
           
            doorScript.RemoveActivatedPlate(plateColor);
        }
    }
}