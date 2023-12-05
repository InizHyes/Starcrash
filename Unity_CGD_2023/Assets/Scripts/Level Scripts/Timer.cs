using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float initialTime = 10f; // Initial time in seconds, you can change this in the inspector
    private float timer; // Current timer value
    private bool timerActive = false; // Flag to check if the timer is active

    private void Start()
    {
        timer = initialTime;
    }

    private void Update()
    {
        if (timerActive && timer > 0f)
        {
            timer -= Time.deltaTime; // Decrease timer by the time passed since the last frame

            // You can add additional actions or events here while the timer is running
        }
        else if (timerActive)
        {
            // Timer reached zero, you can add actions or events for when the timer expires

            // For example, you can disable the timer script or perform other actions
            // depending on the specific use case.
            // gameObject.GetComponent<TimerScript>().enabled = false;

            // Reset the timer for the next use
            ResetTimer();
        }
    }

    // This method can be called to reset the timer to its initial value
    public void ResetTimer()
    {
        timer = initialTime;
        timerActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Activate the timer when the player enters the trigger area
            timerActive = true;
        }
    }
}
