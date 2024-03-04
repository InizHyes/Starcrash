using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SafeAreaManager : MonoBehaviour
{
    public Tilemap[] safeAreaPatterns; // Array of tilemaps representing different safe area patterns
    public float patternSwitchInterval = 5f; // Interval between switching patterns
    private int currentIndex = 0; // Index of the current safe area pattern
    private float timer = 0f; // Timer to track pattern switching

    private void Start()
    {
        // Initially activate the first pattern and deactivate others
        for (int i = 0; i < safeAreaPatterns.Length; i++)
        {
            if (i == currentIndex)
                ActivatePattern(i);
            else
                DeactivatePattern(i);
        }
    }

    private void Update()
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

    private void SwitchPattern()
    {
        // Deactivate current pattern
        DeactivatePattern(currentIndex);

        // Increment index or loop back to 0 if reached the end
        currentIndex = (currentIndex + 1) % safeAreaPatterns.Length;

        // Activate new pattern
        ActivatePattern(currentIndex);
    }

    private void ActivatePattern(int index)
    {
        safeAreaPatterns[index].gameObject.SetActive(true); // Activate tilemap
        // Enable other components related to this pattern like colliders, renderers etc.
        // For example:
        // safeAreaPatterns[index].GetComponent<TilemapCollider2D>().enabled = true;
        // safeAreaPatterns[index].GetComponent<SpriteRenderer>().enabled = true;
    }

    private void DeactivatePattern(int index)
    {
        safeAreaPatterns[index].gameObject.SetActive(false); // Deactivate tilemap
        // Disable other components related to this pattern
        // For example:
        // safeAreaPatterns[index].GetComponent<TilemapCollider2D>().enabled = false;
        // safeAreaPatterns[index].GetComponent<SpriteRenderer>().enabled = false;
    }
}
