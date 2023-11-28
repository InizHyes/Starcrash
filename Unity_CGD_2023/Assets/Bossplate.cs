using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    public float activationTime = 7.5f;
    private bool isActivated = false;
    private float currentTime = 0f;

    // Color identifier for the pressure plate
    public ColorType colorType;

    public bool isFirstPressurePlateActivated = false;

    // Reference to the script that handles the global integer
    public GlobalIntegerScript globalIntegerScript;

    // Variable to check if this is the first pressure plate
    public bool isFirstPressurePlate = false;

    // Keep track of players on each plate
    private bool playerOnThisPlate = false;
    private bool playerOnOtherPlate = false;

    private void Update()
    {
        if (isActivated)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= activationTime)
            {
                if (isFirstPressurePlate)
                {
                    //Debug.Log("First pressure plate (" + colorType + ") activated.");
                    isFirstPressurePlateActivated = true;
                    if (playerOnOtherPlate)
                    {
                        // Both players have stood on plates with the same color tag
                        globalIntegerScript.IncreaseGlobalInteger();
                        Debug.Log("Global integer increased.");
                        ResetPressurePlate(); // Reset for future activations
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the pressure plate.");
            isActivated = true;

            if (isFirstPressurePlate)
            {
                playerOnThisPlate = true;
            }
            else
            {
                playerOnOtherPlate = true;
                if (isFirstPressurePlateActivated && playerOnThisPlate)
                {
                    // Both players have stood on plates with the same color tag
                    globalIntegerScript.IncreaseGlobalInteger();
                    Debug.Log("Global integer increased.");
                    ResetPressurePlate(); // Reset for future activations
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFirstPressurePlate)
            {
                playerOnThisPlate = false;
            }
            else
            {
                playerOnOtherPlate = false;
            }
            ResetPressurePlate();
        }
    }

    private void ResetPressurePlate()
    {
        isActivated = false;
        currentTime = 0f;
    }
}

// Enum to represent different color types
public enum ColorType
{
    Orange,
    Blue,
    Purple,
    Green
}
