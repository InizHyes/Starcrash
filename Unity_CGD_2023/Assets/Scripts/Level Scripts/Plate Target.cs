using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public int requiredActivatedPlates = 2;  // Set the required number of activated plates in the Inspector
    private int activatedPlates = 0;
    private bool isOpen = false;
    private TilemapRenderer tilemapRenderer;

    private void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    public void IncreaseActivatedPlates()
    {
        activatedPlates++;

        if (activatedPlates >= requiredActivatedPlates)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log($"Activated plates: {activatedPlates}/{requiredActivatedPlates}");
        }
    }

    private void OpenDoor()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
            isOpen = true;
            Debug.Log("Door opened!");
        }
        else
        {
            Debug.LogError("TilemapRenderer component not found on the door!");
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}