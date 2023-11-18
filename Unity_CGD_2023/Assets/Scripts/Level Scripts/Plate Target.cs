using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetObject : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    void Start()
    {

        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    public void ReactToPressurePlate()
    {
        
        Debug.Log("Player is reacting to the pressure plate!");

        // Disable the TilemapRenderer 
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
        }

        // Enable the TargetObject 
        gameObject.SetActive(true);
    }

    public void ReactToTimer()
    {
        // Do something when the timer reaches zero
        Debug.Log("TargetObject is reacting to the timer!");

        // Enable the TilemapRenderer
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = true;
        }

        // Disable the TargetObject 
        gameObject.SetActive(false);
    }
}
/*

{
    void Start()
    {
        // Disable the TargetObject at the beginning
        gameObject.SetActive(false);
    }

    public void ReactToPressurePlate()
    {
        // Do something when the pressure plate is touched by the player
        Debug.Log("TargetObject is reacting to the pressure plate!");

        // Enable the TargetObject (you can customize this based on your specific actions)
        gameObject.SetActive(true);
    }
}
*/