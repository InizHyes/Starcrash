using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SafeAreaManager : MonoBehaviour
{
    public Tilemap[] safeAreaPatterns; // Array of tilemaps representing different safe area patterns
    public int amountOfTilemaps; // Set the number of tilemaps in inspector
    public float patternSwitchInterval = 5f; // Interval between switching patterns
    public float pulseDuration = 1f; // Duration of each pulse cycle
    public Color fadeToColor = Color.black; // Color the tilemap fades to
    private int[] activationCount; // Array to track the activation count for each tilemap
    private int currentIndex = 0; // Index of the current safe area pattern
    private float timer = 0f; // Timer to track pattern switching
    private bool isFadingOut = false; // Flag to track if currently fading out
    [Tooltip("Must be a minus number and one less than the amount OfTilemaps")]
    public int tilemapTracker = -6;
    private bool roomComplete = false;
    private bool playerEntered = false; // Flag to track if the player has entered the collider
    private Collider2D boxCollider;

    private void Start()
    {
        // Set the number of tilemaps in safeAreaPatterns
        int numberOfTilemaps = safeAreaPatterns.Length;
        activationCount = new int[numberOfTilemaps]; // Initialize the array

        // Initially activate the first pattern and deactivate others
        for (int i = 0; i < numberOfTilemaps; i++)
        {
            if (i == currentIndex)
                ActivatePattern(i);
            else
                DeactivatePattern(i);
        }

        if (playerEntered == true)
        {
            StartCoroutine(PulseTilemap(safeAreaPatterns[0], fadeToColor, pulseDuration));
            activationCount[0]++; // Increment the activation count for the first tilemap

            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();
            // Call LockDoors function after spawning enemy
            doorManager.LockDoors();
        }

    }

    private void Update()
    {
        if (playerEntered == true)
        {
            completeCheck();

            if (!roomComplete)
            {
                // Update timer
                timer += Time.deltaTime;

                // If the timer exceeds the interval, switch pattern
                if (timer >= patternSwitchInterval)
                {
                    SwitchPattern();
                    timer = 0f; // Reset timer
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();

            doorManager.LockDoors();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }

    private void SwitchPattern()
    {
        // Deactivate current pattern
        DeactivatePattern(currentIndex);

        // Increment index or loop back to 0 if reached the end
        currentIndex = (currentIndex + 1) % safeAreaPatterns.Length;

        // Activate new pattern
        ActivatePattern(currentIndex);

        // Toggle fade direction
        isFadingOut = !isFadingOut;

        // Pulse in and out every even-indexed tilemap
        for (int i = 0; i < safeAreaPatterns.Length; i++)
        {
            if (i % 2 != 0) // Check if the index is even
            {
                StartCoroutine(PulseTilemap(safeAreaPatterns[i], fadeToColor, pulseDuration));
                activationCount[i]++; // Increment the activation count for the pulsed tilemap
            }
        }
    }

    private void ActivatePattern(int index)
    {
        safeAreaPatterns[index].GetComponent<TilemapCollider2D>().enabled = true;
        safeAreaPatterns[index].GetComponent<TilemapRenderer>().enabled = true;
    }

    private void DeactivatePattern(int index)
    {
        safeAreaPatterns[index].GetComponent<TilemapCollider2D>().enabled = false;
        safeAreaPatterns[index].GetComponent<TilemapRenderer>().enabled = false;
        tilemapTracker = tilemapTracker + 1;
    }

    private void DeactivateAllPatterns()
    {
        for (int i = 0; i < safeAreaPatterns.Length; i++)
        {
            safeAreaPatterns[i].GetComponent<TilemapCollider2D>().enabled = false;
            safeAreaPatterns[i].GetComponent<TilemapRenderer>().enabled = false;
            tilemapTracker++; // Increment the tracker for each deactivated tilemap
        }
    }

    private IEnumerator PulseTilemap(Tilemap tilemap, Color fadeColor, float duration)
    {
        float timer = 0f;
        float halfDuration = duration / 2f;

        Color startColor = tilemap.color;

        while (true)
        {
            timer += Time.deltaTime;
            float t = Mathf.PingPong(timer, halfDuration) / halfDuration; // PingPong between 0 and 1

            tilemap.color = Color.Lerp(startColor, fadeColor, t);
            yield return null;
        }
    }

    private void completeCheck()
    {
        if (tilemapTracker >= amountOfTilemaps)
        {
            roomComplete = true;
            
            DeactivateAllPatterns();

            // Get the Collider2D component attached to the GameObject
            boxCollider = GetComponent<Collider2D>();

            boxCollider.enabled = false;

            // Find the GameObject with the DoorManager script attached
            GameObject doorManagerObject = GameObject.Find("DoorManager");
            DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();

            doorManager.OpenDoors();

            tilemapTracker = 0;
        }
    }
}
