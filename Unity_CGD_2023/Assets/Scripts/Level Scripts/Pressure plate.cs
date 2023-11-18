using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public TargetObject targetObject; // Drag and drop the TargetObject script here in the Unity Editor

    public float timerDuration = 5f; // Adjust the duration in the Inspector
    private float timer = 0f;

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

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the player is standing on the pressure plate
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player touched plate");
            // Activate the timer
            timer = timerDuration;

            // Call the ReactToPressurePlate function on the TargetObject
            targetObject.ReactToPressurePlate();
        }
    }

    void ReactToTimer()
    {
      
        Debug.Log("Timer reached zero! Reacting to timer.");

        // call a method on TargetObject
        targetObject.ReactToTimer();
    }
}