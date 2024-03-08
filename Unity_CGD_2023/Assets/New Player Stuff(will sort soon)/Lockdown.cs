using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockdown : MonoBehaviour
{
    public float maxValue = 100f; // Maximum value of the bar
    public float decreaseRate = 10f; // Rate at which the bar decreases per second
    public float increaseRate = 20f; // Rate at which the bar increases per second
    public float increaseDelay = 2f; // Delay before the bar starts increasing after hitting zero
    private bool isIncreasingDelayed; // Flag to indicate if the increase process is delayed
    private bool isCollidingWithFloor; // Flag to indicate if colliding with the floor


    private float currentValue; // Current value of the bar
    private float delayTimer; // Timer for delay before increasing

    private Player playerScript;
    private Rigidbody2D rb;

    private bool isClampingLocked; // Flag to indicate if the button is locked

    Player player;
    float shootForce;

    private bool isClamped = false;

    void Start()
    {
        currentValue = maxValue; // Initialize the bar value to its maximum
        isClampingLocked = false;
        isIncreasingDelayed = false; // Increase process is not delayed initially
        delayTimer = 0f; // Initialize delay timer

        GameObject player = GameObject.FindWithTag("Player");
        playerScript = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();


    }

    private void Update()
    {

        // Decrease the bar value if the button is pressed
        if (isClamped)
        {
            currentValue -= decreaseRate * Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue); // Clamp the value to [0, maxValue]

        }
        // Increase the bar value if the button is not pressed and the value is not at maximum
        else if (currentValue < maxValue && !isIncreasingDelayed)
        {
            currentValue += increaseRate * Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue); // Clamp the value to [0, maxValue]
        }
        if (isIncreasingDelayed)
        {
            delayTimer += Time.deltaTime;

            // Check if the delay timer has reached the delay duration
            if (delayTimer >= increaseDelay)
            {
                isIncreasingDelayed = false; // Stop the delay
                delayTimer = 0f; // Reset the delay timer
            }
        }

        // Check if the bar value is zero, if yes, set the button lock flag to true
        if (currentValue <= 0f)
        {

            isClampingLocked = true;
            isClamped = false;

        }
        else if (currentValue >= maxValue && isClampingLocked)
        {
            isClampingLocked = false;
            delayTimer = 0f;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isCollidingWithFloor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            isCollidingWithFloor = false;
        }
    }

    public void Locked()
    {
        if (isCollidingWithFloor && !isClampingLocked)
        {

            isClamped = !isClamped;
            if (isClamped)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePosition;

            }
            else if (!isClamped)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            }
        }

    }
}
