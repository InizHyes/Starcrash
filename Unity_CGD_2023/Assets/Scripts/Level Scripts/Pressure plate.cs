using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public TargetObject targetObject; // Drag and drop the specific TargetObject script here in the Unity Editor

    public float timerDuration = 5f; // Adjust the duration in the Inspector
    private float timer = 0f;

    public static int activePressurePlates = 0; // Static variable to track the number of active pressure plates
    public int requiredPressurePlates = 2; // Adjust the required number in the Inspector

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ReactToTimer();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is standing on the pressure plate
        if (other.CompareTag("Player"))
        {
            // Activate the timer
            timer = timerDuration;

            // Increment the number of active pressure plates
            activePressurePlates++;

            // Call the ReactToPressurePlate function on the specific TargetObject
            targetObject.ReactToPressurePlate();

            // Check if the required number of pressure plates is reached
            if (activePressurePlates >= requiredPressurePlates)
            {
                // Trigger the behavior when the required number is reached
                ActivateRequiredPressurePlates();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the pressure plate
        if (other.CompareTag("Player"))
        {
            // Decrement the number of active pressure plates
            activePressurePlates--;

            // Ensure the number of active pressure plates doesn't go below zero
            activePressurePlates = Mathf.Max(0, activePressurePlates);
        }
    }

    void ReactToTimer()
    {
        // Do something when the timer reaches zero
        Debug.Log("Timer reached zero! Reacting to timer.");

        // Call a method on the specific TargetObject or perform other actions here
        targetObject.ReactToTimer();
    }

    void ActivateRequiredPressurePlates()
    {
        // Do something when the required number of pressure plates is reached
        Debug.Log("Required number of pressure plates reached! Activating something.");
    }
}